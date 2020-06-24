#Requires -Version 5

# Script parameters
param
(
)

# Enforces some best practices
Set-StrictMode -Version 2

# Stop when an error occurs
$ErrorActionPreference = 'Stop'

# Uncomment the following line to have debug statements written
#$DebugPreference = 'Continue'

$TfsPath = 'C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\TF.exe'
$MSBuildPath = 'C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe'
$NuGetPath = 'C:\Users\jason.iwinski\Downloads\Tools\nuget.exe'

$WorkingDirectory = 'C:\BlockIIDrop'
$DropDirectory = Join-Path $WorkingDirectory '\_DROP'

class Project
{
    [ValidateNotNullOrEmpty()]
    [string]
    $Name
    
    [ValidateNotNullOrEmpty()]
    [string]
    $RelativePath

    [ValidateNotNullOrEmpty()]
    [string]
    $RelativeSolutionPath

    [ValidateNotNullOrEmpty()]
    [string]
    $RelativeBuildServerFolderPath

    [ValidateNotNullOrEmpty()]
    [string]
    $RelativeBuildServerBatchFilePath
}

class Workspace
{
    [ValidateNotNullOrEmpty()]
    [string]
    $Name
    
    [ValidateNotNullOrEmpty()]
    [string]
    $TeamCollection

    [ValidateNotNullOrEmpty()]
    [string]
    $ServerName

    [ValidateNotNullOrEmpty()]
    [string]
    $RelativePath

    [array]
    $Projects
}

class ProjectArtifactAssembly
{
    [ValidateNotNullOrEmpty()]
    [string]
    $ProjectName

    [System.IO.FileInfo]
    $File

    [System.Version]
    $Version
}

$Assemblies = @{}
$Workspaces = [System.Collections.ArrayList]@()

function Reset-Script
{
    try
    {
        $StopWatch.Stop()
    }
    catch
    {
        # Swallow
    }

    Write-Host 'Press any key to continue'
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
}
function Initialize-Workspaces
{
    Write-Host 'Initializing workspaces.' -ForegroundColor Blue

    $TempWorkspace = [Workspace]::new()
    $TempWorkspace.Name = 'Block II WST'
    $TempWorkspace.TeamCollection = 'http://ReDefNet-ag-tfs:8080/tfs/Block%20II%20WST'
    $TempWorkspace.ServerName = 'Integration'
    $TempWorkspace.RelativePath = '\BlockIIWST'
    $TempWorkspace.Projects = [System.Collections.ArrayList]@()
    
    $TempProject = [Project]::new()
    $TempProject.Name = 'MR-TCDL'
    $TempProject.RelativePath = '\MR-TCDL\MR-TCDL-Dev'
    $TempProject.RelativeSolutionPath = '\MR-TCDL.sln'
    $TempProject.RelativeBuildServerFolderPath = '\BuildServer'
    $TempProject.RelativeBuildServerBatchFilePath = '\PostBuildServerPrep.bat'
    $TempWorkspace.Projects += $TempProject

    $TempProject = [Project]::new()
    $TempProject.Name = 'TXP-A (DVS)'
    $TempProject.RelativePath = '\DVS\DVS-Dev'
    $TempProject.RelativeSolutionPath = '\Txpa\DVS.sln'
    $TempProject.RelativeBuildServerFolderPath = '\Txpa\BuildServer'
    $TempProject.RelativeBuildServerBatchFilePath = '\PostBuildServerPrep.bat'
    $TempWorkspace.Projects += $TempProject

    $TempProject = [Project]::new()
    $TempProject.Name = 'CML (DVS)'
    $TempProject.RelativePath = '\DVS\DVS-Dev'
    $TempProject.RelativeSolutionPath = '\Cml\Cml.sln'
    $TempProject.RelativeBuildServerFolderPath = '\Cml\BuildServer'
    $TempProject.RelativeBuildServerBatchFilePath = '\PostBuildServerPrep.bat'
    $TempWorkspace.Projects += $TempProject
    
    $TempProject = [Project]::new()
    $TempProject.Name = 'FAB-T'
    $TempProject.RelativePath = '\FAB-T\FAB-T-Dev'
    $TempProject.RelativeSolutionPath = '\FAB-T.sln'
    $TempProject.RelativeBuildServerFolderPath = '\BuildServer'
    $TempProject.RelativeBuildServerBatchFilePath = '\PostBuildServerPrep.bat'
    $TempWorkspace.Projects += $TempProject

    $TempProject = [Project]::new()
    $TempProject.Name = 'UPS Emergency Off'
    $TempProject.RelativePath = '\Reuse\ReDefNet.Wst.UPSEmergencyOff'
    $TempProject.RelativeSolutionPath = '\ReDefNet.Wst.UPSEmergencyOffDevice.sln'
    $TempProject.RelativeBuildServerFolderPath = '\BuildServer'
    $TempProject.RelativeBuildServerBatchFilePath = '\PostBuildServerPrep.bat'
    $TempWorkspace.Projects += $TempProject
    
    $script:Workspaces += $TempWorkspace
    
    $TempWorkspace = [Workspace]::new()
    $TempWorkspace.Name = 'WST 15.2'
    $TempWorkspace.TeamCollection = 'http://ReDefNet-ag-tfs:8080/tfs/TheIsland'
    $TempWorkspace.ServerName = 'Integration'
    $TempWorkspace.RelativePath = '\WST15.2'
    $TempWorkspace.Projects = [System.Collections.ArrayList]@()

    $TempProject = [Project]::new()
    $TempProject.Name = 'MAPS'
    $TempProject.RelativePath = '\MAPS'
    $TempProject.RelativeSolutionPath = '\MAPS.sln'
    $TempProject.RelativeBuildServerFolderPath = '\BuildServer'
    $TempProject.RelativeBuildServerBatchFilePath = '\PostBuildServerPrep.bat'
    $TempWorkspace.Projects += $TempProject

    $script:Workspaces += $TempWorkspace
}

