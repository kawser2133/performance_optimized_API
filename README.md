# Performance Optimized API

This project demonstrates an API optimized for high performance using techniques like Dapper for faster data access, caching strategies (Redis, In-Memory), and efficient query handling with both Entity Framework (EF) and Dapper. It also explores various performance improvement strategies for APIs.

## Key Features

1. **Efficient Data Access**
   - Entity Framework Core for complex queries
   - Dapper for optimized read operations
   - Asynchronous programming for improved responsiveness

2. **Caching Strategies**
   - In-Memory caching for frequently accessed data
   - Distributed caching with Redis for scalability

3. **Performance Optimizations**
   - Pagination for large data sets
   - Sorting capabilities
   - AsNoTracking for read-only queries

4. **Clean Architecture**
   - Repository pattern
   - Separation of concerns
   - Dependency Injection

5. **Data Mapping**
   - AutoMapper for object-to-object mapping

6. **Data Seeding**
   - Bogus library for generating realistic test data

## Getting Started

1. Clone the repository
2. Ensure you have .NET Core SDK installed
3. Set up your database connection string in `appsettings.json`
4. Run `dotnet ef database update` to create the database
5. Run `dotnet run` to start the application

## Configuration

- Database: PostgreSQL
- Caching: Redis (ensure Redis server is running)
- ORM: Entity Framework Core
- Micro-ORM: Dapper

## Usage

1. Seed the database using Bogus (configured in the DataSeeder class).
2. Use the API endpoints to test CRUD operations and observe the performance difference between EF and Dapper.
3. Monitor Redis for cache hits and database access reduction.

## Authors

If you have any questions or need further assistance, please contact the project author at [@kawser2133](https://www.github.com/kawser2133) || [![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/kawser2133)

## Contributing

I want you to know that contributions to this project are welcome. Please open an issue or submit a pull request if you have any ideas, bug fixes, or improvements.  

## License

This project is licensed under the [MIT License](LICENSE).
