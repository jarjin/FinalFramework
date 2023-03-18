package com.helper;

import com.define.AppConst;

public class RedisHelper {
    public void initRedis() {
        if (!AppConst.USE_REDIS) return;
    }

    public Object get(String dbname, String uid, String key) {
        return AppConst.USE_REDIS ? getInner(dbname, uid, key) : null;
    }

    private Object getInner(String dbname, String uid, String key) {
        return null;
    }

    public void set(String tbname, String uid, String key, Object value) {
        if (!AppConst.USE_REDIS) return;
        setInner(tbname, uid, key, value);
    }

    private void setInner(String tbname, String uid, String key, Object value) {
    }

    public void closeRedis() {
        if (!AppConst.USE_REDIS) return;
    }
}
