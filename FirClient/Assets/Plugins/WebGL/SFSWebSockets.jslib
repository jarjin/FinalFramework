var LibraryWebSockets = {
	$webSocketInstances: [],

	SocketCreate: function(url)
	{
		var urlString = Pointer_stringify(url);
		var socket = {
			socket: new WebSocket(urlString),
			buffer: new Uint8Array(0),
			error: null,
			messages: [],
			encoding: 'utf-8'
		}
		socket.socket.onmessage = function (e) {
			
			if (typeof e.data == 'string')
			{
				// SFS2X websocket communication is based on strings
				// We have to encode it to ArrayBuffer
				var encodedString;
				
				if ('TextEncoder' in window)
				{
					var encoder = new TextEncoder(socket.encoding);
					encodedString = encoder.encode(e.data);
				}
				else
				{
					var utf8 = unescape(encodeURIComponent(e.data));
					encodedString = [];
					for (var i = 0; i < utf8.length; i++) {
					    encodedString.push(utf8.charCodeAt(i));
					}
				}
				
				socket.messages.push(encodedString);
			}
			else if (typeof e.data == 'object')
			{
				if (e.data instanceof Blob)
				{
					var reader = new FileReader();
					reader.addEventListener("loadend", function() {
						var array = new Uint8Array(reader.result);
						socket.messages.push(array);
					});
					reader.readAsArrayBuffer(e.data);
				}
			}
			
			// Handle other data types?
		};
		socket.socket.onclose = function (e) {
			if (e.reason != null && e.reason.length > 0)
				socket.error = e.reason;
			else
			{
				switch (e.code)
				{
					case 1000: 
						socket.error = "Disconnected.";
						break;
					case 1001: 
						socket.error = "Endpoint going away.";
						break;
					case 1002: 
						socket.error = "Protocol error.";
						break;
					case 1003: 
						socket.error = "Unsupported message.";
						break;
					case 1005: 
						socket.error = "No status.";
						break;
					case 1006: 
						socket.error = "Abnormal disconnection.";
						break;
					case 1009: 
						socket.error = "Data frame too large.";
						break;
					default:
						socket.error = "Error " + e.code;
				}
			}
		}
		var instance = webSocketInstances.push(socket) - 1;
		return instance;
	},

	SocketState: function (socketInstance)
	{
		var socket = webSocketInstances[socketInstance];
		if (socket != null)
			return socket.socket.readyState;
		else
			return 0;
	},

	SocketError: function (socketInstance)
	{
		var socket = webSocketInstances[socketInstance];
		var returnStr = "";
		
	 	if (socket.error != null)
			returnStr = socket.error;
		
		var buffer = _malloc(returnStr.length + 1);
		//writeStringToMemory(returnStr, buffer); // DEPRECATED
		stringToUTF8(returnStr, buffer, returnStr.length + 1);
		return buffer;
	},

	SocketSend: function (socketInstance, array, size, asString)
	{
		var socket = webSocketInstances[socketInstance];
		var arrBuff = HEAPU8.buffer.slice(array, array + size);
		
		if (asString)
		{
			// SFS2X websocket communication is based on strings
			// We have to decode the ArrayBuffer
			var decodedString;
			
			if ('TextDecoder' in window)
			{
				var dataView = new DataView(arrBuff);
				var decoder = new TextDecoder(socket.encoding);
		        decodedString = decoder.decode(dataView);
			}
			else
			{
				var uint8array = new Uint8Array(arrBuff);
				var encodedString = String.fromCharCode.apply(null, uint8array),
				decodedString = decodeURIComponent(escape(encodedString));
			}
			
			socket.socket.send(decodedString);
		}
		else
			socket.socket.send(arrBuff);
	},

	SocketRecvLength: function(socketInstance)
	{
		var socket = webSocketInstances[socketInstance];
		if (socket.messages.length == 0)
			return 0;
		return socket.messages[0].length;
	},

	SocketRecv: function (socketInstance, array, size)
	{
		var socket = webSocketInstances[socketInstance];
		if (socket.messages.length == 0)
			return 0;
		if (socket.messages[0].length > size)
			return 0;
		HEAPU8.set(socket.messages[0], array);
		socket.messages = socket.messages.slice(1);
	},

	SocketClose: function (socketInstance)
	{
		var socket = webSocketInstances[socketInstance];
		socket.socket.close();
	}
};

autoAddDeps(LibraryWebSockets, '$webSocketInstances');
mergeInto(LibraryManager.library, LibraryWebSockets);
