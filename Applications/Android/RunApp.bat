@echo off
echo Iniciar instalacao (pressione qualquer tecla)...
pause
goto Instalar
:Retorno
set /p _continue=Deseja instalar novamente (y/n)?
echo Você escolheu %_continue%
if !_continue! == y (goto Instalar) else (goto End)
:Instalar
echo Iniciando instalacao
cd C:\Users\lksfx\android-sdks\platform-tools\
adb connect 192.168.0.100
adb uninstall com.lksfx.virusbyte
adb install C:\Users\lksfx\OneDrive\Unity\VirusByte\Applications\Android\VirusByte.apk
adb shell am start -n com.lksfx.virusbyte/com.unity3d.player.UnityPlayerActivity
echo Instacao concluída. (pressione qualquer tecla)...
pause
goto Retorno
:End