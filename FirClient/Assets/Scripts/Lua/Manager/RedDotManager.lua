local RedDotManager = class("RedDotManager")

function RedDotManager:Initialize()
	self._cacheData 		= {} 	--缓存红点数据
end

function RedDotManager:GetRedDotInfo(key)
	local info 					= self._cacheData[key]
	if not info then
		info 					= {}
		info.isActive 			= false		--默认会为false
		self._cacheData[key] 	= info
	end
	return info
end

-- 设置红点是否激活了
function RedDotManager:SetDotIsActivite(valueData)
	if "table" == type(valueData) then
		local info 			= self:GetRedDotInfo(valueData.key)
		info.isActive 		= valueData.isActive
		info.parentKey 		= valueData.parentKey
	end
end

--红点是否激活了
function RedDotManager:GetDotIsActive(key)
	local info 			= self:GetRedDotInfo(key)
	return info.isActive
end

--通过父类key获取,是否有红点
function RedDotManager:GetDotIsActiveByParentKey(parentKey, isNum)
	local num 		= 0
	if parentKey then
		if isNum then
			for k, v in pairs(self._cacheData) do
				if v.isActive and parentKey == v.parentKey then
					num = num + 1
				end 
			end
		else
			for k, v in pairs(self._cacheData) do
				if v.isActive and parentKey == v.parentKey then
					num = 1
					break
				end 
			end
		end
	end
	return 0 ~= num, num
end

--[[
prefabPath  	红点的prefab路径，必须是已经加载好的，默认使用红点
dotNode 		挂在到哪个节点上
keyList 		该红点跟哪些key关联
rejectKeyList 	该红点与哪些红点互斥，互斥的红点有显示的话，那么该红点就不会显示 
parentKeyList 	这里的是为了处理类似背包，有很多的cell情况，这样依赖cell红点的其他元素，只需要添加parentKey就行,其他情况基本不需要这个值
--]]
function RedDotManager:CreateDotView(data)
	if data then
		local resPath 	= data.prefabPath or "" 
		local resMgr = MgrCenter:GetManager(ManagerNames.Resource)
		local componentMgr = MgrCenter:GetManager(ManagerNames.Component)
		resMgr:LoadAssetAsync(resPath, {"RedDot"}, typeof(GameObject), function(objs) 
			if objs ~= nil and objs[0] ~= nil then
				local redDotObj = componentMgr:AddComponent(ComponentNames.RedDot, objs[0])
				redDotObj:SetViewNode(data.dotNode)
				redDotObj:Awake()
				redDotObj:UpdateRedDot(data.keyList, data.parentKeyList, data.rejectKeyList)
			end
		end)
	end
	-- return redDotObj
end

-- 供外部修改key
function RedDotManager:UpdateRedDotKey(obj, keyList, parentKeyList, rejectKeyList)
	if obj then obj:UpdateRedDot(keyList, parentKeyList, rejectKeyList) end
end

function RedDotManager:Clear()
	self._cacheData 		= {}
end

return RedDotManager