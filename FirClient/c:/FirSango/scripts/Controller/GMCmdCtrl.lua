local GMCmdCtrl = class("GMCmdCtrl")

function GMCmdCtrl:Execute(text)
	local resMgr = ManagerCenter:GetManager(ManagerNames.Resource)
	if resMgr ~= nil then
		local array = text:split(':')
		if string.len(array[2]) == 0 then
			return
		end
		local cmd = array[2]
		if cmd == "dump" then
			resMgr:TakeSnapshot()
		elseif cmd == 'diff' then
			resMgr:DiffSnapshot()
		elseif cmd == 'cldump' then
			resMgr:ClearSnapshot()
		end
		logError('GMCmdCtrl:Execute--->'..cmd)
	end
end

return GMCmdCtrl