local BattleModule = class("BattleModule")

MsgType = {
	Text = 0,
	Image = 1,
}

local mChatDemoStrList = {
    "欢迎进入游戏世界~~Waiting for we...",
}
local mBattleDataList = {}
local configMgr = nil
local refCount = 0

function BattleModule:Initialize()
	configMgr = MgrCenter:GetManager(ManagerNames.Config)

	local count = #mChatDemoStrList
	for i = 1, count do
		local item = {
			mMsgType = MsgType.Text,
			mSrtMsg = mChatDemoStrList[i]
		}
		table.insert(mBattleDataList, item)
	end
end

function BattleModule:AddMsgItem(storyid, pageid, dlgid)
	local msgData = configMgr:GetDialogDataByKey(storyid, pageid, dlgid)
	if msgData ~= nil then
		local item = {
			mMsgType = MsgType.Text,
			mSrtMsg = msgData.txtContent
		}
		table.insert(mBattleDataList, item)
	end
end

function BattleModule:GetDataListSize()
	return #mBattleDataList
end

function BattleModule:GetDataByIndex(index)
	return mBattleDataList[index]
end

return BattleModule