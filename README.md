# UserAuth API

A simple **User Authentication & Management API** built with ASP.NET
Core.\
This project includes user registration/login, JWT-based authentication,
user profile, role management, email and password update, and other
basic features.

It also contains **xUnit unit and integration tests** for reliability.

------------------------------------------------------------------------

## 🚀 Features

-   User registration (register)
-   User login (login) + JWT token generation
-   JWT-protected endpoints
-   User profile (`/users/me`)
-   List all users (`/users/all`, Admin only)
-   Change user role (Admin only)
-   Update password
-   Update email
-   Custom exception classes + Global Exception Middleware
-   Unit & Integration tests with xUnit

------------------------------------------------------------------------

## 🛠️ Installation

### Requirements

-   [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
    using)
-   Optional([Rider](https://www.jetbrains.com/rider/), Visual Studio, or VS Code)

### Run the project

``` bash
git clone git@github.com:OlguD/UserAuthantication.git
cd UserAuth
dotnet run --project UserAuth
```

Swagger UI will be available at:\
👉 `http://localhost:5168/swagger/index.html`

------------------------------------------------------------------------

## 🧪 Running Tests

To execute all unit & integration tests:

``` bash
dotnet test
```

Test project: `UserAuth.Tests`

------------------------------------------------------------------------

## 📡 API Endpoints

### Auth

Method   Endpoint               Description
  -------- ---------------------- -----------------------
POST     `/api/Auth/register`   Register a new user
POST     `/api/Auth/login`      Login + get JWT token

### Users

  ------------------------------------------------------------------------------------
Method   Endpoint                       Description                         Access
  -------- ------------------------------ ----------------------------------- --------
GET      `/api/Users/me`                Get current user profile            User

GET      `/api/Users/all`               Get all users                       Admin

PUT      `/api/Users/change-role`       Change a user role                  Admin

PUT      `/api/Users/change-password`   Update user password                User

PUT      `/api/Users/change-email`      Update user email address           User
------------------------------------------------------------------------------------

------------------------------------------------------------------------

## 🔑 Example Usage

### Register

``` http
POST /api/auth/register
Content-Type: application/json

{
  "username": "test_username",
  "password": "test_password",
  "email": "test@example.com",
  "name": "Test",
  "surname": "Test Surname"
}
```

### Login

``` http
POST /api/auth/login
Content-Type: application/json

{
  "username": "test_username",
  "password": "test_password"
}
```

**Response:**

``` json
{
  "token": "<jwt-token>"
}
```

Use this token in requests with the header:\
`Authorization: Bearer <token>`

------------------------------------------------------------------------

## 🧱 Project Structure

    UserAuth/
     ┣ Controllers/         # API controllers
     ┣ Exceptions/          # Custom exception classes
     ┣ Middlewares/         # Exception middleware
     ┣ Models/              # Entity models (User, LoginRequest, etc.)
     ┣ Services/            # IUserService + UserService
     ┣ Program.cs           # Application entry point
    UserAuth.Tests/      # xUnit tests (unit + integration)
     ┣AuthControllerTests.cs
     ┣AuthIntegrationTests.cs
     ┣ExceptionTests.cs
     ┣UserServiceTests.cs


------------------------------------------------------------------------

## ✨ Roadmap

-   [ ] Add EF Core + SQLite/PostgreSQL support (currently in-memory)\
-   [ ] Implement refresh tokens\
-   [x] Role-based authorization (Admin/User/Moderator)\
-   [ ] Docker support

------------------------------------------------------------------------

## 👨‍💻 Contributing

PRs and suggestions are welcome 🚀
