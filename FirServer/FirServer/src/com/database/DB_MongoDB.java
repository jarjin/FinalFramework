package com.database;

import com.define.enums.DBType;
import com.helper.MongoDBHelper;
import com.helper.RedisHelper;
import com.interfaces.IDatabase;

public class DB_MongoDB implements IDatabase {
    private RedisHelper mcHelper;
    private MongoDBHelper helper;

    public DB_MongoDB() {
        helper = new MongoDBHelper();
        mcHelper = new RedisHelper();
    }

    @Override
    public void Initialize() {
        helper.initMongoDB();
        mcHelper.initRedis();
    }

    @Override
    public DBType GetType() {
        return DBType.MongoDB;
    }

    @Override
    public Object Get(String dbname, String uid, String key) {
        Object obj = mcHelper.get(dbname, uid, key);
        if (obj == null) {
            obj = helper.get(dbname, uid, key);
            if (obj != null) {
                mcHelper.set(dbname, uid, key, obj);
            }
        }
        return helper.get(dbname, uid, key);
    }

    @Override
    public void Set(String tbname, String uid, String key, Object value) {
        helper.set(tbname, uid, key, value);
        mcHelper.set(tbname, uid, key, value);
    }

    @Override
    public void Close() {
        helper.CloseMongoDB();
        mcHelper.closeRedis();
    }

    @Override
    public void OnDispose() {
        helper = null;
        mcHelper = null;
    }
}
