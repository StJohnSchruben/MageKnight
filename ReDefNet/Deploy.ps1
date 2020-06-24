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
#$DebugPreference = 'Continue'

# IP address of the file server
$FileServer = '192.168.17.95'

# The array of all the IP addresses of all WST positions to update,
# including the IOS.
#
# Do NOT include the file server in this list as it is handled
# slightly different.
$Positions = @(
    '192.168.17.11',
    '192.168.17.171',
    '192.168.17.172',
    '192.168.17.173',
    '192.168.17.181',
    '192.168.17.182',
    '192.168.17.183',
	'192.168.17.191',
    '192.168.17.201',
    '192.168.17.202',
    '192.168.17.203',
    '192.168.17.211',
    '192.168.17.212',
    '192.168.17.213'
)


$SubNets = @(
    '17',
    '27',
    '37',
    '47'
)

# Windows credentials for remotely connecting to WST positions
$WstUsername = 'Administrator'
$WstPassword = 'e6bwst'

# Location of the Sysinternals utilities
$PsExecPath = 'C:\Windows\System32\psexec.exe'
$PsServicePath = 'C:\E6-B_WST\Bin\Common\PsService.exe'
$PsKillPath = 'C:\E6-B_WST\Bin\Common\pskill.exe'

# Location of the .NET regasm utility
$RegAsmPath = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe'

# True, if the WST has been stopped (i.e. nuke has been run)
# False, if unknown or it may be running
[bool]$IsWstStopped = $false

# The IP address of the current machine
$CurrentMachineIpAddress = (Get-NetIPAddress -AddressFamily IPv4 | Where-Object {$_.IPAddress -like "192.168.*"} | Select-Object -Property IPAddress -First 1).IPAddress

Write-Host "Your IP address is $CurrentMachineIpAddress." -ForegroundColor DarkGray

if (!$Positions.Contains($CurrentMachineIpAddress))
{
    Write-Error "You are running this script from an unknown or unsupported position, such as the file server."

    exit
}

# A set of file patterns for files under the BlockII folder that will
# never be deployed to the WST.
#
# These are to ensure non-production files aren't accidentally deployed
# to the WST.
$BlockIIIgnoredDeployFiles = @(

    # Should not be used; use ReDefNet.Wst.Service.Client instead
    'FabtServiceClient.*',

    # Should not be used; use ReDefNet.Wst.ServerHost instead
    'FabtServiceConsole.*',
    '*.ConsoleServer.*',

    # Startup and prototype projects are for development only
    '*Dvs.Startup.*',
    '*Mrtcdl.Startup.*',
    '*Prototype*',

    # Workstation launcher is for development only
    '*.DevLauncher.*',

    # XML documentation files are for Visual Studio intellisense
    'Microsoft.*.xml',
    'log4net.xml',
    'ReDefNet*.xml',
    'NAudio.xml',
    'System*.xml',

    # Unit testing libraries should never be used in production code
    'NSubstitute.*',
    '*.UnitTestFramework.*',

    # Visual Studio host files are only for debugging in Visual Studio
    '*.vshost.*',

    # These are parts of WPF Extended Toolkit that are never used
    'Xceed.Wpf.AvalonDock.*',
    'Xceed.Wpf.Datagrid.*',

    # WCF tracing files and log files should never be deployed
    '*.svclog',
    '*.log',

    # Unit test files are not production code
    '*.Test.*',
    '*.Tests.*',

    # Type library files are not needed
    '*.tlb',

    # Other files that aren't really production code
    '*.manifest',
    '*.tmp',
    '*.txt',
	
	# These are old files that should never be deployed
	'ReDefNet.Wst.Audio.Model.dll',
	'ReDefNet.Wst.Audio.Service.dll'
)

# A set of directory names under the BlockII folder that will never be
# deployed to the WST.
#
# These are to ensure non-production files aren't accidentally deployed
# to the WST.
$BlockIIIgnoredDeployDirs = @(

    # WPF Extended Toolkit language directories that aren't necessary
    'de',
    'en',
    'es',
    'fr',
    'hu',
    'it',
    'ja',
    'ko',
    'pt-BR',
    'ro',
    'ru',
    'sv',
    'zh-Hans',
    'zh-Hant',

    # Logs should never be deployed
    'Logs'
)

