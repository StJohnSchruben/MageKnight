# Script parameters
[CmdletBinding()]
param(
)

Clear-Host

# Enforces some best practices
Set-StrictMode -Version 2

# Stop when an error occurs
$ErrorActionPreference = 'Stop'

# Uncomment the following line to have debug statements written
# $DebugPreference = 'Continue'

# IP address of the file server
$FileServer = '192.168.17.100'

# Windows credentials for remotely connecting to WST positions
$WstUsername = 'Administrator'
$WstPassword = 'e6bwst'

# Location of the Sysinternals psexec.exe utility
$PsExecPath = 'C:\Windows\System32\psexec.exe'

# Location of the .NET regasm utility
$RegAsmPath = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe'

# True, if the WST has been stopped (i.e. nuke has been run)
# False, if unknown or it may be running
[bool]$IsWstStopped = $false

# =============================================================================
# Stops the WST, audio router and block II services.
# =============================================================================
function Stop-Wst
{
    Write-Host 'Stopping the audio router services...'
                   
    $AudioRouter = "WstAudioRoutingService"
    $BlockIIService = "WstBlock2Service"

    $CmdArgs = "\\192.168.17.11 -u $WstUsername -p $WstPassword -h -i cmd /c `"taskkill /f /IM CommplanEditor.exe`""
    Write-Host "Executing: psexec $CmdArgs"

    $Process = Start-Process `
        -PassThru `
        -PSPath $PsExecPath `
        -ArgumentList $CmdArgs

    foreach ($Port in $SubNets)
    {      
		Write-Host "Processing subnet $Port"
        #$PsExecArgs = "\\$FileServer -u $WstUsername -p $WstPassword -i cmd /c `"net stop WstAudioRoutingService$Port`""
        #
        #Write-Debug "Executing: psexec $PsExecArgs"
        #
        #$Process = Start-Process -Wait `
        #    -PassThru `
        #    -PSPath $PsExecPath `
        #    -ArgumentList $PsExecArgs
        #
        #if ($Process.ExitCode -ge 1)
        #{
        #    Write-Warning "Stopping the audio router service failed. Trying to forcibly terminate it."
        #}


        $ServicePID = (Get-WmiObject -ComputerName $FileServer -Class Win32_Service -Filter name="'$AudioRouter$Port'" | Select-Object -Property ProcessID).ProcessID

        if ($ServicePID -gt 0)
        {
            Write-Host "Audio router service is running on process $ServicePID." -ForegroundColor DarkGray

            $PsExecArgs = "\\$FileServer -u $WstUsername -p $WstPassword -h -i cmd /c `"taskkill /f /pid $ServicePID`""

            Write-Debug "Executing: psexec $PsExecArgs"

            $Process = Start-Process `
                -PassThru `
                -PSPath $PsExecPath `
                -ArgumentList $PsExecArgs

            if ($Process.ExitCode -ge 1)
            {
                Write-Error "Could not stop the audio router service. Stop it manually and re-run this script."

                #exit
            }
        }
    }

    Write-Host 'Stopping the block II services...'

    foreach ($Port in $SubNets)
    {
	
		Write-Host "Processing subnet $Port"
        #$PsExecArgs = "\\$FileServer -u $WstUsername -p $WstPassword -i cmd /c `"net stop WstBlock2Service$Port`""
        #
        #Write-Debug "Executing: psexec $PsExecArgs"
        #
        #$Process = Start-Process -Wait `
        #    -PassThru `
        #    -PSPath $PsExecPath `
        #    -ArgumentList $PsExecArgs
        #
        #if ($Process.ExitCode -ge 1)
        #{
        #    Write-Warning "Stopping the block II service failed. Trying to forcibly terminate it."
        #}

        $ServicePID = (Get-WmiObject -ComputerName $FileServer -Class Win32_Service -Filter name="'$BlockIIService$Port'" | Select-Object -Property ProcessID).ProcessID

        if ($ServicePID -gt 0)
        {
            Write-Host "Block II service is running on process $ServicePID." -ForegroundColor DarkGray

            $PsExecArgs = "\\$FileServer -u $WstUsername -p $WstPassword -h -i cmd /c `"taskkill /f /pid $ServicePID`""

            Write-Debug "Executing: psexec $PsExecArgs"

            $Process = Start-Process `
                -PassThru `
                -PSPath $PsExecPath `
                -ArgumentList $PsExecArgs

            if ($Process.ExitCode -ge 1)
            {
                Write-Error "Could not stop the block II service. Stop it manually and re-run this script."

                #exit
            }
        }
    }

	# Reset the system time
    $CmdTime = "\\192.168.17.11 -u $WstUsername -p $WstPassword -h -i cmd /c `"c:\_WSTCmds\FetchTimeFrom.bat 192.168.17.100`""
    Write-Host "Executing: psexec $CmdTime"

    $Process = Start-Process `
        -PassThru `
        -PSPath $PsExecPath `
        -ArgumentList $CmdTime

	exit
    
}

$SubNets = @(
    '17'
)
# $SubNets = @(
    # '17',
    # '27',
    # '37',
    # '47'
# )

# =============================================================================
# Main script block.
# =============================================================================
Stop-Wst

