local PanelManager = class("PanelManager")

function PanelManager:Initialize()
	self.mPanels = {}
	self.mPrefabs = {}
	self.mCtrls = {}
	logWarn('PanelManager:InitializeOK...')
end

function PanelManager:GetPanelCtrl(uiCtrlName)
	return self.mCtrls[uiCtrlName]
end

function PanelManager:OnUiShow(uiCtrlName)
	local ctrl = self.mCtrls[uiCtrlName]
	if ctrl ~= nil and ctrl.OnShow ~= nil then
		ctrl:OnShow()
	end
end

function PanelManager:CreatePanel(ctrl, layer, abName, createOK)
	local panelName = abName.."Panel";
	local uiMgr = MgrCenter:GetManager(ManagerNames.UI)
	local parent = uiMgr:GetLayer(layer).transform;
	if parent:Find(panelName) ~= nil then
		self:OnUiShow(abName)
		return
	end
	local abPath = "Prefabs/UI/"..panelName;
	local resMgr = MgrCenter:GetManager(ManagerNames.Resource)
	resMgr:LoadAssetAsync(abPath, { panelName }, typeof(GameObject), function(objs) 
		if objs ~= nil and objs[0] ~= nil then
			self.mCtrls[abName] = ctrl
			self:CreatePanelInternal(ctrl, panelName, objs[0], parent, createOK)
		end
	end)
	logWarn("CreatePanel::>>"..abName)
end

function PanelManager:CreatePanelInternal(ctrl, panelName, prefab, parent, createOK)
	local gameObj = newObject(prefab)
	gameObj.name = panelName;
	gameObj.layer = LayerMask.NameToLayer("UI");
	gameObj.transform:SetParent(parent);
	gameObj.transform.localScale = Vector3.one;
	gameObj.transform.localPosition = Vector3.zero;

	local behaviour = gameObj:AddComponent(typeof(FirClient.Behaviour.LuaBehaviour))
	if createOK ~= nil then
		createOK(ctrl, behaviour)
	end
	self.mPanels[panelName] = gameObj
end

function PanelManager:DestroyPanel(name)
	local panelName = name.."Panel";
	local removeItem = table.removeKey(self.mPanels, panelName)
	if removeItem ~= nil then
		destroy(removeItem)
	end
end

function PanelManager:ClosePanel(name)
	self:DestroyPanel(name)
	logWarn('ClosePanel:>>'..name)
end
 
return PanelManager