# A hashset of the paths to .NET assemblies that need to be registered as COM devices with their respective CLSIDs.
#
# These paths should be absolute paths.
$NetComDevices = @{}
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Cdu.ComAdapter.dll'] = '628EE77F-E346-45E0-8689-113D5F689045'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Cdu.ComDevice.dll'] = 'FACDA003-EEEB-4604-B2CD-ECF5E19126C3'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Ios.ComAdapters.IosAdapter.dll'] = '9F8C35BC-8361-45B0-8B69-463189DEEFC0'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Txpa.ComAdapters.TxpaAdapter.dll'] = '628EE77F-E346-45E0-8689-113D5F687F37'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Txpa.ComDevices.TxpaDevice.dll'] = 'FACDA003-EEEB-4604-B2CD-ECF5E19126C5'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Uva.ComDevice.dll'] = 'B6641E19-29E0-4BC4-B899-1E1598F5FBB7'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Fabt.ComAdapters.MilstarControlPanelDevice.dll'] = 'E147F746-493F-4929-9C6C-F4D04BE45194'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Fabt.ComAdapters.MilstarDevice.dll'] = '628EE77F-E346-45E0-8689-113D5F687F30'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Fabt.ComAdapters.MilstarDisplayDevice.dll'] = '7DEED215-DF73-46EB-B234-DCB53CB18797'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Fabt.ComDevices.Ocp.dll'] = 'B6641E19-29E0-4BC4-B899-1E1598F5FBB6'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Fabt.ComDevices.Opu.dll'] = 'FACDA003-EEEB-4604-B2CD-ECF5E19126C1'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Mrtcdl.ComAdapters.OcpDevice.dll'] = '4EFE1CC9-4A63-4E54-9B44-C2A9A884EFAB'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Mrtcdl.ComAdapters.PduDevice.dll'] = '628EE77F-E346-45E0-8689-113D5F687F34'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Mrtcdl.ComDevices.Ocp.dll'] = '5A19C142-9880-4B99-BAFD-700E963368B8'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Mrtcdl.ComDevices.Pdu.dll'] = 'FACDA003-EEEB-4604-B2CD-ECF5E19126C4'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Workstation.ComDevice.dll'] = '76CC094B-C6D4-486D-A1AF-5575512955AA'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.UPSEmergencyOff.ComDevice.dll'] = 'B6641E19-29E0-4BC4-B899-1E1598F5FBB8'

#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.FWDConsoleBay1Upper.exe']        = '26BDDD8B-CCC0-4D8D-B3D4-9684670BD03F'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.FWDConsoleBay1Lower.exe']        = 'D1066E45-7BE3-4CFE-AFB2-3C67CF25B622'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.FWDConsoleBay2Upper.exe']        = '4C559B7D-C371-4AC6-9F3C-43083C313C8D'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.FWDConsoleBay2Lower.exe']        = '4C754F7E-B392-42D6-86C5-98E07FF99958'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.FWDConsoleBay3Upper.exe']        = '9E61CB03-272B-4E76-87AF-DC6AE156F9C2'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.FWDConsoleBay3Lower.exe']        = '5723D023-B5CE-40A9-A634-A65F011B4084'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.FWDConsoleBay4Lower.exe']        = '94202412-72A9-4A53-B7DD-C111F2B1A044'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.FWDConsoleBay5Lower.exe']        = '3450B6F4-A302-4022-B570-0EBBC6EE3418'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.AFTConsoleBay1Upper.exe']        = '653D05D7-49EE-4CD6-80D6-7090AE528D8C'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.AFTConsoleBay1Lower.exe']        = '10BEA84C-021B-4735-9F0E-5F7D8F015E6D'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.AFTConsoleBay2Lower.exe']        = 'D232E6FA-3DB1-4E9F-BB06-55E7EDDE881D'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.AFTConsoleBay3Lower.exe']        = '190E5856-B7C1-47A1-9EC8-55EADCBA386F'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.AFTConsoleBay4Lower.exe']        = 'DD56BDBD-6AB9-4DF9-BCB7-3C239E7BA4CD'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.AFTConsoleBay5Lower.exe']        = '30EF0800-69DD-471E-B57D-A675DDD6FF48'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.AFTConsoleBay6Lower.exe']        = 'E7A08AB1-0A59-4744-9208-797962061284'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.AcoConsole.exe']                 = '02828E93-B07B-4DB0-9C7B-D914CB5BF65C'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.Cmlan.Unclass.exe']              = 'F3975BDF-AC25-44A2-B5D2-F44D731C625A'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.Cmlan.Secret.exe']               = '7E4BB3C9-517D-4EC1-BD47-AF74DD2EE59B'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.Cmlan.TopSecret.exe']            = '8BA2F360-D6BB-4AD9-ADB2-6C27C844EA70'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.BattleStaff1.exe']               = '480865CE-6188-44DF-B9DB-75DDF6126EFC'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.BattleStaff2.exe']               = '282A970C-E2A3-4C42-8AC2-EBDC9BA948A7'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.BattleStaff3.exe']               = '93AAE1CB-8652-4414-8C8A-7CFA221C1EAC'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.BattleStaff4.exe']               = 'D59AA127-8092-431D-AEE2-7326EB0CA990'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.BattleStaff5.exe']               = 'B92F07A1-C42C-4C5B-8A06-1AECB54A91EF'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.BattleStaff6.exe']               = '6FFFAF30-18BE-46CD-BDEB-37FE42393607'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.BattleStaff7.exe']               = '3DC546A6-CD37-4BDF-8875-E0AD44EAFF09'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.BattleStaff8.exe']               = 'A5AA177A-3612-4606-8687-298BB76E7E19'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Bay.BattleStaff9.exe']               = '5D4B5176-6C04-4D95-8C40-5103AB26FC11'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.NC3.ComAdapter.dll']                 = '091FBED2-BCE9-49D1-84DC-EC40EE83C751'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Fabt.ComAdapters.MilstarDevice.dll'] = '628EE77F-E346-45E0-8689-113D5F687F30'
#$NetComDevices['C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.Ste.ComAdapter.dll']             = 'CF482276-A2C3-4993-9BB8-D81CEAB911E9'

