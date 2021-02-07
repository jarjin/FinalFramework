local DungeonModule = class("DungeonModule")
--[[
    to check the index'th item is a TreeItem or a TreeChildItem.for example,

    0  TreeItem0
    1      TreeChildItem0_0
    2      TreeChildItem0_1
    3      TreeChildItem0_2
    4      TreeChildItem0_3
    5  TreeItem1
    6      TreeChildItem1_0
    7      TreeChildItem1_1
    8      TreeChildItem1_2
    9  TreeItem2
    10     TreeChildItem2_0
    11     TreeChildItem2_1
    12     TreeChildItem2_2

    the first column value is the param 'index', for example, if index is 1,
    then we should return TreeChildItem0_0 to SuperScrollView, and if index is 5,
    then we should return TreeItem1 to SuperScrollView
]]
function DungeonModule:Initialize()
	self.mTreeItemDataList = {}

	local configMgr = MgrCenter:GetManager(ManagerNames.Config)
	local chapters = configMgr:GetChapterList()
	local chapterList = chapters:GetEnumerator()

	local i = 1
	while chapterList:MoveNext() do
		local chapterItem = chapterList.Current
		local treeItem = {
			mName = chapterItem.Value.name,
			mIsExpand = true,
			mChilds = {},
			mBeginIndex = 0,
			mEndIndex = 0,
			mTreeItemIndex = i,
		}
		local j = 1
		local dungeonList = chapterItem.Value.dungeonDatas:GetEnumerator()
		while dungeonList:MoveNext() do
			local dungeonItem = dungeonList.Current
			local childItem = {
				mName = dungeonItem.Value.name,
				mChildIndex = j,
				mDesc = "Desc For "..dungeonItem.Value.name,
			}
			table.insert(treeItem.mChilds, childItem)
			j = j + 1
		end
		table.insert(self.mTreeItemDataList, treeItem)
		i = i + 1
	end
	self:UpdateAllTreeItemDataIndex()
end

function DungeonModule:GetDataListSize()
	return #self.mTreeItemDataList
end

function DungeonModule:GetTotalItemAndChildCount()
    local count = #self.mTreeItemDataList
    if count == 0 then
        return 0
    end
    return self.mTreeItemDataList[count].mEndIndex + 1
end

function DungeonModule:QueryTreeItemByTotalIndex(totalIndex)
	if totalIndex < 0 then
		return nil
	end
	local count = self:GetDataListSize()
	if count == 0 then
		return nil
	end
	for i = 1, #self.mTreeItemDataList do
		local data = self.mTreeItemDataList[i]
		if data.mBeginIndex <= totalIndex and data.mEndIndex >= totalIndex then
			return data
		end
	end
	return nil
end

function DungeonModule:GetDataByIndex(index)
	if index < 0 or index > #self.mTreeItemDataList then
		return nil
	end
	return self.mTreeItemDataList[index]
end

function DungeonModule:GetItemChildDataByIndex(itemIndex, childIndex)
	local item = self:GetDataByIndex(itemIndex)
	if item == nil then
		return nil
	end
	return item.mChilds[childIndex]
end

function DungeonModule:ToggleItemExpand(treeIndex)
	local item = self:GetDataByIndex(treeIndex)
	if item ~= nil then
		item.mIsExpand = not item.mIsExpand
	end
end

function DungeonModule:UpdateAllTreeItemDataIndex()
	local count = #self.mTreeItemDataList
	if count == 0 then return end

	local curEnd = 0
	for i = 1, #self.mTreeItemDataList do
		local data = self.mTreeItemDataList[i]
		if i == 1 then
			data.mBeginIndex = 0
			data.mEndIndex = data.mIsExpand and #data.mChilds or 0
		else
            data.mBeginIndex = curEnd + 1
            data.mEndIndex = data.mBeginIndex + (data.mIsExpand and #data.mChilds or 0)
		end
        curEnd = data.mEndIndex
	end
end

function DungeonModule:IsChild(index, treeItem)
	return index ~= treeItem.mBeginIndex
end

function DungeonModule:GetChildIndex(index, treeItem)
	if not self:IsChild(index, treeItem) then
		return -1
	end
	return index - treeItem.mBeginIndex
end

return DungeonModule