local Behaviour = class("Behaviour")

function Behaviour:__index(t)
	---------manager----------
	if t == 'mapMgr' then
		self.mapMgr = MgrCenter:GetManager(ManagerNames.Map)
		return self.mapMgr
	end
	if t == 'tableMgr' then
		self.tableMgr = MgrCenter:GetManager(ManagerNames.Table)
		return self.tableMgr
	end
	if t == 'configMgr' then
		self.configMgr = MgrCenter:GetManager(ManagerNames.Config)
		return self.configMgr
	end
	if t == 'moduleMgr' then
		self.moduleMgr = MgrCenter:GetManager(ManagerNames.Module)
		return self.moduleMgr
	end
	if t == 'ctrlMgr' then
		self.ctrlMgr = MgrCenter:GetManager(ManagerNames.Ctrl)
		return self.ctrlMgr
	end
	if t == 'netMgr' then
		self.netMgr = MgrCenter:GetManager(ManagerNames.Network)
		return self.netMgr
	end
	if t == 'componentMgr' then
		self.componentMgr = MgrCenter:GetManager(ManagerNames.Component)
		return self.componentMgr
	end
	if t == 'resMgr' then
		self.resMgr = MgrCenter:GetManager(ManagerNames.Resource)
		return self.resMgr
	end
	if t == 'uiMgr' then
		self.uiMgr = MgrCenter:GetManager(ManagerNames.UI)
		return self.uiMgr
	end
	if t == 'shaderMgr' then
		self.shaderMgr = MgrCenter:GetManager(ManagerNames.Shader)
		return self.shaderMgr
	end
	if t == 'eventMgr' then
		self.eventMgr = MgrCenter:GetManager(ManagerNames.Event)
		return self.eventMgr
	end
	if t == 'panelMgr' then
		self.panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
		return self.panelMgr
	end
	if t == 'levelMgr' then
		self.levelMgr = MgrCenter:GetManager(ManagerNames.Level)
		return self.levelMgr
	end
	if t == 'adapterMgr' then
		self.adapterMgr = MgrCenter:GetManager(ManagerNames.Adapter)
		return self.adapterMgr
	end
	if t == 'handlerMgr' then
		self.handlerMgr = MgrCenter:GetManager(ManagerNames.Handler)
		return self.handlerMgr
	end
	if t == 'redDotMgr' then
		self.redDotMgr = MgrCenter:GetManager(ManagerNames.RedDot)
		return self.redDotMgr
	end

	---------module----------
	if t == 'battleModule' then
		self.battleModule = self.moduleMgr:GetModule(ModuleNames.Battle)
		return self.battleModule
	end
	if t == 'dungeonModule' then
		self.dungeonModule = self.moduleMgr:GetModule(ModuleNames.Dungeon)
		return self.dungeonModule
	end
	if t == 'userModule' then
		self.userModule = self.moduleMgr:GetModule(ModuleNames.User)
		return self.userModule
	end
	if t == 'heroModule' then
		self.heroModule = self.moduleMgr:GetModule(ModuleNames.Hero)
		return self.heroModule
	end
	if t == 'mainRoleModule' then
		self.mainRoleModule = self.moduleMgr:GetModule(ModuleNames.MainRole)
		return self.mainRoleModule
	end
end

return Behaviour