function Initialize-Script
{
    Initialize-Workspaces

    Write-Host 'Checking for existence of commandline tools.' -ForegroundColor Blue

    if (![System.IO.File]::Exists("$TfsPath"))
    {
        Write-Error "Could not locate the TFS commandline utility at '$TfsPath'."

        Reset-Script

        exit 1
    }

    if (![System.IO.File]::Exists("$NuGetPath"))
    {
        Write-Error "Could not locate the NuGet commandline utility at '$NuGetPath'."

        Reset-Script

        exit 1
    }

    if (![System.IO.File]::Exists("$MSBuildPath"))
    {
        Write-Error "Could not locate the MSBuild commandline utility at '$MSBuildPath'."

        Reset-Script

        exit 1
    }

    Write-Host 'Preparing drop location.' -ForegroundColor Blue

    if ([System.IO.Directory]::Exists($DropDirectory))
    {
        try 
        {
            Remove-Item $DropDirectory -Recurse -Force
        }
        catch 
        {
            Write-Error "Removing existing drop location '$DropDirectory' threw an exception: $($_Exception.Message)"
        
            Reset-Script

            exit 1
        }
    }
    try 
    {
        mkdir $DropDirectory | Out-Null
    }
    catch 
    {
        Write-Error "Creating drop location '$DropDirectory' threw an exception: $($_Exception.Message)"

        Reset-Script

        exit 1
    }
}

