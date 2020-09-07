local MainAdapter = class("MainAdapter")

local enterMapId = nil
local enterOK = nil
local mapMgr = nil

--进入场景--
function MainAdapter:EnterScene(mapid, action)
	enterMapId = mapid
	enterOK = action
	local ctrlMgr = MgrCenter:GetManager(ManagerNames.Ctrl)
	local loaderCtrl = ctrlMgr:GetCtrl(UiNames.Loader)
	if loaderCtrl ~= nil then
		loaderCtrl:InitLoader(function () self:InitBatchTask() end)
	end
end

--进入关卡--
function MainAdapter:OnEnterLevel(action)
	mapMgr = MgrCenter:GetManager(ManagerNames.Map)
	Main.ShowUI(UiNames.Main)
	if action ~= nil then
		execAction(action)
	end
	LuaHelper.InitBeginPlay(4000)	--初始化服务器数据-
end

--执行批次任务--
function MainAdapter:InitBatchTask()
    LuaUtil.InitBatchTask(self.OnProgressText, self.OnProgressValue, self.OnEnterLevelOK)
    LuaUtil.AddBatchTask("CreateMapObject", "正在创建地图场景", self.CreateMapObject)
    LuaUtil.AddBatchTask("LoadSceneMap", "正在加载场景地图", self.LoadSceneMap)
    LuaUtil.StartBatchTask()      --启动批次任务加载队列--
end

function MainAdapter:CreateMapObject(action)
	mapMgr:CreateMap(function ()
		log('Map Object Created!~')
		if action ~= nil then action(self) end
	end)
end

function MainAdapter:LoadSceneMap(action)
	mapMgr:LoadSceneMap(enterMapId, function ()
		log('LoadSceneMap:>'..enterMapId..' OK!!~')
		if action ~= nil then action(self) end
	end)
end

function MainAdapter:OnProgressText(text)
	LuaUtil.ShowLoaderProgressText(text)
	log("OnProgressText--->>"..text)
end

function MainAdapter:OnProgressValue(curr, total)
	LuaUtil.UpdateLoaderProgress(curr, total)
	log("OnProgressValue curr:"..curr.." total:"..total)
end

--进入场景关卡完成--
function MainAdapter:OnEnterLevelOK()
	local co = coroutine.start(function ()
		print('OnEnterLevelOK Coroutine start...')  

		coroutine.wait(0.1)
		Main.CloseUI(UiNames.Loader)
		if enterOK ~= nil then
			execAction(enterOK)
		end
		enterOK = nil
		enterMapId = nil

		Main.ShowTips("初始化完成啦~~~")
		print('OnEnterLevelOK Coroutine end...')
	end)
end

--离开关卡--
function MainAdapter:OnLeaveLevel(action)
	if action ~= nil then
		execAction(action)
	end
	mapMgr = nil
end

return MainAdapter