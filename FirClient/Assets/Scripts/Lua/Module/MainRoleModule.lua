local MainRoleModule = class("MainRoleModule")

function MainRoleModule:Initialize()
	self.mMainRoleDataList = {}
	self.mCurrMainRoleData = {}

	local i = 1
	local tableMgr = MgrCenter:GetManager(ManagerNames.Table)
	local items = tableMgr.npcTable:GetItems()
	local iter = items:GetEnumerator()
	while iter:MoveNext() do
		if iter.Current.isMainCharacter then
			local item = {
				index = i,
				mIsExpand = false,
			}
			table.insert(self.mMainRoleDataList, item)
		end
		i = i + 1
	end
end

function MainRoleModule:GetDataListSize()
	return #self.mMainRoleDataList
end

function MainRoleModule:GetDataByIndex(index)
	return self.mMainRoleDataList[index]
end

function MainRoleModule:AssignMainRoleData(roleid, rolesex)
	self.mCurrMainRoleData['roleid'] = roleid
	self.mCurrMainRoleData['rolesex'] = rolesex
end

function MainRoleModule:GetMainRoleData()
	return self.mCurrMainRoleData
end

return MainRoleModule