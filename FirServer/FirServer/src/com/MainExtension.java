package com;
import com.common.MgrCenter;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class MainExtension extends SFSExtension {
    @Override
    public void init() {
        MgrCenter.Initialize(this);
    }

    ///添加消息处理器
    public void AddMsgHandler(String name, Class<?> classType) {
        this.addRequestHandler(name, classType);
    }

    ///移除消息处理器
    public void RemoveMsgHandler(String name) {
        this.removeRequestHandler(name);
    }
    
    @Override
    public void destroy() {
        super.destroy();
    }
}
