local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UIItemTipsCtrl = class("UIItemTipsCtrl", UIBaseCtrl)

local isShow = false
local luaAnim = nil
local animatorList = nil

function UIItemTipsCtrl:Awake()
	animatorList = {}
	panelMgr:CreatePanel(self, UILayer.Top, UiNames.ItemTips, self.OnCreateOK)
	logWarn("UIItemTipsCtrl.Awake--->>")
end

--启动事件--
function UIItemTipsCtrl:OnCreateOK()
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

--关闭事件--
function UIItemTipsCtrl:Close()
	isShow = false
	animatorList = nil
	self.gameObject = nil

	panelMgr:ClosePanel(UiNames.ItemTips)
end

function UIItemTipsCtrl:ShowUI()
	if isShow == false then
		return
	end
	if #RoleDataModule.ItemtipsRewards <= 0 then
		return
	end
	local prefab = self.prefab_tipsItem
	local gameObj = newObject(prefab).gameObject
	gameObj.transform:SetParent(gameObject.transform)
	gameObj.transform.localScale = Vector3.one
	gameObj.transform.localPosition = Vector3.zero
	gameObj:SetActive(true)
	gameObj:GetComponent("Animator").enabled = true

	luaAnim = addLuaAnimator(gameObj, self, self.AnimPlayEnd)
	if luaAnim == nil then
		return
	end
	luaAnim:Play("itemTipsPanel")
	table.insert(animatorList, gameObj)

	local info = table.remove(RoleDataModule.ItemtipsRewards, #RoleDataModule.ItemtipsRewards)
	local itemTableData = TableManager.ItemTable.GetDataByKey(info.baseid)
	local itemBox = gameObj.transform:FindChild("obg_itembox"):GetComponent(ItemBox.GetClassType())        
	local tipsTxt = gameObj.transform:FindChild("txt_tips"):GetComponent('Text')
	if itemBox ~= nil then
		itemBox:SetItemID(info.baseid, info.num, 0) 
	end
	if itemTableData ~= nil then
		tipsTxt.text = itemTableData.name.."×"..info.num
		--EquipBuildAgent.RefreshQualityColor()
		--if itemTableData.quality <= #EquipBuildAgent.qualityColors and itemTableData.quality > 0 then
		--	tipsTxt.color = EquipBuildAgent.qualityColors[itemTableData.quality].color         
		--end
	end
end

function UIItemTipsCtrl:CheckNeedPlay()
    if isShow == false then
		return
    end
    if #RoleDataModule.ItemtipsRewards > 0 then
		self:ShowUI()
		local component = self.gameObject.transform:GetComponent(CItemTips.GetClassType())
		if not isnil(component) then
			component:OnEnable()
		end
	end   
end

function UIItemTipsCtrl:AnimPlayEnd(obj)
	if isShow == false then
		return
	end
	obj:SetActive(false)
	local itemBox = obj.transform:FindChild("obg_itembox"):GetComponent('ItemBox')
	if itemBox ~= nil then
		itemBox:Dispose()
		itemBox = nil
	end
	table.removeValue(animatorList, obj)
	obj:GetComponent("Animator").enabled = false
	if #animatorList <= 0 then
		if #RoleDataModule.ItemtipsRewards <= 0 then
			self:Close()
		else
			self:CheckNeedPlay()
		end
	end
end

function UIItemTipsCtrl:Close()
	self:Dispose()
end

return UIItemTipsCtrl