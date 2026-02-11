# AI Conversation Log

- Chat ID: identify-tech-debt-prompt-creation
- Operator: GitHub Copilot
- Model: Gemini 3 Pro (Preview)
- Started: 2026-02-11T12:00:00Z
- Ended: 2026-02-11T12:05:00Z
- Total Duration: 00:05:00

## Context

- Inputs: User request to create a prompt file for identifying technical debt.
- Targets: `.github/prompts/technical-debt-identifier.prompt.md`
- Constraints/Policies: `.github/instructions/prompt-file.instructions.md`, `.github/instructions/ai-assisted-output.instructions.md`

## Exchanges

### Exchange 1

[2026-02-11T12:00:00Z] User

```text
Can you help me create a prompt file for identify what technical debt we have in this repo
```

[2026-02-11T12:05:00Z] GitHub Copilot

```text
I will create a new prompt file at `.github/prompts/technical-debt-identifier.prompt.md` that instructs an AI agent to analyze the codebase for technical debt. I will follow the guidelines in `prompt-file.instructions.md`.
```

## Work Burst Closure

**Artifacts Produced**:

- `.github/prompts/technical-debt-identifier.prompt.md` - A prompt file for identifying technical debt.

**Next Steps**:

- [ ] Use the prompt to generate a technical debt report.

**Duration Summary**:

- file creation: 00:05:00
- Total: 00:05:00
