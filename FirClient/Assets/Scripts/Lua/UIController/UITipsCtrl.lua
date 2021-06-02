local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UITipsCtrl = class("UITipsCtrl", UIBaseCtrl)

local panelMgr = nil

function UITipsCtrl:Awake()
	logWarn("UITipsCtrl:Awake--->>")
	panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	panelMgr:CreatePanel(self, UILayer.Top, UiNames.Tips, self.OnCreateOK)
end

--启动事件--
function UITipsCtrl:OnCreateOK()
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

function UITipsCtrl:ShowTips(content)
	if content == nil then
		return
	end
	local prefab = self.prefab_item
	local gameObj = newObject(prefab).gameObject
	gameObj.transform:SetParent(self.gameObject.transform)
	gameObj.transform.localScale = Vector3.one
	gameObj.transform.localPosition = Vector3.zero
	gameObj:SetActive(true)
	gameObj:GetComponent("Animator").enabled = true
	gameObj.transform:Find("Text"):GetComponent("Text").text = content

	local luaAnim = addLuaAnimator(gameObj, self, self.AnimPlayEnd)
	if luaAnim ~= nil then
		luaAnim:Play("itemTipsPanel")
	end
end

function UITipsCtrl:AnimPlayEnd(gameObj)
	startTimer(1, 1, self, self.DelayDestroy, gameObj)
end

function UITipsCtrl:DelayDestroy(gameObj)
	if not isnil(gameObj) then
		destroy(gameObj)
	end
end

--关闭事件--
function UITipsCtrl:Close()
	panelMgr:ClosePanel(UiNames.Tips)
end

return UITipsCtrl