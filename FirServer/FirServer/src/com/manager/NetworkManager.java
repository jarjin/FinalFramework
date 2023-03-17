package com.manager;

import com.MainExtension;
import com.common.ManagementCenter;
import com.define.AppConst;
import com.google.protobuf.InvalidProtocolBufferException;
import com.protos.Person;
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
        mainExt.AddMsgHandler("protoc", ClientRequest.class);
    }
	
    @Override
    public void OnDispose() {
        mainExt.RemoveMsgHandler("protoc");
    }
}

class ClientRequest extends BaseClientRequestHandler {
    private HandlerManager handlerMgr;

    public ClientRequest() {
        handlerMgr = (HandlerManager)ManagementCenter.GetManager(HandlerManager.class);
    }

    @Override
    public void handleClientRequest(User user, ISFSObject inObj) 
    {
        String protoName = inObj.getUtfString(AppConst.ProtoNameKey);
        byte[] bytes = inObj.getByteArray(AppConst.ByteArrayKey);
        if (protoName != null && bytes != null) {
            handlerMgr.OnRecvMessage(user, protoName, bytes);
        }
    }
}