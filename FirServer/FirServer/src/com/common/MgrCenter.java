package com.common;

import java.io.File;
import java.io.IOException;
import java.util.LinkedHashMap;
import java.util.Map;

import com.MainExtension;
import com.define.AppDefine;
import com.interfaces.IManager;
import com.manager.*;

public class MgrCenter {
    private static LogManager logMgr;
    private static MainExtension extension;
    private static Map<String, IManager> managers = new LinkedHashMap<String, IManager>();

    /// <summary>
    /// 初始化管理器
    /// </summary>
    public static void Initialize(MainExtension extension)
    {
        MgrCenter.extension = extension;
        try {
            InitAppServerInfo();
            AddManager(LogManager.class);
            AddManager(TableManager.class);
            AddManager(ConfigManager.class);
            AddManager(DataManager.class);
            AddManager(HandlerManager.class);
            AddManager(NetworkManager.class);
            AddManager(ModelManager.class);
            AddManager(WorldManager.class);
        } catch (InstantiationException | IllegalAccessException | IOException e) {
            e.printStackTrace();
        }
        for (IManager de : managers.values()) {
            if (de != null) {
                de.Initialize();
            }
        }
        logMgr = (LogManager)GetManager(LogManager.class);
        logMgr.Trace("Initialize Success!!! AppDefine.DataPath:" + AppDefine.DataPath);
    }

    public static MainExtension GetMainExtension() {
        return extension;
    }

    private static void InitAppServerInfo() throws IOException {
        File directory = new File(".");
        String rootPath = directory.getCanonicalPath().replace('\\', '/');
        AppDefine.DataPath = rootPath + "/" + extension.getCurrentFolder();
    }

    public static IManager AddManager(Class<?> classType) 
            throws InstantiationException, IllegalAccessException {
        IManager manager = (IManager)classType.newInstance();
        String typeName = manager.getClass().getName();
        if (!managers.containsKey(typeName)) {
            managers.put(typeName, (IManager)manager);
        }
        return manager;
    }

    public static void AddWithInitManager(Class<?> class1) {
        try {
            IManager manager = AddManager(class1);
            if (manager != null) {
                manager.Initialize();
            }
        } catch (InstantiationException | IllegalAccessException e) {
            e.printStackTrace();
        }
    }

    public static IManager GetManager(Class<?> classType) {
        return GetManager(classType.getName());
    }

    public static IManager GetManager(String key) {
        IManager manager = null;
        if (managers.containsKey(key))
        {
            manager = managers.get(key);
        }
        return manager;
    }

    public static void RemoveManager(String key) {
        if (managers.containsKey(key)) {
            managers.remove(key);
        }
    }

    public static void OnDispose() {
        for (IManager de : managers.values()) {
            if (de != null) {
                de.OnDispose();
            }
        }
    }
}
