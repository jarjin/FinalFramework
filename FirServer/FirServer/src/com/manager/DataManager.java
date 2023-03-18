package com.manager;

import com.database.DB_MongoDB;
import com.database.DB_MySQL;
import com.define.AppConst;
import com.define.enums.DBType;
import com.interfaces.IDatabase;

public class DataManager extends BaseManager {
    private IDatabase db;

    @Override
    public void Initialize() {
        switch(AppConst.DbType) {
            case MySQL: 
                db = new DB_MySQL();
            break;
            case MongoDB:
                db = new DB_MongoDB();
            break;
            default: break;
        }
        if (db != null) {
            db.Initialize();
        }
    }

    public void TestDB() {
        String strMsg = db != null ? db.GetType().toString() : "None";
        logMgr().Trace("Current Database Type:" + strMsg);

        if (AppConst.DbType == DBType.None) return;
        Set("users", "10000", "money", 99999);

        Object obj = Get("users", "10000", "money");
        logMgr().Trace("TestDB:>>>" + obj);
    }

    public Object Get(String dbname, String uid, String key) {
        return db.Get(dbname, uid, key);
    }

    public void Set(String tbname, String uid, String key, Object value) {
        db.Set(tbname, uid, key, value);
    }

    public void CloseDB() {
        db.Close();
    }

    @Override
    public void OnDispose() {
    }
}
