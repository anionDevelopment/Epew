# Epew

This is the repository-level reference. See the [ReadMe.md](../../ReadMe.md) for what Epew is and how to use it, and [Epew/Other/Reference/ReferenceContent](../../Epew/Other/Reference/ReferenceContent) for the technical (API-)reference of the `Epew`-codeunit.

## Technical documentation

There is currently no further repository-level technical documentation besides the generated lines-of-code-diagram in `Technical/Diagrams`.

## Features

| id | state | description |
| --- | --- | --- |
| feat-1 | available | Wrap program-calls and add timestamps to the output. |
| feat-2 | available | Pass arguments base64-encoded to avoid escaping-problems (`--ArgumentIsBase64Encoded`). |
| feat-3 | available | Capture stdout, stderr and exitcode of the executed program into files (`--StdOutFile`, `--StdErrFile`, `--ExitCodeFile`). |
| feat-4 | available | Abort the executed program after a configurable timeout (`--TimeoutInMilliseconds`). |
| feat-5 | available | Print output to the console and log it into a file at the same time, with optional log-overhead and log-rotation. |
| feat-6 | available | Resolve the program via the `PATH`-environment-variable and resolve environment-variables. |
| feat-7 | available | Run the program synchronously or asynchronously (`--NotSynchronous`). |
| feat-8 | available | Run the program as a given user (`--User`, `--Password`). |
| feat-9 | available | Run with arguments specified in a file via the `RunFile`-verb. |
| feat-10 | available | Available for Windows, Linux and macOS. |
| feat-11 | unavailable | Installation via winget (Windows). |
| feat-12 | unavailable | Installation via apt (Linux). |

## Findings

This section collects all findings of this repository.
The rules for how findings are handled are defined in the [findings-handling specification](./FindingsHandling.md).

| Id    | Priority | State | Description                                                                                                                     | Impact                                                                                                                                                                                                                                                  | Notes                                                                                                                                                                                                                                                                                                                                                                                     |
| ----- | -------- | ----- | ------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| SEC-1 | low      | open  | The credentials for the `--User`- and `--Password`-options are passed as plaintext command-line arguments.                      | On most operating systems the command-line arguments of a running process are readable by other local processes and users (process-table), and they are commonly persisted in the shell-history, so the password can be disclosed to other local users. | By-design limitation of the current CLI-interface (the `--Password`-option is documented in the ReadMe). A mitigation would be to accept the password via an environment-variable or via stdin instead of as a visible argument. Source: `Epew/Epew/Verbs/RunCLI.cs` (`Password`-option), consumed in `Epew/Epew/Runner/RunWithArgumentsFromCLI.cs`. Severity: low (local exposure only). |
| BUG-1 | mid      | open  | The `--PrintErrorsAsInformation` (`-i`) option is declared and documented but never evaluated during the execution.             | The documented behaviour ("treat errors as information") has no effect; stderr is always handled the same way, so users relying on this switch get a behaviour that differs from the documentation.                                                     | Defined in `Epew/Epew/Verbs/RunCLI.cs` and never referenced in `Epew/Epew/Runner/RunWithArgumentsFromCLI.cs`. Non-trivial because implementing it requires adjusting how stderr is logged and treated. Verified to still be present in the current code.                                                                                                                                  |
| OTH-1 | low      | open  | The top-level `catch` in `Program.Main` swallows every exception and returns the fatal-error exitcode without logging anything. | An unexpected error terminates Epew with exitcode 2147393801 and no output, which makes such failures hard to diagnose.                                                                                                                                 | Located in `Epew/Epew/Helper/ProgramStarter.cs` (`Main`). The inner execution-layer does log its own errors, so this only concerns errors outside `RunImplementation`. Severity: low.                                                                                                                                                                                                     |
