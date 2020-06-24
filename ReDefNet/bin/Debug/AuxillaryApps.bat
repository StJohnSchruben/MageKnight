ECHO OFF

if not DEFINED IS_MINIMIZED set IS_MINIMIZED=1 && start "" /min "%~dpnx0" %* && exit

START C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Txpa.Standalone.exe --config="C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Txpa.Standalone_IOS.config"
START C:\E6-B_WST\MissionEquipment\BlockII\CommplanEditor.exe --config="C:\E6-B_WST\MissionEquipment\BlockII\CommplanEditor_IOS.config"
START C:\E6-B_WST\Bin\IOS\DssVideoController\DssVideoController.exe --config="C:\E6-B_WST\Bin\IOS\DssVideoController\DssVideoController.exe.config"
#START C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.DirectLite.exe --config="C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.DirectLite.exe.config"
START C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Oat.IosAdapter.exe --config="C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Oat.IosAdapter_IOS.config"

EXIT