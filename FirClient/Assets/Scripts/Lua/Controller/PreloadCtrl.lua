local CAtlas = require "Component/CAtlas"
local BaseCtrl = require 'Controller.BaseCtrl'
local PreloadCtrl = class("PreloadCtrl", BaseCtrl)

local mAtlasList = {
	"Atlas/UI/Shared/Shared", 
	"Atlas/UI/Quality/Quality",
    "Atlas/UI/HeadIcon/HeadIcon", 
}

local mActions = nil

function PreloadCtrl:Initialize(initOK)
	mActions = {}
	table.insert(mActions, self.PreloadAtlas)
	self:ProcessAction(initOK)
end

function PreloadCtrl:ProcessAction(initOK)
	if 0 == #mActions then
		execAction(initOK)
		return
	end
	local action = table.remove(mActions, 1)
	if action ~= nil then
		local loadOK = function ()
			self:ProcessAction(initOK)
		end
		action(self, loadOK)
	end
end

function PreloadCtrl:PreloadAtlas(loadOK)
	local refCount = 0
	local count = #mAtlasList
	for i = 1, count do
		local atName = mAtlasList[i]
		self.resMgr:LoadAssetAsync(atName, nil, typeof(Sprite), function (objs)
			logWarn('PreloadAtlas!!!:>'..atName)
			if objs ~= nil then
				local assetName = Path.GetFileNameWithoutExtension(atName)
                self.componentMgr:AddComponent(ComponentNames.Atlas, assetName, objs)
			end
			refCount = refCount + 1
			if refCount == count then
				if loadOK ~= nil then
					loadOK()
				end
			end
		end)
	end
end

return PreloadCtrl