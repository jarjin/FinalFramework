local LoginAdapter = class("LoginAdapter")

function LoginAdapter:OnEnterLevel(execOK)
	Main.ShowUI(UiNames.Login)
	Main.CloseUI(UiNames.Loader)
	if execOK ~= nil then
		execAction(execOK)
	end
	self:ConnectServer()
end

function LoginAdapter:ConnectServer()
	self.netMgr = MgrCenter:GetManager(ManagerNames.Network)
	if self.netMgr ~= nil then
		local ip = AppConst.SocketAddress
		local port = tointeger(AppConst.SocketPort)
		self.netMgr:Connect(ip, port, self, self.OnConnectOK)
	end
end

function LoginAdapter:StartLogin()
	local levelMgr = MgrCenter:GetManager(ManagerNames.Level)
	if levelMgr ~= nil then
		levelMgr:LoadLevel(LevelType.Main)
	end
	Main.CloseUI(UiNames.Login)
end

function LoginAdapter:OnConnectOK(disReason)
	if disReason then
		logError('Connection failed!!! reason: '..disReason)
	else
		logWarn("OnConnectOK---->>>")
	end
end

function LoginAdapter:OnLeaveLevel(execOK)
	if execOK ~= nil then
		execAction(execOK)
	end
end

return LoginAdapter