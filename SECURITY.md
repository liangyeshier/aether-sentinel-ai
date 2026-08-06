# Security Policy

## Open Source Boundary

AETHER SENTINEL AI is intended to be open source where practical.

The following must not be committed to the public repository:

- Private server endpoints that are not intended for public use.
- API keys.
- Signing certificates.
- Update signing keys.
- Personal access tokens.
- Production telemetry credentials.
- Private infrastructure configuration.
- User-specific local settings.

Use `.env.example` only as a public template. Real local values must stay in ignored files such as `.env`.

## Reporting Security Issues

Do not open a public issue for suspected vulnerabilities that expose private keys, credentials, update infrastructure, or unsafe optimization behavior.

Use private disclosure channels when available.

## Security Requirements For Future Implementation

Future optimization and update systems must preserve:

- Explicit user consent for risky operations.
- Backup before reversible changes.
- Execution logs.
- Verification after changes.
- Rollback when supported.
- Package verification before updates.
- No hardcoded secrets.

## Supported Versions

Phase 00 contains documentation foundation only. No production application version is currently supported.
