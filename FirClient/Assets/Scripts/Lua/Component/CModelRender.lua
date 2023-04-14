local BaseComponent = require "Component.BaseComponent"
local CModelRender = class("CModelRender", BaseComponent)

function CModelRender:initialize(gameObject)
    self.gameObject = gameObject
end

return CModelRender