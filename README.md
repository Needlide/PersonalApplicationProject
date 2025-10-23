
# PersonalApplicationProject

Contains the backend build with ASP.NET Core, .NET 9, using Entity Framework Core with PostgreSQL database and Angular single-page application. Allows users to create, find, join and manipulate events.

# Prerequisites
1. Docker and Docker Compose
2. .NET SDK 9.0
3. Angular v20
4. Node.js and npm

# Getting Started
Clone the repository
```
git clone https://github.com/Needlide/PersonalApplicationProject
```
Enter the newly created directory and set up secrets files:
in the root of the project create `secrets` directory, then create two files inside it:
`secrets/db_user.txt` (contains desired database username)
`secrets/db_password` (contains password for the database user)

Run with Docker Compose: `docker-compose up --build`

Once the containers are up and running, you can access the application:
 - Frontend: `http://localhost:4200`
 - Backend: `http://localhost:8080`
 - Swagger: `http://localhost:8080/swagger/index.html`

 To stop the application you should use `docker-compose down`
