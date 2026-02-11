# Session Summary: Create Technical Debt Prompt

**Session ID**: identify-tech-debt-prompt-creation
**Date**: 2026-02-11
**Operator**: GitHub Copilot
**Model**: Gemini 3 Pro (Preview)
**Duration**: 00:05:00

## Objective

Create a standardized prompt file to facilitate the identification of technical debt within the repository.

## Work Completed

### Primary Deliverables

1.  **Technical Debt Prompt** (`.github/prompts/technical-debt-identifier.prompt.md`)
    - Defined a structured prompt for AI agents to analyze the codebase.
    - Included steps for scanning comments (TODO, FIXME), analyzing architecture, and reviewing code quality.
    - Specified the output format for the technical debt report.

## Key Decisions

### Prompt Structure

**Decision**: Use `agent` mode.
**Rationale**:
- The task involves scanning multiple files and generating a comprehensive report, which is best suited for an autonomous agent workflow.

### Output Location

**Decision**: `.github/prompts/`
**Rationale**: Adheres to the repository's convention for storing prompt files.

## Artifacts Produced

| Artifact | Type | Purpose |
| :--- | :--- | :--- |
| `.github/prompts/technical-debt-identifier.prompt.md` | Prompt File | Instructions for finding technical debt |

## Next Steps

### Immediate

- Run the prompt to generate the initial technical debt report.

## Chat Metadata

```yaml
chat_id: identify-tech-debt-prompt-creation
started: 2026-02-11T12:00:00Z
ended: 2026-02-11T12:05:00Z
total_duration: 00:05:00
operator: GitHub Copilot
model: Gemini 3 Pro (Preview)
artifacts_count: 1
files_modified: 1
```

---

**Summary Version**: 1.0.0
**Created**: 2026-02-11T12:05:00Z
**Format**: Markdown