function Update-Workspace
{
    param
    (
        [Parameter(Mandatory = $true)]
        [Workspace]
        $Workspace
    )

    Write-Host "Updating $($Workspace.Name) workspace." -ForegroundColor Blue
    
    $FullWorkspacePath = Join-Path $WorkingDirectory $Workspace.RelativePath

    Set-Location $FullWorkspacePath

    Write-Host "Current directory is now '$FullWorkspacePath'." -ForegroundColor DarkGray
    Write-Host "Undoing any pending changes."

    try
    {
        & $TfsPath vc undo * /recursive /workspace:"$($Workspace.ServerName)" /collection:"$($Workspace.TeamCollection)" /noprompt
    }
    catch
    {
        Write-Error "Undoing pending changes for workspace $($Workspace.Name) threw an exception: $($_Exception.Message)."

        Reset-Script

        exit 1
    }

    if ($LASTEXITCODE -gt 1)
    {
        Write-Error "Undoing pending changes for workspace $($Workspace.Name) returned exit code $LASTEXITCODE."

        Reset-Script

        exit 1
    }

    Write-Host "Getting latest changes from server."

    foreach ($Project in $Workspace.Projects)
    {
        $FullProjectPath = Join-Path $FullWorkspacePath $Project.RelativePath

        Set-Location $FullProjectPath

        Write-Host "Current directory is now '$FullProjectPath'." -ForegroundColor DarkGray

        Write-Host "Getting latest changes from server for project $($Project.Name)."

        try 
        {
            & $TfsPath vc get * /recursive /noprompt
        }
        catch 
        {
            Write-Error "Getting latest changes for project $($Project.Name) in workspace $($Workspace.Name) threw an exception: $($_Exception.Message)."

            Reset-Script

            exit 1
        }

        if ($LASTEXITCODE -gt 1)
        {
            Write-Error "Getting latest changes for project $($Project.Name) in workspace $($Workspace.Name) returned exit code $LASTEXITCODE."

            Reset-Script

            exit 1
        }
    }

    Set-Location $FullWorkspacePath

    Write-Host "Current directory is now '$FullWorkspacePath'." -ForegroundColor DarkGray

    Write-Host "Cleaning workspace."

    try 
    {
        & $TfsPath vc reconcile /clean /recursive /diff /noprompt *
    }
    catch 
    {
        Write-Error "Cleaning workspace $($Workspace.Name) threw an exception: $($_Exception.Message)."

        Reset-Script

        exit 1
    }

    if ($LASTEXITCODE -gt 1)
    {
        Write-Error "Cleaning workspace $($Workspace.Name) returned exit code $LASTEXITCODE."

        Reset-Script

        exit 1
    }

    Write-Host "Workspace $($Workspace.Name) was updated successfully." -ForegroundColor Green
}

function Update-AllWorkspaces
{
    Write-Host 'Updating all workspaces.' -ForegroundColor Blue

    foreach ($Workspace in $Workspaces)
    {
        Update-Workspace $Workspace
    }

    Write-Host 'All workspaces updated successfully.' -ForegroundColor Green
}

function Restore-AllProjectPackages
{
    Write-Host 'Restoring all NuGet packages.' -ForegroundColor Blue

    foreach ($Workspace in $Workspaces)
    {
        foreach ($Project in $Workspace.Projects)
        {
            Write-Host "Restoring NuGet packages for project $($Project.Name)." -ForegroundColor Blue

            $FullSolutionPath = Join-Path $WorkingDirectory $Workspace.RelativePath
            $FullSolutionPath = Join-Path $FullSolutionPath $Project.RelativePath
            $FullSolutionPath = Join-Path $FullSolutionPath $Project.RelativeSolutionPath

            try 
            {
                & $NuGetPath restore "$FullSolutionPath" -Verbosity quiet
            }
            catch 
            {
                Write-Error "Restoring NuGet packages for project $($Project.Name) in workspace $($Workspace.Name) threw an exception: $($_Exception.Message)."

                Reset-Script

                exit 1
            }

            if ($LASTEXITCODE -gt 0)
            {
                Write-Error "Restoring NuGet packages for project $($Project.Name) in workspace $($Workspace.Name) returned exit code $LASTEXITCODE."

                Reset-Script

                exit 1
            }

            Write-Host "NuGet packages for project $($Project.Name) restored successfully." -ForegroundColor Green
        }
    }

    Write-Host 'All NuGet packages restored successfully.' -ForegroundColor Green
}

