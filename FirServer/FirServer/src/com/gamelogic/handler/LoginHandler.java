package com.gamelogic.handler;

import com.common.Protocal;
import com.google.protobuf.InvalidProtocolBufferException;
import com.handler.BaseHandler;
import com.protos.Person;
import com.smartfoxserver.v2.entities.User;
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
        netMgr().SendData(user, ProtoType.LuaProtoMsg, Protocal.ResLogin, person);
    }
}

