# AETHER SENTINEL AI GitHub Workflow

## Purpose

This document defines repository management rules for AETHER SENTINEL AI.

The workflow is designed for maintainability, clear history, structured planning, and safe release preparation.

## Required Repository Files

Root-level required files:

- `README.md`
- `CHANGELOG.md`
- `ROADMAP.md`
- `ARCHITECTURE.md`
- `VERSION`

Canonical documentation files:

- `docs/ARCHITECTURE.md`
- `docs/BRAND_GUIDELINE.md`
- `docs/UI_DESIGN_SYSTEM.md`
- `docs/GITHUB_WORKFLOW.md`
- `docs/MILESTONES.md`
- `docs/PHASE-00-REPORT.md`

## Branching

Recommended branch types:

- `main`: stable project state.
- `phase/phase-xx-name`: phase-level work.
- `feature/name`: feature work.
- `fix/name`: bug fixes.
- `docs/name`: documentation updates.
- `release/version`: release preparation.

## Commit Rules

Use:

```text
type(scope): description
```

Examples:

```text
feat(core): create analyzer interface
docs(phase00): update architecture
fix(ui): resolve layout issue
```

Allowed types:

- `feat`
- `fix`
- `docs`
- `style`
- `refactor`
- `test`
- `build`
- `ci`
- `chore`

Avoid meaningless commit messages:

- `update`
- `modify`
- `change`
- `fix`

## Pull Request Expectations

Each pull request should include:

- Purpose.
- Scope.
- Changed files summary.
- Testing or verification.
- Risks.
- Screenshots when UI changes exist.
- Related issues.

## Issue System

Issue templates are stored in:

```text
.github/ISSUE_TEMPLATE/
```

Required templates:

- `bug_report.md`
- `feature_request.md`
- `architecture_task.md`
- `optimization_rule.md`

## Phase Management

Each phase must define:

- Goal.
- Scope.
- Deliverables.
- Non-goals.
- Acceptance criteria.
- Completion report.

Every future phase completion must generate:

```text
docs/PHASE-XX-REPORT.md
```

The report must include:

- Completed.
- Changed files.
- Testing.
- Known issues.
- Risk.
- Next planning.

## Milestone Management

GitHub milestones should align with project phases and lifecycle stages.

Milestone categories:

- Foundation.
- Development.
- Testing.
- Release.
- Maintenance.

Milestone details are maintained in:

```text
docs/MILESTONES.md
```

## Documentation Rules

- Important decisions must exist inside `docs/`.
- Do not keep important design decisions only in conversation.
- Architecture changes must update `docs/ARCHITECTURE.md`.
- Brand or visual changes must update `docs/BRAND_GUIDELINE.md`.
- UI component or navigation changes must update `docs/UI_DESIGN_SYSTEM.md`.
- Workflow changes must update `docs/GITHUB_WORKFLOW.md`.

## Open Source And Secrets

The repository may be public, but private operational details must stay out of Git history.

Do not commit:

- API keys.
- Personal access tokens.
- Private server endpoints.
- Signing certificates.
- Update signing keys.
- Production telemetry credentials.
- Local machine configuration.

Use `.env.example` for public placeholders only. Real values must be stored in ignored local files or GitHub Secrets.

## Automatic Update System Reservation

The repository reserves:

```text
.github/workflows/build.yml
.github/workflows/release.yml
.github/workflows/update.yml
```

These files are reserved for future build, release, and update automation.

Phase 00 creates placeholder workflow files only. Production CI/CD behavior is not implemented in this phase.
