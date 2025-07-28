Thanks for clarifying that the project is an **ASP.NET Core API Authentication** application. Here’s the rewritten **README.md summary** tailored for an ASP.NET Core API project:

---

# API Authentication (ASP.NET Core)

This project provides a secure **ASP.NET Core Web API** for user authentication and authorization. It implements **JWT (JSON Web Token)** based authentication, allowing users to register, log in, and access protected endpoints.

---

## 🚀 Features

* **User Registration** – Create a new user account with hashed passwords.
* **User Login** – Authenticate users and generate a JWT token.
* **JWT Authentication** – Secure API endpoints using JWT bearer tokens.
* **Role-Based Authorization** – Restrict access based on user roles.
* **Entity Framework Core** – For database operations.

---

## 🛠 Tech Stack

* **Framework**: ASP.NET Core 7.0 (or 6.0)
* **Authentication**: JWT Bearer Authentication
* **ORM**: Entity Framework Core
* **Database**: SQL Server (or SQLite for development)

---

## 📦 Installation & Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/santhoshdonthi/APIAuthentication.git
   cd APIAuthentication
   ```

2. **Update `appsettings.json`** with your database connection string:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=AuthDB;Trusted_Connection=True;MultipleActiveResultSets=true"
   },
   "Jwt": {
     "Key": "YourSecretKeyHere",
     "Issuer": "YourApp",
     "Audience": "YourAppUsers",
     "ExpireMinutes": 60
   }
   ```

3. **Run database migrations:**

   ```bash
   dotnet ef database update
   ```

4. **Run the application:**

   ```bash
   dotnet run
   ```

The API will be available at:

```
https://localhost:5001
```

---

## 🔑 Authentication Workflow

### 1. **Register a new user**

* **POST** `/api/auth/register`
* **Request Body:**

  ```json
  {
    "username": "yourusername",
    "email": "youremail@example.com",
    "password": "YourPassword123!"
  }
  ```
* **Response:**

  ```json
  {
    "message": "User registered successfully"
  }
  ```

### 2. **Login and get JWT token**

* **POST** `/api/auth/login`
* **Request Body:**

  ```json
  {
    "username": "yourusername",
    "password": "YourPassword123!"
  }
  ```
* **Response:**

  ```json
  {
    "token": "your-jwt-token",
    "expiration": "2025-07-28T14:15:22Z"
  }
  ```

### 3. **Access protected routes**

* Add the JWT token in the `Authorization` header:

  ```
  Authorization: Bearer your-jwt-token
  ```
* Example protected endpoint:
  **GET** `/api/user/profile`

---

## ✅ Example Endpoints

| Method | Endpoint             | Description           | Auth Required |
| ------ | -------------------- | --------------------- | ------------- |
| POST   | `/api/auth/register` | Register new user     | No            |
| POST   | `/api/auth/login`    | Login & get JWT token | No            |
| GET    | `/api/user/profile`  | Get user profile      | Yes           |

---

## 🔐 Security Best Practices

* Store the JWT **secret key** securely (e.g., in Azure Key Vault or environment variables).
* Use **HTTPS** for all API requests in production.
* Set token **expiration** and implement **refresh tokens** for long sessions.
* Enable **account lockout** for failed login attempts to prevent brute-force attacks.

---


