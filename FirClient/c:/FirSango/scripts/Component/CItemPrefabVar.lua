local CItemPrefabVar = class("CItemPrefabVar")

function CItemPrefabVar:initialize(gameObject, componentMgr)
    self.gameObject = gameObject
    self.componentMgr = componentMgr
    self.values = {}
    self.components = {}
end

function CItemPrefabVar:Awake()
    self:FindComponent("img_", typeof(Image));
    self:FindComponent("txt_", typeof(Text));
    self:FindComponent("btn_", typeof(Button));
    self:FindComponent("trans_", typeof(RectTransform));
    self:FindComponent("itembox_", typeof(CLuaComponent), ComponentNames.ItemBox);
end

function CItemPrefabVar:FindComponent(prefix, type, typeName)
    local array = self.gameObject:GetComponentsInChildren(type, true)
    local len = array.Length
    for i = 0, len - 1 do
        local name = array[i].name
        if string.startsWith(name, prefix) then
            local component = nil
            if typeName == nil then
                self.components[name] = array[i]
            else
                local gameObj = array[i].gameObject
                local component = self.componentMgr:AddComponent(typeName, gameObj)
                if component.Awake ~= nil then
                    component:Awake(component)
                end
                self.components[name] = component
            end
        end
    end
end

function CItemPrefabVar:TryGetComponent(varName)
    return self.components[varName]
end

function CItemPrefabVar:SetText(varName, value)
    local c = self:TryGetComponent(varName)
    if c ~= nil then
        c.text = value
    end
end

function CItemPrefabVar:SetImage(varName, value)
    local c = self:TryGetComponent(varName)
    if c ~= nil then
        c.sprite = value
    end
end

function CItemPrefabVar:GetValue(varName)
    return self.values[varName]
end

function CItemPrefabVar:SetValue(varName, value)
    self.values[varName] = value
end

return CItemPrefabVar