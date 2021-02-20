local BattleModule = class("BattleModule")

function BattleModule:Initialize()
	self.mBattleDataList = {}
	self.mChatDemoStrList = {
		"欢迎进入游戏世界~~Waiting for we...",
	}
	self.configMgr = MgrCenter:GetManager(ManagerNames.Config)

	local count = #self.mChatDemoStrList
	for i = 1, count do
		local item = {
			mMsgType = MsgType.Text,
			mSrtMsg = self.mChatDemoStrList[i]
		}
		table.insert(self.mBattleDataList, item)
	end
end

function BattleModule:AddMsgItem(storyid, pageid, dlgid)
	local msgData = self.configMgr:GetDialogDataByKey(storyid, pageid, dlgid)
	if msgData ~= nil then
		local item = {
			mMsgType = MsgType.Text,
			mSrtMsg = msgData.txtContent
		}
		table.insert(self.mBattleDataList, item)
	end
end

function BattleModule:GetDataListSize()
	return #self.mBattleDataList
end

function BattleModule:GetDataByIndex(index)
	return self.mBattleDataList[index]
end

return BattleModule