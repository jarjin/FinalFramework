package com.database;

import com.define.enums.DBType;
import com.helper.MySQLHelper;
import com.helper.RedisHelper;
import com.interfaces.IDatabase;

public class DB_MySQL implements IDatabase {
    private RedisHelper mcHelper;
    private MySQLHelper helper;

    public DB_MySQL() {
        helper = new MySQLHelper();
        mcHelper = new RedisHelper();
    }

    @Override
    public void Initialize() {
        helper.initMySQL();
        mcHelper.initRedis();
    }

    @Override
    public DBType GetType() {
        return DBType.MySQL;
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
        helper.closeMySQL();
        mcHelper.closeRedis();
    }

    @Override
    public void OnDispose() {
        helper = null;
        mcHelper = null;
    }
}
