package com.interfaces;

import com.smartfoxserver.v2.entities.User;

public interface IHandler {
    void OnMessage(User user, byte[] bytes);
}
