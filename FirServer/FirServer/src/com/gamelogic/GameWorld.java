package com.gamelogic;

import com.common.Protocal;
import com.define.classes.*;
import com.gamelogic.common.GameBehaviour;
import com.gamelogic.handler.LoginHandler;
import com.gamelogic.model.BackpackModel;
import com.gamelogic.model.UserModel;
import com.interfaces.IWorld;
import com.tables.Tables.*;

public class GameWorld extends GameBehaviour implements IWorld  {
    @Override
    public void Initialize() {
        TestTable();
        InitManager();
        RegHandler();
        TestDBServer();
    }

    ///初始化管理器
    void InitManager() {
        modelMgr().AddModel(ModelNames.User, new UserModel());
        modelMgr().AddModel(ModelNames.Backpack, new BackpackModel());
    }

    ///注册处理器
    void RegHandler() {
        handlerMgr().AddHandler(Protocal.ReqLogin, new LoginHandler());
    }

    ///Test Table
    void TestTable() {
        GlobalConfigTableItem item = tableMgr().globalConfigTable.GetItemByKey("CommonWhite");
        logMgr().Trace(String.format("id={%s} value={%s}", item.id, item.value));
    }

    /**
     * 测试数据库
     */
    void TestDBServer() {
        dataMgr().TestDB();
    }

    @Override
    public void OnDispose() {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'OnDispose'");
    }
}
