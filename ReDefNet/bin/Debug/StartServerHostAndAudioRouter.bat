rem @echo off

:: Restart the audio router and block II services
call "C:\Windows\System32\psexec.exe" \\192.168.17.100 -u "Administrator" -p "e6bwst" -i cmd /c "taskkill /f /im ReDefNet.Wst.Dvs.AudioRouter.exe"
call "C:\Windows\System32\psexec.exe" \\192.168.17.100 -u "Administrator" -p "e6bwst" -i cmd /c "taskkill /f /im ReDefNet.Wst.ServerHost.exe"
call "C:\Windows\System32\psexec.exe" \\192.168.17.100 -u "Administrator" -p "e6bwst" -i cmd /c "net start WstBlock2Service"
call "C:\Windows\System32\psexec.exe" \\192.168.17.100 -u "Administrator" -p "e6bwst" -i cmd /c "net start WstAudioRoutingService"

SLEEP 30

start "COMMPLAN EDITOR" /D "C:\E6-B_WST\MissionEquipment\BlockII" CommplanEditor.exe --config="CommplanEditor_IOS.config"