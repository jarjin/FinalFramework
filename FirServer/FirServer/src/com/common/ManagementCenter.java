package com.common;

import java.io.File;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

import org.apache.log4j.Logger;

import com.MainExtension;
import com.define.AppDefine;
import com.interfaces.IManager;
import com.manager.ConfigManager;
import com.manager.HandlerManager;
import com.manager.ModelManager;
import com.manager.NetworkManager;
import com.manager.TableManager;
import com.manager.WorldManager;

public class ManagementCenter {
    private static Logger logger = Logger.getLogger(ManagementCenter.class);
    private static MainExtension extension;
    private static Map<String, IManager> managers = new HashMap<String, IManager>();

    /// <summary>
    /// 初始化管理器
    /// </summary>
    public static void Initialize(MainExtension extension)
    {
        ManagementCenter.extension = extension;
        try {
            InitAppServerInfo();
            AddManager(TableManager.class);
            AddManager(ConfigManager.class);
            AddManager(NetworkManager.class);
            AddManager(HandlerManager.class);
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
        logger.info("Initialize Success!!! AppDefine.DataPath:" + AppDefine.DataPath);
    }

    public static MainExtension GetMainExtension() {
        return extension;
    }

    private static void InitAppServerInfo() throws IOException {
        File directory = new File(".");
        String rootPath = directory.getCanonicalPath().replace('\\', '/');
        AppDefine.DataPath = rootPath + "/" + extension.getCurrentFolder();
    }

    public static IManager AddManager(Class<?> class1) 
            throws InstantiationException, IllegalAccessException {
        IManager manager = (IManager)class1.newInstance();
        String typeName = manager.getClass().getName();
        if (!managers.containsKey(typeName)) {
            managers.put(typeName, (IManager)manager);
            logger.info("Add Manager:" + typeName + " " + manager);
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

    public static IManager GetManager(Class<?> class1) {
        return GetManager(class1.getName());
    }

    public static IManager GetManager(String key) {
        IManager manager = null;
        if (managers.containsKey(key))
        {
            manager = managers.get(key);
        }
        return manager;
    }

    public static void OnDispose() {
        for (IManager de : managers.values()) {
            if (de != null) {
                de.OnDispose();
            }
        }
    }
}
