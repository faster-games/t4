param (
		[string]
		$Version = "2.2.1"
)

$WorkingDirectory="dist/.t4"
$LibDirectory="Editor/TextTemplating"

try {
	Remove-Item -Recurse -Force "$WorkingDirectory"
} catch {}

if (!(Test-Path $WorkingDirectory))
{
	[System.IO.Directory]::CreateDirectory($WorkingDirectory)
}


Invoke-WebRequest -Uri "https://github.com/mono/t4/archive/refs/tags/v$Version.zip" -OutFile "$WorkingDirectory/lib.zip"
Expand-Archive -Path "$WorkingDirectory/lib.zip" -DestinationPath "$WorkingDirectory/lib/"
Copy-Item -Recurse -Path "$WorkingDirectory/lib/t4-$Version/Mono.TextTemplating/" -Destination "$WorkingDirectory/TextTemplating/"
Remove-Item -Recurse -Force "$WorkingDirectory/lib"
Remove-Item -Force "$WorkingDirectory/TextTemplating/Mono.TextTemplating.csproj"
Remove-Item -Force "$WorkingDirectory/TextTemplating/AssemblyInfo.cs"
Remove-Item -Force "$WorkingDirectory/lib.zip"
try {
	Remove-Item -Recurse -Force "$LibDirectory/"
} catch {}
Copy-Item -Recurse -Path "$WorkingDirectory/TextTemplating/" -Destination "$LibDirectory/"
