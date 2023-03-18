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
    public Object Get(String dbname, String uid, String key) {
        return helper.get(dbname, uid, key);
    }

    @Override
    public void Set(String tbname, String uid, String key, String value) {
        helper.set(tbname, uid, key, value);
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
