# TVMaze Information API

## Description

This is an ASP.NET Core Web API application designed to expose information from the TVMaze database of TV shows. 
It allows you to retrieve and manage TV show information through a set of secure API endpoints.

## Key Features
* **Data Synchronization:**
	* A background job periodically updates the database with the latest TV show data from the TVMaze API.
	* A manual synchronization endpoint allows you to trigger an immediate update of the database.
* **API Endpoints:**
	* Expose endpoints for retrieving all shows and individual shows based on their ID.
	* Implement a token generation endpoint for obtaining a JWT token required for accessing API endpoints.
* **API Key Authentication:**
	* Securely protects API endpoints using an API key for authentication.
* **Layered Architecture:**
	* The application follows a layered architecture, separating concerns and promoting maintainability.
* **Unit Testing:**
	* Unit tests are implemented to ensure the correctness and reliability of the core logic.

## Architecture

* **Layered Model:** The project is organized into layers for better modularity and maintenance:
    * **Domain:** Defines the core business logic and entities, such as Show, Episode, Cast, Crew, etc. This layer is responsible for encapsulating the business concepts and rules.
    * **Data:** Contains the Entity Framework Core context (TVMazeContext), which interacts with the database for data persistence and retrieval.
    * **Infrastructure:** Handles infrastructure concerns like data access and communication with the TVMaze API.
    * **Application:** Implements the business logic services, such as ShowService, responsible for managing and retrieving show information.
    * **Presentation:** Provides the ASP.NET Core Web API controllers (ShowsController, TokenController) that expose the API endpoints for external communication.
* **Background Job:** A background job (`FetchAndStoreShowsJob`) is used to periodically sync TVMaze data.
* **Unit Testing:** Unit tests have been included to validate the behavior of the repository and show service.

## Installation and Execution

1. **Requirements:**
    * .NET 8 SDK
    * SQL Server (or a compatible database)
2. **Restore dependencies:**
    ```bash
    cd TVMazeInfoAPI
    dotnet restore
    ```
3. **Configure the database:**
    * Create a new database in SQL Server.
    * Configure the connection string in the `appsettings.json` file.
4. **Create the database:**
    ```bash
    dotnet ef database update
    ```
5. **Run the application:**
    ```bash
    dotnet run
    ```

## Endpoints

* **GET `/Shows`:** Returns a list of all shows stored in the database. Requires a valid JWT token in the authorization header.
* **GET `/Shows/{id}`:** Returns the information for a show by its ID. Requires a valid JWT token in the authorization header.
* **POST `/Shows/Sync`:** Initiates a manual sync of TV show information from TVMaze. Requires a valid JWT token in the authorization header.
* **POST `/Token`:** Generates a JWT token for authentication. Requires an API key as a query parameter.


## API Documentation

API documentation is available using Swagger:

* Start the application.
* Open your web browser and visit `[your_url]/swagger`.

## Notes

* The background job runs periodically to sync TVMaze information. The execution frequency can be adjusted in the appsettings.json file.
* This solution prioritizes clean code practices, modularity, reusability, and separation of concerns.
* The API key is stored in the appsettings.json file. You can modify it to your own API key.

## Acknowledgements

* TVMaze API: https://www.tvmaze.com/api
* Entity Framework Core: https://docs.microsoft.com/en-us/ef/core/