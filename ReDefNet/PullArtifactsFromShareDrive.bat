echo off

robocopy "\\ReDefNet-ag-fs1\Public\_WSTBlockIIBuildServerIntegrationArtifacts\MAPS" "%BUILD_SOURCESDIRECTORY%\BlockII" /v /s /r:3 /w:5 /purge /np
robocopy "\\ReDefNet-ag-fs1\Public\_WSTBlockIIBuildServerIntegrationArtifacts\CML" "%BUILD_SOURCESDIRECTORY%\BlockII" /v /s /r:3 /w:5 /np
robocopy "\\ReDefNet-ag-fs1\Public\_WSTBlockIIBuildServerIntegrationArtifacts\TXPA" "%BUILD_SOURCESDIRECTORY%\BlockII" /v /s /r:3 /w:5 /np
robocopy "\\ReDefNet-ag-fs1\Public\_WSTBlockIIBuildServerIntegrationArtifacts\FABT" "%BUILD_SOURCESDIRECTORY%\BlockII" /v /s /r:3 /w:5 /np
robocopy "\\ReDefNet-ag-fs1\Public\_WSTBlockIIBuildServerIntegrationArtifacts\MRTCDL" "%BUILD_SOURCESDIRECTORY%\BlockII" /v /s /r:3 /w:5 /np
robocopy "\\ReDefNet-ag-fs1\Public\_WSTBlockIIBuildServerIntegrationArtifacts\UPSEmergencyOff" "%BUILD_SOURCESDIRECTORY%\BlockII" /v /s /r:3 /w:5 /np

:: Workaround due to the non-standard return codes used by robocopy
if %errorlevel% GEQ 7 (
    exit /B 1
) else (
    exit /B 0
)