local BaseCtrl = require 'Controller.BaseCtrl'
local RedDotController = class("RedDotController", BaseCtrl)

function RedDotController:Initialize()
	Event.AddListener(EventType.RED_DOT_UPDATE_EVENT, handler(self.UpdateRedDot, self))
end

function RedDotController:Exit()
	self.redDotMgr:Clear()
	Event.Brocast(EventType.RED_DOT_CLEAR_EVENT)
	Event.RemoveListener(EventType.RED_DOT_UPDATE_EVENT, handler(self.UpdateRedDot, self))
end

function RedDotController:UpdateRedDot(redDotData)
	if redDotData then
		local isActive = self.redDotMgr:GetDotIsActive(redDotData.key)
		local isChange = redDotData.isActive ~= isActive
		--有变化才进行操作,父有变化，子必定是有变化
		if isChange then
			self.redDotMgr:SetDotIsActivite(redDotData)
			Event.Brocast(EventType.RED_DOT_VIEW_EVENT, redDotData)
		end
	end
end

return RedDotController