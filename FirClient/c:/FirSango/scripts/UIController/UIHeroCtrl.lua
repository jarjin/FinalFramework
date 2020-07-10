local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UIHeroCtrl = class("UIHeroCtrl", UIBaseCtrl)

local loopView = nil
local heroModule = nil
local panelMgr = nil

function UIHeroCtrl:Awake()
	local moduleMgr = MgrCenter:GetManager(ManagerNames.Module)
	heroModule = moduleMgr:GetModule(ModuleNames.Hero)
	heroModule.Initialize()

	panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	panelMgr:CreatePanel(self, UILayer.Common, UiNames.Hero, self.OnCreateOK)
	logWarn("UIHeroCtrl.Awake--->>") 
end

--启动事件--
function UIHeroCtrl:OnCreateOK(behaviour)
	self.gameObject = behaviour.gameObject
	self:InitBase()
	self:SetUiLayout()		--设置UI布局--

	local scrollView = self.gameObject.transform:Find("ScrollViewRoot")
	if not isnil(scrollView) then
		local totalCount = heroModule:GetDataListSize()
		loopView = LuaUtil.GetComponent(scrollView.gameObject, ComponentNames.LoopListBox)
		loopView:InitListView(self, totalCount, self.OnItemUpdate)
	end
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

function UIHeroCtrl:OnItemUpdate(index)
	local item = loopView:NewListViewItem("ItemPrefab")
	self:SetItemData(index, item.gameObject)
	return item
end

function UIHeroCtrl:SetItemData(index, gameObj)
	index = index + 1
	local item = heroModule:GetDataByIndex(index)

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
	self:Dispose()
	panelMgr:ClosePanel(UiNames.Skill)
end

function UIHeroCtrl:Show(isShow)
	self:ShowBottomUI(isShow)
end

return UIHeroCtrl