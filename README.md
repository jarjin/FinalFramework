# FinalFramework

#### 开发目标
FinalFramework（简称FF）不是为了初学者入门，所以初学者会不适应它的风格，FF的目标是打造独立游戏开发的闭环，从客户端、服务器端+DB、各种独立工具链（剧情编辑器、打表、ProtoBuff等）、WebServer（HTTP埋点，管理后台，通过后台管理Jenkins打包版本）、多端定义共享功能来方便开发者。

![image](https://github.com/jarjin/FinalFramework/raw/master/screenshot.jpg)      

#### 运行环境（版本太低的需要自力更生了）
FirClient： Unity 2019.4.20f1 (64-bit) + Visual Studio 2019  
FirServer:  .Net Core 3.1 + Visual Studio 2019 + MongoDB 4.29  
FirToolkit: Visual Studio 2019 

#### 框架工作流使用介绍：	
（1）proto添加消息，定义req、res结构，打协议（自动copy到客户端、服务器端pb目录）  
（2）在FirCommon添加Protocal协议，Build完成工程自动copy到客户端、服务端。  
（3）在策划Excel目录定义数据表，然后打表（自动copy到客户端+服务器Table目录）  
（4）在FirServer里添加对于模块Handler（消息），Model（数据库），Manager（管理器）  
（5）在前端lua添加MsgHandler，Module，Manager，就可以接入View逻辑了。  

#### 2021.02.23 更新日志：
（1）开源多端共享工程FirCommon，公共定义都放此工程。

#### 2021.02.20 更新日志：
（1）升级打表工具TableTool V2。
（2）升级剧情编辑器StoryEditor V2。 

#### 2021.02.11 更新日志：
（1）梳理了前后端通讯流程，客户端工程设置里面有“NetworkMode”,默认关闭。 

#### 框架特征：
（0）采用tolua53分支代码(lua5.3.5版本) + pbc3.0 + sproto最新版。    
（1）逻辑层、视图层代码分离，中间使用消息组件通信消息，可以完全隔离。  
（2）仿照UE4+Unlua的蓝图式组件访问方式，彻底抛弃了Lua View层导出代码，代码简洁性能更好。  
（3）Lua层代码OOP架构方式，完全的单向访问（Lua->C#）避免交叉访问带来的弊端。  
（4）基于DLL插件式的配套服务器端框架，游戏只需要封装到一个程序集，完全不入侵的服务器端框架。  
（5）完善的Excel打表工作流工具，你可以直接根据打表规则，生成客户端表结构、服务端表结构。  
（6）完善的Protobuf生成工作流工具，你可以生成客户端（C#+LuaPB），服务器端C#。  
（7）一个可以用于开发独立游戏的独立于Unity3D的剧情编辑器。  
（8）Lua层的自定义组件库，你可以完全不需要修改C#的情况下，新增或者修改自定义组件。  
（9）可视化的资源打包管理系统。  
（10）可视化资源导入管线设置。  
（11）场景事件管理系统，整个框架的战斗完全是基于场景事件驱动。  
（12）一个简单版本的Patch系统。  
（13）红点系统（实现一个红点通用框架）。  作者：DustYang

#### 已知问题
（1）英雄战斗状态机会停下。（已修复）npcData.skillConsume决定是否要释放技能。

#### 待加功能
（1）UI骨骼系统（解决UI频繁创建销毁造成Mono内存增长）。  

#### 最后闲话：
这也是我为大家分享最后一个框架，很感谢大家这么多年的陪伴，我也将自己觉得拿的出手的经验教训完全整合到这个框架，以不定期更新的方式迭代，为更多的独立游戏开发者节省开发时间，让独立游戏开发更加现实。框架初期不保证完美运行，有问题加我微信（jarjin）,进微信技术群讨论。如果你用过或者知道LuaFramework系列，请给这个点一颗Star，不尽感激。^o^

