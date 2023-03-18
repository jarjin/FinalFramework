package com.helper;

import org.bson.Document;
import org.bson.conversions.Bson;

import com.define.AppConst;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoClients;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoCursor;
import com.mongodb.client.MongoDatabase;
import com.mongodb.client.model.Filters;
import com.mongodb.client.model.UpdateOptions;
import com.mongodb.client.model.Updates;
import com.mongodb.client.result.UpdateResult;

public class MongoDBHelper {
    private static MongoClient client = null;
    private static MongoDatabase database = null;

    public void initMongoDB() {
        String url = String.format("mongodb://%s:%d", 
            AppConst.MONGO_IP, AppConst.MONGO_PORT);
        client = MongoClients.create(url);
        database = client.getDatabase(AppConst.MONGO_DBNAME);
    }

    public MongoClient getClient() {
		return client;
	}

    public MongoCollection<Document> getCollection(String collName) {
        return database.getCollection(collName);
    }

    //String sql = "select " + key + " from " + tbname + " where userid='" + uid + "'";
    public Object get(String tbname, String uid, String key) {
        try (MongoCursor<Document> cursor = getCollection(tbname).find().filter(Filters.eq("userid", uid)).iterator()) {
            while (cursor.hasNext()) { 
                return cursor.next().get(key);
            }
        }
        return null;
	}
		
    //String strKey = tbname + "_" + uid + "_" + key;
	//executeUpdate("update " + tbname + " set " + key + "=" + value + " where userid='" + uid + "'");
	public long set(String tbname, String uid, String key, Object value) {
        Bson updates = Updates.set(key, value);
        UpdateOptions options = new UpdateOptions().upsert(true);
        UpdateResult result = getCollection(tbname).updateOne(Filters.eq("userid", uid), updates, options);
        return result.getModifiedCount();
	}

    public void CloseMongoDB() {
        client.close();
    }
}
