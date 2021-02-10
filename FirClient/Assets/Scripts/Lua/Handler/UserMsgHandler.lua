local UserMsgHandler = class("UserMsgHandler")

function UserMsgHandler:Initialize()
    self.moduleMgr = MgrCenter:GetManager(ManagerNames.Module)
    self.userModule = self.moduleMgr:GetModule(ModuleNames.User)
end

function UserMsgHandler:OnRecvLogin(data)
    if type(data) ~= 'table' then return end
    if self.userModule then
        self.userModule:ResLogin(data)
    end
end

UserMsgHandler.MsgFuncs = 
{
    ["pb_user.ResLogin"] = UserMsgHandler.OnRecvLogin,
}

return UserMsgHandler