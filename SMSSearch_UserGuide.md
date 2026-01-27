# SMS Search - User Guide

**SMS Search** is a specialized utility for searching and querying SQL Server databases, designed for technical users who need quick access to Functions, Totalizers, and Field definitions. It offers advanced search capabilities, direct SQL execution, and a background launcher service.

---

## Table of Contents

1. [Installation & Prerequisites](#installation--prerequisites)
2. [Getting Started](#getting-started)
3. [Search Interface](#search-interface)
   - [Functions & Totalizers](#functions--totalizers)
   - [Fields & Tables](#fields--tables)
   - [Custom SQL](#custom-sql)
4. [Working with Results](#working-with-results)
   - [Filtering & Highlighting](#filtering--highlighting)
   - [Grid Features](#grid-features)
   - [Exporting Data](#exporting-data)
5. [Advanced Features](#advanced-features)
   - [Clean SQL](#clean-sql)
   - [Unarchive Tool](#unarchive-tool)
   - [Background Launcher](#background-launcher)
6. [Configuration](#configuration)

---

## Installation & Prerequisites

### Prerequisites
- **OS:** Windows 10 or newer.
- **Framework:** .NET Framework 4.8.
- **Database:** Access to a Microsoft SQL Server instance with the required schema (Tables: `FCT_TAB`, `TLZ_TAB`, `RB_FIELDS`, etc.).

### Installation
1. Download the latest release archive (zip).
2. Extract the contents to a folder of your choice (e.g., `C:\Tools\SMSSearch`).
3. Run `SMSSearch.exe`.

> **Note:** The application is portable and stores its configuration in `SMSSearch_settings.json` within the same directory.

---

## Getting Started

### First Run
Upon the first launch, if no valid configuration is found, the **Configuration** window will open automatically. You must configure the database connection to proceed.

![Database Configuration Screen](docs/images/placeholder_db_config.png)

1. **Server:** Enter the SQL Server hostname or IP address.
2. **Database:** Enter the target database name.
3. **Authentication:**
   - **Integrated Security (Recommended):** Check "Use Windows Authentication" to use your current Windows credentials.
   - **SQL Authentication:** Uncheck the box and provide the `User` and `Password`.
4. Click **Test Connection** to verify settings.
5. Click **Save** to launch the main application.

---

## Search Interface

The main window is divided into three primary tabs, each tailored for a specific type of search.

### Functions & Totalizers
The **FCT** (Functions) and **TLZ** (Totalizers) tabs operate similarly:

- **Number:** Search by the specific ID (e.g., `F1063`).
- **Description:** Search by text description. Supports wildcards:
  - `*` : Matches any string of characters.
  - `?` : Matches any single character.
- **Custom SQL:** Enter a raw SQL `WHERE` clause or a full query.

### Fields & Tables
The **Fields** tab provides schema exploration tools:

- **Search by Number/Description:** Find specific columns across the database.
- **Table Lookup:** Select a table from the dropdown to view its schema (columns).
  - **Show Fields:** Lists column definitions (Name, Type, Description).
  - **Show Records:** Displays the actual data within the table (Top 1000).
  - **Last Transaction:** (Visible for specific tables like `SAL_HDR`) Filters for the most recent transaction data.

### Custom SQL
All tabs include a "Custom SQL" input area.
- You can type a partial `WHERE` clause (e.g., `fct_id LIKE '%10%'`) or a full `SELECT` statement.
- **Build Query Buttons:** Clicking the `Build Query` button (icon) will generate the SQL statement based on your current inputs and display it in the Custom SQL box for inspection or modification.

---

## Working with Results

### Filtering & Highlighting
The results grid includes a powerful client-side filter box at the bottom.

- **Filter:** Type text to instantly filter visible rows.
- **Match Highlighting:** Matches are highlighted in **Yellow** (configurable).
- **Navigation:** Use the **Next** (`>`) and **Previous** (`<`) buttons to jump between matching cells.
- **Match Count:** Displays "Match X of Y" to track your position.

![Match Highlighting Example](docs/images/placeholder_highlight.png)

### Grid Features
- **Sorting:** Click any column header to sort. Shift-click for multi-column sorting.
- **Row Numbers:** Displayed in the row header (toggleable in Settings).
- **Context Menu:** Right-click anywhere in the grid for options:
  - **Copy:** Copy selected cells (with or without headers).
  - **Resize to fit:** Auto-size columns to content.
  - **Clear result:** Reset the grid.
  - **Export results to CSV:** Save current view to a file.

### Exporting Data
To export large datasets:
1. Perform your search.
2. Right-click the grid -> **Export results to CSV**.
3. Choose a filename.
4. Select whether to include headers in the prompt.

> **Performance:** The grid uses "Virtual Mode" to handle large result sets efficiently. Exporting streams data directly from the database to keep memory usage low.

---

## Advanced Features

### Clean SQL
A utility to format or "clean" SQL queries, useful when copying code from other tools (like trace logs).
- **Usage:** Paste dirty SQL into the Custom SQL box and click the **Clean SQL** button (broom icon).
- **Configuration:** You can define Regex-based replacement rules in **Settings > Search > Clean SQL**.

### Unarchive Tool
Accessible via the target icon (or `Ctrl+T`), this opens a separate window for retrieving archived transaction data. It allows cross-referencing between live and archive tables.

### Keyboard Shortcuts
- **Switch Tabs:** `Ctrl+1` (Functions), `Ctrl+2` (Totalizers), `Ctrl+3` (Fields).
- **Toggle Unarchive:** `Ctrl+T`.
- **Select All:** `Ctrl+A` (in text boxes).
- **Execute Search:** `F5`.

### Background Launcher
The application can run a lightweight background listener to allow quick access via hotkeys.
- **Status:** The colored dot in the Settings > Launcher screen indicates status:
  - 🟢 **Green:** Running & Registered.
  - 🟠 **Orange:** Stopped.
  - ⚪ **Gray:** Not configured.
- **Hotkey:** Configurable global hotkey to bring the app to the front (Default: `Ctrl+Alt+S`).

---

## Configuration

Access settings via the **Gear Icon** in the top-right corner.

### General
- **Application:**
  - **Startup Location:** Choose where the window appears (Last position, Center Screen, etc.).
  - **Minimize to Tray:** Keep app running in the notification area.
  - **Always on Top:** Keep window floating above others.
- **Display:**
  - **Show Row Numbers:** Toggle grid row headers.
  - **Highlight Matches:** Enable/Disable yellow highlighting.
  - **Auto Resize:** Automatically resize columns after load (Warning: can be slow on large datasets).
- **Logging:** Configure log retention policies.

### Search
- **Behavior:** Set default tab on startup.
- **Clean SQL:** Manage Regex rules for the SQL cleaner.

### Database
Update connection strings, timeouts, and authentication methods.

### Launcher
- **Register Service:** Install the background listener.
- **Hotkey:** Set the global key combination to summon the app.

---

## Troubleshooting

**Q: The grid says "Loading..." but never finishes.**
A: Check your network connection. Large queries may take time. Check the logs (`%AppData%` or app folder) for timeout errors.

**Q: "Virtual Load Error" appears.**
A: The application failed to use the optimized loading method. It will automatically fall back to "Legacy Load", which is slower but more compatible.

**Q: Global Hotkey isn't working.**
A: Ensure the Launcher status is Green in settings. If Orange, click "Start/Restart". Ensure no other application is using the same hotkey.
