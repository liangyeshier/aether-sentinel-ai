# Contributing

Thanks for helping improve AETHER SENTINEL AI.

## Project Philosophy

AETHER SENTINEL AI should remain native, lightweight, explainable, and safe.

Optimization behavior must follow:

```text
Understand -> Analyze -> Explain -> Recommend -> Optimize -> Verify
```

## Commit Format

Use:

```text
type(scope): description
```

Examples:

```text
docs(phase00): update foundation report
feat(core): add scanner contract
fix(ui): correct dashboard layout
```

## Documentation

Important decisions must be documented in `docs/`.

Architecture changes should update:

```text
docs/ARCHITECTURE.md
```

Brand and UI changes should update:

```text
docs/BRAND_GUIDELINE.md
docs/UI_DESIGN_SYSTEM.md
```

## Security And Secrets

Do not commit secrets, local credentials, signing keys, private endpoints, or production infrastructure configuration.

Use `.env.example` for public placeholders only.
