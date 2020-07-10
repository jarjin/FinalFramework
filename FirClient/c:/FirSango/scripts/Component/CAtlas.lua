local CAtlas = class("CAtlas")

function CAtlas:initialize(name, objs)
    self.mAtlas = {}
    for i = 0, objs.Length - 1 do
        local sprite = objs[i]
        self.mAtlas[sprite.name] = sprite
    end
    logWarn("New UIAtlas-------:>"..name)
end

function CAtlas:GetSprite(name)
    return self.mAtlas[name]
end

function CAtlas:RemoveSprite(name)
    return table.removeKey(self.mAtlas, name)
end

return CAtlas