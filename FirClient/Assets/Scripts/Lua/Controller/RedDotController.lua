local RedDotController = class("RedDotController")

function RedDotController:Initialize()
	Event.AddListener(EventType.RED_DOT_UPDATE_EVENT, handler(self.UpdateRedDot, self))
end


function RedDotController:Exit()
	MgrCenter:GetManager(ManagerNames.RedDot):Clear()
	Event.Brocast(EventType.RED_DOT_CLEAR_EVENT)
	Event.RemoveListener(EventType.RED_DOT_UPDATE_EVENT, handler(self.UpdateRedDot, self))
end


function RedDotController:UpdateRedDot(redDotData)
	if redDotData then
		local redDotMgr 		=  MgrCenter:GetManager(ManagerNames.RedDot)
		local isActive 			= redDotMgr:GetDotIsActive(redDotData.key)
		local isChange 			= redDotData.isActive ~= isActive
		--有变化才进行操作,父有变化，子必定是有变化
		if isChange then
			redDotMgr:SetDotIsActivite(redDotData)
			Event.Brocast(EventType.RED_DOT_VIEW_EVENT, redDotData)
		end
	end
end

return RedDotController