local UIManager = class("UIManager")

function UIManager:Initialize()
	self.mLayers = {}
	self.mHudObjs = {}
	self.hudPrefab = nil
	self.floatingTextPrefab = nil

	self:InitUILayers()
	logWarn('UIManager:InitializeOK...')
end

function UIManager:InitUILayers()
	self:CreateLayer('MapAbout', UILayer.MapAbout)
	self:CreateLayer('Fixed', UILayer.Fixed)
	self:CreateLayer('Common', UILayer.Common)
	self:CreateLayer('Movie', UILayer.Movie)
	self:CreateLayer('Effect', UILayer.Effect)
	self:CreateLayer('Top', UILayer.Top)
end

function UIManager:CreateLayer(name, layerType)
	local layerName = name..'_Layer'
	local layerObj = GameObject.New(layerName)
	layerObj.layer = LayerMask.NameToLayer("UI")
	layerObj.transform:SetParent(uiCanvas.transform)
	layerObj.transform.localScale = Vector3.one

	local rectType = typeof(RectTransform)
	local rect = layerObj:AddComponent(rectType)
	rect.anchorMin = Vector2.zero
	rect.anchorMax = Vector2.one
	rect.sizeDelta = Vector2.zero
	rect.anchoredPosition3D = Vector3.zero
	rect:SetSiblingIndex(layerType)

	self.mLayers[layerType] = layerObj
end

function UIManager:GetLayer(layerType)
	return self.mLayers[layerType]
end

function UIManager:InitHUD()
	local type = typeof(GameObject)
	local resMgr = MgrCenter:GetManager(ManagerNames.Resource)
	resMgr:LoadAssetAsync("Prefabs/HUD/HudPrefab", {"HudPrefab"}, type, function(objs) 
		self.hudPrefab = objs[0]
	end)
	resMgr:LoadAssetAsync("Prefabs/HUD/FloatingText", {"FloatingText"}, type, function(objs) 
		self.floatingTextPrefab = objs[0]
	end)
end

function UIManager:AddHudObject(parent)
	if isnil(self.hudPrefab) then
		logError('self.hudPrefab was nil..')
		return nil
	end
	local hudObj = newObject(self.hudPrefab)
	hudObj.name = "hudObject"
	hudObj.transform:SetParent(parent.transform)
	hudObj.transform.localScale = Vector3.New(0.01, 0.01, 1)
	hudObj.transform.localPosition = Vector3.New(0, 1.1, 0)

	self.mHudObjs[parent.name] = hudObj
	return hudObj
end

function UIManager:RemoveHudObject(name)
	local hudObj = table.removeKey(self.mHudObjs, name)
	if not isnil(hudObj) then
		destroy(hudObj.gameObject)
	end
end

function UIManager:InitHudRes()
	self:InitHUD()
	self:InitGrayMatial()
end

function UIManager:InitGrayMatial()
	local grayMat = _G.GrayMat
	if grayMat == nil then
		local shaderMgr = MgrCenter:GetManager(ManagerNames.Shader)
		local grayShader = shaderMgr:GetShader(_G.GrayShaderName);
		if grayShader ~= nil then
			_G.GrayMat = Material.New(grayShader)
		end
	end
end

function UIManager:GetFloatingTextPrefab()
	return self.floatingTextPrefab
end

return UIManager