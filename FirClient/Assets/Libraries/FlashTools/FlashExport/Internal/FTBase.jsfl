if (!Array.prototype.peek) {
	Array.prototype.peek = function () {
		return this[this.length - 1];
	};
}

if (!String.prototype.format) {
	String.prototype.format = function () {
		var args = arguments;
		return this.replace(/{(\d+)}/g, function (match, number) {
			return typeof args[number] != 'undefined' ? args[number] : match;
		});
	};
}

if (!String.prototype.startsWith) {
	String.prototype.startsWith = function (searchString, position) {
      position = position || 0;
      return this.indexOf(searchString, position) === position;
    };
}

ft = {};

ft.trace = function () {
	fl.outputPanel.trace(
		Array.prototype.join.call(arguments, " "));
};

ft.trace_fmt = function (format) {
	var args = Array.prototype.slice.call(arguments, 1);
	ft.trace(format.format.apply(format, args));
};

ft.clear_output = function () {
	fl.outputPanel.clear();
};

ft.assert = function (expr, format) {
	if (!expr) {
		if (format === undefined) {
			throw "!!!Assert!!!";
		} else {
			var args = Array.prototype.slice.call(arguments, 2);
			throw "!!!Assert!!! " + format.format.apply(format, args);
		}
	}
};

ft.type_assert = function (item, type) {
	var type_is_string = (typeof type === 'string');
	ft.assert(
		(type_is_string && typeof item === type) ||
		(!type_is_string && item instanceof type),
		"Type error: {0} != {1}",
		typeof item,
		type_is_string ? type : type.constructor.name);
};

ft.type_assert_if_defined = function (item, type) {
	if (item && item !== undefined) {
		ft.type_assert(item, type);
	}
};

ft.is_function = function (func) {
	return func && typeof(func) === 'function';
};

ft.profile_function = function (verbose, func, msg) {
	ft.type_assert(verbose, 'boolean');
	ft.type_assert(func, Function);
	ft.type_assert(msg, 'string');
	if (!ft.profile_function_stack) {
		ft.profile_function_stack = [];
	}
	if (!ft.profile_function_level) {
		ft.profile_function_level = 0;
	}
	var stack_index = ft.profile_function_stack.length;
	ft.profile_function_stack.push({
		msg: msg,
		level: ft.profile_function_level,
		time: 0
	});
	++ft.profile_function_level;
	var func_time = ft.get_call_function_time(func);
	--ft.profile_function_level;
	ft.profile_function_stack[stack_index].time = func_time;
	if (stack_index === 0) {
		for (var i = 0; i < ft.profile_function_stack.length; ++i) {
			var info = ft.profile_function_stack[i];
			var ident = "-";
			for (var j = 0; j < info.level; ++j) {
				ident += "-";
			}
			if (verbose) {
				ft.trace_fmt("{0} [Profile] {1} ({2}s)", ident, info.msg, info.time);
			}
		}
		ft.profile_function_stack = [];
	}
};

ft.get_call_function_time = function (func) {
	ft.type_assert(func, Function);
	var b_time = Date.now();
	func();
	var e_time = Date.now();
	return (e_time - b_time) / 1000;
};

ft.escape_path = function (path) {
	ft.type_assert(path, 'string');
	return path.replace(/ /g, '%20');
};

ft.escape_string = function (str) {
	ft.type_assert(str, 'string');
	return str
		.replace(/\&/g, '&amp;')
		.replace(/\"/g, '&quot;')
		.replace(/\'/g, '&apos;')
		.replace(/</g, '&lt;')
		.replace(/>/g, '&gt;');
};

ft.combine_path = function (lhs, rhs) {
	ft.type_assert(lhs, 'string');
	ft.type_assert(rhs, 'string');
	return ft.escape_path(lhs) + ft.escape_path(rhs);
};

ft.array_any = function (arr, func) {
	ft.type_assert(arr, Array);
	ft.type_assert(func, Function);
	for (var index = 0; index < arr.length; ++index) {
		var value = arr[index];
		if (func(value)) {
			return true;
		}
	}
	return false;
};

ft.array_foldl = function (arr, func, acc) {
	ft.type_assert(arr, Array);
	ft.type_assert(func, Function);
	for (var index = 0; index < arr.length; ++index) {
		var value = arr[index];
		acc = func(value, acc);
	}
	return acc;
};

ft.array_foldr = function (arr, func, acc) {
	ft.type_assert(arr, Array);
	ft.type_assert(func, Function);
	for (var index = arr.length - 1; index >= 0; --index) {
		var value = arr[index];
		acc = func(value, acc);
	}
	return acc;
};

ft.array_clone = function (arr) {
	ft.type_assert(arr, Array);
	return arr.concat();
};

ft.array_filter = function (arr, filter) {
	ft.type_assert(arr, Array);
	ft.type_assert(filter, Function);
	var new_arr = [];
	for (var index = 0; index < arr.length; ++index) {
		var value = arr[index];
		if (filter(value, index)) {
			new_arr.push(value);
		}
	}
	return new_arr;
};

ft.array_merge = function (arrA, arrB) {
	ft.type_assert(arrA, Array);
	ft.type_assert(arrB, Array);
	return arrA.concat(ft.array_filter(arrB, function (value) {
		return !ft.array_contains(arrA, value);
	}));
};

ft.array_contains = function (arr, elem) {
	ft.type_assert(arr, Array);
	return arr.indexOf(elem) >= 0;
};

ft.array_foreach = function (arr, func, filter) {
	ft.type_assert(arr, Array);
	ft.type_assert(func, Function);
	ft.type_assert_if_defined(filter, Function);
	for (var index = 0; index < arr.length; ++index) {
		var value = arr[index];
		if (filter === undefined || filter(value, index)) {
			func(value, index);
		}
	}
};

ft.array_group_by = function (arr, func) {
	return ft.array_foldl(arr, function (value, acc) {
		if (acc.length > 0 && func(acc.peek().peek()) === func(value)) {
			acc.peek().push(value);
		} else {
			acc.push([value]);
		}
		return acc;
	}, []);
};

ft.array_reverse_foreach = function (arr, func, filter) {
	ft.type_assert(arr, Array);
	ft.type_assert(func, Function);
	ft.type_assert_if_defined(filter, Function);
	for (var index = arr.length - 1; index >= 0; --index) {
		var value = arr[index];
		if (filter === undefined || filter(value, index)) {
			func(value, index);
		}
	}
};

ft.approximately = function(a, b, precision) {
	ft.type_assert(a, 'number');
	ft.type_assert(b, 'number');
	ft.type_assert(precision, 'number');
	return Math.abs(b - a) < Math.abs(precision);
};

ft.gen_unique_name = function () {
	if (!ft.gen_unique_name_index) {
		ft.gen_unique_name_index = 0;
	}
	++ft.gen_unique_name_index;
	return "ft_unique_name_" + ft.gen_unique_name_index;
};