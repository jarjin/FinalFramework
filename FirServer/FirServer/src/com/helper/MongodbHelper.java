package com.helper;

import org.bson.Document;

import com.define.AppConst;
import com.mongodb.MongoClient;
import com.mongodb.client.MongoClients;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoDatabase;

public class MongoDBHelper {
    private static MongoClient client = null;

    public void initMongoDB() {
        String url = String.format("mongodb://%s:%d", 
            AppConst.MONGO_IP, AppConst.MONGO_PORT);
        client = (MongoClient) MongoClients.create(url);
    }

    public MongoClient getClient() {
		return client;
	}

    public MongoDatabase getDatabase(String dbName) {
        return client.getDatabase(dbName);
    }

    public MongoCollection<Document> getAll(String dbName, String collName) {
        MongoDatabase db = getDatabase(dbName);
        return db.getCollection(collName);
    }

    public Object get(String dbname, String uid, String key) {
        //String sql = "select " + key + " from " + dbname + " where userid='" + uid + "'";
		return null;
	}
	
	public void set(String tbname, String uid, String key, String value) {
		//String strKey = tbname + "_" + uid + "_" + key;
		//Memcache.delete(strKey);	//
		//executeUpdate("update " + tbname + " set " + key + "=" + value + " where userid='" + uid + "'");
	}

    public void CloseMongoDB() {
        client.close();
    }
}
