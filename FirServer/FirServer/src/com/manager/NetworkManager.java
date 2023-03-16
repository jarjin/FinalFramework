package com.manager;

import com.MainExtension;
import com.common.ManagementCenter;
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
        mainExt = ManagementCenter.extension;
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
    @Override
    public void handleClientRequest(User user, ISFSObject inObj) 
    {
        trace("Welcome new user: " + user.getName());
        byte[] bytes = inObj.getByteArray("protobuf");
        
        try {
            Person person = Person.parseFrom(bytes);
            System.out.println("Person Count: " + person.getName());
        } catch (InvalidProtocolBufferException e) {
            e.printStackTrace();
        }
    }
}