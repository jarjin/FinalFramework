package com.manager;

import java.util.LinkedHashMap;
import java.util.Map;

import com.interfaces.IModel;

public class ModelManager extends BaseManager {
    private static Map<String, IModel> models = new LinkedHashMap<String, IModel>();

    @Override
    public void Initialize() {
    }

    public void AddModel(String typeName, IModel model) {
        if (!models.containsKey(typeName)) {
            models.put(typeName, model);
        }
    }

    public IModel GetModel(String typeName) {
        IModel model = null;
        if (models.containsKey(typeName))
        {
            model = models.get(typeName);
        }
        return model;
    }

    public void RemoveModel(String typeName) {
        if (models.containsKey(typeName)) {
            models.remove(typeName);
        }
    }

    @Override
    public void OnDispose() {
    }
}
