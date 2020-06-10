local RedDotController = class("RedDotController")

function RedDotController:Initialize()
    GlobalDispatcher:addListener(EventType.RED_DOT_UPDATE_EVENT, self._updateRedDot, self)
end


function RedDotController:onReset()
	RedDotHelper.instance:clear()
	GlobalDispatcher:dispatch(EventType.RED_DOT_RESET)
	-- GlobalDispatcher:removeListener(EventType.RED_DOT_UPDATE_EVENT, self._updateRedDot, self)
end


function RedDotController:UpdateRedDot(redDotData)
	if redDotData then
		local isActive 			= RedDotHelper.instance:getDotIsActive(redDotData.key)
		local isChange 			= redDotData.isActive ~= isActive
		--有变化才进行操作,父有变化，子必定是有变化
		if isChange then
			-- local isParentChange 	= RedDotHelper.instance:getDotIsActiveByParentKey(redDotData.parentKey)		--先取变化之前的值
			RedDotHelper.instance:setDotIsActivite(redDotData)
			-- redDotData.isParentChange = true--isParentChange ~= RedDotHelper.instance:getDotIsActiveByParentKey(redDotData.parentKey)--与变化之后的值对比，是否有改变
			self:notify(EventType.RED_DOT_VIEW_EVENT, redDotData)
		end
	end
end

return RedDotController