function Update-AllProjectPackages
{
    Write-Host 'Updating all NuGet packages to latest.' -ForegroundColor Blue

    foreach ($Workspace in $Workspaces)
    {
        foreach ($Project in $Workspace.Projects)
        {
            Write-Host "Updating NuGet packages for project $($Project.Name)." -ForegroundColor Blue

            $FullSolutionPath = Join-Path $WorkingDirectory $Workspace.RelativePath
            $FullSolutionPath = Join-Path $FullSolutionPath $Project.RelativePath
            $FullSolutionPath = Join-Path $FullSolutionPath $Project.RelativeSolutionPath

            try 
            {
                & $NuGetPath update "$FullSolutionPath" -Verbosity quiet
            }
            catch 
            {
                Write-Error "Updating NuGet packages for project $($Project.Name) in workspace $($Workspace.Name) threw an exception: $($_Exception.Message)."

                Reset-Script

                exit 1
            }

            if ($LASTEXITCODE -gt 0)
            {
                Write-Error "Updating NuGet packages for project $($Project.Name) in workspace $($Workspace.Name) returned exit code $LASTEXITCODE."

                Reset-Script

                exit 1
            }

            Write-Host "NuGet packages for project $($Project.Name) updated successfully." -ForegroundColor Green
        }
    }

    Write-Host 'All NuGet packages restored successfully.' -ForegroundColor Green
}

function Invoke-AllProjectBuilds
{
    Write-Host 'Building all projects.' -ForegroundColor Blue

    foreach ($Workspace in $Workspaces)
    {
        foreach ($Project in $Workspace.Projects)
        {
            Write-Host "Building project $($Project.Name)." -ForegroundColor Blue

            $FullSolutionPath = Join-Path $WorkingDirectory $Workspace.RelativePath
            $FullSolutionPath = Join-Path $FullSolutionPath $Project.RelativePath
            $FullSolutionPath = Join-Path $FullSolutionPath $Project.RelativeSolutionPath

            try 
            {
                & $MSBuildPath "$FullSolutionPath" /property:Configuration=Release /verbosity:quiet
            }
            catch 
            {
                Write-Error "Building project $($Project.Name) in workspace $($Workspace.Name) threw an exception: $($_Exception.Message)."

                Reset-Script

                exit 1
            }

            if ($LASTEXITCODE -gt 0)
            {
                Write-Error "Building project $($Project.Name) in workspace $($Workspace.Name) returned exit code $LASTEXITCODE."

                Reset-Script

                exit 1
            }

            Write-Host "Project $($Project.Name) built successfully." -ForegroundColor Green
            Write-Host "Running post-build batch file on project $($Project.Name)." -ForegroundColor Blue

            $FullBuildServerPath = Join-Path $WorkingDirectory $Workspace.RelativePath
            $FullBuildServerPath = Join-Path $FullBuildServerPath $Project.RelativePath
            $FullBuildServerPath = Join-Path $FullBuildServerPath $Project.RelativeBuildServerFolderPath
            
            Set-Location $FullBuildServerPath

            Write-Host "Current directory is now '$FullBuildServerPath'." -ForegroundColor DarkGray

            $BuildServerBatchFilePath = Join-Path $FullBuildServerPath $Project.RelativeBuildServerBatchFilePath

            try 
            {
                cmd.exe /c "$BuildServerBatchFilePath"
            }
            catch 
            {
                Write-Error "Running post-build batch file for project $($Project.Name) in workspace $($Workspace.Name) threw an exception: $($_Exception.Message)."

                Reset-Script

                exit 1
            }

            if ($LASTEXITCODE -gt 0)
            {
                Write-Error "Running post-build batch file for project $($Project.Name) in workspace $($Workspace.Name) returned exit code $LASTEXITCODE."

                Reset-Script

                exit 1
            }

            Write-Host "Post-build batch file for project $($Project.Name) executed successfully." -ForegroundColor Green
        }
    }

    Write-Host 'All projects built successfully.' -ForegroundColor Green
}

