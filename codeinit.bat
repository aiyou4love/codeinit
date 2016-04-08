@echo off
if not exist %USERPROFILE%\.ssh (
	mkdir %USERPROFILE%\.ssh
)
copy /Y c:\unseen\id_rsa %USERPROFILE%\.ssh\id_rsa
cd ..
@echo on
