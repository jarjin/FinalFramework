@echo on

set SFS_Path=D:\SmartFoxServer_2X\SFS2X\extensions\MainExtension

if not exist %SFS_Path% (
	mkdir %SFS_Path%
)

xcopy /y /e /i /q config %SFS_Path%\config
xcopy /y /e /i /q Tables %SFS_Path%\Tables

jar -cvf %SFS_Path%\MainExtension.jar -C .\FirServer\bin\ .

@pause