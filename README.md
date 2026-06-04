# Joy Stream — Enterprise Birthday Tracking Workspace

Joy Stream is a high-end, secure ASP.NET Core MVC workspace application designed to log, track, and manage corporate milestone profiles. Engineered with a premium dark-mode glassmorphic user interface, the system combines real-time data calculations, interactive user alerts, and strict multi-tier data security boundaries.

---

## System Overview

Joy Stream shifts away from multi-page database tracking tools toward a unified, single-screen software-as-a-service grid workspace. 

The application architecture coordinates data parsing through four core pillars:
1. The Identity Engine: Enforces authentication and tracks security roles (User vs. Admin).
2. The Workspace View: Displays an entry module, a live searchable tracking directory table, and analytical sidebar cards simultaneously.
3. The Rules Engine: Restricts record modification rights using contextual creator data tags.
4. The Notification Pipeline: Computes active calendar dates on load to trigger target alerts and micro-animations.

---

## Core Features

* Unified Workspace UI: Add entries, search profiles, view directories, and review alerts instantly from a single dashboard area.
* Context-Aware Data Security: Users maintain total control over their own entries, while standard users are blocked from touching other profiles.
* Administrative Override Capability: Administrators possess elevated permissions to purge any profile line entry directly from the directory to maintain database integrity.
* Live System Metrics: Live calculation of total monitored database records.
* Automated Calendar Reminders: A dynamic alert section flags birthdays occurring on the current system date, triggering celebratory client-side micro-animations.

---

## Tech Stack & Dependencies

* Framework: ASP.NET Core MVC (Model-View-Controller)
* Security Layer: Microsoft ASP.NET Core Identity Core
* Data Access & ORM: Entity Framework Core
* Database Target: Microsoft SQL Server (LocalDB configuration)
* Frontend Framework: Razor View Pages, Bootstrap 5.3
* Client Scripts: JavaScript DOM Integration (Confetti Module Engine)

---

## Authorization Logic Matrix

The platform enforces strict role and ownership access control levels across the database endpoints:

* Create Birthday Tracking Log: Allowed for all authenticated users.
* Read Directory Table: Allowed for all authenticated users.
* Edit/Update Target Record: Allowed only for the original record creator. Blocked for everyone else.
* Delete Database Profile Line: Allowed for the original record creator or users assigned to the Admin role.

---

## Project Architecture

```text
BirthdayApp/
│
├── Controllers/
│   ├── AccountController.cs    # Handles login, registration, and user session assignment
│   └── BirthdayController.cs   # Orchestrates main workspace logic and CRUD transactions
│
├── Data/
│   └── AppDbContext.cs         # Coordinates Entity Framework database connectivity configuration
│
├── Models/
│   ├── ApplicationUser.cs      # Extends Identity User properties (e.g., IsApproved status flags)
│   ├── Birthday.cs             # Core tracking entity schema (Id, Name, DateOfBirth, CreatedBy)
│   └── SystemLog.cs            # Tracks framework audit trails
│
└── Views/
    ├── Birthday/
    │   ├── Index.cshtml        # Unified dark-mode dashboard view grid
    │   ├── Edit.cshtml         # Secure profile parameter update screen
    │   └── Delete.cshtml       # Data destruction confirmation gateway
    └── Shared/
        └── _Layout.cshtml      # Master application shell framework
