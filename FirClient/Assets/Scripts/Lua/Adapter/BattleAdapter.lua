local BaseAdapter = require 'Adapter.BaseAdapter'
local BattleAdapter = class("BattleAdapter", BaseAdapter)

function BattleAdapter:OnEnterLevel(execOK)
	if execOK ~= nil then
		execAction(execOK)
	end
end

function BattleAdapter:EnterDungeon(chapterid, dungeonid, execOK)
	local dungeonData = self.configMgr:GetDungeonData(chapterid, dungeonid)
	if dungeonData ~= nil then
		self:OnBattleDungeon(dungeonData, execOK)
	end
end

function BattleAdapter:LeaveDungeon(execOK)
	self:OnBattleDungeon(nil, execOK)
end

function BattleAdapter:OnBattleDungeon(dungeonData, execOK)
	local co = coroutine.start(function ()
		print('BattleAdapter Coroutine start...')  
		self:ShowBattleLoadingUI(true)

		coroutine.wait(0.1)
		if dungeonData == nil then
			self:ClearBattleSceneMap()
		else
			self.mapMgr:LoadBattleMap(dungeonData)
		end

		coroutine.wait(0.1)
		self:UseBattleCamera(dungeonData ~= nil)

		coroutine.wait(0.1)
		self:ShowBattleLoadingUI(false)

		coroutine.wait(0.1)
		if execOK ~= nil then 
			execAction(execOK)
		end
		print('BattleAdapter Coroutine end...')  
	end)
end

function BattleAdapter:ClearBattleSceneMap()
    local battleMap = self.mapMgr:GetBattleMapObject()
    if battleMap ~= null then
		local type = typeof(UnityEngine.SpriteRenderer)
        local battleMapRenders = battleMap:GetComponentsInChildren(type)
		local count = battleMapRenders.Length
		for i = 0, count - 1 do
			local render = battleMapRenders[i]	
			if not isnil(render) then
				render.sprite = nil
			end
		end
    end
end

function BattleAdapter:UseBattleCamera(isUse)
    --local cullingMask = isUse ? 1 << 8 : 1 << 0
    --battleCamera.cullingMask = cullingMask
end

function BattleAdapter:ShowBattleLoadingUI(isshow)
	Main.ShowBattleLoadingUI(isshow)
end

function BattleAdapter:OnLeaveLevel(execOK)
	if execOK ~= nil then
		execAction(execOK)
	end
end

return BattleAdapter