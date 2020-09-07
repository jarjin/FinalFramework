local HeroModule = class("HeroModule")

local mHeroDataList = {}

function HeroModule:Initialize()
	local tableMgr = MgrCenter:GetManager(ManagerNames.Table)
	local items = tableMgr.npcTable:GetItems()
	local iter = items:GetEnumerator()

	while iter:MoveNext() do
		local npcItem = iter.Current
		if not npcItem.isMainCharacter and npcItem.country >= 1 and npcItem.country <= 3 then
			local item = {
				name = npcItem.name,
				itemid = npcItem.itemid,
			}
			table.insert(mHeroDataList, item)
		end
	end
end

function HeroModule:GetDataListSize()
	return #mHeroDataList
end

function HeroModule:GetDataByIndex(index)
	return mHeroDataList[index]
end

return HeroModule