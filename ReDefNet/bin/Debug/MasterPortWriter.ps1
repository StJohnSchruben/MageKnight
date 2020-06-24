# Script parameters
[CmdletBinding()]
param([string]$SubNet, 
[string]$ICSetPath='')


# Windows credentials for remotely connecting to WST positions
$WstUsername = 'Administrator'
$WstPassword = 'e6bwst'
# IP address of the file server

$FileServer = '192.168.'+$SubNet+'.100'

$NetWork = '192.168.' + $SubNet

# Location of the Sysinternals psexec.exe utility
$PsExecPath = 'C:\Windows\System32\psexec.exe'

# =============================================================================
# This is what calls the write-port scripts on the clients.
# =============================================================================

function WritePorts
{
    foreach ($Position in $Positions)
    {
        $PsExecArgs = "\\$Position -u $WstUsername -p $WstPassword -w `"C:\E6-B_WST\MissionEquipment\BlockII`" -h -i cmd /c `"C:\E6-B_WST\MissionEquipment\BlockII\$WritePortFileName.bat`""
		
        Write-Host "\\$Position -u $WstUsername -p $WstPassword -w `"C:\E6-B_WST\MissionEquipment\BlockII`" -h -i cmd  /c `"C:\E6-B_WST\MissionEquipment\BlockII\$WritePortFileName.bat`""

        $Process = Start-Process -PSPath $PsExecPath `
            -ArgumentList $PsExecArgs
    }
}

# ===============================================================================================
# Starts audio router, block II services, and writes the path to the selected ICSet for snapshot.
# ===============================================================================================
function Start-Services
{
   $FilePath = "\\"+$FileServer+"\C$\E6-B_WST\MissionEquipment\BlockII\SelectedICSetPath.txt"
   out-file -filepath "$FilePath" -inputobject "$ICSetPath"

   $PsExecArgs = "\\$FileServer -u $WstUsername -p $WstPassword -h -i cmd /c `"net start WstBlock2Service$SubNet`""
   
   Write-Host "Executing: psexec $PsExecArgs"
   #Read-Host "About to start the service"
   $Process = Start-Process -Wait `
   	-PassThru `
   	-PSPath $PsExecPath `
   	-ArgumentList $PsExecArgs
   
   if ($Process.ExitCode -ge 1)
   {
   	Write-Host "Started block II service"
   }

   #Read-Host "is the service started?"
   
   $PsExecArgs = "\\$FileServer -u $WstUsername -p $WstPassword -h -i cmd /c `"net start WstAudioRoutingService$SubNet`""
   
   Write-Debug "Executing: psexec $PsExecArgs"
   
   $Process = Start-Process -Wait `
   	-PassThru `
   	-PSPath $PsExecPath `
   	-ArgumentList $PsExecArgs
   
   if ($Process.ExitCode -ge 1)
   {
   	Write-Host "Started audio service"
   }

   #Read-Host "is the audio router started"
}

# =============================================================================
# Main script block.
# =============================================================================
do
{   
   Start-Services
    
    switch ($SubNet)
    {
        '17'
        {
			$WritePortFileName = 'WriteSubNetTo17'
			$Positions = @($Network + '.11')
            $Positions = @($Network + '.12')
        }
    
        '27'
        {
			$WritePortFileName = 'WriteSubNetTo27'
			$Positions = @($Network + '.12')
        }
		
		'37'
        {
			$WritePortFileName = 'WriteSubNetTo37'
			$Positions = @($Network + '.IPEND')
        }
		
		'47'
        {
			$WritePortFileName = 'WriteSubNetTo47'
			$Positions = @($Network + '.IPEND')
			
        }

    }

	$Positions += $Network +'.171'
	$Positions += $Network +'.172'
	$Positions += $Network +'.173'

	$Positions += $Network +'.181'
	$Positions += $Network +'.182'
	$Positions += $Network +'.183'

	$Positions += $Network +'.201'
	$Positions += $Network +'.202'
	$Positions += $Network +'.203'		

	$Positions += $Network +'.211'
	$Positions += $Network +'.212'
	$Positions += $Network +'.213'		

	$Positions += $Network +'.191'
	
    WritePorts
	$done = 1
} until($done = '1')