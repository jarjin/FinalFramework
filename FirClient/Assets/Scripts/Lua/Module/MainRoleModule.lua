local MainRoleModule = class("MainRoleModule")

local mMainRoleDataList = {}
local mCurrMainRoleData = {}

function MainRoleModule:Initialize()
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
			table.insert(mMainRoleDataList, item)
		end
		i = i + 1
	end
end

function MainRoleModule:GetDataListSize()
	return #mMainRoleDataList
end

function MainRoleModule:GetDataByIndex(index)
	return mMainRoleDataList[index]
end

function MainRoleModule:AssignMainRoleData(roleid, rolesex)
	mCurrMainRoleData['roleid'] = roleid
	mCurrMainRoleData['rolesex'] = rolesex
end

function MainRoleModule:GetMainRoleData()
	return mCurrMainRoleData
end

return MainRoleModule