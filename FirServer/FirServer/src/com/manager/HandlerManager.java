package com.manager;

import java.util.LinkedHashMap;
import java.util.Map;

import com.interfaces.IHandler;

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

    @Override
    public void OnDispose() {
    }
}
