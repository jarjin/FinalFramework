local CAtlas = require "Component/CAtlas"
local CBatchTask = require "Component/CBatchTask"
local CItemBox = require "Component/CItemBox"
local CItemTips = require "Component/CItemTips"
local CLoopListBox = require "Component/CLoopListBox"
local CModelRender = require "Component/CModelRender"
local CItemPrefabVar = require "Component/CItemPrefabVar"
local CRedDot = require "Component/CRedDot"

local ComponentManager = class("ComponentManager")

function ComponentManager:Initialize()
    self.mAtlas = {}
    self.mBatchTasks = {}
    self.mItemBoxs = {}
    self.mItemTips = {}
    self.mLoopListBoxs = {}
    self.mModelRenders = {}
    self.mItemPrefabVars = {}
    self.mRedDot = {}
    self:AddComponent(ComponentNames.BatchTask, "default")
end

function ComponentManager:AddComponent(typeName, ...)
    local args = {...}
    if typeName == ComponentNames.Atlas then
        local name = args[1]
        local objs = args[2]
        self.mAtlas[name] = CAtlas:new(name, objs)
    elseif typeName == ComponentNames.BatchTask then
        local name = args[1]
        self.mBatchTasks[name] = CBatchTask:new(name)
    elseif typeName == ComponentNames.ItemBox then
        local gameObj = args[1]
        return self:BindItemBox(gameObj)
    elseif typeName == ComponentNames.ItemTips then
        local gameObj = args[1]
        return self:BindItemTips(gameObj)
    elseif typeName == ComponentNames.LoopListBox then
        local gameObj = args[1]
        return self:BindLoopListBox(gameObj)
    elseif typeName == ComponentNames.ModelRender then
        local gameObj = args[1]
        return self:BindModelRender(gameObj)
    elseif typeName == ComponentNames.ItemPrefabVar then
        local gameObj = args[1]
        return self:BindItemPrefabVar(gameObj)
    elseif typeName == ComponentNames.RedDot then
        local gameObj = args[1]
        return self:BindRedDot(gameObj)
    end
end

function ComponentManager:GetComponent(typeName, nameOrId)
    if typeName == ComponentNames.Atlas then
        return self.mAtlas[nameOrId]
    elseif typeName == ComponentNames.BatchTask then
        return self.mBatchTasks[nameOrId]
    elseif typeName == ComponentNames.ItemBox then
        return self.mItemBoxs[nameOrId]
    elseif typeName == ComponentNames.ItemTips then
        return self.mItemTips[nameOrId]
    elseif typeName == ComponentNames.LoopListBox then
        return self.mLoopListBoxs[nameOrId]
    elseif typeName == ComponentNames.ModelRender then
        return self.mModelRenders[nameOrId]
    elseif typeName == ComponentNames.ItemPrefabVar then
        return self.mItemPrefabVars[nameOrId]
    elseif typeName == ComponentNames.RedDot then
        return self.mRedDot[nameOrId]
    end
    return nil
end

function ComponentManager:RemoveComponent(typeName, nameOrId)
    if typeName == ComponentNames.Atlas then
        table.removeKey(self.mAtlas, nameOrId)
    elseif typeName == ComponentNames.BatchTask then
        table.removeKey(self.mBatchTasks, nameOrId)
    elseif typeName == ComponentNames.ItemBox then
        table.removeKey(self.mItemBoxs, nameOrId)
    elseif typeName == ComponentNames.ItemTips then
        table.removeKey(self.mItemTips, nameOrId)
    elseif typeName == ComponentNames.LoopListBox then
        table.removeKey(self.mLoopListBoxs, nameOrId)
    elseif typeName == ComponentNames.ModelRender then
        table.removeKey(self.mModelRenders, nameOrId)
    elseif typeName == ComponentNames.ItemPrefabVar then
        table.removeKey(self.mItemPrefabVars, nameOrId)
    elseif typeName == ComponentNames.RedDot then
        table.removeKey(self.mRedDot, nameOrId)
    end
end

function ComponentManager:Call(typeName, uniqueid, funcName)
    local component = self:GetComponent(typeName, uniqueid)
    if component ~= nil then
        local func = component[funcName]
        if func ~= nil then
            func(component)
        end
    end
end

function ComponentManager:BindItemPrefabVar(gameObj)
    local instanceId = gameObj:GetInstanceID()
    local component = self.mItemPrefabVars[instanceId]
    if component == nil then
        component = CItemPrefabVar:new(gameObj, self)
        self.mItemPrefabVars[instanceId] = component
    end
    return component
end

function ComponentManager:BindModelRender(gameObj)
    local instanceId = gameObj:GetInstanceID()
    local component = self.mModelRenders[instanceId]
    if component == nil then
        component = CModelRender:new(gameObj)
        self.mModelRenders[instanceId] = component
    end
    return component
end

function ComponentManager:BindLoopListBox(gameObj)
    local instanceId = gameObj:GetInstanceID()
    local component = self.mLoopListBoxs[instanceId]
    if component == nil then
        component = CLoopListBox:new(gameObj)
        self.mLoopListBoxs[instanceId] = component
    end
    return component
end

function ComponentManager:BindItemTips(gameObj)
    local instanceId = gameObj:GetInstanceID()
    local component = self.mItemTips[instanceId]
    if component == nil then
        component = CItemTips:new(gameObj)
        self.mItemTips[instanceId] = component
    end
    return component
end

function ComponentManager:BindItemBox(gameObj)
    local instanceId = gameObj:GetInstanceID()
    local component = self.mItemBoxs[instanceId]
    if component == nil then
        component = CItemBox:new(gameObj)
        self.mItemBoxs[instanceId] = component
    end
    return component
end

function ComponentManager:BindRedDot(gameObj)
    local instanceId = gameObj:GetInstanceID()
    local component = self.mRedDot[instanceId]
    if component == nil then
        component = CRedDot:new(gameObj)
        self.mRedDot[instanceId] = component
    end
    return component
     
end

return ComponentManager