@echo on

set SFS_Path=D:\SmartFoxServer_2X\SFS2X\extensions
set EXT_Path=%SFS_Path%\MainExtension

if not exist %SFS_Path% (
	mkdir %SFS_Path%
)

xcopy /y /e /i /q config %EXT_Path%\config
xcopy /y /e /i /q Tables %EXT_Path%\Tables

copy .\FirServer\lib\*.* %SFS_Path%\__lib__

jar -cvf %EXT_Path%\MainExtension.jar -C .\FirServer\bin\ .

@pause