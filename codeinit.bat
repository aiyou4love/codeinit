@echo off
if not exist %USERPROFILE%\.ssh (
	mkdir %USERPROFILE%\.ssh
)
copy /Y c:\unseen\id_rsa %USERPROFILE%\.ssh\id_rsa
cd ..
git clone git@github.com:aiyou4love/unseen.git
git clone git@github.com:aiyou4love/autopack.git
git clone git@github.com:aiyou4love/webui.git
git clone https://github.com/valnoel/rapidxml.git
git clone git://code.csdn.net/Tencent/rapidjson.git
git clone https://github.com/curl/curl.git
@echo on
