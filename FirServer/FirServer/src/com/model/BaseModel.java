package com.model;

import com.common.BaseBehaviour;
import com.interfaces.IModel;

public class BaseModel extends BaseBehaviour implements IModel {
    @Override
    public void Initialize() {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'Initialize'");
    }

    public void OnUpdate(float deltaTime) {
    }

    @Override
    public void OnDispose() {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'OnDispose'");
    }
}
