set Client_Proto_Path=.\FirClient\Assets\Scripts\Network\ProtoCS
set Server_Proto_Path=.\FirServer\FirServer\src\

if not exist %Client_Proto_Path% (
	mkdir %Client_Proto_Path%
)
if not exist %Server_Proto_Path% (
	mkdir %Server_Proto_Path% 
)

.\Tools\protoc --proto_path=.\Protos --java_out=%Server_Proto_Path% .\Protos\*.proto
.\Tools\protoc --proto_path=.\Protos --csharp_out=%Client_Proto_Path% .\Protos\*.proto