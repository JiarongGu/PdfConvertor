function Install-NativeDlls($project, $where, $filenames)
{
	$filenames |
		?{ ($project.ProjectItems | %{ $_.Name }) -notcontains $_ } |
		%{ join-path $where $_ } |
		%{ $project.ProjectItems.AddFromFile($_) }

# 'Copy if newer'
	$filenames |
		%{ $project.ProjectItems.Item($_).Properties.Item('CopyToOutputDirectory').Value = 2 }
}

function Remove-NativeDlls($project, $filenames)
{
	$items = $filenames |
		?{ ($project.ProjectItems | %{ $_.Name }) -contains $_ } |
		%{ $project.ProjectItems.Item($_) }

	$outDirs = $project.ConfigurationManager |
		%{ $_.Properties.Item('OutputPath').Value } |
		get-unique |
		%{ join-path $project.Properties.Item('FullPath').Value $_ } |
		?{ test-path $_ }

	$outDirs |
		%{
			$dir = $_
			$items | %{ join-path $dir $_.Name }
		} |
		?{ test-path $_ } |
		%{ rm $_ -ErrorAction Continue }

	$items | %{ $_.Remove() }
}
