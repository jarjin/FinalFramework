package com.network;

import com.common.MgrCenter;
import com.define.AppConst;
import com.smartfoxserver.v2.entities.User;
import com.manager.HandlerManager;
import com.manager.LogManager;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class ClientRequest extends BaseClientRequestHandler {
    private LogManager logMgr;
    private HandlerManager handlerMgr;

    public ClientRequest() {
        logMgr = (LogManager)MgrCenter.GetManager(LogManager.class);
        handlerMgr = (HandlerManager)MgrCenter.GetManager(HandlerManager.class);
    }

    @Override
    public void handleClientRequest(User user, ISFSObject inObj) {
        String protoName = inObj.getUtfString(AppConst.ProtoNameKey);
        byte[] bytes = inObj.getByteArray(AppConst.ByteArrayKey);

        logMgr.Trace(protoName, bytes);
        if (protoName != null && bytes != null) {
            handlerMgr.OnRecvMessage(user, protoName, bytes);
        }
    }
}
