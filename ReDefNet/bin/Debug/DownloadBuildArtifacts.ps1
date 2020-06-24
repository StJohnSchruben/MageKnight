[CmdletBinding()]
param(
    [Parameter(Mandatory=$True)]
    [string]
    $teamCollectionName, # = 'Block%20II%20WST',

    [Parameter(Mandatory=$True)]
    [string]
    $teamProjectName, # = 'DVS',

    [Parameter(Mandatory=$True)]
    [string]
    $buildDefinitionName, # = 'CML%20CI%20Build',

    [Parameter(Mandatory=$True)]
    [string]
    $buildArtifactName, # = 'All%20Artifacts',

    [Parameter(Mandatory=$True)]
    [string]
    $artifactDestinationFolder, # = 'C:\Users\some.user\Downloads\artifacts',

    [Parameter(Mandatory=$True)]
    [string]
    $artifactTempFolder # = 'C:\Users\some.user\Downloads\temp'
)

$ErrorActionPreference = "Stop"

Write-Output "*** Downloading Build Artifacts Script ***"
Write-Output ('--> Team collection name: ' + $teamCollectionName)
Write-Output ('--> Team project name: ' + $teamProjectName)
Write-Output ('--> Build definition: ' + $buildDefinitionName)
Write-Output ('--> Build artifact name: ' + $buildArtifactName)
Write-Output ('--> Artifact destination folder: ' + $artifactDestinationFolder)
Write-Output ('--> Artifact temp folder: ' + $artifactTempFolder)

$tfsUrl = 'http://ReDefNet-ag-tfs:8080/tfs/' + $teamCollectionName + '/' + $teamProjectName + '/'
Write-Output ('TFS URL: ' + $tfsUrl)

$buildDefinitionsUrl = $tfsUrl + '_apis/build/definitions?api-version=2.0&name=' + $buildDefinitionName
Write-Output ('Build definitions URL: ' + $buildDefinitionsUrl)

Write-Output ('Getting build definitions.')
$buildDefinitions = Invoke-RestMethod -Uri $buildDefinitionsUrl -Verbose -Method Get -Headers @{ Authorization = "Bearer $env:SYSTEM_ACCESSTOKEN"
}

$buildDefinitionId = ($buildDefinitions.value).id;
Write-Output ('Build definition ID: ' + $buildDefinitionId)

$latestCompletedBuildUrl = $tfsUrl + '_apis/build/builds?definitions=' + $buildDefinitionId + '&statusFilter=completed&resultFilter=succeeded&$top=1&api-version=2.0'
Write-Output ('Latest completed build URL: ' + $latestCompletedBuildUrl)

Write-Output ('Getting latest completed build information.')
$builds = Invoke-RestMethod -Uri $latestCompletedBuildUrl -Verbose -Method Get -Headers @{ Authorization = "Bearer $env:SYSTEM_ACCESSTOKEN"
}

$buildId = ($builds.value).id;
Write-Output ('Build ID: ' + $buildId)

$buildArtifactsUrl = $tfsUrl + '_apis/build/builds/' + $buildId + '/artifacts?api-version=2.0'
Write-Output ('Build artifacts URL: ' + $buildArtifactsUrl)

Write-Output ('Getting all artifact download URLs.')
$allArtifactDownloadUris = (Invoke-RestMethod -Uri $buildArtifactsUrl -Verbose -Method Get -Headers @{ Authorization = "Bearer $env:SYSTEM_ACCESSTOKEN"
}).Value.Resource.downloadUrl
Write-Output ('All artifact download URLs: ' + $allArtifactDownloadUris)

Write-Output ('Processing artifacts...')

$artifactCount = 0
foreach ($artifactDownloadUri in $allArtifactDownloadUris) {
    if ($artifactDownloadUri.Contains($buildArtifactName)) {
        $artifactCount = $artifactCount + 1

        Write-Output ('--> Artifact #' + $artifactCount + ': Artifact download URL: ' + $artifactDownloadUri)

        $tempFolder = ([System.IO.Path]::GetRandomFileName()).Split('.')[0]
        Write-Output ('--> Artifact #' + $artifactCount + ': Temp artifact folder name: ' + $tempFolder)

        $zipFileName = 'artifacts.zip'
        Write-Output ('--> Artifact #' + $artifactCount + ': Artifact zip file name: ' + $zipFileName)

        $zipFolder = Join-Path $artifactTempFolder $tempFolder
        Write-Output ('--> Artifact #' + $artifactCount + ': Artifact unzip folder: ' + $zipFolder)

        $zipPath = Join-Path $zipFolder $zipFileName
        Write-Output ('--> Artifact #' + $artifactCount + ': Full artifact zip file path: ' + $zipPath)

        $zipSubFolder = Join-Path $zipFolder ([Uri]::UnescapeDataString($buildArtifactName))
        Write-Output ('--> Artifact #' + $artifactCount + ': Archive unzipped subfolder to copy: ' + $zipSubFolder)

        Write-Output ('--> Artifact #' + $artifactCount + ': Creating artifact temp folder if it doesn''t exist.')
        New-Item -ItemType Directory -Force -Path $zipFolder -Verbose

        Write-Output ('--> Artifact #' + $artifactCount + ': Downloading archive file...')
        Invoke-WebRequest -Uri $artifactDownloadUri -Verbose -OutFile $zipPath -Headers @{ Authorization = "Bearer $env:SYSTEM_ACCESSTOKEN"
        }

        Write-Output ('--> Artifact #' + $artifactCount + ': Archive file downloaded. Unzipping...')
        Add-Type -AssemblyName 'System.IO.Compression.FileSystem'
        [System.IO.Compression.ZipFile]::ExtractToDirectory($zipPath, $zipFolder)

        Write-Output ('--> Artifact #' + $artifactCount + ': Extraction complete. Copying artifact unzipped sub-folder to destination.')
        Copy-Item -Path $zipSubFolder -Destination $artifactDestinationFolder -Recurse -Force -Verbose

        Write-Output ('--> Artifact #' + $artifactCount + ': Copy complete. Finished processing artifact.')
    }
}