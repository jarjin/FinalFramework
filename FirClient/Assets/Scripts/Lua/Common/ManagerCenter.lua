local ctrlManager = require "Manager/CtrlManager"
local adapterManager = require "Manager/AdapterManager"
local mapManager = require "Manager/MapManager"
local levelManager = require "Manager/LevelManager"
local moduleManager = require "Manager/ModuleManager"
local uiManager = require "Manager/UIManager"
local panelManager = require "Manager/PanelManager"
local componentManager = require "Manager/ComponentManager"
local networkManager = require "Manager/NetworkManager"
local handlerManager = require "Manager/HandlerManager"

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
	self:AddManager(ManagerNames.Ctrl, ctrlManager, true)
	self:AddManager(ManagerNames.Adapter, adapterManager, true)
	self:AddManager(ManagerNames.Map, mapManager, true)
	self:AddManager(ManagerNames.Level, levelManager, true)
	self:AddManager(ManagerNames.Module, moduleManager, true)
	self:AddManager(ManagerNames.Network, networkManager, true)
	self:AddManager(ManagerNames.Handler, handlerManager, true)

	self:AddManager(ManagerNames.UI, uiManager, true)
	self:AddManager(ManagerNames.Panel, panelManager, true)
	self:AddManager(ManagerNames.Component, componentManager, true)

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
		manager:Initialize()
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