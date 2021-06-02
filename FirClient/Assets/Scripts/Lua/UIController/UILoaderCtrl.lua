local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UILoaderCtrl = class("UILoaderCtrl", UIBaseCtrl)

local panelMgr = nil
local sliderLoadingBar = nil
local loaderCallback = nil

function UILoaderCtrl:Awake()
	panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	panelMgr:CreatePanel(self, UILayer.Top, UiNames.Loader, self.OnCreateOK)
	logWarn("UILoaderCtrl.Awake--->>")
end

--启动事件--
function UILoaderCtrl:OnCreateOK()
	local rect = self.gameObject:GetComponent('RectTransform')
	if rect ~= nil then
		rect.offsetMin = Vector2.zero
		rect.offsetMax = Vector2.zero
	end
	self:TryShowUi()
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

function UILoaderCtrl:InitLoader(func)
	loaderCallback = func
	if isnil(self.gameObject) then
		Main.ShowUI(UiNames.Loader)
	else
		self:TryShowUi()
	end
end

function UILoaderCtrl:TryShowUi()
	if loaderCallback ~= nil then
		loaderCallback(self)
		loaderCallback = nil
	end
end

function UILoaderCtrl:ShowProgressText(text)
	self.txt_content.text = text
end

function UILoaderCtrl:ShowProgressValue(curr, total)
	if sliderLoadingBar == nil then
		sliderLoadingBar = self.slider_loadingBar
		sliderLoadingBar.gameObject:SetActive(true)
	end
	sliderLoadingBar.value = curr / total
end

--单击事件--
function UILoaderCtrl:OnClick(go)
	this.Close()
end

--关闭事件--
function UILoaderCtrl:Close()
	panelMgr:ClosePanel(UiNames.Loader)
end

return UILoaderCtrl