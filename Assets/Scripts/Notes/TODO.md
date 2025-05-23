# TODO

## Features

### New

1. Add line numbers to notepad

### Bugs and Fixes

1. Fix all scenes with correct animations
2. Finish the Onboarding Scene

   - BUG: Make sure you can only click certain things during that step, and not click other button to progress the tutorial.

   #### Explanation

   • For each step:

   - Only one or a few “main” interactables are the focus — these are the ones that advance the tutorial when interacted with.

   - All previous main interactables from earlier steps should remain interactable, but they should not progress the tutorial anymore.

   • The total set of interactables should grow over time as you progress through steps — it should include all main interactables from previous steps, plus the new main interactables of the current step.

   • Don’t want to manually define “allowedInteractables” for each step — instead, should automatically be derived from the main interactables of the current and all previous steps.

   • The only objects that should trigger step advancement are the main interactables of the current step.
