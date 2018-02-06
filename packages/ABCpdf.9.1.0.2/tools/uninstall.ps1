param($installPath, $toolsPath, $package, $project)

. (join-path $toolsPath 'props.ps1')
. (join-path $toolsPath 'funcs.ps1')

remove-nativedlls $project $nativeDlls
