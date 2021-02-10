local UserModule = class("UserModule")

function UserModule:Initialize()
    self.loginCallback = nil
    self.netMgr = MgrCenter:GetManager(ManagerNames.Network)
end

--封包发送--
function UserModule:ReqLogin(username, password, callback)
    local sendData = {
        name = username,
        pass = password,
    }
    self.loginCallback = callback
    self.netMgr:SendMessage("pb_user.ReqLogin", sendData)
end

--解包使用--
function UserModule:ResLogin(data)
    self.loginData = table.deepcopy(data)
    if type(self.loginData.userinfo) == 'table' then
        if self.loginCallback then
            self.loginCallback(self.loginData.userinfo)
        end
    end
end

return UserModule