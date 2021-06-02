local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UIMainRoleCtrl = class("UIMainRoleCtrl", UIBaseCtrl)

local loopView = nil
local mainRoleModule = nil
local panelMgr = nil

function UIMainRoleCtrl:Awake()
	local moduleMgr = MgrCenter:GetManager(ManagerNames.Module)
	mainRoleModule = moduleMgr:GetModule(ModuleNames.MainRole)
	mainRoleModule:Initialize()

	panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	panelMgr:CreatePanel(self, UILayer.Common, UiNames.MainRole, self.OnCreateOK)
	logWarn("UIMainRoleCtrl.Awake--->>")
end

--启动事件--
function UIMainRoleCtrl:OnCreateOK()
	self:SetUiLayout()		--设置UI布局--
	local scrollView = self.gameObject.transform:Find("ScrollViewRoot")
	if not isnil(scrollView) then
		local count = mainRoleModule:GetDataListSize()
		loopView = LuaUtil.GetComponent(scrollView.gameObject, ComponentNames.LoopListBox)
		loopView:InitListView(self, count, self.OnItemUpdate)
	end
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

--滚动项更新--
function UIMainRoleCtrl:OnItemUpdate(index)
	local item = loopView:NewListViewItem("ItemPrefab")
	self:SetItemData(index, item.gameObject)
	return item
end

function UIMainRoleCtrl:SetItemData(index, gameObj)
	index = index + 1
	local rt = gameObj:GetComponent('RectTransform')
	local prefabVar = LuaUtil.GetComponent(gameObj, ComponentNames.ItemPrefabVar)

	local mExpandBtn = prefabVar:TryGetComponent('btn_ExpandButton')
	self.behaviour:AddClick(mExpandBtn, self, self.OnExpandClick)

	prefabVar:SetValue("index", index)

	local tableMgr = MgrCenter:GetManager(ManagerNames.Table)
	local itemData = tableMgr.itemTable:GetItemByKey(index)
	if itemData then
		prefabVar:SetText("txt_TextName", itemData.name)

		local itembox = prefabVar:TryGetComponent('itembox_icon')
		if itembox ~= nil then
			itembox:SetItem(itemData.id)
		end
	end
	local item = mainRoleModule:GetDataByIndex(index)
	self:OnExpandChanged(rt, prefabVar, item)
end

function UIMainRoleCtrl:OnItemClick(btnTrans)
	logError(btnTrans.name)
end

function UIMainRoleCtrl:OnExpandClick(btnObj)
	if isnil(btnObj) then
		return
	end
	local trans = btnObj.transform.parent
    local rt = trans:GetComponent('RectTransform')
	local prefabVar = LuaUtil.GetComponent(trans.gameObject, ComponentNames.ItemPrefabVar)

	local index = prefabVar:GetValue("index")
	local item = mainRoleModule:GetDataByIndex(index)

	item.mIsExpand = not item.mIsExpand
	self:OnExpandChanged(rt, prefabVar, item)

    local item2 = trans:GetComponent('LoopListViewItem2')
    item2.ParentListView:OnItemSizeChanged(item2.ItemIndex)
end

function UIMainRoleCtrl:OnExpandChanged(rt, prefabVar, item)
	local mClickTip = prefabVar:TryGetComponent('txt_ClickTip')
	local mExpandContentRoot = prefabVar:TryGetComponent('trans_ExpandContent')

    if item.mIsExpand then
        mExpandContentRoot.gameObject:SetActive(true)
        mClickTip.text = "Shrink"
        rt:SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 228)
    else
        mExpandContentRoot.gameObject:SetActive(false)
        mClickTip.text = "Expand"
        rt:SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 118)
    end
end

--关闭事件--
function UIMainRoleCtrl:Close()
	panelMgr:ClosePanel(UiNames.MainRole)
end

function UIMainRoleCtrl:Show(isShow)
	self:ShowBottomUI(isShow)
end

return UIMainRoleCtrl