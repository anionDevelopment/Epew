using Epew.Core.Helper;

namespace Epew.Core.Runner
{
    public abstract class RunBase
    {
        protected readonly ProgramStarter _ProgramStarter;

        protected RunBase(ProgramStarter programStarter)
        {
            this._ProgramStarter = programStarter;
        }

        /// <summary>
        /// Publishes this instance as <see cref="ProgramStarter.Result"/> (so callers, for example tests, can inspect
        /// the concrete runner after the execution) and then executes the verb.
        /// </summary>
        /// <returns>The exitcode of the executed program, or one of the fixed Epew-exitcodes documented in the ReadMe if the program could not be executed as requested.</returns>
        public int Run()
        {
            this._ProgramStarter.Result = this;
            return this.RunImplementation();
        }

        /// <summary>
        /// Executes the concrete verb. Implementations must not throw for expected failure-conditions (for example an
        /// invalid workingdirectory or a program which can not be started); such cases are reported via the returned
        /// exitcode instead so that Epew itself always terminates with a defined exitcode.
        /// </summary>
        protected abstract int RunImplementation();
    }
}
