echo off

:: Prepare the final block II drop by creating the final directory structure

robocopy "%BUILD_SOURCESDIRECTORY%" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\Bin\Common" "DeployBlockII.bat" /r:3 /w:5 /v /np
robocopy "%BUILD_SOURCESDIRECTORY%" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\Bin\Common" "PullBlockIIChanges.bat" /r:3 /w:5 /v /np

robocopy "%BUILD_SOURCESDIRECTORY%" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\DataFiles" "e6b_startup_configuration.ini" /r:3 /w:5 /v /np
robocopy "%BUILD_SOURCESDIRECTORY%" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\DataFiles\Student" "e6b_station_configuration.ini" /r:3 /w:5 /v /np

robocopy "%BUILD_SOURCESDIRECTORY%" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\MissionEquipment" "WindowScrollDownArrowDeviceDLL.dll" /r:3 /w:5 /v /np
robocopy "%BUILD_SOURCESDIRECTORY%" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\MissionEquipment" "WindowScrollUpArrowDeviceDLL.dll" /r:3 /w:5 /v /np

::robocopy "%BUILD_SOURCESDIRECTORY%" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\MissionEquipment\BlockII" "StartCommplanEditor.bat" /r:3 /w:5 /v /np
robocopy "%BUILD_SOURCESDIRECTORY%" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\MissionEquipment\BlockII" "StartServerHostAndAudioRouter.bat" /r:3 /w:5 /v /np
robocopy "%BUILD_SOURCESDIRECTORY%" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\MissionEquipment\BlockII" "IOS_TXPA.bat" /r:3 /w:5 /v /np

robocopy "%BUILD_SOURCESDIRECTORY%\BlockII" "%BUILD_ARTIFACTSTAGINGDIRECTORY%\All Artifacts\E-6B_WST\MissionEquipment\BlockII" /e /r:3 /w:5 /v /np

:: Workaround due to the non-standard return codes used by robocopy
if %errorlevel% GEQ 7 (
    exit /B 1
) else (
    exit /B 0
)