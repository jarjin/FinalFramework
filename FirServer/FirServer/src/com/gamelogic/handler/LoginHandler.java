package com.gamelogic.handler;

import com.google.protobuf.InvalidProtocolBufferException;
import com.handler.BaseHandler;
import com.protos.Person;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.tables.enums.ProtoType;

public class LoginHandler extends BaseHandler {
    @Override
    public void OnMessage(User user, byte[] bytes) {
        Person person = null;
        try {
            person = Person.parseFrom(bytes);
        } catch (InvalidProtocolBufferException e) {
            e.printStackTrace();
        }
        if (person != null) {
            logMgr().Trace("Person Count: " + person.getName());
        }
        ISFSObject reply = new SFSObject();
        netMgr().SendData(user, ProtoType.LuaProtoMsg, reply);
    }
}

