# SMS Search

**SMS Search** is a specialized utility for technical users to search and query SQL Server databases. It streamlines the process of looking up system Functions, Totalizers, and Field definitions, offering a powerful alternative to standard database management tools for these specific tasks.

## Key Features

- **Advanced Search**: Quickly find Functions, Totalizers, and database Fields by number, description, or custom SQL.
- **Database Exploration**: Browse tables, view schema definitions, and inspect raw data.
- **Background Launcher**: A lightweight background service allows you to summon the application instantly via a global hotkey.
- **Unarchive Tool**: dedicated utility to retrieve and cross-reference archived transaction data.
- **Clean SQL**: Built-in tool to format and "clean" SQL queries for easier reading and sharing.
- **Automatic Updates**: Keeps the application up-to-date with the latest features and fixes.
- **Customizable**: Configurable database connections, UI behaviors, and hotkeys.

## Documentation

For comprehensive usage instructions, detailed feature breakdowns, and troubleshooting, please refer to the **[User Guide](SMSSearch_UserGuide.md)**.

## Prerequisites

- **Operating System**: Windows 10 or newer.
- **Framework**: [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48).
- **Database**: Access to a Microsoft SQL Server instance containing the required schema (e.g., `FCT_TAB`, `TLZ_TAB`).

## Installation & Getting Started

1.  **Download**: Get the latest release from the repository.
2.  **Run**: Launch `SMSSearch.exe`. The application is portable.
3.  **Configure**: On first run, the configuration window will appear. Enter your SQL Server details and click **Save**.

## Development & Building

To build the project from source, the following environment is recommended:

- **IDE**: Visual Studio 2022 (or newer).
- **Workload**: .NET Desktop Development.
- **SDK**: .NET Framework 4.8 Developer Pack.

### Build Steps
1.  Clone the repository.
2.  Open `SMS Search.sln` in Visual Studio.
3.  Restore NuGet packages.
4.  Build the solution (Debug or Release).

---
*Maintained by the SMS Search Team.*
