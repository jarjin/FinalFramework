local BaseManager = require 'Manager.BaseManager'
local AdapterManager = class("AdapterManager", BaseManager)

function AdapterManager:Initialize()
	self.adapters = {}
	self:AddAdapter(LevelType.Main, (require "Adapter/MainAdapter"):new())
	self:AddAdapter(LevelType.Loader, (require "Adapter/LoaderAdapter"):new())
	self:AddAdapter(LevelType.Login, (require "Adapter/LoginAdapter"):new())
	self:AddAdapter(LevelType.Battle, (require "Adapter/BattleAdapter"):new())

	logWarn('AdapterManager:InitializeOK...')
end

function AdapterManager:AddAdapter(level, adapter)
	if level == nil or adapter == nil then
		logError('AdapterManager:AddAdapter Error!! was nil.')
		return
	end
	self.adapters[level] = adapter
end

function AdapterManager:GetAdapter(level)
	return self.adapters[level]
end

function AdapterManager:RemoveAdapter(level)
	return table.removeKey(self.adapters, level)
end
 
return AdapterManager