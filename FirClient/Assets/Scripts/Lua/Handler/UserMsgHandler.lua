local BaseMsgHandler = require 'Handler.BaseMsgHandler'
local UserMsgHandler = class("UserMsgHandler", BaseMsgHandler)

function UserMsgHandler:Initialize()
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