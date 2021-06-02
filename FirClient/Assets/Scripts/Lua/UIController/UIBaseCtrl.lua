local UIBaseCtrl = class("UIBaseCtrl")
UIBaseCtrl.gameObject = nil

function UIBaseCtrl:Awake()
end

function UIBaseCtrl:InitUI(behaviour)
	self.behaviour = behaviour
	self.gameObject = behaviour.gameObject
	self:InitUIBinder()
end

function UIBaseCtrl:ShowBottomUI(isShow)
	if isShow == nil or isShow == true then
		if isnil(self.gameObject) then
			self:Awake()
		else
			self.gameObject:SetActive(true)
		end
	else
		if not isnil(self.gameObject) then
			self.gameObject:SetActive(false)
		end
	end
end

function UIBaseCtrl:SetUiLayout()
	local rect = self.gameObject:GetComponent('RectTransform')
	if not isnil(rect) then
		rect.offsetMin  = Vector3.zero
		rect.offsetMax = Vector3.zero
	end
end

function UIBaseCtrl:ShowImage(img, sprite)
	if not isnil(img) then
		local c = img.color
		img.sprite = sprite
		img.color = Color.New(c.r, c.g, c.b, 1)
	end
end

function UIBaseCtrl:DisableImage(img)
	if not isnil(img) then
		local c = img.color
		img.sprite = sprite
		img.color = Color.New(c.r, c.g, c.b, 0)
	end
end

function UIBaseCtrl:InitUIBinder()
	if isnil(self.gameObject) then 
		return
	end
	self.mPrefabVars = {}
	local VarType = FirClient.Component.VarType
	local prefabVar = self.gameObject:GetComponent('CPrefabVar')
	if not isnil(prefabVar) then
		local varData = prefabVar:GetVarArray()
		local iter = varData:GetEnumerator()
		while iter:MoveNext() do
			local varObj = iter.Current
			if not isnil(varObj) then
				table.insert(self.mPrefabVars, varObj.name)
				--print(self.gameObject.name, varObj.name, varObj.type)
				if varObj.type == VarType.GameObject then
					self[varObj.name] = varObj.objValue
				elseif varObj.type == VarType.Transform then
					self[varObj.name] = varObj.tranValue
				elseif varObj.type == VarType.Image then
					self[varObj.name] = varObj.imgValue
				elseif varObj.type == VarType.Text then
					self[varObj.name] = varObj.txtValue
				elseif varObj.type == VarType.Button then
					self[varObj.name] = varObj.btnValue
				elseif varObj.type == VarType.TMP_InputField then
					self[varObj.name] = varObj.inputValue
				elseif varObj.type == VarType.Toggle then
					self[varObj.name] = varObj.toggleValue
				elseif varObj.type == VarType.Slider then
					self[varObj.name] = varObj.sliderValue
				elseif varObj.type == VarType.CMultiProgressBar then
					self[varObj.name] = varObj.multiProgreValue
				end
			end
		end
	end
end

function UIBaseCtrl:ClearUIBinder()
	if self.mPrefabVars ~= nil then
		for _, value in ipairs(self.mPrefabVars) do
			self[value] = nil
		end
		self.mPrefabVars = nil
	end
end

function UIBaseCtrl:Dispose()
	self:ClearUIBinder()
end

return UIBaseCtrl