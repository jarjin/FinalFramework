package com.manager;

import com.MainExtension;
import com.common.ManagementCenter;
import com.define.AppConst;
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
        mainExt = ManagementCenter.GetMainExtension();
    }

    @Override   
    public void Initialize() {
        mainExt.AddMsgHandler(AppConst.ExtCmdName, ClientRequest.class);
    }

    public void SendData(User user, ProtoType type, byte[] params) {
        ISFSObject reply = new SFSObject();
        reply.putInt(AppConst.MsgTypeKey, type.getValue());
        reply.putByteArray(AppConst.ByteArrayKey, params);
        mainExt.send(AppConst.ExtCmdName, reply, user);
    }
	
    @Override
    public void OnDispose() {
        mainExt.RemoveMsgHandler(AppConst.ExtCmdName);
    }
}