# A list of native assemblies that need to be registerd as COM devices.
#
# These paths should be absolute paths.
$NativeComDevices = @(
    'C:\E6-B_WST\MissionEquipment\WindowScrollDownArrowDeviceDLL.dll',
    'C:\E6-B_WST\MissionEquipment\WindowScrollUpArrowDeviceDLL.dll',
    'C:\E6-B_WST\MissionEquipment\KY100M_PowerSwitchDeviceDLL.dll'
)

# =============================================================================
# Shows the main menu.
# =============================================================================
function Show-MainMenu
{
    Write-Host '==============================================================================='
    Write-Host '                                  MAIN MENU'
    Write-Host '==============================================================================='
    Write-Host ''
    Write-Host ''
    Write-Host 'Please select a menu option:'
    Write-Host ''
    Write-Host '    [1] Deploy'
    Write-Host '    [2] Utilities'
    Write-Host ''
    Write-Host '    [0] Exit'
    Write-Host ''
    Write-Host '==============================================================================='
    Write-Host ''
}

# =============================================================================
# Shows the utilities menu.
# =============================================================================
function Show-UtilitiesMenu
{
    Write-Host '==============================================================================='
    Write-Host '                              UTILITIES MENU'
    Write-Host '==============================================================================='
    Write-Host ''
    Write-Host ''
    Write-Host 'Please select a menu option:'
    Write-Host ''
    Write-Host '    [1] Stop the WST (i.e. run nuke)'
    Write-Host '    [2] Reinstall audio router and block II services'
    Write-Host '    [3] Reinstall COM devices'
    Write-Host '    [4] Stop services only'
    Write-Host ''
    Write-Host '    [9] Back to the main menu'
    Write-Host '    [0] Exit'
    Write-Host ''
    Write-Host '==============================================================================='
    Write-Host ''
}

