package com.interfaces;

import com.define.enums.DBType;

public interface IDatabase extends IObject {
    void Initialize();
    DBType GetType();
    void OnDispose();
}
