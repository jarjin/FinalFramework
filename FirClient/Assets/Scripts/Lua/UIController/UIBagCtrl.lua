local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UIBagCtrl = class("UIBagCtrl", UIBaseCtrl)

local itemList = nil
local panelMgr = nil

function UIBagCtrl:Awake()
	panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	panelMgr:CreatePanel(self, UILayer.Common, UiNames.Bag, self.OnCreateOK)
	logWarn("UIBagCtrl.Awake--->>")
end

--启动事件--
function UIBagCtrl:OnCreateOK()
	self:SetUiLayout()		--设置UI布局--
	self:InitPanel()
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

function UIBagCtrl:InitPanel()
	itemList = {}
	local prefab = self.prefab_Item
	local parent = self.obj_BagItems
	for i = 1, 16 do
		local gameObj = newObject(prefab)
		gameObj.name = tostring(i)
		gameObj.transform:SetParent(parent.transform)
		gameObj.transform.localScale = Vector3.one
		gameObj:SetActive(true)

		self:OnItemUpdate(gameObj)
		table.insert(itemList, gameObj)
	end
end

function UIBagCtrl:OnItemUpdate(gameObj)
	local prefabVar = LuaUtil.GetComponent(gameObj, ComponentNames.ItemPrefabVar)
	if prefabVar ~= nil then
		local itembox = prefabVar:TryGetComponent("itembox_item")
		if itembox ~= nil then
			if gameObj.name == '1' then
				itembox:SetItem('2000', self, self.OnItemClick)
			end
		end
	end
end

--单击事件--
function UIBagCtrl:OnItemClick(go)
	logError(go)
end

--关闭事件--
function UIBagCtrl:Close()
	panelMgr:ClosePanel(UiNames.Bag)
end

function UIBagCtrl:Show(isShow)
	self:ShowBottomUI(isShow)
end

return UIBagCtrl