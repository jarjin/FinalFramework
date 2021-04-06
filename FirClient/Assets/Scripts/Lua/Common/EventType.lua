local UIDUtil = require "Common/UIDUtil"

local EventType = {}

EventType.RED_DOT_UPDATE_EVENT      = tostring(UIDUtil.GetEventUID())
EventType.RED_DOT_CLEAR_EVENT       = tostring(UIDUtil.GetEventUID())
EventType.RED_DOT_VIEW_EVENT        = tostring(UIDUtil.GetEventUID())
return EventType