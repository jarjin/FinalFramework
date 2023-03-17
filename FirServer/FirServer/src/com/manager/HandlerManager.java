package com.manager;

import java.util.LinkedHashMap;
import java.util.Map;

import com.interfaces.IHandler;
import com.smartfoxserver.v2.entities.User;

public class HandlerManager extends BaseManager {
    private static Map<String, IHandler> handlers = new LinkedHashMap<String, IHandler>();

    @Override
    public void Initialize() {
    }

    public void AddHandler(String typeName, IHandler handler) {
        if (!handlers.containsKey(typeName)) {
            handlers.put(typeName, handler);
        }
    }

    public IHandler GetHandler(String typeName) {
        IHandler handler = null;
        if (handlers.containsKey(typeName))
        {
            handler = handlers.get(typeName);
        }
        return handler;
    }

    public void RemoveHandler(String typeName) {
        if (handlers.containsKey(typeName)) {
            handlers.remove(typeName);
        }
    }

    public void OnRecvMessage(User user, String protoName, byte[] bytes) {
        IHandler handler = GetHandler(protoName);
        if (handler != null) {
            handler.OnMessage(user, bytes);
        }
    }

    @Override
    public void OnDispose() {
    }


}
