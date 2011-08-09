root_folder = File.expand_path("#{File.dirname(__FILE__)}/..")
require "buildscripts/project_details"

# The folders array denoting where to place build artifacts

FOLDERS = {
  :root => root_folder,
  :src => "src",
  :build => "build",
  :binaries => "placeholder - environment.rb sets this depending on target",
  :tools => "tools",
  :tests => "build/tests",
  :nuget => "build/nuget",
  :nuspec => "build/nuspec"
}

FOLDERS.merge({

  :a => {
      :test_dir => 'QuartzTesting.Tests',
      :nuspec => '#{FOLDERS[:nuspec]}/#{PROJECTS[a][:nuget_key]}',
      :out => 'placeholder - environment.rb will sets this',
      :test_out => 'placeholder - environment.rb sets this'
  },
  
})

FILES = {
  :sln => "src/ConsoleApplication1.sln",
  
  :a => {
    :nuspec => File.join(FOLDERS[:a][:nuspec], "#{PROJECTS[:a][:nuget_key]}.nuspec")
  },
  
}

COMMANDS = {
  :nunit => File.join(FOLDERS[:nunit], "nunit-console.exe"),
  :nuget => File.join(FOLDERS[:tools], "NuGet.exe"),
  :ilmerge => File.join(FOLDERS[:tools], "ILMerge.exe")
}

URIS = {
  :nuget_offical => "http://packages.nuget.org/v1/"
}