local ManagerCenter = class("ManagerCenter")

function ManagerCenter:Initialize()
	self.managers = {}
	--C# Manager--
	self:AddManager(ManagerNames.Shader, self:GetCSharpManager("ShaderManager"))
	self:AddManager(ManagerNames.Resource, self:GetCSharpManager("ResourceManager"))
	self:AddManager(ManagerNames.NPC, self:GetCSharpManager("NPCManager"))
	self:AddManager(ManagerNames.Socket, self:GetCSharpManager("NetworkManager"))

	--C# Ext Manager--
	self:AddManager(ManagerNames.Timer, self:GetExtManager("TimerManager"))
	self:AddManager(ManagerNames.Table, self:GetExtManager("TableManager"))
	self:AddManager(ManagerNames.Config, self:GetExtManager("ConfigManager"))

	--Lua Manager--
	self:AddManager(ManagerNames.Ctrl, require "Manager.CtrlManager", true)
	self:AddManager(ManagerNames.Adapter, require "Manager.AdapterManager", true)
	self:AddManager(ManagerNames.Map, require "Manager.MapManager", true)
	self:AddManager(ManagerNames.Level, require "Manager.LevelManager", true)
	self:AddManager(ManagerNames.Network, require "Manager.NetworkManager", true)
	self:AddManager(ManagerNames.Handler, require "Manager.HandlerManager", true)
	self:AddManager(ManagerNames.UI, require "Manager.UIManager", true)
	self:AddManager(ManagerNames.Panel, require "Manager.PanelManager", true)
	self:AddManager(ManagerNames.Component, require "Manager.ComponentManager", true)
	self:AddManager(ManagerNames.Module, require "Manager.ModuleManager", true)

	for _, value in pairs(self.managers) do
		if value.needInit then
			value.mgr:Initialize()
		end
	end
	logWarn('ManagerCenter:InitializeOK...')
end

function ManagerCenter:AddManager(name, manager, hasInit)
	if name == nil or manager == nil then
		logError('ManagerCenter:AddManager Error!! '..name..' was nil.')
		return
	end
	self.managers[name] = {
		mgr = manager,
		needInit = hasInit or false
	}
end

function ManagerCenter:GetManager(name)
	return self.managers[name].mgr
end

function ManagerCenter:RemoveManager(name)
	return table.removeKey(self.managers, name)
end

function ManagerCenter:GetCSharpManager(name)
	return ManagementCenter.GetManager(name)
end

function ManagerCenter:GetExtManager(name)
	return ManagementCenter.GetExtManager(name)
end

return ManagerCenter