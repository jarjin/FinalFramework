local BattleModule = require "Module/BattleModule"
local DungeonModule = require "Module/DungeonModule"
local HeroModule = require "Module/HeroModule"
local MainRoleModule = require "Module/MainRoleModule"

local ModuleManager = class("ModuleManager")

function ModuleManager:Initialize()
    self.modules = {}
	self:AddModule(ModuleNames.Battle, BattleModule)
	self:AddModule(ModuleNames.Dungeon, DungeonModule)
	self:AddModule(ModuleNames.Hero, HeroModule)
	self:AddModule(ModuleNames.MainRole, MainRoleModule)
    logWarn('ModuleManager:InitializeOK...')
end

function ModuleManager:AddModule(name, Module)
	if name == nil or Module == nil then
		logError('ModuleManager:AddModule Error!! was nil.')
		return
	end
	self.modules[name] = Module
end

function ModuleManager:GetModule(name)
	return self.modules[name]
end

function ModuleManager:RemoveModule(name)
	table.removeKey(self.modules, name)
end

return ModuleManager