local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UIChooseActorCtrl = class("UIChooseActorCtrl", UIBaseCtrl)

local roleSex = nil
local vocation = nil
local roleSprites = nil
local panelMgr = nil

function UIChooseActorCtrl:Awake()
	panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	panelMgr:CreatePanel(self, UILayer.Common, UiNames.ChooseActor, self.OnCreateOK)
	logWarn("UIChooseActorCtrl.Awake--->>")
end

--启动事件--
function UIChooseActorCtrl:OnCreateOK(behaviour)
	self.gameObject = behaviour.gameObject
	self:InitBase()
	behaviour:AddClick(self.btn_Create, self, self.OnCreateClick)
	behaviour:AddToggleClick(self.toggle_master, self, self.OnMasterClick)
	behaviour:AddToggleClick(self.toggle_ghost, self, self.OnGhostClick)
	behaviour:AddToggleClick(self.toggle_warrior, self, self.OnWarriorClick)

	behaviour:AddToggleClick(self.toggle_man, self, self.OnSexMan)
	behaviour:AddToggleClick(self.toggle_women, self, self.OnSexWoman)

	self:ShowSelectedRole('warrior')
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

--单击事件--
function UIChooseActorCtrl:OnCreateClick(gameObj)
	PlayerPrefs.SetInt("rolesex", roleSex)
	PlayerPrefs.SetInt("roleid", vocationToRoleid(vocation))

	local ctrlMgr = MgrCenter:GetManager(ManagerNames.Ctrl)
	local loginCtrl = ctrlMgr:GetCtrl(UiNames.Login)
	if loginCtrl ~= nil then
		loginCtrl:OnShow()
	end
	self:Close()
end

function UIChooseActorCtrl:OnSexMan(gameObj)
	roleSex = NpcSex.Man
	self:ShowSelectedRole(vocation)
end

function UIChooseActorCtrl:OnSexWoman(gameObj)
	roleSex = NpcSex.Woman
	self:ShowSelectedRole(vocation)
end

function UIChooseActorCtrl:OnWarriorClick(gameObj)
	self:ShowSelectedRole('warrior')
end

function UIChooseActorCtrl:OnMasterClick(gameObj)
	self:ShowSelectedRole('master')
end

function UIChooseActorCtrl:OnGhostClick(gameObj)
	self:ShowSelectedRole('ghost')
end

--显示选择的角色--
function UIChooseActorCtrl:ShowSelectedRole(roleName)
	vocation = roleName
	if roleSex == nil then
		roleSex = NpcSex.Woman
	end
	local spriteName = roleName..'_'..(roleSex == NpcSex.Man and "Man" or "Woman")
	self:LoadRoleAsset(spriteName, function (sprite)
		self:ShowImage(self.img_ShowActor, sprite)
	end)
end

function UIChooseActorCtrl:LoadRoleAsset(spriteName, func)
	if roleSprites == nil then
		roleSprites = {}
	end
	local sprite = roleSprites[spriteName]
	if sprite ~= nil then
		func(sprite)
		return
	end
	local path = "Textures/MainRole/"..spriteName
	local resMgr = MgrCenter:GetManager(ManagerNames.Resource)
	resMgr:LoadAssetAsync(path, { spriteName }, typeof(Sprite), function (objs)
		if objs ~= nil and objs[0] ~= nil then
			local spriteObj = objs[0]
			roleSprites[spriteObj.name] = spriteObj
			func(spriteObj)
		end
	end)
end

--关闭事件--
function UIChooseActorCtrl:Close()
	self:Dispose()
	panelMgr:ClosePanel(UiNames.ChooseActor)
	roleSex = nil
	roleSprites = nil
	vocation = nil
end

return UIChooseActorCtrl