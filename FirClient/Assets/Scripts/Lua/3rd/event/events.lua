--[[
Author: liu kai
Date: 2021-03-05 14:16:26
LastEditTime: 2021-03-05 19:50:30
LastEditors: Do not edit
FilePath: \Lua\3rd\event\events.lua
Description: 
--]]
--[[
Auth:Chiuan
like Unity Brocast Event System in lua.
]]
local EventLib = require "3rd/event/eventlib"

local Event = {}
local events = {}

function Event.AddListener(event, handler)
    if not event or type(event) ~= "string" then
        error("event parameter in addlistener function has to be string, " .. type(event) .. " not right.")
    end
    if not handler or type(handler) ~= "function" then
        error("handler parameter in addlistener function has to be function, " .. type(handler) .. " not right")
    end

    if not events[event] then
        --create the Event with name
        events[event] = EventLib:new(event)
    end

    --conn this handler
    events[event]:connect(handler)
end

function Event.Brocast(event, ...)
    --logWarn("Event.Brocast")
    
    if not events[event] then
        --以后可以改成error
        -- error("brocast " .. event .. " has no event.")
    else
        --dump(events[event])
        events[event]:fire(...)
    end
end

function Event.RemoveListener(event, handler)
    if not events[event] then
        --以后可以改成error
        print("remove " .. event .. " has no event.")
    else
        events[event]:disconnect(handler)
    end
end

return Event
