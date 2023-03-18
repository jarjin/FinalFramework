package com.gamelogic.model;

import com.model.BaseModel;

public class UserModel extends BaseModel {
    private String TbName = "testdemo";

    public String GetName(String userid) {
        return dataMgr().Get(TbName, userid, "name").toString();
    }

    public void SetName(String userid, String value) {
        dataMgr().Set(TbName, userid, "name", value);
    }

    public long GetMoney(String userid) {
        Object obj_money = dataMgr().Get(TbName, userid, "money");
        return (long)obj_money;
    }

    public void SetMoney(String userid, long value) {
        dataMgr().Set(TbName, userid, "money", value);
    }
}
