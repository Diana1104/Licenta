
$outputPath = "$PSSCriptRoot\Output"

if (Test-Path $outputPath) { Remove-Item $outputPath -Force -Recurse }

$msBuildExe = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"

$solutionPath = "$PSSCriptRoot\Licenta.ORM.sln"
        
& "$($msBuildExe)" "$($solutionPath)" /p:OutputPath=$outputPath /m

$sqlScriptPath = "$PSSCriptRoot\DB.sql"

Invoke-Sqlcmd -InputFile $sqlScriptPath -ServerInstance ".\SQLEXPRESS"

$password = ConvertTo-SecureString -String "1234" -Force -AsPlainText

$certificate = Import-PfxCertificate -FilePath "$PSSCriptRoot\Licenta.pfx" -CertStoreLocation Cert:\LocalMachine\My -Password $password

[System.Reflection.Assembly]::LoadFile("$outputPath\Licenta.ORM.dll")

[Licenta.ORM.AesConfiguration]::CreateDefault($certificate.Thumbprint, "$outputPath\crypto.config")