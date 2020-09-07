dotnet publish -o ../Publish
xcopy /y /e /i /q config Publish\config
xcopy /y /e /i /q Tables Publish\Tables