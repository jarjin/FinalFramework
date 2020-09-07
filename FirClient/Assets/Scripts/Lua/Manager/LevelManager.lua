local LevelManager = class("LevelManager")

function LevelManager:Initialize()
	logWarn("LevelManager:Initialize...")
end

function LevelManager:LoadLevel(levelType)
	local ctrlMgr = MgrCenter:GetManager(ManagerNames.Ctrl)
	local loaderCtrl = ctrlMgr:GetCtrl(UiNames.Loader)
	if loaderCtrl ~= nil then
		loaderCtrl:InitLoader(function ()
			LuaHelper.LoadLevel(levelType, self, self.OnLeaveLevel, self.OnEnterLevel)
		end)
	end
end

--进入新场景后，清理所有数据--
function LevelManager:OnLeaveLevel(levelType, action)
	local adapterMgr = MgrCenter:GetManager(ManagerNames.Adapter)
	local adapter = adapterMgr:GetAdapter(levelType)
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
	local adapterMgr = MgrCenter:GetManager(ManagerNames.Adapter)
	local adapter = adapterMgr:GetAdapter(levelType)
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