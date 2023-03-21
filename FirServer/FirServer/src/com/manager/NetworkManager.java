package com.manager;

import com.MainExtension;
import com.common.MgrCenter;
import com.define.AppConst;
import com.google.protobuf.GeneratedMessageV3;
import com.network.ClientRequest;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.tables.enums.ProtoType;

/**
 * @author Administrator
 *
 */
public class NetworkManager extends BaseManager 
{
    private MainExtension mainExt;

	public NetworkManager() {
        mainExt = MgrCenter.GetMainExtension();
    }

    @Override   
    public void Initialize() {
        mainExt.AddMsgHandler(AppConst.ExtCmdName, ClientRequest.class);
    }

    public void SendData(User user, ProtoType type, String protoName, GeneratedMessageV3 messageV3) {
        Integer msgType = type.getValue();
        ISFSObject reply = new SFSObject();
        reply.putInt(AppConst.MsgTypeKey, msgType);
        reply.putUtfString(AppConst.ProtoNameKey, protoName);
        reply.putByteArray(AppConst.ByteArrayKey, messageV3.toByteArray());
        mainExt.send(AppConst.ExtCmdName, reply, user);
    }
	
    @Override
    public void OnDispose() {
        mainExt.RemoveMsgHandler(AppConst.ExtCmdName);
    }
}