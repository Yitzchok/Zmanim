properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\lib"
  $build_dir = "$base_dir\build" 
  $buildartifacts_dir = "$build_dir\" 
  $sln_file = "$base_dir\Zmanim.sln" 
  $version = "1.2.0.0"
  $humanReadableversion = "1.2"
  $tools_dir = "$base_dir\Tools"
  $release_dir = "$base_dir\Release"
  $src_dir = "$base_dir\src"
  $sample_dir = "$base_dir\Samples"
} 

include .\psake_ext.ps1

task default -depends Release

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
		-clsCompliant "false" `
		-copyright "Copyright © Eliyahu Hershfeld 2010"
		
	Generate-Assembly-Info `
		-file "$src_dir\ZmanimTests\Properties\AssemblyInfo.cs" `
		-title "Zmanim Tests $version" `
		-description "Zmanim Test Library" `
		-company "" `
		-product "Zmanim Test $version" `
		-version $version `
		-copyright "Copyright © Adminjew 2010"
			
	new-item $release_dir -itemType directory 
	new-item $buildartifacts_dir -itemType directory 
#	cp $tools_dir\NUnit\*.* $build_dir
} 

task Compile -depends Init { 
	$v4_net_version = (ls "C:\Windows\Microsoft.NET\Framework\v4.0*").Name
    exec "C:\Windows\Microsoft.NET\Framework\$v4_net_version\MSBuild.exe" """$sln_file"" /p:OutDir=""$buildartifacts_dir\"""
} 

task Test -depends Compile {
  $old = pwd
  cd $build_dir
  exec "$tools_dir\NUnit\nunit-console.exe" "$build_dir\ZmanimTests.dll"
  cd $old
}

task Release -depends Test {
	& $tools_dir\zip.exe -9 -A -j $release_dir\Zmanim-$humanReadableversion.zip `
	$build_dir\Zmanim.dll `
	$build_dir\Zmanim.xml `
	$build_dir\IKVM.*.dll `
	lgpl.txt `
	acknowledgements.txt
	
	& $tools_dir\zip.exe -9 -A -j $release_dir\Zmanim.Cli-$humanReadableversion.zip `
	$build_dir\Zmanim.dll `
	$build_dir\Zmanim.xml `
	$build_dir\Zmanim.Cli.exe `
	$build_dir\IKVM.*.dll `
	lgpl.txt `
	acknowledgements.txt
	
	if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute ZIP command"
    }
}