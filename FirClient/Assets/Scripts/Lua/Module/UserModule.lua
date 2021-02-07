local UserModule = class("UserModule")

function UserModule:Initialize()
    self.netMgr = MgrCenter:GetManager(ManagerNames.Network)
end

--封包发送--
function UserModule:Req_UserLogin(username, password)
    local sendData = {
        name = username,
        pass = password,
    }
    self.netMgr:SendMessage("pb_user.ReqLogin", sendData)
end

--解包使用--
function UserModule:Res_UserLogin(data)
    self.loginData = table.deepcopy(data)
    print("Res_UserLogin", self.loginData.userid)
end

function UserModule:Req_UserRegister(username, password)
    local sendData = {
        name = username,
        pass = password,
    }
    self.netMgr:SendMessage("pb_user.ReqRegister", sendData)
end

function UserModule:Res_UserRegister(data)
    self.regData = table.deepcopy(data)
    print("Res_UserRegister", self.regData.userid)
end

return UserModule