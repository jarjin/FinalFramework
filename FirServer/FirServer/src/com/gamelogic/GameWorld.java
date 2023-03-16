package com.gamelogic;

import com.common.Protocal;
import com.gamelogic.common.GameBehaviour;
import com.gamelogic.handler.LoginHandler;
import com.interfaces.IWorld;
import com.tables.Tables.*;

public class GameWorld extends GameBehaviour implements IWorld  {

    @Override
    public void Initialize() {
        InitManager();
        RegHandler();
    }

    ///初始化管理器
    void InitManager()
    {
    }

    ///注册处理器
    void RegHandler() 
    {
        handlerMgr().AddHandler(Protocal.ReqLogin, new LoginHandler());
    }

    void TestTable()
    {
        //Test Table
        GlobalConfigTableItem item = tableMgr().globalConfigTable.GetItemByKey("CommonWhite");
        //logger.Info(string.Format("id={0} value={1}", item.id, item.value));
    }

    @Override
    public void OnDispose() {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'OnDispose'");
    }
}
