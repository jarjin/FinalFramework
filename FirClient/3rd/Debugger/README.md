# Debugger

just a log system for tolua#. (detail in chinese)

一个logger系统，并且包含部分扩展函数。

可以自定义接口形式运行时替换unity Debug.Log。 减少不必要的堆栈获取行为。

为什么不放在unity工程内部

通过封装Debugger.Log 到dll可以在点击log时，避免错误跳到Debugger.Log内部情况

当lua堆栈离开游戏进程时，对于unity log 从一定程度能避免卡死。（具体见tolua# wiki）
