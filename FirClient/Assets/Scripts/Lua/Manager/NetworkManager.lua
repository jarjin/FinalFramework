local protobuf = require "3rd/pbc/protobuf"

local NetworkManager = class("NetworkManager")

function NetworkManager:Initialize()
    self.ProtoMsgs = {}
    self:RegProtoPB()
    self.socket = MgrCenter:GetManager(ManagerNames.Socket)
end

function NetworkManager:RegProtoPB()
    local pb = require "ProtoMsg/pb"
    for _, value in ipairs(pb) do
        local path = Util.DataPath..'Datas/LuaPB/'..value
        local addr = io.open(path, "rb")
        local buffer = addr:read "*a"
        addr:close()
        protobuf.register(buffer)
        log("RegProto:>"..path)
    end
end

function NetworkManager:RegMsgHandler(handler)
    if handler ~= nil and handler.MsgFuncs ~= nil then
        for key, func in pairs(handler.MsgFuncs) do
            if self.ProtoMsgs[key] == nil then
                self.ProtoMsgs[key] = {}
            end
            local funcs = self.ProtoMsgs[key]
            local canInsert = true
            if #funcs > 0 then
                for _, value in ipairs(funcs) do
                    if value.call == func then
                        canInsert = false
                        break
                    end
                end
            end
            if canInsert then
                table.insert(self.ProtoMsgs[key], { call = func, obj = handler })
            end
        end
    end
end

function NetworkManager:RemoveMsgHandler(handler)
    if handler ~= nil and handler.MsgFuncs ~= nil then
        for key, func in pairs(handler.MsgFuncs) do
            local funcs = self.ProtoMsgs[key]
            if funcs ~= nil then
                for i = #funcs, 1 do
                    if funcs[i].call == func then
                        table.remove(funcs, i)
                    end
                end
            end
        end
    end
end

function NetworkManager:SendMessage(name, data)
    local bytes = protobuf.encode(name, data)
    if bytes ~= nil then
        self.socket:SendData(name, bytes);
    end
end

function NetworkManager:OnReceived(name, bytes)
    local data, err = protobuf.decode(name, bytes)
    if err ~= nil then
        logError('recv proto protobuf.decode '..name..' failed!~'..err)
        return
    end
    local funcs = self.ProtoMsgs[name]
    if funcs ~= nil then
        for _, func in ipairs(funcs) do
            if func ~= nil then
                func.call(func.obj, data)
            end
        end
    end
end

function NetworkManager:Connect(ip, port, caller, onConnected, onDisconnected)
    if self.socket then
        self.socket:Connect(ip, port, caller, onConnected, onConnected)
        log("Connect Server [ip]:"..ip.." [port]:"..port)
    end
end

return NetworkManager