package com.manager;

import com.MainExtension;
import com.common.BaseBehaviour;
import com.interfaces.IManager;

public class BaseManager extends BaseBehaviour implements IManager {
    protected MainExtension mainExt;

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
