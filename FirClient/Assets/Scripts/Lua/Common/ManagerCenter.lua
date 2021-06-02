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
	self:AddManager(ManagerNames.Config, self:GetExtManager("ConfigManager"))

	--Lua Manager--
	self:AddManager(ManagerNames.Ctrl, require "Manager.CtrlManager", true)
	self:AddManager(ManagerNames.Adapter, require "Manager.AdapterManager", true)
	self:AddManager(ManagerNames.Map, require "Manager.MapManager", true)
	self:AddManager(ManagerNames.Level, require "Manager.LevelManager", true)
	self:AddManager(ManagerNames.Network, require "Manager.NetworkManager", true)
	self:AddManager(ManagerNames.Table, require "Data.TableManager", true)
	self:AddManager(ManagerNames.UI, require "Manager.UIManager", true)
	self:AddManager(ManagerNames.Panel, require "Manager.PanelManager", true)
	self:AddManager(ManagerNames.Component, require "Manager.ComponentManager", true)
	self:AddManager(ManagerNames.Module, require "Manager.ModuleManager", true)
	self:AddManager(ManagerNames.Handler, require "Manager.HandlerManager", true)
	self:AddManager(ManagerNames.RedDot, require "Manager.RedDotManager", true)
	self:AddManager(ManagerNames.Event, require "Manager.EventManager")

	logWarn('ManagerCenter:InitializeOK...')
end

function ManagerCenter:AddManager(name, manager, needInit)
	if name == nil or manager == nil then
		logError('ManagerCenter:AddManager Error!! '..name..' was nil.')
		return
	end
	self.managers[name] = manager

	needInit = needInit or nil
	if needInit == true then
		if name == ManagerNames.Table then
			manager.Initialize()
		else
			manager:Initialize()
		end
	end
end

function ManagerCenter:GetManager(name)
	return self.managers[name]
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