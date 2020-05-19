local CItemTips = class("CItemTips")

local hideTimeSecond = 0.4

function CItemTips:initialize(gameObject)
    self.callback = args[1]
    self.gameObject = gameObject
end

function CItemTips:Start()
end

function CItemTips:OnEnable()
    self:InvokeLuaFunc()
end

function CItemTips:InvokeLuaFunc()
    local co = coroutine.start(function ()
        coroutine.wait(hideTimeSecond)
        if self.callback ~= nil then
            self.callback(self)
        end
    end)
end

function CItemTips:OnDestroy()
    self.callback = nil
end

return CItemTips