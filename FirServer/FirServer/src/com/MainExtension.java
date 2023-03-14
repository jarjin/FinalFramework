package com;
import com.manager.NetworkManager;
import com.smartfoxserver.v2.extensions.SFSExtension;

public class MainExtension extends SFSExtension {
    @Override
    public void init()
    {
        addRequestHandler("protoc", NetworkManager.class);
        trace("Hello, this is my first SFS2X Extension!");
    }
    
     @Override
    public void destroy()
    {
        // Always make sure to invoke the parent class first
        super.destroy();
        trace("Destroy is called!");
    }
}
