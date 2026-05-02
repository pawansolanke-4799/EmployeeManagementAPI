# **Employee Management API**

A production-style ASP.NET Core Web API project built using **.NET 8** and **MySQL**.

This project demonstrates backend development concepts such as **JWT Authentication, Role-Based Authorization, Refresh Tokens, Pagination, Search, Repository Pattern, API Versioning, Global Exception Handling, Dockerized Database Setup, and Clean API Architecture**.

---

## **Features**

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core with MySQL
- JWT Authentication
- Refresh Token Implementation
- Role-Based Authorization (Admin / Employee)
- CRUD Operations for Employees and Departments
- Pagination
- Search Functionality
- API Versioning
- Repository Pattern
- Service Layer Architecture
- Global Exception Handling Middleware
- Response Caching
- Swagger / OpenAPI Documentation
- Dockerized MySQL Setup
- Clean Folder Structure
- Async/Await Implementation

---

## **Technologies Used**

- C#
- ASP.NET Core Web API
- Entity Framework Core
- MySQL
- JWT Tokens
- Docker & Docker Compose
- Swagger / Swashbuckle
- VS Code

---

## **Authentication & Authorization**

Implemented JWT-based authentication with:

- Access Token
- Refresh Token
- Token Expiry Handling

Role-based authorization is implemented using:

```csharp
[Authorize(Roles = "Admin")]
```

### **Example**

- Admin can create departments
- Employee role receives **403 Forbidden**

---

## **API Features**

### **Employee APIs**

- Get All Employees
- Get Employee By Id
- Create Employee
- Update Employee
- Delete Employee

### **Department APIs**

- Get Departments
- Create Department

### **Auth APIs**

- Register
- Login
- Refresh Token

---

## **Pagination & Search**

Implemented pagination and search using query parameters.

### **Example**

```http
GET /api/v1/Employee?pageNumber=1&pageSize=5&search=John
```

---

## **API Versioning**

Implemented API versioning using URL versioning.

### **Example**

```http
/api/v1/Employee
```

---

## **Exception Handling**

Implemented centralized exception handling using custom middleware.

### **Features**

- Global error handling
- Clean error responses
- Internal server error handling

---

## **Docker Setup**

MySQL database runs inside Docker container using Docker Compose.

### **Run Containers**

```bash
docker compose up --build
```

### **Stop Containers**

```bash
docker compose down
```

---

## **Project Structure**

```text
Controllers/
DTOs/
Entities/
Repositories/
Services/
Middleware/
Data/
Migrations/
```

---

## **Swagger Documentation**

Swagger UI is enabled for API testing and documentation.

### **Example**

```text
http://localhost:8080/swagger
```

---

## **Future Improvements**

- Unit Testing
- Redis Caching
- Serilog File Logging Improvements
- AutoMapper
- CI/CD Pipeline
- Angular Frontend Integration

---

## **Author**

**Pawan Solanke**  
.NET Backend Developer
