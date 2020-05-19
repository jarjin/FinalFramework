local gmCmdCtrl = require "Controller/GMCmdCtrl"
local preloadCtrl = require "Controller/PreloadCtrl"

local uiMainCtrl = require "UIController/UIMainCtrl"
local uiLoaderCtrl = require "UIController/UILoaderCtrl"
local uiLoginCtrl = require "UIController/UILoginCtrl"
local uiMainRoleCtrl = require "UIController/UIMainRoleCtrl"
local uiBattleCtrl = require "UIController/UIBattleCtrl"
local uiHeroCtrl = require "UIController/UIHeroCtrl"
local uiDungeonCtrl = require "UIController/UIDungeonCtrl"
local uiTipsCtrl = require "UIController/UITipsCtrl"
local uiItemTipsCtrl = require "UIController/UIItemTipsCtrl"
local uiBaseMapCtrl = require "UIController/UIBaseMapCtrl"
local uiBagCtrl = require "UIController/UIBagCtrl"
local uiChooseActorCtrl = require "UIController/UIChooseActorCtrl"

local CtrlManager = class("CtrlManager")

function CtrlManager:Initialize()
	self.controllers = {}
	--lua controller--
	self:AddCtrl(CtrlNames.GMCmd, gmCmdCtrl)
	self:AddCtrl(CtrlNames.Preload, preloadCtrl)
	
	--ui controller--
	self:AddCtrl(UiNames.Main, uiMainCtrl)
	self:AddCtrl(UiNames.Loader, uiLoaderCtrl)
	self:AddCtrl(UiNames.Login, uiLoginCtrl)
	self:AddCtrl(UiNames.MainRole, uiMainRoleCtrl)
	self:AddCtrl(UiNames.Battle, uiBattleCtrl)
	self:AddCtrl(UiNames.Hero, uiHeroCtrl)
	self:AddCtrl(UiNames.Dungeon, uiDungeonCtrl)
	self:AddCtrl(UiNames.Tips, uiTipsCtrl)
	self:AddCtrl(UiNames.ItemTips, uiItemTipsCtrl)
	self:AddCtrl(UiNames.BaseMap, uiBaseMapCtrl)
	self:AddCtrl(UiNames.Bag, uiBagCtrl)
	self:AddCtrl(UiNames.ChooseActor, uiChooseActorCtrl)

	logWarn('CtrlManager:InitializeOK...')
end

function CtrlManager:AddCtrl(name, ctrl)
	if name == nil or ctrl == nil then
		logError('CtrlManager:AddCtrl Error!! was nil.')
		return
	end
	self.controllers[name] = ctrl
end

function CtrlManager:GetCtrl(name)
	return self.controllers[name]
end

function CtrlManager:RemoveCtrl(name)
	table.removeKey(self.controllers, name)
end

return CtrlManager