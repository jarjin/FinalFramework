local TestMsgHandler = class("TestMsgHandler")

function TestMsgHandler:OnTestProto(data)
    --解包使用--
    print("OnTestProto", data.name)

    --封包发送--
    local newData = {
        name = 'Jeffry lee'
    }
    local netMgr = MgrCenter:GetManager(ManagerNames.Network)
    netMgr:SendData("Person", newData)
end

TestMsgHandler.MsgFuncs = 
{
    ["Test.ProtoName"] = TestMsgHandler.OnTestProto,
}

return TestMsgHandler