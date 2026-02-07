# Agent Instructions: SMS Search (C# WinForms)

## 1. Communication & Context Recall
- **Context Bootstrapping:** At the start of every session, you must read the `.sln` and all `.csproj` files to map the project structure accurately.
- **Multi-Phase Planning:** Do not execute code without providing a summary of requirements and a **Pros/Cons Table** for implementation options.
- **Follow-up Integrity:** Directly answer user "Why" or "How" questions before resuming any automated plans.

## 2. Architecture: Modular Repository Pattern
- **Interface-First:** All data access must be defined in an `Interfaces` namespace (e.g., `ISearchRepository`).
- **Dependency Injection:** Use constructor injection. Forms should not instantiate their own repositories.
- **Async Standards:** Use `Task<T>` for all I/O. Use `CancellationToken` for search operations to allow UI-driven cancellation.

## 3. State Management & UI Safety
- **Search State:** When a search is active, the plan must include logic to disable relevant UI controls (Search button, inputs) and re-enable them in a `finally` block.
- **Thread Marshaling:** Ensure all UI updates from background tasks use `IProgress<T>` or `Control.Invoke` to remain thread-safe.

## 4. Error Handling & Logging (Serilog)
- **Global Catch Blocks:** Every background task or Repository method must have a `try-catch` block.
- **Logging Mapping:** - 0: Fatal | 1: Error | 2: Warning | 3: Info | 4: Debug
- **Graceful Failure:** Never let an exception crash the UI. Log the error and show a user-friendly `MessageBox`.

## 5. Security & Style Guide
- **DPAPI:** Proactively scan and migrate any hard-coded credentials to DPAPI.
- **SQL:** No string interpolation in queries. Parameters only.
- **Documentation:** Public members require XML `/// <summary>` tags.
- **Naming:** `_camelCase` for private fields, `PascalCase` for public members.