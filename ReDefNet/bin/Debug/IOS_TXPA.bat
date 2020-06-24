ECHO OFF

if not DEFINED IS_MINIMIZED set IS_MINIMIZED=1 && start "" /min "%~dpnx0" %* && exit

SLEEP 30

START C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Txpa.Standalone.exe --config="C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Txpa.Standalone_IOS.config"

EXIT