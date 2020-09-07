
--红点view
local CRedDot = class("CRedDot")

function CRedDot:ctor(compContainer)
	-- self._redDotObj 	= compContainer.gameObject 			--物体对象
	self._viewNode 		= false
	self._keyMap 		= {}
	self._rejectMap 	= {} 								--排斥的key，当排斥key的isActive为true时，就隐藏，而不管keyMap中的key是否显示
	self._redDotCount 	= 0 								--记录当前关联的且显示的红点有多少个 
	self._redDotNumNode = goutil.findChildTextComponent(compContainer.gameObject, "num")
end

--注意这里不要自己使用RED_DOT_VIEW_EVENT事件，而应该使用RED_DOT_UPDATE_EVENT事件去驱动它
function CRedDot:Awake( )
	GlobalDispatcher:addListener(EventType.RED_DOT_VIEW_EVENT, self.OnEvent, self)
	GlobalDispatcher:addListener(EventType.RED_DOT_RESET, self.OnReset, self)
end

function CRedDot:OnDestroy()
	GlobalDispatcher:removeListener(EventType.RED_DOT_VIEW_EVENT, self.OnEvent, self)
	GlobalDispatcher:removeListener(EventType.RED_DOT_RESET, self.OnReset, self)
end

function CRedDot:SetViewNode(viewNode)
	self._viewNode 	= viewNode
end

function CRedDot:OnReset()
	self._redDotCount = 0
	self:SetActive(false)
end

function CRedDot:OnEvent(redDotData)
	if redDotData then
		local isUpdate = false
		if redDotData.parentKey and self._keyMap[redDotData.parentKey] then
			--父类的节点有变化时，才会更新,否则计数器就不准确,parentKey的作用，见RedDotHelper中的说明
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
	goutil.setActive(self._viewNode, isActive)
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
	for k, v in pairs(self._rejectMap) do
		isRejectActive 	= RedDotHelper.instance:getDotIsActive(k)
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
-- parentKeyList用法建RedDotHelper
function CRedDot:UpdateRedDot(keyList, parentKeyList, rejectKeyList)
    if keyList or parentKeyList then self:UpdateKeyMap(keyList, parentKeyList, rejectKeyList) end
    for k, v in pairs(keyList or {}) do

    	if RedDotHelper.instance:getDotIsActive(v) then
    		self:DealRedDotCount(1)
    	end
    end
    local isFlag, num = false, 0
    for k, v in pairs(parentKeyList or {}) do
    	isFlag, num = RedDotHelper.instance:getDotIsActiveByParentKey(v, self._redDotNumNode)
    	if isFlag then
        	self:DealRedDotCount(num)
    	end
    end
    self:UpdateDotView()
end

return CRedDot