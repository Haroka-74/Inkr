<div style = "font-family: 'Roboto', sans-serif;">

<div align = "center">

# **🖋️ Inkr - Blog Platform API**

</div>

- **Inkr** is a modern blog backend built with ASP.NET Core designed to support content publishing, user management, and authentication flows using secure, scalable, and modular architecture.
- It’s ideal for:
    - Developers building a full-stack blog or CMS
    - Teams needing a customizable blogging engine
    - Anyone looking to start a modern content-driven project

<div align = "center">

# **🚀 Features**

</div>

- 🔐 Secure authentication with JWT + Refresh Tokens
- 📧 Email confirmation, password recovery, reset forms
- 👤 User profile view and update
- 🧱 Clean and modular service-based structure
- 💡 Easy to extend with posts, comments, likes (coming soon)

<div align = "center">

## 🔐 Auth API Endpoints

| Method | Endpoint                                | Description                                               |
|--------|-----------------------------------------|-----------------------------------------------------------|
| POST   | `/api/auth/register`                    | Register a new user                                       |
| POST   | `/api/auth/login`                       | Log in with email and password                            |
| POST   | `/api/auth/refresh-token`               | Refresh access token using a refresh token                |
| POST   | `/api/auth/revoke-token`                | Revoke a refresh token (logout)                           |
| GET    | `/api/auth/profile`                     | Get current user profile (JWT required)                   |
| PUT    | `/api/auth/profile`                     | Update user profile (JWT required)                        |
| GET    | `/api/auth/confirm-email`               | Confirm user email with a token                           |
| POST   | `/api/auth/resend-email-confirmation`   | Resend the confirmation email                             |
| POST   | `/api/auth/forgot-password`             | Trigger password reset email                              |
| GET    | `/api/auth/reset-password-form`         | Render the reset password HTML form                       |
| POST   | `/api/auth/reset-password`              | Reset the user password with a valid token                |

</div>

<div align = "center">

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-6DB33F?style=for-the-badge&logo=nuget&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Postman](https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=postman&logoColor=white)

</div>

</div>