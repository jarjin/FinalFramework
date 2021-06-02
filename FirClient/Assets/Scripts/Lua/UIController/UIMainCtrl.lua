local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UIMainCtrl = class("UIMainCtrl", UIBaseCtrl)

local btnSelectImage = nil
local bottomUI = nil
local panelMgr = nil

function UIMainCtrl:InitBottomUI()
	local ctrlMgr = MgrCenter:GetManager(ManagerNames.Ctrl)
	bottomUI = {
		{ name = 'MainRole', button = 'Button1', ctrl = ctrlMgr:GetCtrl(UiNames.MainRole) },
		{ name = 'Bag', button = 'Button2', ctrl = ctrlMgr:GetCtrl(UiNames.Bag) },
		{ name = 'Battle', button = 'Button3', ctrl = ctrlMgr:GetCtrl(UiNames.Battle) },
		{ name = 'Hero', button = 'Button4', ctrl = ctrlMgr:GetCtrl(UiNames.Hero) },
		{ name = 'Dungeon', button = 'Button5', ctrl = ctrlMgr:GetCtrl(UiNames.Dungeon) },
	}
end

function UIMainCtrl:Awake()
	self.InitBottomUI()
	panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	panelMgr:CreatePanel(self, UILayer.Fixed, UiNames.Main, self.OnCreateOK)
	logWarn("UIMainCtrl.Awake--->>")
end

--启动事件--
function UIMainCtrl:OnCreateOK()
	local bottom = self.gameObject.transform:Find("Bottom")
	for i = 0, bottom.childCount - 1 do 
		local button = bottom:GetChild(i):GetComponent("Button")
		self.behaviour:AddClick(button, self, self.OnClick)
	end

	local rect = self.gameObject:GetComponent('RectTransform')
	if rect ~= nil then
		rect.offsetMin = Vector2.zero
		rect.offsetMax = Vector2.zero
	end
	Main.ShowUI(UiNames.Battle)
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

--单击事件--
function UIMainCtrl:OnClick(go)
	self:SelectImage(go)
	self:ShowSelectUI(go)
end

function UIMainCtrl:SelectImage(go)
	local image = go.transform:Find('Image'):GetComponent('Image')
	if image ~= nil then
		if btnSelectImage ~= nil then
			btnSelectImage.material = nil
		end
		image.material = GrayMat
		btnSelectImage = image
	end
end

function UIMainCtrl:ShowSelectUI(go)
	for i = 1, #bottomUI do
		local item = bottomUI[i]
		if item.button == go.name then
			item.ctrl:Show()
		else
			item.ctrl:Show(false)
		end
	end
end

--关闭事件--
function UIMainCtrl:Close()
	panelMgr:ClosePanel(UiNames.Main)
end

return UIMainCtrl