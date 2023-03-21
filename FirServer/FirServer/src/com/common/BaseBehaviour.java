package com.common;

import com.manager.*;

public class BaseBehaviour {
    private static ConfigManager _configMgr = null;
    protected static ConfigManager configMgr()
    {
        if (_configMgr == null)
        {
            _configMgr = (ConfigManager)MgrCenter.GetManager(ConfigManager.class);
        }
        return _configMgr;
    }

    private static NetworkManager _netMgr = null;
    protected static NetworkManager netMgr()
    {
        if (_netMgr == null)
        {
            _netMgr = (NetworkManager)MgrCenter.GetManager(NetworkManager.class);
        }
        return _netMgr;
    }

    private static HandlerManager _handlerMgr = null;
    protected static HandlerManager handlerMgr()
    {
        if (_handlerMgr == null)
        {
            _handlerMgr = (HandlerManager)MgrCenter.GetManager(HandlerManager.class);
        }
        return _handlerMgr;
    }

    private static TableManager _tableMgr = null;
    protected static TableManager tableMgr()
    {
        if (_tableMgr == null)
        {
            _tableMgr = (TableManager)MgrCenter.GetManager(TableManager.class);
        }
        return _tableMgr;
    }

    private static ModelManager _modelMgr = null;
    protected static ModelManager modelMgr()
    {
        if (_modelMgr == null)
        {
            _modelMgr = (ModelManager)MgrCenter.GetManager(ModelManager.class);
        }
        return _modelMgr;
    }

    private static LogManager _logMgr = null;
    protected static LogManager logMgr()
    {
        if (_logMgr == null)
        {
            _logMgr = (LogManager)MgrCenter.GetManager(LogManager.class);
        }
        return _logMgr;
    }

    private static DataManager _dataMgr = null;
    protected static DataManager dataMgr()
    {
        if (_dataMgr == null)
        {
            _dataMgr = (DataManager)MgrCenter.GetManager(DataManager.class);
        }
        return _dataMgr;
    }
}
