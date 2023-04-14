local BaseManager = require 'Manager.BaseManager'
local ModuleManager = class("ModuleManager", BaseManager)

function ModuleManager:Initialize()
    self.modules = {}
	self:AddModule(ModuleNames.Battle, (require "Module.BattleModule"):new())
	self:AddModule(ModuleNames.Dungeon, (require "Module.DungeonModule"):new())
	self:AddModule(ModuleNames.Hero, (require "Module.HeroModule"):new())
	self:AddModule(ModuleNames.MainRole, (require "Module.MainRoleModule"):new())
	self:AddModule(ModuleNames.User, (require "Module.UserModule"):new())

	for _, module in pairs(self.modules) do
		if module then
			module:Initialize()
		end
	end
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