function Get-AssemblyVersion
{
    param
    (
        [Parameter(Mandatory = $true)]
        [System.IO.FileInfo]
        $File
    )

    try 
    {
        $Assembly = [System.Reflection.Assembly]::LoadFile($File.FullName)
        $AssemblyName = $Assembly.GetName()
        $Version = $AssemblyName.Version

        return [System.Version]$Version
    }
    catch 
    {
        # Swallow

        return $null
    }
}

function Add-AssemblyVersion
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string]
        $ProjectName,

        [Parameter(Mandatory = $true)]
        [System.IO.FileInfo]
        $File,

        [Parameter(Mandatory = $true)]
        [System.Version]
        $Version
    )

    if ($script:Assemblies.ContainsKey($File.Name))
    {
        $TempProjectArtifact = [ProjectArtifactAssembly]::new()
        $TempProjectArtifact.ProjectName = $ProjectName
        $TempProjectArtifact.File = $File
        $TempProjectArtifact.Version = $Version

        $script:Assemblies["$($File.Name)"] += $TempProjectArtifact
    }
    else
    {
        $Temp = [System.Collections.ArrayList]@()

        $TempProjectArtifact = [ProjectArtifactAssembly]::new()
        $TempProjectArtifact.ProjectName = $ProjectName
        $TempProjectArtifact.File = $File
        $TempProjectArtifact.Version = $Version
        $Temp.Add($TempProjectArtifact) | Out-Null

        $script:Assemblies."$($File.Name)" = $Temp
    }
}
function Get-AllProjectAssemblyVersions
{
    Write-Host 'Analyzing assembly versions of all project build artifacts.' -ForegroundColor Blue

    foreach ($Workspace in $Workspaces)
    {
        foreach ($Project in $Workspace.Projects)
        {
            Write-Host "Analyzing assembly versions of project $($Project.Name) build artifacts." -ForegroundColor Blue

            $FullArtifactsPath = Join-Path $WorkingDirectory $Workspace.RelativePath
            $FullArtifactsPath = Join-Path $FullArtifactsPath $Project.RelativePath
            $FullArtifactsPath = Join-Path $FullArtifactsPath $Project.RelativeBuildServerFolderPath
            $FullArtifactsPath = Join-Path $FullArtifactsPath '\bin\Release'
            
            Set-Location $FullArtifactsPath

            Write-Host "Current directory is now '$FullArtifactsPath'." -ForegroundColor DarkGray

            foreach ($Dll in Get-ChildItem -Filter *.dll -Recurse)
            {
                [System.Version]$Version = Get-AssemblyVersion([System.IO.FileInfo]$Dll)

                if ($null -eq $Version)
                {
                    continue
                }

                Add-AssemblyVersion $Project.Name $Dll $Version
            }

            foreach ($Exe in Get-ChildItem -Filter *.exe -Recurse)
            {
                [System.Version]$Version = Get-AssemblyVersion([System.IO.FileInfo]$Dll)

                if ($null -eq $Version)
                {
                    continue
                }

                Add-AssemblyVersion $Project.Name $Dll $Version
            }

            Write-Host "Analyzed assembly versions of build artifacts for project $($Project.Name) successfully." -ForegroundColor Green
        }
    }

    Write-Host 'Assembly versions of all project build artifacts were analyzed successfully.' -ForegroundColor Green
}

