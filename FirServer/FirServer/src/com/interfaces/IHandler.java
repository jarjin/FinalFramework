package com.interfaces;

import com.smartfoxserver.v2.entities.User;

public interface IHandler extends IObject {
    void OnMessage(User user, byte[] bytes);
}
