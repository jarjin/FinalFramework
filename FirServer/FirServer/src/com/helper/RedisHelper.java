package com.helper;

import com.define.AppConst;

import redis.clients.jedis.Jedis;
import redis.clients.jedis.JedisPool;

//https://github.com/redis/jedis
public class RedisHelper {
    private JedisPool pool = null;
    private Jedis client = null;

    public void initRedis() {
        if (!AppConst.USE_REDIS_CACHE) return;
        pool = new JedisPool(AppConst.REDIS_IP, AppConst.REDIS_PORT);
        client = pool.getResource();
    }

    public Object get(String tbname, String uid, String key) {
        return AppConst.USE_REDIS_CACHE ? getInner(tbname, uid, key) : null;
    }

    private Object getInner(String tbname, String uid, String key) {
        String strKey = tbname + "_" + uid + "_" + key;
        return client.get(strKey);
    }

    public void set(String tbname, String uid, String key, Object value) {
        if (!AppConst.USE_REDIS_CACHE) return;
        setInner(tbname, uid, key, value);
    }

    private void setInner(String tbname, String uid, String key, Object value) {
		String strKey = tbname + "_" + uid + "_" + key;
        client.set(strKey, value.toString());
    }

    public void remove(String tbname, String uid, String key) {
        String strKey = tbname + "_" + uid + "_" + key;
        client.del(strKey);
    }

    public void closeRedis() {
        if (!AppConst.USE_REDIS_CACHE) return;
        client.close();
        pool.close();
    }
}
