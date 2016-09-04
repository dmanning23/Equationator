rm *.nupkg
nuget pack .\Equationator.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget push *.nupkg