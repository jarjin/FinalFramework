local BaseModule = require 'Module.BaseModule'
local HeroModule = class("HeroModule", BaseModule)

function HeroModule:Initialize()
	self.mHeroDataList = {}
	local items = self.tableMgr.npcTable:GetItems()
	local iter = items:GetEnumerator()

	while iter:MoveNext() do
		local npcItem = iter.Current
		if not npcItem.isMainCharacter then
			local item = {
				name = npcItem.name,
				itemid = npcItem.itemid,
			}
			table.insert(self.mHeroDataList, item)
		end
	end
end

function HeroModule:GetDataListSize()
	return #self.mHeroDataList
end

function HeroModule:GetDataByIndex(index)
	return self.mHeroDataList[index]
end

return HeroModule