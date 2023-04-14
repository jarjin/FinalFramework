local BaseAdapter = require 'Adapter.BaseAdapter'
local LoginAdapter = class("LoginAdapter", BaseAdapter)

function LoginAdapter:OnEnterLevel(execOK)
	Main.ShowUI(UiNames.Login)
	Main.CloseUI(UiNames.Loader)
	if execOK ~= nil then
		execAction(execOK)
	end
	if AppConst.NetworkMode then
		self:ConnectServer()
	end
end

function LoginAdapter:ConnectServer()
	if self.netMgr ~= nil then
		local ip = AppConst.SocketAddress
		local port = tointeger(AppConst.SocketPort)
		self.netMgr:Connect(ip, port, self, self.OnConnectOK, self.OnDisconnected)
	end
end

function LoginAdapter:StartLogin()
	if self.levelMgr ~= nil then
		levelMgr:LoadLevel(LevelType.Main)
	end
	Main.CloseUI(UiNames.Login)
end

function LoginAdapter:OnConnectOK(disReason)
	if disReason then
		log("OnConnectOK---->>>")
	else
		logError('Connection failed!!!')
	end
end

function LoginAdapter:OnDisconnected(disReason)
	logError('server disconnected!!! reason:', disReason)
end

function LoginAdapter:OnLeaveLevel(execOK)
	if execOK ~= nil then
		execAction(execOK)
	end
end

return LoginAdapter