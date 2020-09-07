@echo off

set clientUrl="svn://39.98.224.112/sango"
set clientPath="E:\Workspaces\firsango"

set user="jarjin"
set passwd="510521fls"

echo Update Project...
cd %clientPath%
if exist %clientPath% (
	svn up %clientPath% --username %user% --password %passwd% --force
) else (
	svn checkout %clientUrl% %clientPath% --username %user% --password %passwd% --force
)

echo Commit Update...
svn commit -m "update res && code" %clientPath% --username %user% --password %passwd%

echo Update Commit OK!!!
pause    