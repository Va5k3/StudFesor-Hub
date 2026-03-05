
StudFesor Hub is web application built with **C#/.NET** and **SQL Server**, designed to help **students and professors** manage academic activities in one place — schedule, activity tracking, and upcoming tasks.

> This repository is a **student project** created for learning and portfolio purposes, focusing on practical full‑stack fundamentals (UI, business logic, data access, and persistence).

**UI language:** Serbian (for now).

---

## Features (Current)

- Authentication: **Register / Login**
- **Role-based accounts**: Student / Professor  
  - Admin role planned
- **Schedule import (.docx)** + schedule overview table
- **Activity tracking** (create and manage activities)

---

## Roadmap

- **Telegram Bot notifications** for upcoming activities (reminders / alerts)
- Admin role + additional management features

---

## Tech Stack

- **C# / .NET 8**
- **Blazor Server** (Interactive Server Components)
- **SQL Server** (data access via `Microsoft.Data.SqlClient`)
- `.docx` parsing/import: **Open XML SDK** (`DocumentFormat.OpenXml`)
- Layered solution structure:
  - `Entities` – domain models
  - `DAL` – data access (repositories)
  - `BusinessLayer` – business logic/services
  - `Core` – shared abstractions/constants
- Testing projects: `UnitTest`, `UnitTestt`, `Test`

---

## Screenshots

**Landing page**  
![WhatsApp Image 2026-03-04 at 21 57 42](https://github.com/user-attachments/assets/69ea48f3-b9f6-4f1b-a6a7-d200929faac5)

**Login**  
![WhatsApp Image 2026-03-04 at 21 57 47](https://github.com/user-attachments/assets/b5b4cbbf-81b9-4fec-83d1-63a17e0dc202)

**Dashboard**  
![WhatsApp Image 2026-03-04 at 21 58 50](https://github.com/user-attachments/assets/ddc3f63d-6f0c-47ad-ada6-1c223e2603e9)

**Schedule**  
![WhatsApp Image 2026-03-04 at 21 58 52](https://github.com/user-attachments/assets/13453564-adc1-44fc-a043-121dea5d63ea)





---

## ER Diagram

<img width="1386" height="608" alt="ER-diagram" src="https://github.com/user-attachments/assets/fdb8d4e8-2582-4661-a192-526b861276ce" />

---

## Repository Structure (High-level)

- `StudfesorHub.sln` – Visual Studio solution
- `Web App - Blazor/` – Blazor Server UI
- `BusinessLayer/`, `DAL/`, `Entities/`, `Core/` – application layers
- `ScheduleImporter/` – schedule import logic/tooling
- `UnitTest*/` – tests

---

## Running Locally

### Prerequisites
- Visual Studio 2022 / Rider
- .NET SDK
- SQL Server (LocalDB is used by default)

### Steps
1. Clone the repository
2. Open `StudfesorHub.sln`
3. Set the project inside `Web App - Blazor/` as the startup project
4. Run the app locally (HTTPS)

### Database
The connection string is currently defined in code:
- `Core/Constant/DBConstant.cs`

---  


