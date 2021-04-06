--红点view
local CRedDot = class("CRedDot")

function CRedDot:ctor(compContainer)
	-- self._redDotObj 	= compContainer.gameObject 			--物体对象
	self._viewNode 		= false
	self._keyMap 		= {}
	self._rejectMap 	= {} 								--排斥的key，当排斥key的isActive为true时，就隐藏，而不管keyMap中的key是否显示
	self._redDotCount 	= 0 								--记录当前关联的且显示的红点有多少个 
	self._redDotNumNode = false								--数字节点
	
end

--注意这里不要自己使用RED_DOT_VIEW_EVENT事件，而应该使用RED_DOT_UPDATE_EVENT事件去驱动它
function CRedDot:Awake( )
	Event.AddListener(EventType.RED_DOT_VIEW_EVENT, handler(self.OnEvent, self))
	Event.AddListener(EventType.RED_DOT_CLEAR_EVENT, handler(self.OnClear, self))
end

function CRedDot:OnDestroy()
	Event.RemoveListener(EventType.RED_DOT_VIEW_EVENT, handler(self.OnEvent, self))
	Event.RemoveListener(EventType.RED_DOT_CLEAR_EVENT, handler(self.OnClear, self))
end

function CRedDot:SetViewNode(viewNode)
	self._viewNode 	= viewNode
end

function CRedDot:OnClear()
	self._redDotCount = 0
	self:SetActive(false)
end

function CRedDot:OnEvent(redDotData)
	if redDotData then
		local isUpdate = false
		if redDotData.parentKey and self._keyMap[redDotData.parentKey] then
			--父类的节点有变化时，才会更新,否则计数器就不准确,parentKey的作用，见redDotMgr中的说明
			isUpdate = true
		else
			isUpdate = self._keyMap[redDotData.key]
		end
		if isUpdate then
			self:DealRedDotCount(self:DealRedOffsetValue(redDotData.isActive))
			self:UpdateDotView()
		elseif self._rejectMap[redDotData.key] then
			self:UpdateDotView()
		end
	end
end

function CRedDot:SetActive(isActive)
	self._viewNode:SetActive(isActive)
	if isActive and self._redDotNumNode then
		self._redDotNumNode.text = self._redDotCount
	end
end

function CRedDot:DealRedOffsetValue(isActive)
    return isActive and 1 or -1 
end
function CRedDot:DealRedDotCount(offsetValue)
    self._redDotCount   = self._redDotCount + offsetValue
    self._redDotCount   = 0 > self._redDotCount and 0 or self._redDotCount
end

function CRedDot:UpdateDotView()
	local isRejectActive= false
	local redDotMgr 		=  MgrCenter:GetManager(ManagerNames.RedDot)
	for k, v in pairs(self._rejectMap) do
		isRejectActive 	= redDotMgr:GetDotIsActive(k)
        if isRejectActive then
        	break
        end
    end
	self:SetActive(0 < self._redDotCount and (not isRejectActive))
end

function CRedDot:KeyMapInit(keyMap, keyList)
	for k, v in pairs(keyList or {}) do
	   keyMap[v] = true
 	end
end

function CRedDot:UpdateKeyMap(keyList, parentKeyList, rejectKeyList)	
   	self._keyMap = {}
 	self._rejectMap = {}
   	self._redDotCount 	= 0 	--更新keylist时，需要置0
   	self:KeyMapInit(self._keyMap, keyList)
   	self:KeyMapInit(self._keyMap, parentKeyList)
   	self:KeyMapInit(self._rejectMap, rejectKeyList)
end

-- rejectKeyList排斥的key
-- parentKeyList用法建redDotMgr
function CRedDot:UpdateRedDot(keyList, parentKeyList, rejectKeyList)
	if keyList or parentKeyList then self:UpdateKeyMap(keyList, parentKeyList, rejectKeyList) end
	local redDotMgr 		=  MgrCenter:GetManager(ManagerNames.RedDot)
    for k, v in pairs(keyList or {}) do

    	if redDotMgr:GetDotIsActive(v) then
    		self:DealRedDotCount(1)
    	end
    end
    local isFlag, num = false, 0
    for k, v in pairs(parentKeyList or {}) do
    	isFlag, num = redDotMgr:GetDotIsActiveByParentKey(v, self._redDotNumNode)
    	if isFlag then
        	self:DealRedDotCount(num)
    	end
    end
    self:UpdateDotView()
end

return CRedDot