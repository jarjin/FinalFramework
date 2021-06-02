local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UIDungeonCtrl = class("UIDungeonCtrl", UIBaseCtrl)

local loopView = nil
local dungeonModule = nil
local panelMgr = nil

function UIDungeonCtrl:Awake()
	local moduleMgr = MgrCenter:GetManager(ManagerNames.Module)
	dungeonModule = moduleMgr:GetModule(ModuleNames.Dungeon)
	dungeonModule:Initialize()

	panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	panelMgr:CreatePanel(self, UILayer.Common, UiNames.Dungeon, self.OnCreateOK)
	logWarn("UIDungeonCtrl.Awake--->>")
end

--启动事件--
function UIDungeonCtrl:OnCreateOK()
	self:SetUiLayout()		--设置UI布局--
	local scrollView = self.gameObject.transform:Find("ScrollViewRoot")
	if not isnil(scrollView) then
		local totalCount = dungeonModule:GetTotalItemAndChildCount()
		loopView = LuaUtil.GetComponent(scrollView.gameObject, ComponentNames.LoopListBox)
		loopView:InitListView(self, totalCount, self.OnItemUpdate)
	end
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

--单击事件--
function UIDungeonCtrl:OnItemUpdate(index)
	if index < 0 then
		return nil
	end
	local treeItem = dungeonModule:QueryTreeItemByTotalIndex(index)
	if treeItem == nil then
		return nil
	end
	--logError(index..' '..tostring(treeItem.mName)..' '..treeItem.mBeginIndex..' '..treeItem.mEndIndex)
	local item = nil
	local mTreeItemIndex = treeItem.mTreeItemIndex		 

	if not dungeonModule:IsChild(index, treeItem) then
		item = loopView:NewListViewItem("ItemPrefab1")
        item.UserIntData1 = mTreeItemIndex - 1		--c# array index begin from 0--
        item.UserIntData2 = 0
		self:SetItem1Data(index, item.gameObject, treeItem)
	else
		local childIndex = dungeonModule:GetChildIndex(index, treeItem)
		local childItem = dungeonModule:GetItemChildDataByIndex(mTreeItemIndex, childIndex)

		item = loopView:NewListViewItem("ItemPrefab2")
        item.UserIntData1 = mTreeItemIndex - 1		--c# array index begin from 0--
        item.UserIntData2 = childIndex - 1			--c# array index begin from 0--
		self:SetItem2Data(index, item.gameObject, childItem)
	end
	return item
end

function UIDungeonCtrl:SetItem1Data(index, gameObj, treeItem)
	local mExpandBtn = gameObj:GetComponent('Button')
	self.behaviour:AddClick(mExpandBtn, self, self.OnItemClick)

	local prefabVar = LuaUtil.GetComponent(gameObj, ComponentNames.ItemPrefabVar)
	if prefabVar ~= nil then
		local txt_TextName = prefabVar:TryGetComponent("txt_TextName")
		if not isnil(txt_TextName) then
			txt_TextName.text = treeItem.mName
		end
		prefabVar:SetValue("index", treeItem.mTreeItemIndex)
	end
end

function UIDungeonCtrl:SetItem2Data(index, gameObj, item)
	local prefabVar = LuaUtil.GetComponent(gameObj, ComponentNames.ItemPrefabVar)
	if prefabVar ~= nil then
		local txt_TextName = prefabVar:TryGetComponent("txt_TextName")
		if not isnil(txt_TextName) then
			txt_TextName.text = item.mName
		end
		local txt_TextDesc2 = prefabVar:TryGetComponent("txt_TextDesc2")
		if not isnil(txt_TextDesc2) then
			txt_TextDesc2.text = item.mDesc
		end
	end
end

function UIDungeonCtrl:OnItemClick(gameObj)
	local prefabVar = LuaUtil.GetComponent(gameObj, ComponentNames.ItemPrefabVar)
	local index = prefabVar:GetValue("index")

    dungeonModule:ToggleItemExpand(index)
	dungeonModule:UpdateAllTreeItemDataIndex()

	local allCount = dungeonModule:GetTotalItemAndChildCount()
	local listView = loopView:GetLoopListView()
	if not isnil(listView) then
		listView:SetListItemCount(allCount, false)
		listView:RefreshAllShownItem()
	end
end

--关闭事件--
function UIDungeonCtrl:Close()
	panelMgr:ClosePanel(UiNames.Dungeon)
end

function UIDungeonCtrl:Show(isShow)
	self:ShowBottomUI(isShow)
end

return UIDungeonCtrl