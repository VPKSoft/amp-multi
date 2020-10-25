<#
MIT License

Copyright (c) 2020 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
#>

Write-Output "Init GitHub release..."

$output_file = "amp\CryptEnvVar.exe"

$download_url = "https://www.vpksoft.net/toolset/CryptEnvVar.exe"

$output_file_signtool = "signtool.exe"

Write-Output "Download file:  $download_url ..."
# No need to remove this: Remove-Item $output_file
(New-Object System.Net.WebClient).DownloadFile($download_url, $output_file)
Write-Output "Download done."

$output_file_signtool = "amp\CryptEnvVar.exe"
$download_url = "https://www.vpksoft.net/toolset/signtool.exe"

Write-Output "Download file:  $output_file_signtool ..."
# No need to remove this: Remove-Item $output_file
(New-Object System.Net.WebClient).DownloadFile($output_file_signtool, $output_file_signtool)
Write-Output "Download done."

# application parameters..
$application = "amp"
$environment_cryptor = "CryptEnvVar.exe"

# create the digital signature..
$arguments = @("-s", $Env:SECRET_KEY, "-e", "CERT_1;CERT_2;CERT_3;CERT_4;CERT_5;CERT_6;CERT_7;CERT_8", "-f", "C:\vpksoft.pfx", "-w", "80", "-i", "-v")
& (-join($application, "\", $environment_cryptor)) $arguments

$signtool = (-join($application, "\", $output_file_signtool))

# register the certificate to the CI image..
$certpw=ConvertTo-SecureString $Env:PFX_PASS –asplaintext –force 
Import-PfxCertificate -FilePath "C:\vpksoft.pfx" -CertStoreLocation Cert:\LocalMachine\My -Password $certpw | Out-Null

$release_exe = "..\amp\bin\Release\net47\win10-x64\amp.exe"

# sign and release tags..
#if ([string]::IsNullOrEmpty($Env:CIRCLE_TAG)) # only release for tags..
#{
    $files = Get-ChildItem $Env:CIRCLE_WORKING_DIRECTORY -r -Filter *amp*.msi # use the mask to discard possible third party packages..
    for ($i = 0; $i -lt $files.Count; $i++) 
    { 
        $file = $files[$i].FullName

        # sign the MSI installer packages (SHA256).
	    Write-Output (-join("Signing installer package (MSI, SHA256): ", $file, " ..."))

        $arguments = @("sign", "/f", "C:\vpksoft.pfx", "/p", $Env:PFX_PASS, "/fd", "sha256", "/tr", "http://timestamp.comodoca.com/?td=sha256", "/td", "sha256", "/as", "/v", $file)

	    # on the second time, something about 'Keyset does not exist'. TODO::Clean temp?
        & $signtool $arguments > null 2>&1
	    Write-Output (-join("Installer package (MSI) signed: ", $file, "."))

        # After signing, clean up the temporary folder, if this helps with the multiple package signing..
        Remove-Item -Recurse -Force (-join($Env:LocalAppData, "\Temp\*.*"))

        # sign the MSI installer packages (SHA1).
	    Write-Output (-join("Signing installer package (MSI, SHA1): ", $file, " ..."))

        $arguments = @("sign", "/f", "C:\vpksoft.pfx", "/p", $Env:PFX_PASS, "/t", "http://timestamp.comodoca.com/?td=sha256", "/v", $file)

	    # on the second time, something about 'Keyset does not exist'. TODO::Clean temp?
        & $signtool $arguments > null 2>&1
	    Write-Output (-join("Installer package (MSI) signed: ", $file, "."))

        # After signing, clean up the temporary folder, if this helps with the multiple package signing..
        Remove-Item -Recurse -Force (-join($Env:LocalAppData, "\Temp\*.*"))

	    Write-Output (-join("Publishing release:", $file, " ..."))
        $version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($release_exe)
        $versionString = (-join("v.", $version.FileMajorPart, ".", $version.FileMinorPart, ".", $version.FileBuildPart))

        ghr -t ${GITHUB_TOKEN} -u ${CIRCLE_PROJECT_USERNAME} -r ${CIRCLE_PROJECT_REPONAME} -c ${CIRCLE_SHA1} -delete $versionString ./

	    Write-Output (-join("Package released:", $file, "."))
    }
    Write-Output "Release."
#}
<#
else
{
    Write-Output (-join("PR detected, no package publish: ", $Env:CIRCLE_PR_NUMBER))
}
#>