# =============================================================================
# Attempts to forcibly kill a service on the file server.
# =============================================================================
function Kill-Service
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$ServiceName = ""
    )

    Write-Host "Attempting to forcibly stop service $ServiceName..."

    $ServicePID = (Get-WmiObject -ComputerName $FileServer -Class Win32_Service -Filter "name=`"$ServiceName`"" | Select-Object -Property ProcessID).ProcessID

    if ($ServicePID -le 0)
    {
        Write-Warning "Could not locate the process ID for service $ServiceName. You may have to stop the service manually."
    }
    else
    {
        Write-Host "Service $ServiceName is running on process $ServicePID." -ForegroundColor DarkGray
    
        $PsKillArgs = "\\$FileServer -u $WstUsername -p $WstPassword $ServicePID -nobanner"
    
        Write-Debug "Executing: pskill $PsKillArgs"
    
        $Psi = New-Object System.Diagnostics.ProcessStartInfo
        $Psi.FileName = $PsKillPath
        $Psi.RedirectStandardError = $true
        $Psi.RedirectStandardOutput = $true
        $Psi.UseShellExecute = $false
        $Psi.Arguments = $PsKillArgs
        $Process = New-Object System.Diagnostics.Process
        $Process.StartInfo = $Psi
        $Process.Start() | Out-Null
        $Process.WaitForExit()
        $StdOut = $Process.StandardOutput.ReadToEnd()
        $StdError = $Process.StandardError.ReadToEnd()

        Write-Host "Forcibily stopping service $ServiceName exited with error code $($Process.ExitCode))." -ForegroundColor DarkGray

        #Write-Host "Exit code was $($Process.ExitCode)" -ForegroundColor DarkGray
        #Write-Host ("Standard output was:`r`n" + $StdOut) -ForegroundColor DarkGray
        #Write-Host ("Standard error was:`r`n" + $StdError) -ForegroundColor DarkGray

        if ($Process.ExitCode -gt 0)
        {
            Write-Warning "Forcibily stopping $ServiceName failed. You may have to stop if manually."
        }
    }
}

function Get-ServiceStatus
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$ServiceName = ""
    )

    Write-Host "Getting status of service $ServiceName..."

    $PsServiceArgs = "\\$FileServer -u $WstUsername -p $WstPassword query $ServiceName -nobanner"

    Write-Host "Executing: psservice $PsServiceArgs"

    $Psi = New-Object System.Diagnostics.ProcessStartInfo
    $Psi.FileName = $PsServicePath
    $Psi.RedirectStandardError = $true
    $Psi.RedirectStandardOutput = $true
    $Psi.UseShellExecute = $false
    $Psi.Arguments = $PsServiceArgs
    $Process = New-Object System.Diagnostics.Process
    $Process.StartInfo = $Psi
    $Process.Start() | Out-Null
    Write-Host "9"
    $Process.WaitForExit()
    Write-Host "10"
    $StdOut = $Process.StandardOutput.ReadToEnd()
    Write-Host "11"
    $StdError = $Process.StandardError.ReadToEnd()

    Write-Host "Getting status of service $ServiceName exited with error code $($Process.ExitCode))." -ForegroundColor DarkGray

    #Write-Host "Exit code was $($Process.ExitCode)" -ForegroundColor DarkGray
    #Write-Host ("Standard output was:`r`n" + $StdOut) -ForegroundColor DarkGray
    #Write-Host ("Standard error was:`r`n" + $StdError) -ForegroundColor DarkGray

    if (($Process.ExitCode -eq 0) -and ($StdOut))
    {
        $ParseIndex = $StdOut.IndexOf("DISPLAY_NAME: $ServiceName")

        if ($ParseIndex -gt 0)
        {
            try
            {
                $StdOut = $StdOut.Substring($ParseIndex)
                $Match = [regex]::Match($StdOut, 'STATE\s+:\s+\d+\s+(?<State>\S+)\s+')

                if ($Match.Success -eq $false)
                {
                    return "ERROR"
                }

                $ServiceState = $Match.Groups['State'].Value
                Write-Host "Service $ServiceName has state $ServiceState" -ForegroundColor DarkGray

                return $ServiceState
            }
            catch
            {
                return "ERROR"
            }
        }
        else
        {
            return "ERROR"
        }
    }
    else
    {
        return "ERROR"
    }
}

# =============================================================================
# Stops a service on the file server.
# =============================================================================
function Stop-Service
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$ServiceName = ""
    )

    Write-Host "Stopping service $ServiceName..."

    $ServiceState = Get-ServiceStatus $ServiceName

    if ($ServiceState -eq 'STOPPED')
    {
        Write-Host "Service $ServiceName is already stopped." -ForegroundColor DarkGray

        return
    }

    if ($ServiceState -ne 'RUNNING')
    {
        # If it's not actually running or stopped, it may be in a pending status, so kill it
        Kill-Service -ServiceName $ServiceName

        return
    }

    $PsServiceArgs = "\\$FileServer -u $WstUsername -p $WstPassword stop $ServiceName -nobanner"

    Write-Debug "Executing: psservice $PsServiceArgs"

    $Psi = New-Object System.Diagnostics.ProcessStartInfo
    $Psi.FileName = $PsServicePath
    $Psi.RedirectStandardError = $true
    $Psi.RedirectStandardOutput = $true
    $Psi.UseShellExecute = $false
    $Psi.Arguments = $PsServiceArgs
    $Process = New-Object System.Diagnostics.Process
    $Process.StartInfo = $Psi
    $Process.Start() | Out-Null
    $Process.WaitForExit()
    $StdOut = $Process.StandardOutput.ReadToEnd()
    $StdError = $Process.StandardError.ReadToEnd()

    Write-Host "Stopping service $ServiceName exited with exit code $($Process.ExitCode)." -ForegroundColor DarkGray

    if ($Process.ExitCode -eq 0)
    {
        if (($StdError) -and ($StdError.Contains('error code 1060')))
        {
            Write-Host "Service $ServiceName doesn't exist." -Foreground DarkGray
        }
    }
    else
    {
        Write-Warning "Stopping service $ServiceName failed."

        Kill-Service -ServiceName $ServiceName
    }

    #Write-Host "Exit code was $($Process.ExitCode)" -ForegroundColor DarkGray
    #Write-Host ("Standard output was:`r`n" + $StdOut) -ForegroundColor DarkGray
    #Write-Host ("Standard error was:`r`n" + $StdError) -ForegroundColor DarkGray
}

# =============================================================================
# Stops audio router and block II services.
# =============================================================================
function Stop-Services
{
    Write-Host 'Stopping the audio router services...'

    foreach ($SubNet in $SubNets)
    {
        $ServiceName = "WstAudioRoutingService$SubNet"

        Stop-Service -ServiceName $ServiceName
    }

    Write-Host 'Stopping the block II services...'

    foreach ($SubNet in $SubNets)
    {
        $ServiceName = "WstBlock2Service$SubNet"

        Stop-Service -ServiceName $ServiceName
    }

    Write-Host 'WST services are now stopped.' -ForegroundColor Green
}

# =============================================================================
# Stops the WST, audio router and block II service.
# =============================================================================
function Stop-Wst
{
    param
    (
        [Parameter(Mandatory = $false, Position = 1)]
        [switch]$Force = $false
    )

    if ($Force)
    {
        Write-Host 'Forcing WST to stop...'
    }
    else
    {
        Write-Host 'Checking if WST is running...'

        if ($script:IsWstStopped)
        {
            Write-Host 'WST is not running.' -ForegroundColor DarkGray

            return
        }
    }

    Write-Host 'WST *IS* running.' -ForegroundColor DarkGray
    Write-Host 'Running Nuke on all positions...'

    foreach ($Position in $Positions)
    {
        $PsExecArgs = "\\$Position -u $WstUsername -p $WstPassword -w `"C:\_wstCMDs`" -i cmd /c `"C:\_wstCMDs\NukeWST.bat`""

        Write-Debug "Executing: psexec $PsExecArgs"

        $Process = Start-Process `
            -PassThru `
            -PSPath $PsExecPath `
            -ArgumentList $PsExecArgs

        Write-Host "Position $Position nuke batch file returned exit code $($Process.ExitCode)." -ForegroundColor DarkGray
    }

    Stop-Services
    
    Write-Host 'WST is now stopped.' -ForegroundColor Green

    $script:IsWstStopped = $true
}

# =============================================================================
# Removes all ignored files and folders from the local BlockII folder prior
# to deployment.
# =============================================================================
function Remove-Ignored
{
    Write-Host 'Cleaning up the local BlockII folder prior to deployment.'

    foreach ($IgnoredDir in $BlockIIIgnoredDeployDirs)
    {
        Write-Host "Removing unnecessary directory: $IgnoredDir..." -ForegroundColor DarkGray

        try 
        {
            Get-ChildItem "C:\E6-B_WST\MissionEquipment\BlockII" -Directory -Filter "$IgnoredDir" | Remove-Item -Recurse -Force
        }
        catch 
        {
            Write-Warning "Could not remove local BlockII directory '$IgnoredDir'."

            continue
        }
    }

    foreach ($IgnoredFile in $BlockIIIgnoredDeployFiles)
    {
        Write-Host "Removing unnecessary file: $IgnoredFile..." -ForegroundColor DarkGray

        try 
        {
            Get-ChildItem "C:\E6-B_WST\MissionEquipment\BlockII" -File -Filter "$IgnoredFile" | Remove-Item -Force
        }
        catch 
        {
            Write-Warning "Could not remove local BlockII file '$IgnoredFile'."

            continue
        }
    }

    Write-Host 'Local machine cleaned successfully.' -ForegroundColor Green
}

# =============================================================================
# Deploys new files to the WST.
# =============================================================================
function Install-Wst
{
    Stop-Wst
    Remove-Ignored

    Write-Host 'Deploying new E6-B_WST files to the WST from the current machine, '$CurrentMachineIpAddress'.'
    Write-Host 'Deploying E6-B_WST files to the file server...'
    
    $SourceDir = "C:\E6-B_WST"
    $DestinationDir = "\\$FileServer\C$\E6-B_WST"

    $RoboCopyOutput = robocopy "$SourceDir" "$DestinationDir" /r:3 /w:5 /np /fp /mir

    Write-Host "Robocopy exited with exit code $LASTEXITCODE." -ForegroundColor DarkGray

    if ($LASTEXITCODE -gt 7)
    {
        Write-Error "Failed to deploy new E6-B_WST files to the file server."

        exit
    }

    foreach ($Position in $Positions)
    {
        Write-Host "Deploying E6-B_WST files to position '$Position'..."

        if ($Position -eq $CurrentMachineIpAddress)
        {
            Write-Host "Skipping current position." -ForegroundColor DarkGray

            continue
        }

        $DestinationDir = "\\$Position\C$\E6-B_WST"

        $RoboCopyOutput = robocopy "$SourceDir" "$DestinationDir" /r:3 /w:5 /np /fp /mir

        Write-Host "Robocopy exited with exit code $LASTEXITCODE." -ForegroundColor DarkGray

        if ($LASTEXITCODE -gt 7)
        {
            Write-Error "Failed to deploy new E6-B_WST files to position '$Position'."

            exit
        }
    }

    Write-Host 'E6-B_WST files deployed successfully.' -ForegroundColor Green

    Write-Host 'Deploying new _wstCMDs files to the WST from the current machine, '$CurrentMachineIpAddress'.'
    Write-Host 'Deploying _wstCMDs files to the file server...'

    $SourceDir = "C:\_wstCMDs"
    $DestinationDir = "\\$FileServer\C$\_wstCMDs"

    $RoboCopyOutput = robocopy "$SourceDir" "$DestinationDir" /r:3 /w:5 /np /fp /mir

    Write-Host "Robocopy exited with exit code $LASTEXITCODE." -ForegroundColor DarkGray

    if ($LASTEXITCODE -gt 7)
    {
        Write-Error "Failed to deploy new _wstCMDs files to the file server."

        exit
    }

    foreach ($Position in $Positions)
    {
        Write-Host "Deploying _wstCMDs files to position '$Position'..."

        if ($Position -eq $CurrentMachineIpAddress)
        {
            Write-Host "Skipping current position." -ForegroundColor DarkGray

            continue
        }

        $DestinationDir = "\\$Position\C$\_wstCMDs"

        $RoboCopyOutput = robocopy "$SourceDir" "$DestinationDir" /r:3 /w:5 /np /fp /mir

        Write-Host "Robocopy exited with exit code $LASTEXITCODE." -ForegroundColor DarkGray

        if ($LASTEXITCODE -gt 7)
        {
            Write-Error "Failed to deploy new _wstCMDs files to position '$Position'."

            exit
        }
    }

    Write-Host '_wstCMDs files deployed successfully.' -ForegroundColor Green
}

# =============================================================================
# Uninstalls a service from the file server.
# =============================================================================
function Uninstall-Service
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$ServiceName = ""
    )

    Write-Host "Uninstalling service $ServiceName..."

    $PsExecArgs = "\\$FileServer -u $WstUsername -p $WstPassword -i cmd /c `"sc delete $ServiceName`""

	Write-Debug "Executing: psexec $PsExecArgs"

    $Psi = New-Object System.Diagnostics.ProcessStartInfo
    $Psi.FileName = $PsExecPath
    $Psi.RedirectStandardError = $true
    $Psi.RedirectStandardOutput = $true
    $Psi.UseShellExecute = $false
    $Psi.Arguments = $PsExecArgs
    $Process = New-Object System.Diagnostics.Process
    $Process.StartInfo = $Psi
    $Process.Start() | Out-Null
    $Process.WaitForExit()
    $StdOut = $Process.StandardOutput.ReadToEnd()
    $StdError = $Process.StandardError.ReadToEnd()

    Write-Host "Uninstalling service $ServiceName exited with exit code $($Process.ExitCode)." -ForegroundColor DarkGray

    #Write-Host "Exit code was $($Process.ExitCode)" -ForegroundColor DarkGray
    #Write-Host ("Standard output was:`r`n" + $StdOut) -ForegroundColor DarkGray
    #Write-Host ("Standard error was:`r`n" + $StdError) -ForegroundColor DarkGray
}

