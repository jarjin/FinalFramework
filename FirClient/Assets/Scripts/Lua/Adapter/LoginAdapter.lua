local LoginAdapter = class("LoginAdapter")

function LoginAdapter:OnEnterLevel(execOK)
	Main.ShowUI(UiNames.Login)
	Main.CloseUI(UiNames.Loader)
	if execOK ~= nil then
		execAction(execOK)
	end
end

function LoginAdapter:OnConnectServer(func)
	local netMgr = MgrCenter:GetManager(ManagerNames.Network)
	if netMgr ~= nil then
		netMgr.Connect(func)
	end
end

function LoginAdapter:StartLogin()
	local levelMgr = MgrCenter:GetManager(ManagerNames.Level)
	if levelMgr ~= nil then
		levelMgr:LoadLevel(LevelType.Main)
	end
	Main.CloseUI(UiNames.Login)
end

function LoginAdapter:OnLoginOK(retCode)
	if retCode == ResultCode.Success then
	else
	--[[
		var dw = new NetDataWriter()
		dw.Put("testName")
		dw.Put("testPass")
		networkMgr.SendData(GameProtocal.Register, dw)
	]]
	end
	logWarn("OnLoginOK:"..retCode)
end

function LoginAdapter:OnRegisterOK(retCode)
	if retCode == ResultCode.Success then
	end
	logWarn("OnRegisterOK:"..retCode)
end

function LoginAdapter:OnLeaveLevel(execOK)
	if execOK ~= nil then
		execAction(execOK)
	end
end

return LoginAdapter