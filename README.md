# Movie Store API

## Project Overview

The Movie Store API is a .NET 8 Web API project for managing a movie store. It supports movie management, director and actor management, customer management, and includes JWT-based authentication and authorization.

## Features

- Movie Management
  - Add, delete, update, list movies
  - Movie attributes: Title, Year, Genre, Director, Actors, Price
- Director Management
  - Add, delete, update, list directors
  - Director attributes: First Name, Last Name, Movies Directed
- Actor Management
  - Add, delete, update, list actors
  - Actor attributes: First Name, Last Name, Movies Acted In
- Customer Management
  - Add, delete customers
  - Customer attributes: First Name, Last Name, Purchased Movies, Favorite Genres
- Purchasing
  - Customers can purchase movies
  - Purchased movies are tracked as orders
- Authentication and Authorization
  - JWT-based authentication and authorization
  - Purchase endpoint is protected and accessible only to authenticated customers
- Data Integrity
  - Maintains data integrity during deletion, ensuring related data is preserved

## Installation

Prerequisites
- .Net8 SDK
- SQL Database

Setup
1. Clone the Repository
   ```sh
   git clone https://github.com/username/movie-store-api.git
   cd movie-store-api
   ```
2. Install Dependencies
   ```sh
   dotnet restore
   ```
3. Configure the Database
   Update the connection string in appsettings.json:
   ```json
   "ConnectionStrings": {
   "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
   }
   ```
4. Apply Migrations
   ```sh
   dotnet ef database update
   ```
5. Run the Application
   ```sh
   dotnet run
   ```

## API Usage

Authentication
- POST /api/auth/register -> Registers a new user.
  - Request Body:
    ```json
    {
    "Username": "user",
    "Email": "user@example.com",
    "Password": "password"
    }
    ```
  - Response:
    ```json
    {
    "Username": "user",
    "Token": "jwt_token_here"
    }
    ```
- POST /api/auth/login -> Authenticates a user and returns a JWT token.
  - Request Body:
    ```json
    {
    "Username": "user",
    "Password": "password"
    }
    ```
  - Response:
    ```json
    {
    "Username": "user",
    "Token": "jwt_token_here"
    }
    ```
Movie Management
- GET /api/movies -> Lists all movies.
- POST /api/movies -> Adds a new movie.
- PUT /api/movies/{id} -> Updates an existing movie.
- DELETE /api/movies/{id} -> Deletes a movie.

Director Management
- GET /api/directors -> Lists all directors.
- POST /api/directors -> Adds a new director.
- PUT /api/directors/{id} -> Updates an existing director.
- DELETE /api/directors/{id} -> Deletes a director.

Actor Management
- GET /api/actors -> Lists all actors.
- POST /api/actors -> Adds a new actor.
- PUT /api/actors/{id} -> Updates an existing actor.
- DELETE /api/actors/{id} -> Deletes an actor.

Purchasing
- POST /api/movies/purchase -> Purchases a movie (authentication required).
   - Request Body:
     ```json
     {
     "MovieId": 1
     }
     ```
  - Response:
     ```json
     {
     "Success": true
     }
     ```

## Tests

Tests are written using xUnit and should cover various aspects of the application. Run the tests with:
```sh
dotnet test
```
