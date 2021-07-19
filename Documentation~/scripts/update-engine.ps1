param (
		[string]
		$Version = "2.0.5",

		[bool]
		$UpdateLib = $true,

		[bool]
		$UpdateTests = $true
)

$WorkingDirectory="dist/.t4"
$LibDirectory="Editor/TextTemplating"
$TestDirectory="Tests/Editor/TextTemplating"

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
Copy-Item -Recurse -Path "$WorkingDirectory/lib/t4-$Version/Mono.TextTemplating.Tests/" -Destination "$WorkingDirectory/TextTemplating.Tests/"
Remove-Item -Recurse -Force "$WorkingDirectory/lib"
Remove-Item -Force "$WorkingDirectory/TextTemplating/Mono.TextTemplating.csproj"
Remove-Item -Force "$WorkingDirectory/TextTemplating.Tests/Mono.TextTemplating.Tests.csproj"
Remove-Item -Force "$WorkingDirectory/TextTemplating.Tests/MSBuildErrorParserTests.cs"
Remove-Item -Force "$WorkingDirectory/TextTemplating/AssemblyInfo.cs"
Remove-Item -Force "$WorkingDirectory/lib.zip"

if ($UpdateLib)
{
	try {
		Remove-Item -Recurse -Force "$LibDirectory/"
	} catch {}
	Copy-Item -Recurse -Path "$WorkingDirectory/TextTemplating/" -Destination "$LibDirectory/"

	git am Documentation~/scripts/file-utils.patch
}

if ($UpdateTests)
{
	try {
		Remove-Item -Recurse -Force "$TestDirectory/"
	} catch {}
	Copy-Item -Recurse -Path "$WorkingDirectory/TextTemplating.Tests/" -Destination "$TestDirectory/"

	git am Documentation~/scripts/lib-visibility.patch
}
