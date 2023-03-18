package com.database;

import com.define.enums.DBType;
import com.helper.MongoDBHelper;
import com.interfaces.IDatabase;

public class DB_MongoDB implements IDatabase {
    private MongoDBHelper helper;

    public DB_MongoDB() {
        helper = new MongoDBHelper();
    }

    @Override
    public void Initialize() {
        helper.initMongoDB();
    }

    @Override
    public DBType GetType() {
        return DBType.MongoDB;
    }

    @Override
    public void Close() {
        helper.CloseMongoDB();
    }

    @Override
    public void OnDispose() {
        helper = null;
    }
}
