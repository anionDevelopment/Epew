# Findings handling

This specification defines how findings are handled in this repository.
A finding is any result of a code-review, security-check, bug-search, performance-analysis or comparable investigation that describes a concrete issue in this product.

## Central collection

All findings are always collected in `Other/Reference/Reference.md` in the section `## Findings` in the table located there.
Findings must not be kept anywhere else in the repository; their single source of truth is that table.

## Identifier

Each finding has an identifier of the form `<prefix>-<ascending number>`.
The number is ascending per prefix, so each prefix has its own independent sequence (for example `SEC-01`, `SEC-02`, `BUG-01`).
An existing identifier of a finding is kept; if a finding has no identifier yet, a fitting new one is created.

The prefix is determined by the following rules, evaluated in this order, and the first matching rule wins:

- `SEC` if the finding is security-relevant. If not:
- `BUG` if the finding is a bug. If not:
- `PER` if the finding has an impact on the performance. If not:
- `OTH` for everything else.

## Table columns

The table in the `## Findings` section has exactly the following columns:

- `id`: the identifier of the finding as defined above.
- `state`: the current state of the finding (for example open, partially fixed, fixed).
- `description`: a description of the finding.
- `impact`: the impact of the finding depending on its topic (for example the security- or performance-impact, or whatever the consequence is).
- `notes`: all additional information, including anything that would otherwise belong into further columns.

## State

The state of a finding is always determined directly from the current source-code of the finding itself, never from documentation or comments.
Whenever findings are processed, the state is re-checked against the code and updated if it changed.

## Keeping findings

Fixed findings are never deleted; they are only marked with the state `fixed` once they are actually fixed.
Only if the state is `fixed`, the identifier in the `id`-column is struck through in markdown (for example `~~SEC-02~~`).
The other columns are never struck through.

## Formatting

The whole `## Findings` section is formatted with the `format-markdown-file`-skill after it has been changed.
