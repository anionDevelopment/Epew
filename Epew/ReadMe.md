# Epew

## Purpose

This is the `Epew`-codeunit of the [Epew](../ReadMe.md)-repository: the .NET-implementation of the commandline-tool. See the repository-level [ReadMe.md](../ReadMe.md) for what Epew does and how to use it.

## Structure

The console-application is located in `Epew` and its testcases are located in `EpewTests`, tied together by `Epew.sln`.

- `Program.cs` contains the entry-point (`Main`), which only delegates to `Helper/ProgramStarter.cs`.
- `Helper/ProgramStarter.cs` parses the commandline-arguments (via `CommandLineParser`), dispatches to the matching verb and handles parsing-errors, `--help` and `--version`.
- `Helper/VerbVisitor.cs` dispatches a parsed verb to its runner.
- `Verbs/RunCLI.cs` and `Verbs/RunFile.cs` are the two supported verbs (arguments given on the commandline respectively read from a file); `Verbs/VerbBase.cs` defines the visitor-abstraction both implement.
- `Runner/RunWithArgumentsFromCLI.cs` and `Runner/RunWithArgumentsFromFile.cs` contain the actual execution-logic per verb; `Runner/RunBase.cs` is their common base-class.
- `EpewTests/Testcases/BasicTests.cs` contains the testcases.

## Build and test

The codeunit is built and tested by the scripts of the CommonProjectStructure, so by the pipeline of the repository (`task bb` in the repository-root) and not by calling `dotnet` directly. The scripts are `Other/Build/Build.py`, `Other/QualityCheck/Linting.py` and `Other/QualityCheck/RunTestcases.py`.

What they run:

- The build runs `dotnet build` for the runtimes declared in `Epew/Epew.csproj` (`win-x64`, `linux-x64`, `linux-arm64`, `osx-arm64`). The result is not self-contained (it requires the matching .NET-runtime to be installed) and is stored as artifact in `Other/Artifacts/BuildResult_DotNet_<runtime>`.
- The linting normalizes the `.cs`-files, formats the `.csproj`-files as XML, verifies that both `.csproj`-files match the standardized CommonProjectStructure-format (for example that every package-reference is pinned to exactly one version), and then builds the solution again to collect the compiler- and analyzer-diagnostics. It fails if any diagnostic has the severity "error".
- The testcases are run by `dotnet test` on the solution, using the coverage-collector configured in `runsettings.xml` ("XPlat Code Coverage", cobertura-format). The resulting coverage-file is stored in `Other/Artifacts/TestCoverage`, and the html-report, the badges and the check of the coverage-threshold are generated from that file.

## Run locally

After the build, run `Other/Artifacts/BuildResult_DotNet_win-x64/Epew.exe` (or the executable of the matching runtime). See the repository-level [ReadMe.md](../ReadMe.md) for the available commandline-arguments.

## Further information

- Which requirements have to be fulfilled to run the scripts of this codeunit is documented in [Hints.md](./Other/Reference/ReferenceContent/Hints.md).
