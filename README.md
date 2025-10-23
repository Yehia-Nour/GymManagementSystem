# 🏋️‍♂️ Gym Management System

A **web-based application** built using **ASP.NET Core MVC** that streamlines gym operations — including managing members, trainers, sessions, and membership plans.

---

## 🚀 Overview

The Gym Management System helps administrators manage gym activities efficiently.  
It supports CRUD operations for members, trainers, sessions, and plans — all under a clean MVC architecture.

### 🎯 Goals
- Centralize members and plans management  
- Manage trainers and session schedules  
- Provide analytics and insights through a dashboard  

---

## ✨ Features

- 👨‍🏫 **Trainer Management** – Full CRUD operations  
- 💪 **Member Management** – Add, update, delete, and view members  
- 🧾 **Plans Management** – Update, deactivate (Soft Delete), view plans  
- 🧍‍♂️ **Membership Management** – Assign training plans to members  
- 📅 **Session Management** – Full CRUD operations  
- 🗓️ **Session Scheduling** – Organize and assign sessions with trainers  
- 📋 **Session Booking** – Book sessions for members with capacity validation  
- 📊 **Dashboard** – Analytics and reports  

---

## 🏗️ Architecture

**Three-Layer Architecture**

| Layer | Description |
|-------|--------------|
| 🎨 **Presentation Layer** | ASP.NET MVC Controllers + Razor Views (Bootstrap for UI) |
| ⚙️ **Business Logic Layer** | Services (e.g., TrainerService, SessionService) containing core logic |
| 🗄️ **Data Access Layer** | Repository pattern wrapping EF Core DbContext |

---

## 🧰 Technology Stack

| Type | Technologies |
|------|---------------|
| **Backend** | ASP.NET Core MVC |
| **ORM** | Entity Framework Core |
| **Database** | Microsoft SQL Server |
| **Frontend** | Razor Views + Bootstrap + Custom CSS |
| **Patterns** | Repository Pattern, Unit of Work, Dependency Injection |
| **Libraries** | AutoMapper for mapping between ViewModels and Entities |

---

## 📦 Core Entities

### 🧍 Member
Represents a registered gym member.  
- Id, Name, Email, Phone, DateOfBirth, Gender  
- Address (BuildingNo, Street, City)  
- JoinDate (auto-generated), Photo  
- **Relationships:**  
  - One HealthRecord  
  - One Plan  
  - Many Sessions  

---

### ❤️ HealthRecord
Stores member health info.  
- Height, Weight, BloodType, Note, LastUpdate  
- Belongs to one Member  

---

### 🧑‍🏫 Trainer
Represents gym trainers responsible for conducting sessions.  
- Name, Email, Phone, DateOfBirth, Gender, Address  
- Specialties (e.g., Yoga, Nutrition)  
- HireDate (auto-generated)  
- Can conduct many sessions  

---

### 🧾 Plan
Defines membership/training plans.  
- Name, Description, DurationDays, Price, IsActive  
- Assigned to many Members  

---

### 🏷️ Category
Defines types of training programs (e.g., Cardio, Strength).  
- CategoryName  
- Associated with many Sessions  

---

### 🗓️ Session
Represents a scheduled training session.  
- Description, Capacity (1–25), StartDate, EndDate  
- Belongs to one Trainer and one Category  
- Attended by many Members  

---

## ⚖️ Business Rules

### 👥 Member
- Email and phone must be unique & valid  
- Cannot delete members with active bookings  
- Egyptian phone format: `(010|011|012|015)XXXXXXXX`  
- HealthRecord required during registration  
- JoinDate auto-calculated  

### 🧑‍🏫 Trainer
- Email & phone must be unique & valid  
- Cannot delete trainers with future sessions  
- Must have a specialty  
- HireDate auto-calculated  

### 🗓️ Session
- Capacity: 1–25 participants  
- EndDate must be after StartDate  
- Valid Trainer and Category required  
- Cannot delete future sessions  

### 🧾 Plan
- Cannot modify active plans with ongoing memberships  
- Duration: 1–365 days  
- Supports activation/deactivation  

### 📅 Booking
1. Member must have an active membership  
2. Session must have available capacity  
3. Cannot double-book the same session  
4. Only future sessions can be booked/cancelled  
5. Attendance can only be marked during the session  
6. `IsAttended` defaults to false  

### 🪪 Membership
1. Member cannot have duplicate active memberships  
2. Only active plans can be assigned  
3. EndDate = StartDate + Plan.DurationDays  
4. Status: “Active” if EndDate > Now, else “Expired”  
5. Cancelling removes member-plan link  

---

## 🧱 Database Design (Summary)

Includes the following tables:  
- **Members**  
- **Trainers**  
- **Plans**  
- **Categories**  
- **Sessions**  
- **HealthRecords**  
- **Memberships**  
- **Bookings**

Implemented with proper constraints, foreign keys, and relationships.  
Soft delete applied to Plans.

---

## 🧩 MVC Components

### 🏠 HomeController
- `Index()` – Displays system overview and dashboard  

### 👥 MemberController
- CRUD operations for members  
- `MemberDetails`, `HealthRecordDetails`, etc.

### 👨‍🏫 TrainerController
- CRUD operations for trainers  
- `Details`, `Edit`, `DeleteConfirmed`, etc.

### 🗓️ SessionController
- CRUD for sessions  
- Linked to Trainers and Categories  

### 🧾 PlanController
- Manage membership plans  
- Activate/Deactivate  

---

## 🔐 Identity Module

### ApplicationUser  
- FirstName, LastName, UserName, Email, Phone  
- Can have multiple roles  

### IdentityRole  
- Name, NormalizedName, ConcurrencyStamp  
- Linked to many Users  

### AccountController  
- Login / Logout  
- Access Denied handling  

---

## ⚙️ Configuration Highlights

- **Validation:** Unique Email & Phone  
- **Default Dates:** Auto-generated (JoinDate, HireDate, BookingDate)  
- **Soft Delete:** For Plans (IsActive flag)  
- **Constraints:** Session capacity (1–25), Plan duration (1–365 days)  

---

## 🧠 Key Design Patterns Used
- Repository Pattern  
- Unit of Work  
- Dependency Injection  
- AutoMapper for mapping  

---

## 💡 Future Enhancements
- Add admin dashboard analytics  
- Implement RESTful API endpoints  
- Integrate payment gateway for plan purchases  
- Add user roles and permissions management  

---
