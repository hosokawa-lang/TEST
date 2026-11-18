param (
    [string]$AppName
)

if (-not $AppName) {
    Write-Host "Usage: Get-UninstallAndRun.ps1 -AppName 'ƒAƒvƒŠ–¼'"
    exit 1
}

$paths = @(
    "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
    "HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall",
    "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
)

$found = $false

foreach ($p in $paths) {
    try {
        $keys = Get-ChildItem $p -ErrorAction Stop
    } catch {
        continue
    }

    foreach ($key in $keys) {
        try {
            $item = Get-ItemProperty $key.PSPath -ErrorAction Stop
            if ($item.DisplayName -like "*$AppName*") {
                $found = $true
                $uninstall = $item.UninstallString
                Write-Host "----------------------------"
                Write-Host "DisplayName    : $($item.DisplayName)"
                Write-Host "DisplayVersion : $($item.DisplayVersion)"
                Write-Host "Publisher      : $($item.Publisher)"
                Write-Host "InstallLocation: $($item.InstallLocation)"
                Write-Host "UninstallString: $uninstall"
                Write-Host "WindowsInstaller: $($item.WindowsInstaller)"
                Write-Host "----------------------------"

            }
        } catch {
            continue
        }
    }
}

if (-not $found) {
    Write-Host "No matching application found for '$AppName'."
}

Write-Output $uninstall

