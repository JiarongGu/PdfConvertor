param($installPath, $toolsPath, $package, $project)

. (join-path $toolsPath 'props.ps1')
. (join-path $toolsPath 'funcs.ps1')

function say($x)
{
    write-host "ABCpdf> $x"
}

$installBin = join-path $installPath 'bin'
$installLib = join-path $installPath (join-path 'lib' 'net40')

install-nativedlls $project $installBin $nativeDlls

say "Thank you for installing!"
say ""
say "For documentation and example projetcs, please download installer bundle from http://www.websupergoo.com/download.htm"

cp (join-path $installLib 'ABCpdf.dll') $installBin
$pdfSettingsExe = join-path $installBin 'PDFSettings.exe'

if([System.Environment]::UserInteractive)
{
    & $pdfSettingsExe "/Register"
}
else
{
    say "Run $pdfSettingsExe to check your license information"
}
