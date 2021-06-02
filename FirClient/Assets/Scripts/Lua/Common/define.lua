--package.path = [[./luaunit/?.lua;]]..package.path

_G.Main = {}
_G.LuaUtil = {}
_G.MgrCenter = nil

_G.Color32 	= require "Common/Color32"
_G.UiNames 	= require "Common/LuaUiNames"
_G.Event 	= 	require "3rd/event/events"
_G.EventType 	= 	require "Common/EventType"

_G.CtrlNames = {
	GMCmd = 'GMCmdCtrl',
	Preload = 'PreloadCtrl',
	RedDot = 'RedDotCtrl',
}

_G.ModuleNames = {
	Battle = "BattleModule",
	Dungeon = "DungeonModule",
	Hero = "HeroModule",
	MainRole = "MainRoleModule",
	User = "UserModule",
}

_G.ManagerNames = {
	Ctrl = "CtrlManager",
	Adapter = "AdapterManager",
	Map = "MapManager",
	Level = "LevelManager",
	Module = "ModuleManager",
	Network = "NetworkManager",
	Handler = "HandlerManager",
	
	Font = "FontManager",
	UI = "UIManager",
	Panel = "PanelManager",
	Component = "ComponentManager",

	Resource = "ResourceManager",
	NPC = "NPCManager",
	Timer = "TimerManager",
	Table = "TableManager",
	Config = "ConfigManager",
	Shader = "ShaderManager",
	Socket = "SocketManager",
	RedDot = "RedDotManager",
	Event = "EventManager",
}

_G.HandlerNames = {
	User = "UserMsgHandler"
}

_G.Path = System.IO.Path

_G.GameObject = UnityEngine.GameObject
_G.RectTransform = UnityEngine.RectTransform
_G.PlayerPrefs = UnityEngine.PlayerPrefs
_G.Camera = UnityEngine.Camera
_G.Material = UnityEngine.Material
_G.Sprite = UnityEngine.Sprite

_G.Image = UnityEngine.UI.Image
_G.Text = UnityEngine.UI.Text
_G.Button = UnityEngine.UI.Button

_G.UILayer = FirClient.Manager.UILayer
_G.Util = FirClient.Utility.Util
_G.LuaHelper = FirClient.Utility.LuaHelper
_G.LevelType = FirClient.Data.LevelType
_G.ResultCode = FirClient.Define.ResultCode
_G.EventNames = FirClient.Define.EventNames
_G.CLuaComponent = FirClient.Component.CLuaComponent

_G.NpcSex = { Man = 0, Woman = 1 }
_G.MapType = { Scene = 0, Battle = 1 }
_G.UILayer = 
{
	MapAbout = 1000,   --地图相关--
	Common = 2000,     --公用层--
	Fixed = 3000,      --固定层--
	Effect = 4000,     --特效层--
	Movie = 5000,      --电影层--
	Top = 6000,        --顶层--
}

_G.uiCanvas = nil
_G.GrayShaderName = "FirGame/Unlit/Transparent Colored Gray"

_G.ComponentNames = {
	Atlas = "CAtlas",
	BatchTask = "CBatchTask",
	ItemBox = "CItemBox",
	ItemTips = "CItemTips",
	LoopListBox = "CLoopListBox",
	ModelRender = "CModelRender",
	ItemPrefabVar = "CItemPrefabVar",
	RedDot = "RedDot",
}

_G.BatchTaskState = {
	Running = 0,
	Stoped = 1,
}

_G.ItemBoxState = {
	Normal = 0,
	Highlighted = 1,
	Disabled = 2
}

_G.MsgType = {
	Text = 0,
	Image = 1,
}