# SMS Search

SMS Search is a Windows Forms application designed to assist in searching and querying SQL Server databases. It provides a specialized interface for looking up Functions, Totalizers, and Fields within a database schema (likely related to a specific retail or inventory management system).

## Features

- **Database Connectivity**: Connects to Microsoft SQL Server using Integrated Security (SSPI).
- **Search Capabilities**:
    - **Functions**: Search by Function Number (`F1063`), Description (`F1064`), or Custom SQL.
    - **Totalizers**: Search by Totalizer Number (`F1034`), Description (`F1039`), or Custom SQL.
    - **Fields**: Search database columns/fields by Number, Description, Table Name, or Custom SQL. Uses metadata tables like `RB_FIELDS`.
- **Custom SQL**: Execute custom SQL queries directly within the application.
- **SQL Cleaner**: Utility to "clean" SQL queries by removing formatting and comments for easier reading or copying.
- **Unarchive Tool**: Includes a helper form (`frmUnarchive`) for handling archived data.
- **Configuration**: Customizable settings via `SMS Search_settings.json` (Database connection, UI preferences).
- **Export/View**: Results are displayed in a DataGridView with sorting and resize capabilities.

## Prerequisites

- **.NET Framework**: Version 4.8
- **Database**: Access to a Microsoft SQL Server instance.
- **Schema**: The application expects specific tables and columns to exist in the database (e.g., `FCT_TAB`, `TLZ_TAB`, `RB_FIELDS`, `sys.tables`, `sys.columns`).

## Installation and Build

1.  Clone the repository.
2.  Open `SMS Search.sln` in Visual Studio (VS 2013 or newer recommended).
3.  Build the solution.
    - Ensure you have the `.NET Framework 4.8` developer pack installed.
    - The project references `Microsoft.SqlServer.TransactSql.ScriptDom.dll`. If this is missing, you may need to install the SQL Server Data Tools (SSDT) or NuGet package.

## Configuration

The application uses `SMS Search_settings.json` for configuration. If the file is missing or the connection fails, the configuration window will open on startup.

**Key Configuration Sections:**
- `[CONNECTION]`: Stores `SERVER` and `DATABASE` names.
- `[GENERAL]`: Settings for Multi-instance (`MULTI_INSTANCE`), EULA (`EULA`), Updates (`CHECKUPDATE`), and UI behaviors.

## Usage

1.  **Launch the application.**
2.  **Connection:** Provide the SQL Server name and Database name if prompted.
3.  **Tabs:**
    -   **Functions (`FCT`):** Use this tab to find specific system functions.
    -   **Totalizers (`TLZ`):** Use this tab for totalizer lookups.
    -   **Fields:** Use this tab to explore database schema, tables, and column descriptions.
4.  **Search:** Enter your search term (supports wildcards `*` or `?`) in the Number or Description fields, or select a Table.
5.  **Populate Grid:** Press `Enter` or click the refresh button to execute the search and view results in the grid.