# =============================================================================
# Installs a block II service on the file server.
# =============================================================================
function Install-Block2Service
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$ServiceName = "",

        [Parameter(Mandatory = $true)]
        [string]$ServicePath = "",

        [Parameter(Mandatory = $true)]
        [string]$ServiceConfigPath = ""
    )

    Write-Host "Installing service $ServiceName..."

    $PsExecArgs = "\\$FileServer -s -u $WstUsername -p $WstPassword -i cmd /c `"sc create $ServiceName binpath= `"\`"$ServicePath\`" --config=\`"$ServiceConfigPath\`"`"`""

	Write-Debug "psexec $PsExecArgs"
		
    $Psi = New-Object System.Diagnostics.ProcessStartInfo
    $Psi.FileName = $PsExecPath
    $Psi.RedirectStandardError = $true
    $Psi.RedirectStandardOutput = $true
    $Psi.UseShellExecute = $false
    $Psi.Arguments = $PsExecArgs
    $Process = New-Object System.Diagnostics.Process
    $Process.StartInfo = $Psi
    $Process.Start() | Out-Null
    $Process.WaitForExit()
    $StdOut = $Process.StandardOutput.ReadToEnd()
    $StdError = $Process.StandardError.ReadToEnd()

    Write-Host "Installing service $ServiceName exited with exit code $($Process.ExitCode)." -ForegroundColor DarkGray

    #Write-Host "Exit code was $($Process.ExitCode)" -ForegroundColor DarkGray
    #Write-Host ("Standard output was:`r`n" + $StdOut) -ForegroundColor DarkGray
    #Write-Host ("Standard error was:`r`n" + $StdError) -ForegroundColor DarkGray
}

