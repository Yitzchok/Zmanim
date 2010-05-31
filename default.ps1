include .\psake_ext.ps1
$framework = "4.0x86"

properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\lib"
  $build_dir = "$base_dir\build" 
  $buildartifacts_dir = "$build_dir\" 
  $sln_file = "$base_dir\Zmanim.sln" 
  $version = "1.2.1.0"
  $humanReadableversion = "1.2.1"
  $tools_dir = "$base_dir\Tools"
  $release_dir = "$base_dir\Release"
  $src_dir = "$base_dir\src"
  $sample_dir = "$base_dir\Samples"
  $sample_lib_dir = "$sample_dir\lib"
} 

task default -depends CopyBuildFilesToSampleDirectory

task Clean { 
  remove-item -force -recurse $buildartifacts_dir -ErrorAction SilentlyContinue 
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
} 

task Init -depends Clean {
	Generate-Assembly-Info `
		-file "$src_dir\Zmanim\Properties\AssemblyInfo.cs" `
		-title "Zmanim $version" `
		-description "Zmanim" `
		-company "" `
		-product "Zmanim $version" `
		-version $version `
		-copyright "Copyright © Eliyahu Hershfeld 2010"
		
	Generate-Assembly-Info `
		-file "$src_dir\Zmanim.Cli\Properties\AssemblyInfo.cs" `
		-title "Zmanim CLI $version" `
		-description "Zmanim CLI" `
		-company "" `
		-product "Zmanim CLI $version" `
		-version $version `
		-copyright "Copyright © Adminjew 2010"
		
	Generate-Assembly-Info `
		-file "$src_dir\ZmanimTests\Properties\AssemblyInfo.cs" `
		-title "Zmanim Tests $version" `
		-description "Zmanim Test Library" `
		-company "" `
		-product "Zmanim Test $version" `
		-version $version `
		-copyright "Copyright © Adminjew 2010"
			
	Generate-Assembly-Info `
		-file "$src_dir\Zmanim.Scheduling\Properties\AssemblyInfo.cs" `
		-title "Zmanim Scheduling $version" `
		-description "Zmanim Scheduling Library" `
		-company "" `
		-product "Zmanim Scheduling $version" `
		-version $version `
		-copyright "Copyright © Adminjew 2010"
		
	Generate-Assembly-Info `
		-file "$src_dir\Zmanim.QuartzScheduling\Properties\AssemblyInfo.cs" `
		-title "Zmanim Quartz Scheduling $version" `
		-description "Zmanim Quartz Scheduling Library" `
		-company "" `
		-product "Zmanim Quartz Scheduling $version" `
		-version $version `
		-copyright "Copyright © Adminjew 2010"

	Generate-Assembly-Info `
		-file "$src_dir\Zmanim.TzDatebase\Properties\AssemblyInfo.cs" `
		-title "Zmanim TzDatebase $version" `
		-description "Zmanim TzDatebase Library" `
		-company "" `
		-product "Zmanim TzDatebase $version" `
		-version $version `
		-copyright "Copyright © Adminjew 2010"
	
	Generate-Assembly-Info `
		-file "$src_dir\Zmanim.Java\Properties\AssemblyInfo.cs" `
		-title "Zmanim Java $version" `
		-description "Zmanim Java Library" `
		-company "" `
		-product "Zmanim Java $version" `
		-version $version `
		-copyright "Copyright © Adminjew 2010"
	
	Generate-Assembly-Info `
		-file "$src_dir\Zmanim.Silverlight\Properties\AssemblyInfo.cs" `
		-title "Zmanim Silverlight $version" `
		-description "Zmanim Silverlight" `
		-company "" `
		-product "Zmanim Silverlight $version" `
		-version $version `
		-copyright "Copyright © Eliyahu Hershfeld 2010"
		
	new-item $release_dir -itemType directory 
	new-item $buildartifacts_dir -itemType directory 
#	cp $tools_dir\NUnit\*.* $build_dir
} 

task Compile -depends Init { 
	exec { msbuild $sln_file /p:OutDir=""$buildartifacts_dir\"" }
} 

task Test -depends Compile {
	$old = pwd
	cd $build_dir
	exec { & $tools_dir\NUnit\nunit-console.exe "$build_dir\ZmanimTests.dll" /nodots }
	cd $old
}

task Release -depends Test {
	& $tools_dir\zip.exe -9 -A -j $release_dir\Zmanim-$humanReadableversion.zip `
	$build_dir\Zmanim.dll `
	$build_dir\Zmanim.xml `
	$base_dir\docs\Documentation.chm `
	lgpl.txt `
	acknowledgements.txt
	
	& $tools_dir\zip.exe -9 -A -j $release_dir\Zmanim.Cli-$humanReadableversion.zip `
	$build_dir\Zmanim.dll `
	$build_dir\Zmanim.xml `
	$build_dir\Zmanim.Cli.exe `
	$build_dir\Zmanim.TzDatebase.dll `
	lgpl.txt `
	acknowledgements.txt
	
	& $tools_dir\zip.exe -9 -A -j $release_dir\Zmanim.Silverlight-$humanReadableversion.zip `
	$build_dir\Zmanim.Silverlight.dll `
	$build_dir\Zmanim.Silverlight.xml `
	$base_dir\docs\Documentation.chm `
	lgpl.txt `
	acknowledgements.txt
	
	if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute ZIP command"
    }
}

task CopyBuildFilesToSampleDirectory -depends Release {
	Write-Host "Copying assemblies to sample lib directory."
	New-Item $sample_lib_dir -type directory -force
	Copy-Item $build_dir\Zmanim.dll $sample_lib_dir
	Copy-Item $build_dir\Zmanim.xml $sample_lib_dir
	Copy-Item $build_dir\Zmanim.TzDatebase.dll $sample_lib_dir
	Copy-Item lgpl.txt $sample_lib_dir
	Copy-Item acknowledgements.txt $sample_lib_dir
	
	New-Item "$sample_lib_dir\Silverlight" -type directory -force
	Copy-Item $build_dir\Zmanim.Silverlight.dll "$sample_lib_dir\Silverlight"
	Copy-Item $build_dir\Zmanim.Silverlight.xml "$sample_lib_dir\Silverlight"
}
