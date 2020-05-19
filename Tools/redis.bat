@echo off

set redispath="D:/Redis-x64-3.2.100/"

%redispath%redis-server.exe %redispath%redis.windows.conf