# =============================================================================
# Installs an audio router service on the file server.
# =============================================================================
function Install-AudioRouterService
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$ServiceName = "",

        [Parameter(Mandatory = $true)]
        [string]$ServicePath = "",

        [Parameter(Mandatory = $true)]
        [string]$ServiceEndpoint = "",

        [Parameter(Mandatory = $true)]
        [string]$ServiceListen = "",

        [Parameter(Mandatory = $true)]
        [string]$ServiceDepend = ""
    )

    Write-Host "Installing service $ServiceName..."

    $PsExecArgs = "\\$FileServer -s -u $WstUsername -p $WstPassword -i cmd /c `"sc create $ServiceName binpath= `"\`"$ServicePath\`" --endpoint=$ServiceEndpoint --listen=$ServiceListen`" depend= $ServiceDepend`""

	Write-Debug "psexec $PsExecArgs"
		
    $Psi = New-Object System.Diagnostics.ProcessStartInfo
    $Psi.FileName = $PsExecPath
    $Psi.RedirectStandardError = $true
    $Psi.RedirectStandardOutput = $true
    $Psi.UseShellExecute = $false
    $Psi.Arguments = $PsExecArgs
    $Process = New-Object System.Diagnostics.Process
    $Process.StartInfo = $Psi
    $Process.Start() | Out-Null
    $Process.WaitForExit()
    $StdOut = $Process.StandardOutput.ReadToEnd()
    $StdError = $Process.StandardError.ReadToEnd()

    Write-Host "Installing service $ServiceName exited with exit code $($Process.ExitCode)." -ForegroundColor DarkGray

    #Write-Host "Exit code was $($Process.ExitCode)" -ForegroundColor DarkGray
    #Write-Host ("Standard output was:`r`n" + $StdOut) -ForegroundColor DarkGray
    #Write-Host ("Standard error was:`r`n" + $StdError) -ForegroundColor DarkGray
}

