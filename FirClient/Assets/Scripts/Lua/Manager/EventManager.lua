
local EventLib = require "3rd.event.eventlib"
local EventManager = class("EventManager")

function EventManager:Initialize()
	self.events = {}
	logWarn('EventManager:InitializeOK...')
end

function EventManager:AddEvent(eventName, func)
	self.events = self.events or {}
	if self.events[eventName] ~= nil then
		logError(eventName, 'event was exist!!!')
		return
	end
	local evobj = EventLib:new()
	self.events[eventName] = {
		obj = evobj,
		con = evobj:Connect(func),
	}
end

function EventManager:FireEvent(eventName, ...)
	local ev = self.events[eventName]
	if ev then
		ev.obj:Fire(...)
	end
end

function EventManager:RemoveEvent(eventName)
	if self.events[eventName] ~= nil then
		local ev = table.removeKey(self.events, eventName)
		if ev then
			ev.con:Disconnect()
		end
	end
end

return EventManager