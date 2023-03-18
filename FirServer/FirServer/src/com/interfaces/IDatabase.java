package com.interfaces;

import com.define.enums.DBType;

public interface IDatabase extends IObject {
    void Initialize();
    DBType GetType();
    Object Get(String dbname, String uid, String key);
    void Set(String tbname, String uid, String key, Object value);
    void Close();
    void OnDispose();
}
