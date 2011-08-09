require "rubygems"
require "bundler"
Bundler.setup
$: << './'

require 'albacore'
require 'rake/clean'
require 'semver'

require 'buildscripts/utils'
require 'buildscripts/paths'
require 'buildscripts/project_details'
require 'buildscripts/environment'

# to get the current version of the project, type 'SemVer.find.to_s' in this rake file.

# task :default => :msbuild
desc "build sln file"
nugetpack :msbuild do |msb|
   msb.solution   = "src/ConsoleApplication1.sln"
   msb.properties :Configuration => :Debug
   msb.targets    :Clean, :Build
end
