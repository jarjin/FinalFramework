package com.database;

import com.define.enums.DBType;
import com.helper.MySQLHelper;
import com.interfaces.IDatabase;

public class DB_MySQL implements IDatabase {
    private MySQLHelper helper;

    public DB_MySQL() {
        helper = new MySQLHelper();
    }

    @Override
    public void Initialize() {
        helper.initMySQL();
    }

    @Override
    public DBType GetType() {
        return DBType.MySQL;
    }

    @Override
    public void Close() {
        helper.closeMySQL();
    }

    @Override
    public void OnDispose() {
        helper = null;
    }
}
