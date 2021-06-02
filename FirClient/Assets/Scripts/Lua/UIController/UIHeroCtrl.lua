local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UIHeroCtrl = class("UIHeroCtrl", UIBaseCtrl)

function UIHeroCtrl:Awake()
	local moduleMgr = MgrCenter:GetManager(ManagerNames.Module)
	self.heroModule = moduleMgr:GetModule(ModuleNames.Hero)
	self.heroModule:Initialize()

	self.panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	self.panelMgr:CreatePanel(self, UILayer.Common, UiNames.Hero, self.OnCreateOK)
	logWarn("UIHeroCtrl.Awake--->>") 
end

--启动事件--
function UIHeroCtrl:OnCreateOK()
	self:SetUiLayout()		--设置UI布局--
	local scrollView = self.gameObject.transform:Find("ScrollViewRoot")
	if not isnil(scrollView) then
		local totalCount = self.heroModule:GetDataListSize()
		self.loopView = LuaUtil.GetComponent(scrollView.gameObject, ComponentNames.LoopListBox)
		self.loopView:InitListView(self, totalCount, self.OnItemUpdate)
	end
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

function UIHeroCtrl:OnItemUpdate(index)
	local item = self.loopView:NewListViewItem("ItemPrefab")
	self:SetItemData(index, item.gameObject)
	return item
end

function UIHeroCtrl:SetItemData(index, gameObj)
	index = index + 1
	local item = self.heroModule:GetDataByIndex(index)

	local prefabVar = LuaUtil.GetComponent(gameObj, ComponentNames.ItemPrefabVar)
	if prefabVar ~= nil then
		prefabVar:SetText("txt_Title", item.name)

		local itembox = prefabVar:TryGetComponent("itembox_icon")
		if itembox ~= nil then
			itembox:SetItem(item.itemid)
		end
	end
end


--单击事件--
function UIHeroCtrl:OnItemClick(go)
end

--关闭事件--
function UIHeroCtrl:Close()
	self.panelMgr:ClosePanel(UiNames.Skill)
end

function UIHeroCtrl:Show(isShow)
	self:ShowBottomUI(isShow)
end

return UIHeroCtrl