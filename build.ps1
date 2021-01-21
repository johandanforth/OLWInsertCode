

# setup a .net framework dev environment in vs code terminal

function Install-ModuleToDirectory {
    [CmdletBinding()]
    [OutputType('System.Management.Automation.PSModuleInfo')]
    param(
        [Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        $Name,

        [Parameter(Mandatory = $true)]
        #[ValidateScript({ Test-Path $_ })]
        [ValidateNotNullOrEmpty()]
        $Destination
    )

    # does directory exist?
    if (-not (Test-Path $Destination)) {
        # create dir
        New-Item -ItemType Directory -Force -Path $Destination
    }

    # Is the module already installed?
    if (-not (Test-Path (Join-Path $Destination $Name))) {
        # Install the module to the custom destination.
        Find-Module -Name $Name -Repository 'PSGallery' | Save-Module -Path $Destination
    }

    # Import the module from the custom directory.
    Import-Module -FullyQualifiedName (Join-Path $Destination $Name)

    return (Get-Module)
}

function Get-LatestMsbuildLocation
{
  Param 
  (
    [bool] $allowPreviewVersions = $false
  )
    if ($allowPreviewVersions) {
      $latestVsInstallationInfo = Get-VSSetupInstance -All -Prerelease | Sort-Object -Property InstallationVersion -Descending | Select-Object -First 1
    } else {
      $latestVsInstallationInfo = Get-VSSetupInstance -All | Sort-Object -Property InstallationVersion -Descending | Select-Object -First 1
    }
    Write-Host "Latest version installed is $($latestVsInstallationInfo.InstallationVersion)"
    if ($latestVsInstallationInfo.InstallationVersion -like "15.*") {
      $msbuildLocation = "$($latestVsInstallationInfo.InstallationPath)\MSBuild\15.0\Bin\msbuild.exe"
    
      Write-Host "Located msbuild for Visual Studio 2017 in $msbuildLocation"
    } else {
      $msbuildLocation = "$($latestVsInstallationInfo.InstallationPath)\MSBuild\Current\Bin\msbuild.exe"
      Write-Host "Located msbuild in $msbuildLocation"
    }

    return $msbuildLocation
}

Install-ModuleToDirectory -Name 'VSSetup' -Destination '.\PSModules'

$msbuildLocation = Get-LatestMsbuildLocation
set-alias msb $msbuildLocation 
## run msbuild.exe and restore nuget packages referenced
msb -t:restore 
msb 
