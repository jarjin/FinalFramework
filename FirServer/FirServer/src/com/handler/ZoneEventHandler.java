/**
 * 
 */
package com.handler;

import com.example.tutorial.protos.Person;
import com.google.protobuf.InvalidProtocolBufferException;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

/**
 * @author Administrator
 *
 */
public class ZoneEventHandler extends BaseClientRequestHandler
{
	public ZoneEventHandler() {}
	
    @Override
    public void handleClientRequest(User user, ISFSObject inObj) 
    {
        trace("Welcome new user: " + user.getName());
        byte[] bytes = inObj.getByteArray("protobuf");
        
        try {
        	Person person = Person.parseFrom(bytes);
			System.out.println("Person Count: " + person.getName());
		} catch (InvalidProtocolBufferException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    }
}