# =============================================================================
# Installs the audio router and block II services.
# =============================================================================
function Install-Services
{
    #Stop-Wst

    Write-Host 'Uninstalling the audio router services...'

	foreach ($SubNet in $SubNets)
	{
        $ServiceName = "WstAudioRoutingService$SubNet"
        #Uninstall-Service -ServiceName $ServiceName
	}

    Write-Host 'Uninstalling the block II services...'

	foreach ($SubNet in $SubNets)
	{
        $ServiceName = "WstBlock2Service$SubNet"
        #Uninstall-Service -ServiceName $ServiceName
	}


    Write-Host 'Installing the block II services...'

    foreach ($SubNet in $SubNets)
	{
        $ServiceName = "WstBlock2Service$SubNet"
        $ServiceConfigPath = "C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.ServerHost_FS$SubNet.config"

        Install-Block2Service -ServiceName $ServiceName `
            -ServicePath 'C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.ServerHost.exe' `
            -ServiceConfigPath $ServiceConfigPath
	}
 
    Write-Host 'Installing the audio router services...'

	foreach ($SubNet in $SubNets)
	{
        $ServiceName = "WstAudioRoutingService$SubNet"
        $ServiceEndpoint = "192.168.$SubNet.100:9000"
        $ServiceDepend = "WstBlock2Service$SubNet"
        $ServiceListen = "192.168.$SubNet.95"
        Install-AudioRouterService -ServiceName $ServiceName `
            -ServicePath 'C:\E6-B_WST\MissionEquipment\BlockII\ReDefNet.Wst.Dvs.AudioRouter.exe' `
            -ServiceEndpoint $ServiceEndpoint `
            -ServiceListen $ServiceListen `
            -ServiceDepend $ServiceDepend
	}

    Write-Host 'Services installed.' -ForegroundColor Green
}

