package com.gamelogic;

import org.apache.log4j.Logger;

import com.common.Protocal;
import com.define.ModelNames;
import com.gamelogic.common.GameBehaviour;
import com.gamelogic.handler.LoginHandler;
import com.gamelogic.model.BackpackModel;
import com.gamelogic.model.UserModel;
import com.interfaces.IWorld;
import com.tables.Tables.*;

public class GameWorld extends GameBehaviour implements IWorld  {
    Logger logger = Logger.getLogger(GameWorld.class);
    
    @Override
    public void Initialize() {
        TestTable();
        InitManager();
        RegHandler();
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
        logger.info(String.format("id={%s} value={%s}", item.id, item.value));
    }

    @Override
    public void OnDispose() {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'OnDispose'");
    }
}
