package com.handler;

import com.common.BaseBehaviour;
import com.interfaces.IHandler;
import com.smartfoxserver.v2.entities.User;

public class BaseHandler extends BaseBehaviour implements IHandler {
    @Override
    public void OnMessage(User user, byte[] bytes) {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'OnMessage'");
    }
}
