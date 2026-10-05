using Epew.Core.Helper;
using Epew.Core.Verbs;
using GRYLibrary.Core.ExecutePrograms;
using System.IO;

namespace Epew.Core.Runner
{
    internal class RunWithArgumentsFromFile :RunBase
    {
        private readonly RunFile _Options;

        public RunWithArgumentsFromFile(ProgramStarter programStarter, RunFile options) : base(programStarter)
        {
            this._Options = options;
        }

        /// <summary>
        /// Reads the commandline-arguments from <see cref="RunFile.File"/> (a relative path is resolved against the
        /// current working-directory, and any line-breaks in the file-content are removed so that the arguments end
        /// up on a single line) and re-runs Epew itself synchronously with these arguments. The re-run instance's own
        /// stdout/stderr are captured internally but not forwarded anywhere, since neither a log-target nor an
        /// output-file is configured for it.
        /// </summary>
        /// <returns>The exitcode of the re-run Epew-invocation.</returns>
        protected override int RunImplementation()
        {
            string argumentsFile = this._Options.File;
            string currentFolder = Directory.GetCurrentDirectory();
            if(GRYLibrary.Core.Misc.Utilities.IsRelativeLocalFilePath(argumentsFile))
            {
                argumentsFile = GRYLibrary.Core.Misc.Utilities.ResolveToFullPath(argumentsFile, currentFolder);
            }
            string argumentAsString = File.ReadAllText(argumentsFile).Replace("\r", string.Empty).Replace("\n", string.Empty);
            ExternalProgramExecutor externalProgramExecutor = new ExternalProgramExecutor(new ExternalProgramExecutorConfiguration()
            {
                Program = "epew",
                Argument = argumentAsString,
                WorkingDirectory = currentFolder,
            });
            externalProgramExecutor.Run();
            return externalProgramExecutor.ExitCode;
        }
    }
}
