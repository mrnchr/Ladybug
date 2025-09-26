@echo off
set appId=%~1
set buildFolder=%~2
if "%buildFolder%"=="" set buildFolder=Ladybug

start npx @yandex-games/sdk-dev-proxy -p ".artifacts/Web/%buildFolder%" -i="%appId%" -с
start cloudpub
start https://cloudpub.cloudpub.ru