function Find-AssemblyVersionConflicts
{
    Write-Host 'Finding assembly version conflicts.' -ForegroundColor Blue

    foreach ($Assembly in $Assemblies.GetEnumerator())
    {
        if ($Assembly.Value.Count -lt 2)
        {
            continue
        }

        [System.Version]$MaxAssemblyVersion = $null

        foreach ($ProjectAssembly in $Assembly.Value)
        {
            if ($null -eq $MaxAssemblyVersion)
            {
                $MaxAssemblyVersion = $ProjectAssembly.Version

                continue
            }

            $ComparisonResult = $MaxAssemblyVersion.CompareTo($ProjectAssembly.Version)

            if ($ComparisonResult -eq 0)
            {
                continue
            }

            if ($ComparisonResult -eq 1)
            {
                continue
            }

            if ($ComparisonResult -eq -1)
            {
                $MaxAssemblyVersion = $ProjectAssembly.Version

                continue
            }
        }

        $OutputFileHeader = $false
        $LocalDefaultVersion = [System.Version]::new(1, 0, 0, 0)
        
        foreach ($ProjectAssembly in $Assembly.Value)
        {
            $ComparisonResult = $MaxAssemblyVersion.CompareTo($ProjectAssembly.Version)

            if ($ComparisonResult -ne 0)
            {
                if ($LocalDefaultVersion.CompareTo($ProjectAssembly.Version) -eq 0)
                {
                    # Ignore the conflict if the version is 1.0.0.0, which means it's probably
                    # the project that generated the NuGet package
                    continue
                }

                if (!$OutputFileHeader)
                {
                    Write-Host "Found conflicting versions of file '$($Assembly.Key)'!" -ForegroundColor DarkYellow
                    Write-Host "`tLatest version is $($MaxAssemblyVersion.ToString()), but:" -ForegroundColor DarkYellow
                    
                    $OutputFileHeader = $true
                }

                Write-Host "`t`tProject $($ProjectAssembly.ProjectName) is using version $($ProjectAssembly.Version.ToString())" -ForegroundColor DarkYellow
            }
        }
    }

    Write-Host 'Finished finding assembly version conflicts.' -ForegroundColor Green
}

function Copy-AllProjectArtifacts
{
    Write-Host 'Copying all project artifacts to drop location.' -ForegroundColor Blue

    foreach ($Workspace in $Workspaces)
    {
        foreach ($Project in $Workspace.Projects)
        {
            Write-Host "Copying project $($Project.Name) artifacts." -ForegroundColor Blue

            $FullArtifactsPath = Join-Path $WorkingDirectory $Workspace.RelativePath
            $FullArtifactsPath = Join-Path $FullArtifactsPath $Project.RelativePath
            $FullArtifactsPath = Join-Path $FullArtifactsPath $Project.RelativeBuildServerFolderPath
            $FullArtifactsPath = Join-Path $FullArtifactsPath '\bin\Release'

            try 
            {
                robocopy "$FullArtifactsPath" "$DropDirectory" /s /r:3 /w:5 /np /ns /nc /nfl /ndl /njh /njs
            }
            catch 
            {
                Write-Error "Copying artifacts for project $($Project.Name) in workspace $($Workspace.Name) threw an exception: $($_Exception.Message)."

                Reset-Script

                exit 1
            }

            if ($LASTEXITCODE -gt 7)
            {
                Write-Error "Copying artifacts for project $($Project.Name) in workspace $($Workspace.Name) returned exit code $LASTEXITCODE."

                Reset-Script

                exit 1
            }

            Write-Host "Artifacts for project $($Project.Name) copied successfully." -ForegroundColor Green
        }
    }

    Write-Host 'All project artifacts were copied successfully.' -ForegroundColor Green
}

$StopWatch = New-Object "System.Diagnostics.Stopwatch"
$StopWatch.Start()

Initialize-Script
Update-AllWorkspaces
Restore-AllProjectPackages
Update-AllProjectPackages
Invoke-AllProjectBuilds
Get-AllProjectAssemblyVersions
Find-AssemblyVersionConflicts
Copy-AllProjectArtifacts

$StopWatch.Stop()

Write-Host 'Drop complete!' -ForegroundColor Green

$TotalDropTime = $StopWatch.Elapsed.ToString("c")

Write-Host "Created drop in $TotalDropTime."
Write-Host 'Press any key to continue'
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
