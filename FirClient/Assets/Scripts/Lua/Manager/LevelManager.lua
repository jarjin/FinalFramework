local BaseManager = require 'Manager.BaseManager'
local LevelManager = class("LevelManager", BaseManager)

function LevelManager:Initialize()
	logWarn("LevelManager:Initialize...")
end

function LevelManager:LoadLevel(levelType)
	local loaderCtrl = self.ctrlMgr:GetCtrl(UiNames.Loader)
	if loaderCtrl ~= nil then
		loaderCtrl:InitLoader(function ()
			LuaHelper.LoadLevel(levelType, self, self.OnLeaveLevel, self.OnEnterLevel)
		end)
	end
end

--进入新场景后，清理所有数据--
function LevelManager:OnLeaveLevel(levelType, action)
	local adapter = self.adapterMgr:GetAdapter(levelType)
	if adapter ~= nil then
		adapter:OnLeaveLevel(action)
	else
		if action ~= nil then
			execAction(action)
		end
	end
	logWarn("OnLeaveLevel--->>"..tostring(levelType))
end

--进入新场景后，初始化所有数据--
function LevelManager:OnEnterLevel(levelType, action)
	local adapter = self.adapterMgr:GetAdapter(levelType)
	if adapter ~= nil then
		adapter:OnEnterLevel(action)
	else
		if action ~= nil then
			execAction(action)
		end
	end
	logWarn("OnEnterLevel--->>"..tostring(levelType))
end

return LevelManager