# AETHER SENTINEL AI UI Design System

## Purpose

This document defines the reusable Avalonia UI design system for AETHER SENTINEL AI.

The design system should help future implementation remain consistent, native-feeling, precise, and maintainable.

## Design Principles

- Native desktop experience.
- Dark, clean, and technical.
- Low visual noise.
- Clear system status at a glance.
- Explain risk before action.
- Keep optimization flows transparent.
- Prefer predictable controls over decorative interfaces.

## Application Structure

Main navigation:

1. Dashboard
2. PC Intelligence
3. Game Optimization
4. Performance Monitor
5. Optimization Center
6. DNS Optimization
7. Network Speed Test
8. AI Advisor
9. History
10. Settings

## Dashboard Design

Purpose:

The user should understand PC status immediately.

Primary dashboard components:

- Performance Score.
- Hardware Status.
- AI Recommendation.
- Optimization History.

Example:

```text
Performance Score

87 / 100

CPU
82%

GPU
96%

Memory
65%

[AI Optimize]
```

## Layout System

Recommended shell:

- Left navigation rail.
- Main content area.
- Optional right insight panel for AI recommendations or selected details.
- Persistent status area for scanning, monitoring, or update state.

Spacing:

- Use an 8 px spacing grid.
- Prefer 8, 12, 16, 24, 32, and 48 px increments.
- Keep dense operational views readable.

Border radius:

- Cards and panels: 8 px or less.
- Buttons and controls: consistent with Avalonia platform conventions.

## Color Tokens

Core tokens:

```text
AetherDark: #0B0F14
DeepSpace: #05070A
AIEnergyBlue: #2F80FF
PerformanceGreen: #2BD576
OptimizationAmber: #F2B84B
AlertRed: #FF4D5E
TextPrimary: #F4F7FB
TextSecondary: #A8B3C2
TextMuted: #6F7A89
BorderSubtle: #1C2531
SurfaceRaised: #101722
```

AETHER Glass tokens:

```text
WindowBackdrop: #05080D -> #071018 -> #05070B
GlassSurface: translucent #121A26 / #0C131D / #071018
GlassSurfaceStrong: translucent #152030 / #0E1723 / #071018
GlassInset: translucent #0A111B
GlassBorder: #4A6682A4
GlassHighlight: #2DFFFFFF
PrimaryButton: #3490FF -> #1676E8
SecondaryButton: translucent #1A293B -> #101A27
```

Usage:

- Background: `DeepSpace`.
- App shell and navigation: `AetherDark`.
- Raised surfaces: `SurfaceRaised`.
- AETHER Glass surfaces: use on panels and cards only when contrast remains readable.
- Primary action: `AIEnergyBlue`.
- Healthy state: `PerformanceGreen`.
- Warning state: `OptimizationAmber`.
- Error or destructive risk: `AlertRed`.

Glass rules:

- Glass is a material accent, not the product identity.
- Text contrast must remain stronger than the material effect.
- Use static layered brushes first; avoid expensive real-time blur until Windows profiling confirms the cost.
- Provide a future low-performance fallback that returns to solid surfaces.
- Do not use large decorative glows, orbs, bokeh, or blurred atmosphere as the main visual effect.

## Typography

Fonts:

- Windows: Segoe UI.
- macOS: SF Pro.
- Fallback: Inter.

Roles:

- Display: major score and top-level product moments.
- Heading: page titles.
- Title: component and panel labels.
- Body: standard UI text.
- Caption: metadata, timestamps, and secondary labels.

Rules:

- Do not use oversized text inside compact panels.
- Keep labels short and scannable.
- Use tabular numbers where possible for metrics.

## Component System

### Button

Purpose:

- Execute explicit user commands.

Variants:

- Primary.
- Secondary.
- Ghost.
- Danger.

Rules:

- Primary buttons should be rare and task-focused.
- Dangerous actions require clear labeling and confirmation.
- Icon buttons should include tooltips.

### Card

Purpose:

- Present a single repeated or grouped unit of information.

Rules:

- Do not nest cards inside cards.
- Use cards for metric groups, history entries, and recommendation summaries.

### Panel

Purpose:

- Structure larger tool areas, settings groups, and persistent side regions.

Rules:

- Panels may contain multiple components.
- Panels should not look like marketing cards.

### MetricCard

Purpose:

- Display one core metric with status context.

Content:

- Metric label.
- Current value.
- Status indicator.
- Optional trend.
- Optional confidence or source label.

### ProgressBar

Purpose:

- Show progress, usage, score, or action execution state.

Rules:

- Use status color only when meaningful.
- Include accessible text for the value.

### StatusBadge

Purpose:

- Indicate health, warning, risk, disabled state, or verification status.

Common states:

- Healthy.
- Notice.
- Warning.
- Critical.
- Verified.
- Unknown.

### Chart

Purpose:

- Show performance trends and monitoring history.

Rules:

- Prefer readable axes and clear legends.
- Avoid decorative chart effects.
- Real-time charts must not cause layout shifting.

### Dialog

Purpose:

- Confirm meaningful decisions, especially optimization and rollback actions.

Rules:

- Explain risk clearly.
- Show backup and rollback availability.
- Avoid blocking the user for low-risk informational messages.

### Notification

Purpose:

- Present temporary status updates.

Rules:

- Use calm language.
- Persistent or risky information belongs in a panel or dialog, not a disappearing toast.

## Interaction Patterns

### AI Optimization Flow

Required stages:

```text
Scan -> Analyze -> Explain -> Recommend -> Confirm -> Execute -> Verify -> Record
```

Rules:

- The user must see what will change before execution.
- The UI must show backup and rollback status where applicable.
- Verification results must be shown after execution.

### Monitoring Flow

Required states:

- Idle.
- Starting.
- Running.
- Paused.
- Error.
- Permission required.

### History Flow

History entries should include:

- Timestamp.
- Trigger source.
- Action summary.
- Before state.
- After state.
- Verification result.
- Rollback availability.

## Accessibility

Requirements:

- Do not rely on color alone for status.
- Maintain readable contrast in dark mode.
- Ensure keyboard navigation for primary workflows.
- Keep focus states visible.

## Avalonia Implementation Notes

Future implementation should define reusable styles and resources for:

- Colors.
- Brushes.
- Typography.
- Spacing.
- Component variants.
- Status states.

Recommended future structure:

```text
src/
  AetherSentinel.UI/
    Styles/
      Colors.axaml
      Typography.axaml
      Components.axaml
```

This is a design-system reservation only. Phase 00 does not create production source code.
