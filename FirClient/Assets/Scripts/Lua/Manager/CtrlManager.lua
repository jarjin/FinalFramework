local BaseManager = require 'Manager.BaseManager'
local CtrlManager = class("CtrlManager", BaseManager)

function CtrlManager:Initialize()
	self.controllers = {}
	--lua controller--
	self:AddCtrl(CtrlNames.GMCmd, (require "Controller.GMCmdCtrl"):new())
	self:AddCtrl(CtrlNames.Preload, (require "Controller.PreloadCtrl"):new())
	self:AddCtrl(CtrlNames.RedDot, (require "Controller.RedDotController"):new())
	
	--ui controller--
	self:AddCtrl(UiNames.Main, (require "UIController.UIMainCtrl"):new())
	self:AddCtrl(UiNames.Loader, (require "UIController.UILoaderCtrl"):new())
	self:AddCtrl(UiNames.Login, (require "UIController.UILoginCtrl"):new())
	self:AddCtrl(UiNames.MainRole, (require "UIController.UIMainRoleCtrl"):new())
	self:AddCtrl(UiNames.Battle, (require "UIController.UIBattleCtrl"):new())
	self:AddCtrl(UiNames.Hero, (require "UIController.UIHeroCtrl"):new())
	self:AddCtrl(UiNames.Dungeon, (require "UIController.UIDungeonCtrl"):new())
	self:AddCtrl(UiNames.Tips, (require "UIController.UITipsCtrl"):new())
	self:AddCtrl(UiNames.ItemTips, (require "UIController.UIItemTipsCtrl"):new())
	self:AddCtrl(UiNames.Bag, (require "UIController.UIBagCtrl"):new())
	self:AddCtrl(UiNames.ChooseActor, (require "UIController.UIChooseActorCtrl"):new())
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