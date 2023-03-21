为了给C# 与 Java生成的proto有特定的包名等信息，参考下面的选项

```protobuf
syntax = "proto3";
package pb_user;

import "common.proto";

// [START java_declaration]
option java_multiple_files = true;
option java_package = "com.protos";
option java_outer_classname = "pbuser";
// [END java_declaration]

// [START csharp_declaration]
option csharp_namespace = "Network.pbuser";
// [END csharp_declaration]

message ReqLogin {
	string name = 1;
	string pass = 2;
}

message ResLogin {
	pb_common.ResultCode result = 1;
	pb_common.UserInfo userinfo = 2;
}
```