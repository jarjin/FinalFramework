var script_dir = fl.scriptURI.replace('FlashExport_HD.jsfl', '');
fl.runScript(script_dir + 'Internal/FTBase.jsfl');
fl.runScript(script_dir + 'Internal/FTMain.jsfl', "ft_main", ft, {
	graphics_scale : 2.0
});