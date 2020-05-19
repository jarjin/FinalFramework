local LoginAdapter = require "Adapter/LoginAdapter"
local MainAdapter = require "Adapter/MainAdapter"
local BattleAdapter = require "Adapter/BattleAdapter"
local LoaderAdapter = require "Adapter/LoaderAdapter"

local AdapterManager = class("AdapterManager")

function AdapterManager:Initialize()
	self.adapters = {}
	self:AddAdapter(LevelType.Main, MainAdapter:new())
	self:AddAdapter(LevelType.Loader, LoaderAdapter:new())
	self:AddAdapter(LevelType.Login, LoginAdapter:new())
	self:AddAdapter(LevelType.Battle, BattleAdapter:new())

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