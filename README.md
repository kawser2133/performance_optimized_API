# Performance Optimized API

This project demonstrates an API optimized for high performance using techniques like Dapper for faster data access, caching strategies (Redis), and efficient query handling with both Entity Framework (EF) and Dapper. It also explores various performance improvement strategies for APIs.

## Features

- **Dapper for High-Performance Read Queries**: Leveraging Dapper to optimize complex data retrieval operations.
- **Entity Framework (EF) for Data Manipulation**: EF is used for handling create, update, and delete operations.
- **Redis Caching**: Utilizes Redis to cache frequently accessed data and minimize database load.
- **Asynchronous Programming**: Ensures all data access and processing are handled asynchronously for better scalability.
- **Benchmarking**: Comparing performance between EF and Dapper for complex queries.
- **Paginated Data Retrieval**: Supports efficient data retrieval with pagination.

## Technologies

- ASP.NET Core Web API
- Entity Framework Core
- Dapper
- Redis
- AutoMapper
- PostgreSQL (or SQL Server)
- Bogus (for seeding data)
  
## Installation

1. Clone the repository:

```bash
git clone https://github.com/your-username/performance_optimized_API.git
```

2. Navigate to the project directory:

```bash
cd performance_optimized_API
```

3. Restore NuGet packages:

```bash
dotnet restore
```

4. Update the database connection string in `appsettings.json`:

```json
"ConnectionStrings": {
    "PrimaryDbConnection": "Your-Database-Connection-String"
}
```

5. Run the application:

```bash
dotnet run
```

## Usage

1. Seed the database using Bogus (configured in the DataSeeder class).
2. Use the API endpoints to test CRUD operations and observe the performance difference between EF and Dapper.
3. Monitor Redis for cache hits and database access reduction.

## License

This project is licensed under the MIT License.

## Authors

If you have any questions or need further assistance, please contact the project author at [@kawser2133](https://www.github.com/kawser2133) || [![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/kawser2133)

## Contributing

I want you to know that contributions to this project are welcome. Please open an issue or submit a pull request if you have any ideas, bug fixes, or improvements.  

## License

This project is licensed under the [MIT License](LICENSE).
