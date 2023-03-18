package com.handler;

import com.common.BaseBehaviour;
import com.google.protobuf.GeneratedMessageV3;
import com.interfaces.IHandler;
import com.smartfoxserver.v2.entities.User;
import com.tables.enums.ProtoType;

public class BaseHandler extends BaseBehaviour implements IHandler {
    @Override
    public void OnMessage(User user, byte[] bytes) {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'OnMessage'");
    }

    ///发送数据
    protected void SendData(User user, ProtoType type, String protoName, GeneratedMessageV3 messageV3) {
        netMgr().SendData(user, type, protoName, messageV3);
    }
}
