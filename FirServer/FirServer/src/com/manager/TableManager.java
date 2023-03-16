package com.manager;

import java.nio.file.Files;
import java.nio.file.Paths;
import java.io.IOException;
import java.nio.charset.StandardCharsets;

import com.define.AppDefine;
import com.google.gson.Gson;
import com.tables.Tables.*;

public class TableManager extends BaseManager {
    public ItemTable itemTable;
    public NpcTable npcTable;
    public ObjectPoolTable objectPoolTable;
    public QualityTable qualityTable;
    public GlobalConfigTable globalConfigTable;
///[APPEND_VAR]


    @Override
    public void Initialize() {
        this.LoadTables();
    }

    public void LoadTables() {
        itemTable = LoadData("Tables/ItemTable.bytes", ItemTable.class);
        itemTable.Initialize();
        npcTable = LoadData("Tables/NpcTable.bytes", NpcTable.class);
        npcTable.Initialize();
        objectPoolTable = LoadData("Tables/ObjectPoolTable.bytes", ObjectPoolTable.class);
        objectPoolTable.Initialize();
        qualityTable = LoadData("Tables/QualityTable.bytes", QualityTable.class);
        qualityTable.Initialize();
        globalConfigTable = LoadData("Tables/GlobalConfigTable.bytes", GlobalConfigTable.class);
        globalConfigTable.Initialize();
///[APPEND_TABLE]
    }
	
    public static <T> T LoadData(String path, Class<T> type) {
        String finalPath = AppDefine.DataPath + path;
        String content = null;
        try {
            byte[] bytes = Files.readAllBytes(Paths.get(finalPath));
            content = new String(bytes, StandardCharsets.UTF_8);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return new Gson().fromJson(content, type);
    }

    @Override
    public void OnUpdate(float deltaTime) {
    }

    @Override
    public void OnDispose() {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'OnDispose'");
    }
}
