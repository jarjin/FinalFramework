package com.database;

import com.define.enums.DBType;
import com.helper.MySQLHelper;
import com.interfaces.IDatabase;

public class DB_MySQL implements IDatabase {
    private MySQLHelper helper;

    @Override
    public void Initialize() {
        helper = new MySQLHelper();
    }

    @Override
    public DBType GetType() {
        return DBType.MySQL;
    }

    @Override
    public void OnDispose() {
        helper = null;
    }
}
