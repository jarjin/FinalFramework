package com.database;

import com.define.enums.DBType;
import com.helper.MongoDBHelper;
import com.interfaces.IDatabase;

public class DB_MongoDB implements IDatabase {
    private MongoDBHelper helper;

    @Override
    public void Initialize() {
        helper = new MongoDBHelper();
    }

    @Override
    public DBType GetType() {
        return DBType.MongoDB;
    }

    @Override
    public void OnDispose() {
        helper = null;
    }
}