# =============================================================================
# Installs the COM devices.
# =============================================================================
function Install-ComDevices
{
    Stop-Wst

    Write-Host 'Unregistering COM devices.'

    foreach ($Position in $Positions)
    {
        Write-Host "Unregistering .NET COM devices on position '$Position'..."

        foreach ($NetComDevice in $NetComDevices.GetEnumerator())
        {
            Write-Host "Unregistering .NET COM device '$($NetComDevice.Key)'." -ForegroundColor DarkGray

            $PsExecArgs = "\\$Position -u $WstUsername -p $WstPassword -i cmd /c `"`"$RegAsmPath`" `"$($NetComDevice.Key)`" /unregister /verbose`""

            Write-Debug "psexec $PsExecArgs"

            $Process = Start-Process -Wait `
                -PassThru `
                -PSPath $PsExecPath `
                -ArgumentList $PsExecArgs

            if ($Process.ExitCode -ge 1)
            {
                Write-Error "Could not unregister .NET COM device '$($NetComDevice.Key)' from position '$Position'."
        
                #exit
            }
                
            # Sometimes the registrations don't actually get unregistered, especially if multiple versions of the same .NET assembly
            # have been registered. So deleting the CLSID key explicitly fixes this.

            <# Write-Host "Ensuring .NET COM device '$($NetComDevice.Key)' is unregistered."

            $PsExecArgs = "\\$Position -u $WstUsername -p $WstPassword -i cmd /c `"reg delete HKCR\WOW6432Node\CLSID\{$($NetComDevice.Value)} /f`""

            Write-Debug "psexec $PsExecArgs"

            $Process = Start-Process -Wait `
                -PassThru `
                -PSPath $PsExecPath `
                -ArgumentList $PsExecArgs

            if ($Process.ExitCode -ge 1)
            {
                Write-Error "Could not delete .NET COM device '$($NetComDevice.Key)' from position '$Position'."
        
                exit
            } #>
        }

        Write-Host "Unregistering native COM devices on position '$Position'..."

        foreach ($NativeComDevice in $NativeComDevices)
        {
            Write-Host "Unregistering native COM device '$NativeComDevice'." -ForegroundColor DarkGray

            $PsExecArgs = "\\$Position -u $WstUsername -p $WstPassword -i cmd /c `"regsvr32 /s /u `"$NativeComDevice`"`""

            Write-Debug "psexec $PsExecArgs"

            $Process = Start-Process -Wait `
                -PassThru `
                -PSPath $PsExecPath `
                -ArgumentList $PsExecArgs

            if ($Process.ExitCode -ge 1)
            {
                if ($Process.ExitCode -eq 5)
                {
                    Write-Warning "Could not unregister native COM device '$NativeComDevice' from position '$Position'. This is likely because it is already unregistered."
                }
                else
                {
                    Write-Warning "Could not unregister native COM device '$NativeComDevice' from position '$Position'."

                    #exit
                }
            }
        }
    }

    Write-Host 'Registering COM devices.'

    foreach ($Position in $Positions)
    {
        Write-Host "Registering .NET COM devices on position '$Position'..."

        foreach ($NetComDevice in $NetComDevices.GetEnumerator())
        {
            Write-Host "Registering .NET COM device '$($NetComDevice.Key)'." -ForegroundColor DarkGray

            $PsExecArgs = "\\$Position -u $WstUsername -p $WstPassword -i cmd /c `"`"$RegAsmPath`" `"$($NetComDevice.Key)`" /codebase /verbose`""

            Write-Debug "psexec $PsExecArgs"

            $Process = Start-Process -Wait `
                -PassThru `
                -PSPath $PsExecPath `
                -ArgumentList $PsExecArgs

            if ($Process.ExitCode -ge 1)
            {
                Write-Error "Could not register .NET COM device '$($NetComDevice.Key)' on position '$Position'."
        
                exit
            }
        }

        Write-Host "Registering native COM devices on position '$Position'..."

        foreach ($NativeComDevice in $NativeComDevices)
        {
            Write-Host "Registering native COM device '$NativeComDevice'." -ForegroundColor DarkGray

            $PsExecArgs = "\\$Position -u $WstUsername -p $WstPassword -i cmd /c `"regsvr32 /s `"$NativeComDevice`"`""

            Write-Debug "psexec $PsExecArgs"

            $Process = Start-Process -Wait `
                -PassThru `
                -PSPath $PsExecPath `
                -ArgumentList $PsExecArgs

            if ($Process.ExitCode -ge 1)
            {
                Write-Error "Could not register native COM device '$NativeComDevice' on position '$Position'."
        
                exit
            }
        }
    }

    Write-Host 'COM devices registered.' -ForegroundColor Green
}

# =============================================================================
# Runs the utilities menu.
# =============================================================================
function Invoke-UtilitiesMenu
{
    do
    {
        Show-UtilitiesMenu
        $Selection = Read-Host "Make a selection"

        switch ($Selection)
        {
            '1'
            {
                # Stop the WST
                Stop-Wst -Force:$true
            }
            '2'
            {
                # Reinstall services
                Install-Services
            }
            '3'
            {
                # Reinstall COM devices
                Install-ComDevices
            }
            '9'
            {
                # Back to the main menu
                return
            }
            '0'
            {
                # Exit
                Write-Host 'Goodbye.' -ForegroundColor Green

                exit
            }
        }
    } while ($true)
}

# =============================================================================
# Main script block.
# =============================================================================


do
{
    Show-MainMenu
    $Selection = Read-Host "Make a selection"

    switch ($Selection)
    {
        '1'
        {
            # Deploy files to the WST
            Install-Wst
        }

        '2'
        {
            # Utilities
            Invoke-UtilitiesMenu
        }
    }
} until ($Selection -eq '0')

# Added this to prevent the window from closing on error
Read-Host -Prompt "Press ENTER to exit."

Write-Host 'Goodbye.' -ForegroundColor Green
