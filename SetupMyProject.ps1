try {
    choco --version
    write-host "`Found Chocolatey: " -fore yellow
}
catch {
    write-host "`Installing Chocolatey: " -fore yellow
    Invoke-WebRequest https://chocolatey.org/install.ps1 -UseBasicParsing | Invoke-Expression
    if ($env:Path -split ';' -notcontains $env:ALLUSERSPROFILE + "\chocolatey\bin") {
        [Environment]::SetEnvironmentVariable("Path", $env:Path + ";%ALLUSERSPROFILE%\chocolatey\bin", "User")
    }
}

choco install sqllocaldb -yr