package com.manager;

import com.database.DB_MongoDB;
import com.database.DB_MySQL;
import com.define.AppConst;
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
    }

    public void TestDB() {
        String strMsg = db != null ? db.GetType().toString() : "None";
        logMgr().Trace("Current Database Type:" + strMsg);
    }

    @Override
    public void OnDispose() {
    }
}
