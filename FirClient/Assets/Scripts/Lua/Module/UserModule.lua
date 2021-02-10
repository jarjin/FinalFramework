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
    local userid = self.loginData.userid
    if userid then
        if self.loginCallback then
            self.loginCallback(userid)
        end
    end
end

function UserModule:ReqRegister(username, password)
    local sendData = {
        name = username,
        pass = password,
    }
    self.netMgr:SendMessage("pb_user.ReqRegister", sendData)
end

function UserModule:ResRegister(data)
    self.regData = table.deepcopy(data)
    print("Res_UserRegister", self.regData.userid)
end

return UserModule