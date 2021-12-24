var target = Argument("target", "Build");
var configuration = Argument("configuration", "Debug");
var solution = "./OperationResultExtensions.sln";
var nugetKey = EnvironmentVariable("NUGET_KEY");
var nugetSource = EnvironmentVariable("NUGET_SOURCE");
var nugetVersion = "1.0.0"; 

Task("Build")
    .Does(() =>
{
    DotNetCoreBuild(solution, new DotNetCoreBuildSettings
    {
        Verbosity = DotNetCoreVerbosity.Minimal,
        Configuration = configuration
    });
});

Task("Publish")
	.IsDependentOn("Build")
    .Does(() =>
{
    NuGetPush($"./src/OperationResult/bin/{configuration}/Divino.OperationResultExtensions.{nugetVersion}.nupkg", new NuGetPushSettings {
        Source = nugetSource,
        ApiKey = nugetKey,
        SkipDuplicate = true
    });
});

RunTarget(target);