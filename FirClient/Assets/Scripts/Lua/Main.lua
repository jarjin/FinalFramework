require "Common/define"
require "Common/functions"
require "Common/LuaUtil"
local ManagerCenter = require "Common/ManagerCenter"

local levelMgr = nil
local ctrlMgr = nil
local adapterMgr = nil
local uiMgr = nil

function Main.Initialize(initOK)
    Main.InitUiCtrl()
	Main.InitManager()
	Main.InitHudRes()
	Main.InitPreloadUI()
	Main.InitPreloadAsset(initOK)
	Main.InitRedDotCtrl()
	
end

--初始化预加载UI--
function Main.InitPreloadUI()
	Main.ShowUI(UiNames.Tips)
end

--初始化管理器--
function Main.InitManager()
	MgrCenter = ManagerCenter:new()
	MgrCenter:Initialize()

	levelMgr = MgrCenter:GetManager(ManagerNames.Level)
	ctrlMgr = MgrCenter:GetManager(ManagerNames.Ctrl)
	adapterMgr = MgrCenter:GetManager(ManagerNames.Adapter)
	uiMgr = MgrCenter:GetManager(ManagerNames.UI)
end

function Main.OnInitOK()
	levelMgr:LoadLevel(LevelType.Login)

	local colorItem = getGlobalItemByKey("CommonWhite")
    logWarn('Main.OnInitOK--->>>'..colorItem.value)

	AddEvent('main.eventtest', function (...)
		logWarn(...)
	end)
	FireEvent('main.eventtest', 100, 'testEvent')
	RemoveEvent('main.eventtest')
end

--初始化UI视图--
function Main.InitUiCtrl()
    for k,v in pairs(UiNames) do
        local ctrlPath = "UIController/UI"..v..'Ctrl'
        require (ctrlPath)
        logWarn('Loading :>'..ctrlPath..' OK!')
	end
	_G.uiCanvas = GameObject.FindWithTag('UICanvas')
    logWarn('Main.InitUiView--->>>OK')
end

--显示UI--
function Main.ShowUI(uiCtrlName)
	local ctrl = ctrlMgr:GetCtrl(uiCtrlName)
	if ctrl ~= nil then
		ctrl:Awake()
	end
	logWarn('Main.ShowUI:>'..uiCtrlName)
end

--关闭UI--
function Main.CloseUI(uiCtrlName)
	local ctrl = ctrlMgr:GetCtrl(uiCtrlName)
	if ctrl ~= nil then
		ctrl:Close()
	end
	logWarn('Main.CloseUI:>'..uiCtrlName)
end

--显示TIPS--
function Main.ShowTips(text)
	local tipsCtrl = ctrlMgr:GetCtrl(UiNames.Tips)
	if tipsCtrl ~= nil then
		tipsCtrl:ShowTips(text)
	end
end

--显示道具提示--
function Main.ShowItemTips()
	local itemTipsCtrl = ctrlMgr:GetCtrl(UiNames.ItemTips)
	if itemTipsCtrl ~= nil then
		itemTipsCtrl:ShowUI()
	end
end

--显示战斗Loading--
function Main.ShowBattleLoadingUI(isshow)
	local battleCtrl = ctrlMgr:GetCtrl(UiNames.Battle)
	if battleCtrl ~= nil then
		battleCtrl:ShowBattleLoadingUI(isshow)
	end
end

--初始化预加载--
function Main.InitPreloadAsset(initOK)
	local preloadCtrl = ctrlMgr:GetCtrl(CtrlNames.Preload)
	if preloadCtrl ~= nil then
		preloadCtrl:Initialize(initOK)
	end
end

-- 红点ctrl
function Main.InitRedDotCtrl()
	local redDotCtrl = ctrlMgr:GetCtrl(CtrlNames.RedDot)
	if redDotCtrl ~= nil then
		redDotCtrl:Initialize()
	end
end

--进入场景--
function Main.EnterScene(mapid, execOK)
	local adapter = adapterMgr:GetAdapter(LevelType.Main)
	if adapter ~= nil then
		adapter:EnterScene(mapid, execOK)
	end
end

--进入副本--
function Main.EnterDungeon(chapterid, dungeonid, execOK)
	local adapter = adapterMgr:GetAdapter(LevelType.Battle)
	if adapter ~= nil then
		adapter:EnterDungeon(chapterid, dungeonid, execOK)
	end
end

--离开副本--
function Main.LeaveDungeon(execOK)
	local adapter = adapterMgr:GetAdapter(LevelType.Battle)
	if adapter ~= nil then
		adapter:LeaveDungeon(execOK)
	end
end

--移动相机--
function Main.MoveCamera(pos, time)
	local parent = find('/MainGame/MainCamera')
	if not isnil(parent) then
		parent.transform:DOMove(pos, time)
	end
end

function Main.AddHudObject(parent)
	if uiMgr ~= nil then
		return uiMgr:AddHudObject(parent)
	end
end

function Main.RemoveHudObject(name)
	if uiMgr ~= nil then
		uiMgr:RemoveHudObject(name)
	end
end

function Main.InitHudRes()
	if uiMgr ~= nil then
		uiMgr:InitHudRes()
	end
end

function Main.GetFloatingTextPrefab()
	if uiMgr ~= nil then
		return uiMgr:GetFloatingTextPrefab()
	end
end

function Main.AddComponent(typeName, gameObj)
	local componentMgr = MgrCenter:GetManager(ManagerNames.Component)
	if componentMgr ~= nil then
		return componentMgr:AddComponent(typeName, gameObj)
	end
end

function Main.CallTableFunc(typeName, uniqueid, funcName)
	local componentMgr = MgrCenter:GetManager(ManagerNames.Component)
	if componentMgr ~= nil then
		return componentMgr:Call(typeName, uniqueid, funcName)
	end
end

function Main.OnReceived(name, bytes)
	local netMgr = MgrCenter:GetManager(ManagerNames.Network)
	if netMgr ~= nil then
		netMgr:OnReceived(name, bytes)
	end
end

function Main.AddMsgItem(storyid, pageid, dlgid)
	local battleCtrl = ctrlMgr:GetCtrl(UiNames.Battle)
	if battleCtrl ~= nil then
		battleCtrl:AddMsgItem(storyid, pageid, dlgid)
	end
end

--销毁--
function Main.Dispose()
	logWarn('Main.Dispose--->>>')
end

return this