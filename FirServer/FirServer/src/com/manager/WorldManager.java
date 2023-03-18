package com.manager;

import com.gamelogic.GameWorld;

public class WorldManager extends BaseManager {
    
    @Override
    public void Initialize() {
        InitAnWorld();
    }

    private void InitAnWorld() {
        new GameWorld().Initialize();
    }

    @Override
    public void OnDispose() {
    }
}
