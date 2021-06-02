local UIBaseCtrl = require "UIController/UIBaseCtrl"
local UIBattleCtrl = class("UIBattleCtrl", UIBaseCtrl)

local cavansGroupUI = nil
local loopView = nil
local battleModule = nil
local gmCmdCtrl = nil
local panelMgr = nil
local cacheMsg = nil

function UIBattleCtrl:Awake()
	local moduleMgr = MgrCenter:GetManager(ManagerNames.Module)
	battleModule = moduleMgr:GetModule(ModuleNames.Battle)
	battleModule:Initialize()

	local ctrlMgr = MgrCenter:GetManager(ManagerNames.Ctrl)
	gmCmdCtrl = ctrlMgr:GetCtrl(CtrlNames.GMCmd)

	local panelMgr = MgrCenter:GetManager(ManagerNames.Panel)
	panelMgr:CreatePanel(self, UILayer.Common, UiNames.Battle, self.OnCreateOK)
	logWarn("UIBattleCtrl.Awake--->>")
end

--启动事件--
function UIBattleCtrl:OnCreateOK()
	local loadingUI = self.gameObject.transform:Find("LoadingUI")
	if loadingUI ~= nil then
		cavansGroupUI = loadingUI:GetComponent('CanvasGroup')
	end

	self:SetUiLayout()		--设置UI布局--

	local scrollView = self.gameObject.transform:Find("ScrollViewRoot")
	if not isnil(scrollView) then
		local totalCount = battleModule:GetDataListSize()
		loopView = LuaUtil.GetComponent(scrollView.gameObject, ComponentNames.LoopListBox)
		loopView:InitListView(self, totalCount, self.OnItemUpdate)

		self:HandleCacheMsg()	--处理缓存消息--
	end
	self.behaviour:AddClick(self.btn_send, self, self.OnBtnSendClick)
	self.behaviour:AddEndEdit(self.input_msg, self, self.OnEndEdit)
	logWarn("OnCreateOK--->>"..self.gameObject.name)
end

function UIBattleCtrl:OnEndEdit(inputObj)
	self:OnBtnSendClick()
end

function UIBattleCtrl:OnBtnSendClick(btnObj)
	local text = self.input_msg.text
	if string.len(text) == 0 then
		return
	end
	if string.sub(text, 1, 3) == 'gm:' then
		if gmCmdCtrl ~= nil then
			gmCmdCtrl:Execute(text)
		end
	else
		logError('send message:'..text)
	end
end

function UIBattleCtrl:HandleCacheMsg()
	if cacheMsg ~= nil and #cacheMsg > 0 then
		for	i = 1, #cacheMsg do
			local storyid = cacheMsg[i].storyId
			local pageid = cacheMsg[i].pageId
			local dlgid = cacheMsg[i].dlgId
			self:AddMsgItem(storyid, pageid, dlgid)
		end
		cacheMsg = nil
	end
end

function UIBattleCtrl:AddMsgItem(storyid, pageid, dlgid)
	if loopView == nil then
		if cacheMsg == nil then
			cacheMsg = {}
		end
		table.insert(cacheMsg, 
		{
			storyId = storyid,
			pageId = pageid,
			dlgId = dlgid
		})
	else
		battleModule:AddMsgItem(storyid, pageid, dlgid)
		local count = battleModule:GetDataListSize()
		loopView:SetListItemCount(count)
		loopView:MoveToItemIndex(count - 1)
	end
end

--滚动项更新--
function UIBattleCtrl:OnItemUpdate(index)
	if battleModule == nil then
		return nil
	end
	local prefabName = index % 2 == 0 and "ItemPrefab1" or "ItemPrefab2"
	local item = loopView:NewListViewItem(prefabName)
	self:SetItemData(index, item.gameObject)
	return item
end

function UIBattleCtrl:SetItemData(index, gameObj)
	index = index + 1
	local prefabVar = LuaUtil.GetComponent(gameObj, ComponentNames.ItemPrefabVar)
	if prefabVar ~= nil then
		local itemData = battleModule:GetDataByIndex(index)
		local mMsgPic = prefabVar:TryGetComponent('img_PicMSg')
		local mMsgText = prefabVar:TryGetComponent('txt_StrMsg')

		prefabVar:SetText("txt_count", tostring(index))

		local mItemBg = prefabVar:TryGetComponent('img_Bg')
		local size = mItemBg:GetComponent('RectTransform').sizeDelta

		if itemData.mMsgType == MsgType.Text then
			mMsgPic.gameObject:SetActive(false)
			
			mMsgText.text = itemData.mSrtMsg
			mMsgText.gameObject:SetActive(true)
			mMsgText:GetComponent('ContentSizeFitter'):SetLayoutVertical()

			size.x = mMsgText:GetComponent('RectTransform').sizeDelta.x + 20
			size.y = mMsgText:GetComponent('RectTransform').sizeDelta.y + 20
		else
			mMsgPic:SetNativeSize()
			mMsgPic.gameObject:SetActive(true)
			size.x = mMsgPic:GetComponent('RectTransform').sizeDelta.x + 20
			size.y = mMsgPic:GetComponent('RectTransform').sizeDelta.y + 20

			mMsgText.gameObject:SetActive(false)
		end
		mItemBg:GetComponent('RectTransform').sizeDelta = size

		local mArrow = prefabVar:TryGetComponent('img_arrow')
		if true then
			mItemBg.color = Color32.new(160, 231, 90, 255)
		else
			mItemBg.color = Color.white
		end
		mArrow.color = mItemBg.color

		local tf = gameObj:GetComponent('RectTransform')
		local y = size.y
		if y < 75 then
			y = 75
		end
		tf:SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y)
	end
end

--显示战斗加载UI进度条--
function UIBattleCtrl:ShowBattleLoadingUI(isshow)
	cavansGroupUI.alpha = isshow and 1 or 0
end

--关闭事件--
function UIBattleCtrl:Close()
	if not isnil(loopView) then
		loopView:Dispose()
	end
	panelMgr:ClosePanel(UiNames.Battle)
end

function UIBattleCtrl:Show(isShow)
	self:ShowBottomUI(isShow)
end

return UIBattleCtrl