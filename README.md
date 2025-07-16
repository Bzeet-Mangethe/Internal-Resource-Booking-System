# 🏢 Internal Resource Booking System (ASP.NET Core MVC)

This is a web-based booking system built using **ASP.NET Core MVC** and **Entity Framework Core**. It allows users to book internal resources (e.g., rooms, company cars).

---
## 💬 Author
Name  : Bhekani Masinga || 
Email : bamasingahbhekanie@gmail.com || 
GitHub: github.com/yourusername

## 🔐 Features

- ✅ User Login (ASP.NET Core Identity)
- 👤 User Registration
- 👤 Role-based access: **Admin** & **Aser**
- 📆 Book resources with **conflict checking**
- 📋 Filter bookings by **resource name** and **date**
- 🧑‍💼 Admin can manage **Resources** and **Bookings**
- 🧑‍💼 Other users can manage **Bookings**


---

## 🧪 Screenshots

### 🧑‍ Registration
![register](https://github.com/user-attachments/assets/83adf8d7-fbb3-4b13-a601-770a56f39d8e)

### 🔐 1st Time Login
![login](https://github.com/user-attachments/assets/89ee4bfd-72d1-4d52-a9b8-eac3a133cfac)

### 🔐 Login
![login](https://github.com/user-attachments/assets/12445289-57d7-4808-8c23-51783073eba9)

### 🔐 Reset Password
![resetPassword](https://github.com/user-attachments/assets/d9a3bddb-aa32-443e-ada9-9b0819c13941)

### 🔐 Resed Email
![resetPassword](https://github.com/user-attachments/assets/b51b6876-a746-4d2a-abc9-85f36ca2753e)

### 🏠 Admin Dashboard
![dashboard](https://github.com/user-attachments/assets/542a2640-ccaf-4323-af7f-ed594e761e0d)

## 🏠 User Dashboard
![userDashboard](https://github.com/user-attachments/assets/7ccbe174-0ab0-43ec-9b52-93c7dd4529da)

### 📝 Admin Booking Form
![bookings](https://github.com/user-attachments/assets/a3df7c69-967f-4d04-9e4e-fd981535917d)

### 📝 User Booking Form
![userbookings](https://github.com/user-attachments/assets/57ab663c-48f1-40b9-a9c1-823bbc780e8c)

### 🧑‍💼 Admin - Manage Resources
![resources](https://github.com/user-attachments/assets/dc2543c5-a96a-487c-97ed-f266b91ec1c5)

## 💻 Tech Stack

- ASP.NET Core MVC (.NET 6 or 7)
- Entity Framework Core
- SQL Server (LocalDB)
- ASP.NET Core Identity
- Bootstrap 5

---

## 🚀 How to Run

1. Clone or unzip the project.
2. Open in **Visual Studio 2022 or higher**
3. Open Package Manager Console and run below:
    ```
    Update-Database
    dotnet ef database update
    ```
4. Run the project (`F5` or `Ctrl + F5`)
5. log in with the seeded admin

---

## 👑 Default Admin Credentials


Email: admin@example.com
Password: Admin@123
