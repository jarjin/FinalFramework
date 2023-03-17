package com.manager;

import com.MainExtension;
import com.common.ManagementCenter;
import com.define.AppConst;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

/**
 * @author Administrator
 *
 */
public class NetworkManager extends BaseManager 
{
    private MainExtension mainExt;

	public NetworkManager() {
        mainExt = ManagementCenter.GetMainExtension();
    }

    @Override   
    public void Initialize() {
        mainExt.AddMsgHandler(AppConst.ExtCmdName, ClientRequest.class);
    }

    public void SendData(User user, ISFSObject params) {
        mainExt.send(AppConst.ExtCmdName, params, user);
    }
	
    @Override
    public void OnDispose() {
        mainExt.RemoveMsgHandler(AppConst.ExtCmdName);
    }
}

class ClientRequest extends BaseClientRequestHandler {
    private LogManager logMgr;
    private HandlerManager handlerMgr;

    public ClientRequest() {
        logMgr = (LogManager)ManagementCenter.GetManager(LogManager.class);
        handlerMgr = (HandlerManager)ManagementCenter.GetManager(HandlerManager.class);
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