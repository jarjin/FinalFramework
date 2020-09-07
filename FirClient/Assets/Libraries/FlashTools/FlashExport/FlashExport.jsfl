var script_dir = fl.scriptURI.replace('FlashExport.jsfl', '');
fl.runScript(script_dir + 'Internal/FTBase.jsfl');
fl.runScript(script_dir + 'Internal/FTMain.jsfl', "ft_main", ft, {
	graphics_scale : 1.0
});