

local UIDUtil = {}
local _eventId = 1000000

--事件名的唯一id
function UIDUtil.GetEventUID()
	_eventId = _eventId + 1
	return _eventId
end

return UIDUtil
