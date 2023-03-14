@echo on

set SFS_Path=D:\SmartFoxServer_2X\SFS2X\extensions
xcopy /y /e /i /q FirServer\bin\*.* %SFS_Path%
xcopy /y /e /i /q config %SFS_Path%\config
xcopy /y /e /i /q Tables %SFS_Path%\Tables

@pause