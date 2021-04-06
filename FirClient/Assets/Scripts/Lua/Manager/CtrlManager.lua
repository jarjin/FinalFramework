local CtrlManager = class("CtrlManager")

function CtrlManager:Initialize()
	self.controllers = {}
	--lua controller--
	self:AddCtrl(CtrlNames.GMCmd, require "Controller.GMCmdCtrl")
	self:AddCtrl(CtrlNames.Preload, require "Controller.PreloadCtrl")
	self:AddCtrl(CtrlNames.RedDot, require "Controller.RedDotController")
	
	--ui controller--
	self:AddCtrl(UiNames.Main, require "UIController.UIMainCtrl")
	self:AddCtrl(UiNames.Loader, require "UIController.UILoaderCtrl")
	self:AddCtrl(UiNames.Login, require "UIController.UILoginCtrl")
	self:AddCtrl(UiNames.MainRole, require "UIController.UIMainRoleCtrl")
	self:AddCtrl(UiNames.Battle, require "UIController.UIBattleCtrl")
	self:AddCtrl(UiNames.Hero, require "UIController.UIHeroCtrl")
	self:AddCtrl(UiNames.Dungeon, require "UIController.UIDungeonCtrl")
	self:AddCtrl(UiNames.Tips, require "UIController.UITipsCtrl")
	self:AddCtrl(UiNames.ItemTips, require "UIController.UIItemTipsCtrl")
	self:AddCtrl(UiNames.Bag, require "UIController.UIBagCtrl")
	self:AddCtrl(UiNames.ChooseActor, require "UIController.UIChooseActorCtrl")
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