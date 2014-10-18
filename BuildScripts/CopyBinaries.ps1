if (!$env:APPVEYOR) {
    $env:CONFIGURATION = "Release";
}

function CopyBinaries($architecture) {
    Write-Output "Copying binaries for $architecture CPU architecture..."
    if (Test-Path bin/$architecture) {
        Remove-Item bin/$architecture -recurse -force
    }
    mkdir bin/$architecture;
    Copy-Item Gw2Plugin/bin/$env:CONFIGURATION/* -destination bin/$architecture -recurse
    Copy-Item lib/$architecture/* -destination bin/$architecture -recurse
}

CopyBinaries("x86");
CopyBinaries("x64");
