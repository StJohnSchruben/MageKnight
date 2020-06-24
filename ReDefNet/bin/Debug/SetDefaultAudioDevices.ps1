#------------------------------------------------READ ME------------------------------------------------
# Get-AudioDevice [parameter]
#          -List                     # Outputs a list of all devices as <AudioDevice>
#          -ID <string>              # Outputs the device with the ID corresponding to the given <string>
#          -Index <int>              # Outputs the device with the Index corresponding to the given <int>
#    	   -Playback                 # Outputs the default playback device as <AudioDevice>
#		   -PlaybackMute             # Outputs the default playback device's mute state as <bool>
#		   -PlaybackVolume           # Outputs the default playback device's volume level on 100 as <float>
#		   -Recording                # Outputs the default recording device as <AudioDevice>
#		   -RecordingMute            # Outputs the default recording device's mute state as <bool>
#		   -RecordingVolume          # Outputs the default recording device's volume level on 100 as <float>
#
#    ie. PS C:\> Get-AudioDevice -Playback
#
#                               Index   : 1
#                               Default : True
#             ....returns:      Type    : Playback
#                               Name    : Speakers (2- High Definition Audio Device)
#                               ID      : {0.0.0.00000000}.{5de27de5-364d-455d-a6c9-0237caf07357}
#                               Device  : CoreAudioApi.MMDevice
#
# Set-AudioDevice [parameter]
#          <AudioDevice>             # Sets the default playback/recording device to the given <AudioDevice>, can be piped
#		   -ID <string>              # Sets the default playback/recording device to the device with the ID corresponding to the given <string>
#		   -Index <int>              # Sets the default playback/recording device to the device with the Index corresponding to the given <int>
#		   -PlaybackMute <bool>      # Sets the default playback device's mute state to the given <bool>
#		   -PlaybackMuteToggle       # Toggles the default playback device's mute state
#		   -PlaybackVolume <float>   # Sets the default playback device's volume level on 100 to the given <float>
#		   -RecordingMute <bool>     # Sets the default recording device's mute state to the given <bool>
#		   -RecordingMuteToggle      # Toggles the default recording device's mute state
#		   -RecordingVolume <float>  # Sets the default recording device's volume level on 100 to the given <float>
#
#    ie. PS C:\> Set-AudioDevice -ID "{0.0.0.00000000}.{5de27de5-364d-455d-a6c9-0237caf07357}"
#        PS C:\> Set-AudioDevice -Index 1
#
# Write-AudioDevice [parameter]
#          -PlaybackMeter            # Writes the default playback device's power output on 100 as a meter
#		   -PlaybackSteam            # Writes the default playback device's power output on 100 as a stream of <int>
#		   -RecordingMeter           # Writes the default recording device's power output on 100 as a meter
#		   -RecordingSteam           # Writes the default recording device's power output on 100 as a stream of <int>
#-------------------------------------------------------------------------------------------------------

# Copies dll from BlockII directory to the path specified by the PSModulePath environment variable.
function Import-AudioDeviceCmdlets
{
    $DllName = "AudioDeviceCmdlets.dll"
    $DllPath = "C:\E6-B_WST\MissionEquipment\BlockII\Modules\AudioDeviceCmdlets\"

    if(![System.IO.File]::Exists("$($profile | split-path)\Modules\AudioDeviceCmdlets\$DllName"))
    {
        New-Item "$($profile | split-path)\Modules\AudioDeviceCmdlets" -Type directory -Force
	    Copy-Item "$DllPath$DllName" "$($profile | split-path)\Modules\AudioDeviceCmdlets\$DllName"
        Set-Location "$($profile | Split-Path)\Modules\AudioDeviceCmdlets"
	    Get-ChildItem | Unblock-File
	    Import-Module AudioDeviceCmdlets
    }
}

# Sets the default audio playback device to a supplied <AudioDevice>.
function Set-AudioPlaybackDevice
{
    [CmdletBinding()]
    Param($PlaybackDevice)
    Set-AudioDevice -ID $PlaybackDevice.ID
}

# Sets the default audio recorder device to a supplied <AudioDevice>.
function Set-AudioRecordingDevice
{
    [CmdletBinding()]
    Param($RecordingDevice)
    Set-AudioDevice -ID $RecordingDevice.ID
}


Import-AudioDeviceCmdlets

$AudioDevices = Get-AudioDevice -List

foreach ($Device in $AudioDevices)
{
    Write-Host $Device.Name
    Write-Host $Device.Index  
    Write-Host $Device.Default
    Write-Host $Device.Type   
    Write-Host $Device.Name   
    Write-Host $Device.ID     
    Write-Host $Device.Device 
    If($Device.Name | Select-String "WST Audio Output")
    {
        Set-AudioPlaybackDevice $Device
    }
    ElseIf($Device.Name | Select-String "WST Microphone")
    {
        Set-AudioRecordingDevice $Device
    }
}
