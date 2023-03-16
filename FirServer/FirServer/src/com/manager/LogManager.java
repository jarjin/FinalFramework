package com.manager;

import com.MainExtension;
import com.common.ManagementCenter;

public class LogManager extends BaseManager {
    private MainExtension mainExt;
    @Override
    public void Initialize() {
        mainExt = ManagementCenter.GetMainExtension();
    }

    @Override
    public void OnDispose() {
    }

    public void Trace(java.lang.Object... args) {
        mainExt.trace(args);
    }
}
