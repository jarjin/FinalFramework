ft_main = function (external_ft, opts) {
	ft = external_ft;
	opts = opts || {};

	//
	// config
	//

	var cfg = {
		profile_mode              : opts.profile_mode              === undefined ? false     : opts.profile_mode,
		verbose_mode              : opts.verbose_mode              === undefined ? false     : opts.verbose_mode,

		graphics_scale            : opts.graphics_scale            === undefined ? 1.0       : opts.graphics_scale,
		scale_precision           : opts.scale_precision           === undefined ? 0.01      : opts.scale_precision,

		optimize_big_items        : opts.optimize_big_items        === undefined ? true      : opts.optimize_big_items,
		optimize_small_items      : opts.optimize_small_items      === undefined ? true      : opts.optimize_small_items,
		optimize_static_items     : opts.optimize_static_items     === undefined ? true      : opts.optimize_static_items,
		optimize_single_graphics  : opts.optimize_single_graphics  === undefined ? true      : opts.optimize_single_graphics,

		open_documents            : opts.open_documents            === undefined ? []        : opts.open_documents,
		export_path_postfix       : opts.export_path_postfix       === undefined ? "_export" : opts.export_path_postfix,
		close_after_conversion    : opts.close_after_conversion    === undefined ? false     : opts.close_after_conversion,
		revert_after_conversion   : opts.revert_after_conversion   === undefined ? true      : opts.revert_after_conversion,
		max_convertible_selection : opts.max_convertible_selection === undefined ? 3900      : opts.max_convertible_selection
	};

	//
	// document
	//

	var ftdoc = {};

	ftdoc.prepare = function (doc) {
		ft.type_assert(doc, Document);
		ft.profile_function(cfg.profile_mode, function() { ftdoc.prepare_folders(doc);        }, "Prepare folders");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.full_exit_edit_mode(doc);    }, "Full exit edit mode");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.remove_unused_items(doc);    }, "Remove unused items");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.prepare_all_bitmaps(doc);    }, "Prepare all bitmaps");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.unlock_all_timelines(doc);   }, "Unlock all timelines");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.prepare_all_labels(doc);     }, "Prepare all labels");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.prepare_all_tweens(doc);     }, "Prepare all tweens");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.prepare_all_groups(doc);     }, "Prepare all groups");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.calculate_item_scales(doc);  }, "Calculate item scales");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.optimize_all_timelines(doc); }, "Optimize all timelines");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.rasterize_all_shapes(doc);   }, "Rasterize all shapes");
		ft.profile_function(cfg.profile_mode, function() { ftdoc.export_swf(doc);             }, "Export swf");
	};

	ftdoc.get_temp = function (doc) {
		ft.type_assert(doc, Document);
		if (!ftdoc.get_temp_ft_temp) {
			ftdoc.get_temp_ft_temp = {};
		}
		if (!ftdoc.get_temp_ft_temp[doc.pathURI]) {
			ftdoc.get_temp_ft_temp[doc.pathURI] = {
				max_scales : {}
			};
		}
		return ftdoc.get_temp_ft_temp[doc.pathURI];
	};

	ftdoc.calculate_item_prefer_scale = function (doc, optional_item) {
		ft.type_assert(doc, Document);
		ft.type_assert_if_defined(optional_item, LibraryItem);
		var final_scale = cfg.graphics_scale;
		if (optional_item && (cfg.optimize_big_items || cfg.optimize_small_items)) {
			var item_name  = optional_item.name;
			var max_scales = ftdoc.get_temp(doc).max_scales;
			if (max_scales && max_scales.hasOwnProperty(item_name)) {
				var max_scale  = max_scales[item_name];
				var big_item   = cfg.optimize_big_items   && (max_scale - cfg.scale_precision > 1.0);
				var small_item = cfg.optimize_small_items && (max_scale + cfg.scale_precision < 1.0);
				if (big_item || small_item) {
					final_scale *= max_scale;
				}
			}
		}
		return final_scale;
	};

	ftdoc.convert_selection_to_bitmap = function (doc, location_name, optional_item) {
		ft.type_assert(doc, Document);
		ft.type_assert(location_name, 'string');
		ft.type_assert_if_defined(optional_item, LibraryItem);
		
		var selection_r = doc.getSelectionRect();
		if ( selection_r ) { // getSelectionRect returns 0 for empty clips
			var selection_w = selection_r.right  - selection_r.left;
			var selection_h = selection_r.bottom - selection_r.top;

			var max_scale    = cfg.max_convertible_selection / Math.max(selection_w, selection_h);
			var prefer_scale = ftdoc.calculate_item_prefer_scale(doc, optional_item);
			var final_scale  = Math.min(prefer_scale, max_scale);

			if (final_scale < prefer_scale) {
				var down_scale = Math.floor(final_scale / prefer_scale * 1000) * 0.001;
				ft.trace_fmt(
					"[Warning] {0}\n" +
					"- Converted element was downscaled ({1}x) to maximum allowed size ({2}px)",
					location_name, down_scale, cfg.max_convertible_selection);
			}
			
			if (ft.approximately(final_scale, 1.0, cfg.scale_precision)) {
				(function() {
					var elem_r  = doc.getSelectionRect();

					var elem_x  = elem_r.left;
					var elem_y  = elem_r.top;
					var elem_w  = elem_r.right  - elem_r.left;
					var elem_h  = elem_r.bottom - elem_r.top;

					var elem_dx = Math.round(elem_x) - elem_x;
					var elem_dy = Math.round(elem_y) - elem_y;
					var elem_dw = Math.round(elem_w) - elem_w;
					var elem_dh = Math.round(elem_h) - elem_h;

					doc.convertSelectionToBitmap();
					var elem = doc.selection[0];

					elem.x      -= elem_dx;
					elem.y      -= elem_dy;
					elem.width  -= elem_dw;
					elem.height -= elem_dh;
				})();
			} else {
				(function() {
					var wrapper_item_name = ft.gen_unique_name();
					var wrapper_item = doc.convertToSymbol("graphic", wrapper_item_name , "top left");
					fttim.recursive_scale_filters(doc, wrapper_item, final_scale);

					var elem = doc.selection[0];
					elem.setTransformationPoint({x: 0, y: 0});
					doc.scaleSelection(final_scale, final_scale);

					var elem_x  = elem.x;
					var elem_y  = elem.y;
					var elem_w  = elem.width;
					var elem_h  = elem.height;

					var elem_dx = Math.round(elem_x) - elem_x;
					var elem_dy = Math.round(elem_y) - elem_y;
					var elem_dw = Math.round(elem_w) - elem_w;
					var elem_dh = Math.round(elem_h) - elem_h;

					doc.convertSelectionToBitmap();
					elem = doc.selection[0];

					elem.x      -= elem_dx;
					elem.y      -= elem_dy;
					elem.width  -= elem_dw;
					elem.height -= elem_dh;

					elem.setTransformationPoint({x: (elem_x - elem.x), y: (elem_y - elem.y)});
					doc.scaleSelection(1.0 / final_scale, 1.0 / final_scale);
					
					fttim.recursive_scale_filters(doc, wrapper_item, 1.0 / final_scale);
				})();
			}
		}
	};

	ftdoc.prepare_folders = function (doc) {
		ft.type_assert(doc, Document);
		var export_folder = ftdoc.get_export_folder(doc);
		if (!FLfile.exists(export_folder) && !FLfile.createFolder(export_folder)) {
			throw "Can't create document export folder ({0})!"
				.format(export_folder);
		}
	};

	ftdoc.get_export_folder = function (doc) {
		ft.type_assert(doc, Document);
		return ft.combine_path(
			ft.escape_path(doc.pathURI),
			cfg.export_path_postfix + "/");
	};

	ftdoc.full_exit_edit_mode = function (doc) {
		ft.type_assert(doc, Document);
		for (var i = 0; i < 100; ++i) {
			doc.exitEditMode();
		}
	};

	ftdoc.remove_unused_items = function (doc) {
		ft.type_assert(doc, Document);
		var unused_items = doc.library.unusedItems;
		if (unused_items && unused_items !== undefined) {
			ft.array_reverse_foreach(unused_items, function(item) {
				if (cfg.verbose_mode) {
					ft.trace_fmt("Remove unused item: {0}", item.name);
				}
				doc.library.deleteItem(item.name);
			});
		}
	};
	
	ftdoc.prepare_all_bitmaps = function (doc) {
		ft.type_assert(doc, Document);
		ftlib.prepare_all_bitmaps(doc.library);
	};

	ftdoc.unlock_all_timelines = function (doc) {
		ft.type_assert(doc, Document);
		ftlib.unlock_all_timelines(doc, doc.library);
		fttim.unlock_all_layers(doc, doc.getTimeline());
	};

	ftdoc.prepare_all_labels = function (doc) {
		ft.type_assert(doc, Document);
		ftlib.prepare_all_labels(doc, doc.library);
		fttim.prepare_all_labels(doc, doc.getTimeline());
	};
	
	ftdoc.prepare_all_tweens = function (doc) {
		ft.type_assert(doc, Document);
		ftlib.prepare_all_tweens(doc, doc.library);
		fttim.prepare_all_tweens(doc, doc.getTimeline());
	};

	ftdoc.prepare_all_groups = function (doc) {
		ft.type_assert(doc, Document);
		var arr1 = ftlib.prepare_all_groups(doc, doc.library);
		var arr2 = fttim.prepare_all_groups(doc, doc.getTimeline());
		var new_symbols = arr1.concat(arr2);
		var process_item = function (item) {
			if (doc.library.editItem(item.name)) {
				var arr3 = fttim.prepare_all_groups(doc, item.timeline);
				new_symbols = new_symbols.concat(arr3);
				doc.exitEditMode();
			}
		};
		while (new_symbols.length > 0) {
			var new_symbols_copy = ft.array_clone(new_symbols);
			new_symbols = [];
			ft.array_foreach(new_symbols_copy, process_item);
		}
	};

	ftdoc.calculate_item_scales = function (doc) {
		ft.type_assert(doc, Document);

		var max_scales = {};

		var walk_by_timeline = function(timeline, func, acc) {
			ft.type_assert(timeline, Timeline);
			ft.type_assert(func, Function);
			ft.array_foreach(timeline.layers, function (layer) {
				ft.array_foreach(layer.frames, function (frame) {
					ft.array_foreach(frame.elements, function (elem) {
						walk_by_timeline(
							elem.libraryItem.timeline,
							func,
							func(elem, acc));
					}, fttim.is_symbol_instance);
				}, fttim.is_keyframe);
			});
		};

		var walk_by_library = function(lib, func, acc) {
			ft.type_assert(lib, Library);
			ft.type_assert(func, Function);
			ft.array_foreach(lib.items, function (item) {
				walk_by_timeline(item.timeline, func, acc);
			}, ftlib.is_symbol_item);
		};

		var x_func = function(elem, acc) {
			var elem_sx   = elem.scaleX * acc;
			var item_name = elem.libraryItem.name;
			max_scales[item_name] = Math.max(
				max_scales.hasOwnProperty(item_name) ? max_scales[item_name] : elem_sx,
				elem_sx);
			return elem_sx;
		};

		var y_func = function(elem, acc) {
			var elem_sy   = elem.scaleY * acc;
			var item_name = elem.libraryItem.name;
			max_scales[item_name] = Math.max(
				max_scales.hasOwnProperty(item_name) ? max_scales[item_name] : elem_sy,
				elem_sy);
			return elem_sy;
		};

		walk_by_library(doc.library, x_func, 1.0);
		walk_by_timeline(doc.getTimeline(), x_func, 1.0);

		walk_by_library(doc.library, y_func, 1.0);
		walk_by_timeline(doc.getTimeline(), y_func, 1.0);
		
		ftdoc.get_temp(doc).max_scales = max_scales;

		if (cfg.verbose_mode) {
			for (var item_name in max_scales) {
				var max_scale = max_scales.hasOwnProperty(item_name) ? max_scales[item_name] : 1.0;
				if (max_scale - cfg.scale_precision > 1.0) {
					ft.trace_fmt("Big item for optimize: {0} - {1}", item_name, max_scale);
				} else if (max_scale + cfg.scale_precision < 1.0) {
					ft.trace_fmt("Small item for optimize: {0} - {1}", item_name, max_scale);
				}
			}
		}
	};

	ftdoc.optimize_all_timelines = function (doc) {
		ft.type_assert(doc, Document);
		if (cfg.optimize_static_items) {
			ft.profile_function(cfg.profile_mode, function () {
				ftlib.optimize_static_items(doc, doc.library);
			}, "Optimize static items");
		}
		if (cfg.optimize_single_graphics) {
			ft.profile_function(cfg.profile_mode, function () {
				ftlib.optimize_single_graphics(doc, doc.library);
			}, "Optimize single graphics");
		}
	};

	ftdoc.rasterize_all_shapes = function (doc) {
		ft.type_assert(doc, Document);
		ftlib.rasterize_all_shapes(doc, doc.library);
		fttim.rasterize_all_shapes(doc, doc.getTimeline());
	};

	ftdoc.export_swf = function (doc) {
		ft.type_assert(doc, Document);
		doc.exportSWF(ftdoc.get_export_swf_path(doc));
	};

	ftdoc.get_export_swf_path = function (doc) {
		ft.type_assert(doc, Document);
		return ft.combine_path(
			ftdoc.get_export_folder(doc),
			doc.name + ".swf");
	};

	//
	// library
	//

	var ftlib = {};

	ftlib.is_folder_item = function (item) {
		ft.type_assert(item, LibraryItem);
		return item.itemType == "folder";
	};

	ftlib.is_bitmap_item = function (item) {
		ft.type_assert(item, LibraryItem);
		return item.itemType == "bitmap";
	};

	ftlib.is_symbol_item = function (item) {
		ft.type_assert(item, LibraryItem);
		return item.itemType == "button" || item.itemType == "graphic" || item.itemType == "movie clip";
	};

	ftlib.find_item_by_name = function (library, item_name) {
		ft.type_assert(library, Library);
		ft.type_assert(item_name, 'string');
		for (var i = 0; i < library.items.length; ++i) {
			var item = library.items[i];
			if (item.name == item_name) {
				return item;
			}
		}
		return null;
	};

	ftlib.edit_all_items = function (doc, library, func, filter) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);
		ft.type_assert(func, Function);
		ft.type_assert_if_defined(filter, Function);
		ft.array_foreach(library.items, function (item) {
			if (library.editItem(item.name)) {
				func(item);
				doc.exitEditMode();
			}
		}, filter);
	};

	ftlib.edit_all_symbol_items = function (doc, library, func) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);
		ft.type_assert(func, Function);
		ftlib.edit_all_items(doc, library, func, ftlib.is_symbol_item);
	};
	
	ftlib.prepare_all_bitmaps = function (library) {
		ft.type_assert(library, Library);
		ft.array_foreach(library.items, function (item) {
			item.compressionType = "lossless";
		}, ftlib.is_bitmap_item);
	};

	ftlib.unlock_all_timelines = function (doc, library) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);
		ftlib.edit_all_symbol_items(doc, library, function (item) {
			fttim.unlock_all_layers(doc, item.timeline);
		});
	};

	ftlib.optimize_static_items = function (doc, library) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);

		var replaces = {};
		ft.array_reverse_foreach(library.items, function (item) {
			var new_item_name = ft.gen_unique_name();
			if (ftlib.bake_symbol_item(doc, library, item.name, new_item_name, 0)) {
				replaces[item.name] = new_item_name;
				if (cfg.verbose_mode) {
					ft.trace_fmt("Optimize static item: '{0}'", item.name);
				}
			} else {
				if (cfg.verbose_mode) {
					ft.trace_fmt("NOT Optimize static item: '{0}'", item.name);
				}
			}
		}, function (item) {
			return ftlib.is_symbol_item(item) && fttim.is_static(item.timeline);
		});

		ftlib.edit_all_symbol_items(doc, library, function (item) {
			fttim.replace_baked_symbols(doc, item.timeline, replaces);
		});
		fttim.replace_baked_symbols(doc, doc.getTimeline(), replaces);
	};

	ftlib.bake_symbol_item = function (doc, library, item_name, new_item_name, first_frame) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);
		ft.type_assert(item_name, 'string');
		ft.type_assert(new_item_name, 'string');
		ft.type_assert(first_frame, 'number');

		if (library.itemExists(new_item_name)) {
			return true;
		}

		var item = ftlib.find_item_by_name(library, item_name);
		if (!item) {
			return false;
		}

		var item_frame_area = fttim.calculate_frame_area(item.timeline, first_frame);
		var item_elems_area = fttim.calculate_elems_area(item.timeline, first_frame);

		if (cfg.verbose_mode) {
			ft.trace_fmt(
				"Library item: '{0}'\n- frame area: {1}\n- elems area: {2}",
				item_name, item_frame_area, item_elems_area);
		}

		if (item_frame_area >= item_elems_area) {
			return false;
		}

		if (!library.addNewItem("graphic", new_item_name)) {
			return false;
		}

		if (!library.editItem(new_item_name)) {
			library.deleteItem(new_item_name);
			return false;
		}

		if (library.addItemToDocument({x: 0, y: 0}, item_name)) {
			var new_item_elem = doc.selection[0];
			new_item_elem.symbolType = "graphic";
			new_item_elem.firstFrame = first_frame;
			new_item_elem.setTransformationPoint({x: 0, y: 0});
			new_item_elem.transformX = 0;
			new_item_elem.transformY = 0;
			var location_name = "Symbol: {0}".format(item_name);
			ftdoc.convert_selection_to_bitmap(doc, location_name, item);
			doc.exitEditMode();
			return true;
		} else {
			doc.exitEditMode();
			library.deleteItem(new_item_name);
			return false;
		}
	};

	ftlib.optimize_single_graphics = function (doc, library) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);
		ft.array_reverse_foreach(library.items, function (item) {
			fttim.optimize_single_graphics(doc, item.timeline, item);
		}, ftlib.is_symbol_item);
		fttim.optimize_single_graphics(doc, doc.getTimeline(), null);
	};

	ftlib.rasterize_all_shapes = function (doc, library) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);
		ftlib.edit_all_symbol_items(doc, library, function (item) {
			fttim.rasterize_all_shapes(doc, item.timeline);
		});
	};

	ftlib.prepare_all_labels = function (doc, library) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);
		ftlib.edit_all_symbol_items(doc, library, function (item) {
			fttim.prepare_all_labels(doc, item.timeline);
		});
	};
	
	ftlib.prepare_all_tweens = function (doc, library) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);
		ftlib.edit_all_symbol_items(doc, library, function (item) {
			fttim.prepare_all_tweens(doc, item.timeline);
		});
	};

	ftlib.prepare_all_groups = function (doc, library) {
		ft.type_assert(doc, Document);
		ft.type_assert(library, Library);
		var new_symbols = [];
		ftlib.edit_all_symbol_items(doc, library, function (item) {
			var arr = fttim.prepare_all_groups(doc, item.timeline);
			new_symbols = new_symbols.concat(arr);
		});
		return new_symbols;
	};

	//
	// timeline
	//

	var fttim = {};
	
	fttim.is_element_locked = function (elem) {
		return elem.locked;
	};

	fttim.is_shape_element = function (elem) {
		return elem.elementType == "shape";
	};

	fttim.is_group_shape_element = function (elem) {
		return fttim.is_shape_element(elem) && elem.isGroup;
	};

	fttim.is_object_shape_element = function (elem) {
		return fttim.is_shape_element(elem) && elem.isDrawingObject;
	};

	fttim.is_simple_shape_element = function (elem) {
		return fttim.is_shape_element(elem) && !elem.isGroup && !elem.isDrawingObject;
	};

	fttim.is_complex_shape_element = function (elem) {
		return fttim.is_shape_element(elem) && (elem.isGroup || elem.isDrawingObject);
	};
	
	fttim.is_instance_element = function (elem) {
		return elem.elementType == "instance";
	};
	
	fttim.is_bitmap_instance = function (elem) {
		return fttim.is_instance_element(elem) && elem.instanceType == "bitmap";
	};

	fttim.is_symbol_instance = function (elem) {
		return fttim.is_instance_element(elem) && elem.instanceType == "symbol";
	};

	fttim.is_symbol_graphic_instance = function (elem) {
		return fttim.is_symbol_instance(elem) && elem.symbolType == "graphic";
	};

	fttim.is_symbol_graphic_single_frame_instance = function (elem) {
		return fttim.is_symbol_instance(elem) && elem.symbolType == "graphic" && elem.loop == "single frame";
	};

	fttim.is_symbol_movie_clip_instance = function (elem) {
		return fttim.is_symbol_instance(elem) && elem.symbolType == "movie clip";
	};
	
	fttim.is_anchor_frame = function (frame) {
		ft.type_assert(frame, Frame);
		return frame.labelType == "anchor";
	};

	fttim.is_tween_frame = function (frame) {
		ft.type_assert(frame, Frame);
		return frame.tweenType != "none";
	};

	fttim.is_shape_tween_frame = function (frame) {
		ft.type_assert(frame, Frame);
		return frame.tweenType == "shape";
	};
	
	fttim.is_motion_tween_frame = function (frame) {
		ft.type_assert(frame, Frame);
		return frame.tweenType == "motion";
	};

	fttim.is_keyframe = function (frame, frame_index) {
		ft.type_assert(frame, Frame);
		ft.type_assert(frame_index, 'number');
		return frame.startFrame == frame_index;
	};

	fttim.is_not_guide_layer = function(layer) {
		ft.type_assert(layer, Layer);
		return layer.layerType != "guide";
	};

	fttim.unlock_all_layers = function (doc, timeline) {
		ft.type_assert(doc, Document);
		ft.type_assert(timeline, Timeline);
		ft.array_foreach(timeline.layers, function (layer, layer_index) {
			layer.locked = false;
			layer.visible = true;
			ft.array_foreach(layer.frames, function (frame, frame_index) {
				var has_locked = ft.array_any(frame.elements, fttim.is_element_locked);
				if (has_locked) {
					timeline.currentFrame = frame_index;
					try {
						doc.unlockAllElements();
					} catch (e) {}
				}
			}, fttim.is_keyframe);
		});
	};

	fttim.calculate_elems_area = function (timeline, frame_index) {
		ft.type_assert(timeline, Timeline);
		ft.type_assert(frame_index, 'number');
		return ft.array_foldl(timeline.layers, function (layer, acc) {
			if (frame_index >= 0 && frame_index < layer.frames.length) {
				return ft.array_foldl(layer.frames[frame_index].elements, function (elem, acc2) {
					return acc2 + Math.round(elem.width) * Math.round(elem.height);
				}, acc);
			} else {
				return acc;
			}
		}, 0);
	};

	fttim.calculate_frame_area = function (timeline, frame_index) {
		ft.type_assert(timeline, Timeline);
		ft.type_assert(frame_index, 'number');
		var bounds = ft.array_foldl(timeline.layers, function (layer, acc) {
			if (frame_index >= 0 && frame_index < layer.frames.length) {
				return ft.array_foldl(layer.frames[frame_index].elements, function (elem, acc2) {
					acc2.left   = Math.min(acc2.left,   elem.left);
					acc2.right  = Math.max(acc2.right,  elem.left + elem.width);
					acc2.top    = Math.min(acc2.top,    elem.top);
					acc2.bottom = Math.max(acc2.bottom, elem.top + elem.height);
					return acc2;
				}, acc);
			} else {
				return acc;
			}
		}, {
			left:   Number.POSITIVE_INFINITY,
			right:  Number.NEGATIVE_INFINITY,
			top:    Number.POSITIVE_INFINITY,
			bottom: Number.NEGATIVE_INFINITY
		});
		var frame_width  = Math.max(0, bounds.right  - bounds.left);
		var frame_height = Math.max(0, bounds.bottom - bounds.top);
		return Math.round(frame_width) * Math.round(frame_height);
	};
	
	fttim.scale_elem_filters = function (elem, scale) {
		if (fttim.is_symbol_instance(elem)) {
			var elem_filters = elem.filters;
			if (elem_filters && elem_filters !== undefined) {
				ft.array_foreach(elem_filters, function (elem_filter, filter_index) {
					elem_filter.blurX *= scale;
					elem_filter.blurY *= scale;
				});
				elem.filters = elem_filters;
			}
		}
		if (fttim.is_group_shape_element(elem)) {
			ft.array_foreach(elem.members, function(member) {
				fttim.scale_elem_filters(member, scale);
			});
		}
	};

	fttim.recursive_scale_filters = function (doc, item, scale, optional_scaled_items) {
		ft.type_assert(doc, Document);
		ft.type_assert(item, LibraryItem);
		ft.type_assert(scale, 'number');
		ft.type_assert_if_defined(optional_scaled_items, Array);
		
		var scaled_items = optional_scaled_items || [];
		if (ft.array_contains(scaled_items, item)) {
			return;
		} else {
			scaled_items.push(item);
		}
		
		ft.array_foreach(item.timeline.layers, function (layer) {
			ft.array_foreach(layer.frames, function (frame, frame_index) {
				ft.array_foreach(frame.elements, function (elem) {
					fttim.scale_elem_filters(elem, scale);
					if (fttim.is_group_shape_element(elem)) {
						ft.array_foreach(elem.members, function(member) {
							fttim.recursive_scale_filters(doc, member.libraryItem, scale, scaled_items);
						}, fttim.is_symbol_instance);
					}
					if (fttim.is_symbol_instance(elem)) {
						fttim.recursive_scale_filters(doc, elem.libraryItem, scale, scaled_items);
					}
				});
			}, fttim.is_keyframe);
		}, fttim.is_not_guide_layer);
	};

	fttim.replace_baked_symbols = function (doc, timeline, replaces) {
		ft.type_assert(doc, Document);
		ft.type_assert(timeline, Timeline);
		ft.array_foreach(timeline.layers, function (layer) {
			ft.array_foreach(layer.frames, function (frame, frame_index) {
				timeline.currentFrame = frame_index;
				doc.selectNone();
				ft.array_foreach(frame.elements, function (elem) {
					if (replaces.hasOwnProperty(elem.libraryItem.name)) {
						doc.selectNone();
						doc.selection = [elem];
						doc.swapElement(replaces[elem.libraryItem.name]);
					}
				}, fttim.is_symbol_instance);
			}, fttim.is_keyframe);
		}, fttim.is_not_guide_layer);
	};

	fttim.optimize_single_graphics = function (doc, timeline, opt_item) {
		ft.type_assert(doc, Document);
		ft.type_assert(timeline, Timeline);
		ft.array_foreach(timeline.layers, function (layer) {
			ft.array_foreach(layer.frames, function (frame, frame_index) {
				ft.array_foreach(frame.elements, function (elem) {
					var lib_item_name = elem.libraryItem.name;
					var lib_item_cache_name = "ft_cache_name_" + lib_item_name + "_" + elem.firstFrame;
					if (ftlib.bake_symbol_item(doc, doc.library, lib_item_name, lib_item_cache_name, elem.firstFrame)) {
						if (cfg.verbose_mode) {
							ft.trace_fmt("Optimize single graphic '{0}' for frame '{1}' in '{2}'",
								lib_item_name, elem.firstFrame, timeline.name);
						}
						if (opt_item === null || doc.library.editItem(opt_item.name)) {
							if (timeline.currentFrame != frame_index) {
								timeline.currentFrame = frame_index;
							}
							doc.selectNone();
							doc.selection = [elem];
							doc.swapElement(lib_item_cache_name);
							doc.selection[0].firstFrame = 0;
							doc.exitEditMode();
						}
					} else {
						if (cfg.verbose_mode) {
							ft.trace_fmt("NOT Optimize single graphic '{0}' for frame '{1}' in '{2}'",
								lib_item_name, elem.firstFrame, timeline.name);
						}
					}
				}, function (elem) {
					return fttim.is_symbol_graphic_single_frame_instance(elem) && !fttim.is_static(elem.libraryItem.timeline);
				});
			}, fttim.is_keyframe);
		}, fttim.is_not_guide_layer);
	};

	fttim.is_static = function (timeline) {
		ft.type_assert(timeline, Timeline);
		if (timeline.frameCount > 1) {
			return false;
		}
		return ft.array_foldl(timeline.layers, function (layer, acc) {
			return ft.array_foldl(layer.frames, function (frame, acc2) {
				return ft.array_foldl(frame.elements, function (elem, acc3) {
					return acc3 && fttim.is_symbol_instance(elem) ? elem.blendMode != "erase" && (fttim.is_symbol_graphic_single_frame_instance(elem) || fttim.is_static(elem.libraryItem.timeline)) : acc3;
				}, acc2);
			}, acc);
		}, true);
	};

	fttim.prepare_all_labels = function (doc, timeline) {
		ft.type_assert(doc, Document);
		ft.type_assert(timeline, Timeline);
		
		var anchor_prefix = "FT_ANCHOR:";
		ft.array_reverse_foreach(timeline.layers, function (layer, layer_index) {
			ft.array_foreach(layer.frames, function (frame, frame_index) {
				if ( fttim.is_anchor_frame(frame) && !frame.name.startsWith(anchor_prefix) ) {
					frame.name = anchor_prefix + frame.name;
				}
			}, fttim.is_keyframe);
		});
	};
	
	fttim.prepare_all_tweens = function (doc, timeline) {
		ft.type_assert(doc, Document);
		ft.type_assert(timeline, Timeline);
		
		var is_end_of_tween = function(frame_index, frames) {
			while (--frame_index >= 0) {
				var frame = frames[frame_index];
				if (fttim.is_keyframe(frame, frame_index)) {
					return fttim.is_motion_tween_frame(frame) && frame.duration > 1;					
				}
			}
			return false;
		};

		ft.array_reverse_foreach(timeline.layers, function (layer, layer_index) {
			ft.array_foreach(layer.frames, function (frame, frame_index) {
				if (fttim.is_shape_tween_frame(frame)) {
					if (ft.is_function(frame.convertToFrameByFrameAnimation)) {
						ft.trace_fmt(
							"[Warning] Timeline: '{0}' Layer: '{1}' Frame: {2}\n" +
							"- Shape tween strongly not recommended because it rasterized to frame-by-frame bitmap sequence.",
							timeline.name, layer.name, frame_index + 1);
						frame.convertToFrameByFrameAnimation();
					} else {
						throw "Animation uses shape tweens. To export this animation you should use Adobe Animate CC or higher!";
					}
				} else if (fttim.is_motion_tween_frame(frame) || is_end_of_tween(frame_index, layer.frames)) {
					var has_shapes = ft.array_any(frame.elements, fttim.is_shape_element);
					if (has_shapes || frame.elements.length > 1) {
						ft.trace_fmt(
							"[Warning] Timeline: '{0}' Layer: '{1}' Frame: {2}\n" +
							"- Frame contains incorrect objects for motion tween.",
							timeline.name, layer.name, frame_index + 1);
						timeline.currentFrame = frame_index;
						doc.selectNone();
						doc.selection = frame.elements;
						doc.convertToSymbol("graphic", ft.gen_unique_name(), "top left");
					}
				}
			}, fttim.is_keyframe);
		}, fttim.is_not_guide_layer);
	};

	fttim.prepare_all_groups = function (doc, timeline) {
		ft.type_assert(doc, Document);
		ft.type_assert(timeline, Timeline);
		
		var check_need_for_ungroup = function (frame, elem) {
			if (fttim.is_tween_frame(frame)) {
				return true;
			}
			if (fttim.is_group_shape_element(elem)) {
				return ft.array_any(elem.members, function (member) {
					if (fttim.is_instance_element(member)) {
						return true;
					}
					return check_need_for_ungroup(frame, member);
				});
			}
			return false;
		};

		var new_symbols = [];
		ft.array_reverse_foreach(timeline.layers, function (layer, layer_index) {
			ft.array_foreach(layer.frames, function (frame, frame_index) {
				var has_complex_shapes = ft.array_any(frame.elements, fttim.is_complex_shape_element);
				if (has_complex_shapes) {
					timeline.currentFrame = frame_index;
					doc.selectNone();
					
					var elements = ft.array_clone(frame.elements);
					ft.array_foreach(elements, function (elem, elem_index) {
						if (fttim.is_simple_shape_element(elem)) {
							// nothing
						} else if (fttim.is_complex_shape_element(elem) && check_need_for_ungroup(frame, elem)) {
							doc.selectNone();
							doc.selection = [elem];
							if (fttim.is_object_shape_element(elem)) {
								doc.breakApart();
								doc.group();
							}
							doc.unGroup();
							try {
								doc.unlockAllElements();
							} catch (e) {}
							var wrapper_item = doc.convertToSymbol("graphic", ft.gen_unique_name(), "top left");
							new_symbols.push(wrapper_item);
						} else {
							doc.selectNone();
							doc.selection = [elem];
							doc.arrange("front");
						}
					});
				}
			}, fttim.is_keyframe);
		}, fttim.is_not_guide_layer);
		return new_symbols;
	};

	fttim.rasterize_all_shapes = function (doc, timeline) {
		ft.type_assert(doc, Document);
		ft.type_assert(timeline, Timeline);

		var rasterize_count = 0;
		ft.array_reverse_foreach(timeline.layers, function (layer, layer_index) {
			ft.array_foreach(layer.frames, function (frame, frame_index) {
				var has_shapes = ft.array_any(frame.elements, fttim.is_shape_element);
				if (has_shapes) {
					timeline.currentFrame = frame_index;
					doc.selectNone();
					var groups_arr = ft.array_group_by(frame.elements, fttim.is_shape_element);
					for (var i = 0; i < groups_arr.length; ++i) {
						var groups = groups_arr[i];
						if (fttim.is_shape_element(groups.peek())) {
							doc.selectNone();
							doc.selection = groups;
							var location_name = "Timeline: {0}".format(timeline.name);
							ftdoc.convert_selection_to_bitmap(doc, location_name, timeline.libraryItem);
							++rasterize_count;
						} else {
							doc.selectNone();
							doc.selection = groups;
							doc.arrange("front");
						}
					}
				}
			}, fttim.is_keyframe);
		}, fttim.is_not_guide_layer);
		
		if (rasterize_count > 0 && cfg.verbose_mode) {
			ft.trace_fmt("Rasterize vector shapes({0}) in '{1}'", rasterize_count, timeline.name);
		}
	};

	//
	// run
	//

	(function () {
		ft.clear_output();
		fl.showIdleMessage(false);
		ft.trace("[Start]");

		if (cfg.open_documents.length > 0) {
			ft.profile_function(cfg.profile_mode, function () {
				ft.array_foreach(cfg.open_documents, function (uri) {
					fl.openDocument(uri);
				});
			}, "Open documents");
		}
		
		ft.profile_function(cfg.profile_mode, function() {
			ft.array_foreach(fl.documents, function (doc) {
				ft.profile_function(cfg.profile_mode, function() {
					try {
						ft.trace_fmt("[Document] '{0}' conversion started...", doc.name);
						ftdoc.prepare(doc);
						ft.trace_fmt("[Document] '{0}' conversion complete!", doc.name);
					} catch (e) {
						ft.trace_fmt("[Document] '{0}' conversion error: '{1}'", doc.name, e);
					}
				}, "Prepare document: '{0}'".format(doc.name));
			});
		}, "Prepare documents");

		if (cfg.revert_after_conversion) {
			ft.profile_function(cfg.profile_mode, function () {
				ft.array_foreach(fl.documents, function (doc) {
					if (doc.canRevert()) {
						fl.revertDocument(doc);
					}
				});
			}, "Revert documents");
		}

		if (cfg.close_after_conversion) {
			ft.profile_function(cfg.profile_mode, function () {
				ft.array_foreach(fl.documents, function (doc) {
					fl.closeDocument(doc, false);
				});
			}, "Close documents");
		}

		ft.trace("[Finish]");
	})();
};
