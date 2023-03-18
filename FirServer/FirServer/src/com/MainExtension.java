package com;
import com.common.ManagementCenter;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class MainExtension extends SFSExtension {
    @Override
    public void init()
    {
        ManagementCenter.Initialize(this);
    }

    ///添加消息处理器
    public void AddMsgHandler(String name, Class<?> classType){
        this.addRequestHandler(name, classType);
    }

    ///移除消息处理器
    public void RemoveMsgHandler(String name) {
        this.removeRequestHandler(name);
    }
    
     @Override
    public void destroy()
    {
        // Always make sure to invoke the parent class first
        super.destroy();
    }
}
