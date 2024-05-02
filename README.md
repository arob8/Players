# Player Service

## Description
This project aims to integrate and persist player data from the CBS Sports API for three major sports: baseball, football, and basketball. It involves building a robust system capable of importing data periodically and updating existing records in the database to ensure the most up-to-date information is available.

## Getting Started

This section guides you through the necessary steps to get this project up and running on your local machine for development and testing purposes.

### Prerequisites
- Ensure you have the **.NET 6 SDK** installed. Verify this by running `dotnet --version` in your terminal. If it's not installed, download it from the [official .NET website](https://dotnet.microsoft.com/download).

### Running the Application

1. **Clone the Repository**
   - Pull the latest version of the code from the repository:
     ```bash
     git clone [repository url]
     ```

2. **Navigate to the API Project**
   - Change into the directory containing the PlayerService.API project:
     ```bash
     cd path/to/PlayerService.API
     ```

3. **Build the Project**
   - Compile the project to resolve dependencies:
     ```bash
     dotnet build
     ```

4. **Run the Project**
   - Start the application:
     ```bash
     dotnet run
     ```

   - Optionally, you can specify the environment:
     ```bash
     dotnet run --environment "Development"
     ```
   
## Endpoints

### Get Player by ID
- **URL**: `/player/{id}`
- **Method**: `GET`
- **Description**: Retrieves player information by ID.
  - **Parameters**:
    - `id` (integer): The unique identifier of the player.
  - **Responses**:
    - `200 OK`: Returns player information if successful.
    - `404 Not Found`: If the player with the specified ID is not found.
    - `500 Internal Server Error`: If an unexpected error occurs.

### Search Players
- **URL**: `/player/search`
- **Method**: `GET`
- **Description**: Searches for players based on specified criteria.
  - **Query Parameters**:
    - `sport` (string): Filter players by sport (optional).
    - `lastNameStartsWith` (string): Filter players whose last name starts with the specified value (optional).
    - `age` (integer): Filter players by exact age (optional).
    - `minAge` (integer): Filter players by minimum age (optional).
    - `maxAge` (integer): Filter players by maximum age (optional).
    - `position` (string): Filter players by position (optional).
  - **Responses**:
    - `200 OK`: Returns a list of players matching the criteria if successful.
    - `400 Bad Request`: If no search criteria are provided.
    - `404 Not Found`: If no players match the criteria.
    - `500 Internal Server Error`: If an unexpected error occurs.

## Clean Architecture
The project follows the principles of Clean Architecture, emphasizing separation of concerns and dependency inversion. The architecture is structured into layers, each with specific responsibilities:

- **API Layer**: Handles HTTP requests and contains controllers and endpoints for player operations.
  
- **Application Layer**: Contains application-specific logic. This layer includes the `PlayerService`, which is responsible for handling API search functionalities such as retrieving player data by ID and performing search queries on player information.

- **Domain Layer**: Contains the core business entities and logic, independent of external concerns. Includes entities like `Player` and specializations such as `FootballPlayer`, `BaseballPlayer`, and `BasketballPlayer`.

- **Infrastructure Layer**: Deals with external concerns such as data access, external API communication, and database interaction. Includes components like `CbsSportsClient`, `PlayerRepository`, `PlayerContext`, `PlayerDataProcessor`, and `PlayerDataBackgroundService`.

## PlayerService.API
- **PlayerController**: Handles HTTP requests related to player operations.

## PlayerService.Application
- **PlayerService**: Implements player-related operations such as fetching player data and searching for players.
- **PlayerDto**: Represents player information in a data transfer object format.
- **PlayerDtoDataMapper**: Maps domain entities (Player) to data transfer objects (PlayerDto).
- **ServiceResult**: Represents the result of a service operation.

## Player.Domain
- **Player**: Represents a player in a sports league.
- **FootballPlayer**, **BaseballPlayer**, **BasketballPlayer**: Specializations of `Player` for specific sports.
- **SportType**: An enumeration representing different types of sports.
- **PlayerNotFoundException**: Represents an exception thrown when a player is not found.

## PlayerService.Infrastructure
- **CbsSportsClient** : Handles communications with the CBS Sports API to retrieve player data. 
- **PlayerData** : Represents detailed attributes of players fetched from the CBS Sports API.
- **PlayerContext** : Acts as the database context within the Entity Framework. Manages entity relationships and database interactions for the players' data model.
- **PlayerRepository** : Manages the persistence operations for player data, including saving updates and retrieving player entities from the database.
- **PlayerSeeder** : Initializes the player database.
- **PlayerDataMapper** : Maps raw player data (PlayerData) to entity objects (Player).
- **PlayerDataCalculator** : Responsible for computing statistical analyses such as average ages by position and position age difference.
- **PlayerDataProcessor** Coordinates the processing of player data, including fetching from external APIs, calculating statistics, mapping to domain entities, and updating the database. It relies on services for data fetching, mapping, calculating, and repository operations to ensure data is current and accurate.
- **PlayerDataBackgroundService**: Runs in the background to periodically fetch player data from external sources and update the database.
