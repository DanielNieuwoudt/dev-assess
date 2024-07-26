
# Welcome to the Todo List 

## Goal Statement

- To meet the criteria for the assessment, which includes the front and back end.
- Creating a local development environment that is representative of a real-world scenario.
- Employ the principles, practices and tools that allow for iterative development and maximise confidence while refactoring.

### My Approach

This implementation provides an overview of the engineering practices, principles, tools, and utilities employed by software and quality assurance engineers. Many of these elements are crucial for designing, developing, and testing applications that are maintainable, scalable, secure and ready for production.

### In practice

Although the implementation is a simplified version of a real-world application, it provides a glimpse into the practices and principles essential for building and operating a production-ready application. 

We may use some or all of the elements for a real-world project depending on the application's complexity, requirements, and constraints.

## Engineering practices and principles

### Agile

- **Frameworks** - Scrum, Kanban, Lean.
- **Principles** - Iterative development, flexibility, customer collaboration, and response to change.

### Application Architecture and Design Patterns 
- **Purpose** - Provide proven solutions to common design problems.

### Automated Testing
- **Types** - Unit tests, contract tests, end-to-end tests.
- **Benefits** - Detects bugs early, ensures new changes do not break existing functionality, and improves code reliability.

### Code Refactoring
- **Purpose** -  Improve code structure and readability without changing functionality.
- **Benefits** - Reduces technical debt, enhances maintainability, and improves performance.

### Code Reviews
- **Purpose** - Improve code quality, ensure adherence to coding standards, and facilitate knowledge sharing.

- **Process** - Peer review of code changes before they are merged into the main codebase

### Continuous Integration and Continuous Deployment (CI/CD)
- **CI** - Automatically integrating and testing code changes frequently.

- **CD** - Automatically deploying code changes to dev, test, staging and production environments.

> Although we do not automatically deploy our code as part of this assessment, a local development environment that resembles a production environments has been provided. 

> A workflow for GitHub Actions has been configured to build, run and test our application and API whenever a Pull Request is created.

### Documentation
- **Types** - Code comments, API documentation, user manuals, architecture diagrams.
- **Benefits** - Facilitates understanding, maintenance, and onboarding of new developers.

### Version Control
- **Purpose** - Enables tracking changes, collaborating with team members, and managing different code versions.

## Developer Experience

### Why is this important

Having a local development environment that closely resembles the production environment enhances our development process and improves the quality of our final product.

Here are some of the benefits:

### Early Detection of Issues

By mirroring the production environment, we can identify and fix environment-specific issues early in the development cycle, reducing the likelihood of encountering critical problems later.

### Consistency and Reliability

Consistent environments help ensure that code behaves the same way locally as it does in production. This consistency will reduce the number of environment-related bugs and discrepancies we encounter.

### Improved Testing

A production-like local environment allows for more accurate and comprehensive testing, including performance and load testing, ensuring that our solution can handle real-world scenarios effectively.

### Faster Feedback Loop

We can test changes and receive immediate feedback on our local machines, speeding up the development process and increasing productivity.

### Reduced Deployment Risks

By validating changes in an environment that closely resembles production, we can be more confident that deployments will be smooth and error-free, reducing the risk of deployment failures.

### Enhanced Debugging

Debugging issues locally in an environment similar to production makes it easier for us to reproduce and diagnose problems, leading to quicker resolution times.

### Improved Developer Confidence

When tested and validated in an environment that closely resembles production, we have greater confidence in our code, leading to higher-quality software.


# Getting started

> _Note_: All endpoints contained in the documentation assume that you are running a `docker compose up`

## Prerequisites

To build and run the solution locally, we will need the following prerequisites installed:

| Prerequisites | Description |
|--|--|
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | Integrated application for building, running, and managing containers. |

## Dependencies

The following dependencies are used in the _form of containers_ to support the development experience and solution:

> There is _no need_ to install these dependencies.

| Tool | Description |
|--|--|
| [Microsoft SQL Server](https://hub.docker.com/r/microsoft/mssql-server) | Official Microsoft SQL Server container image on Linux for Docker Engine. |
| [Redis](https://hub.docker.com/_/redis/) | Redis is a data platform used for caching. |

## Frameworks, Runtimes and SDKs

> To make changes to the solution, the following frameworks, runtimes, and SDKs must be installed.

| Prerequisites | Description |
|--|--|
| [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) | .NET is a free, open-source, cross-platform framework. |
| [NodeJs](https://nodejs.org/en) | Node.js is a free, open-source, cross-platform JavaScript runtime environment. |

**Note:** This solution uses NodeJs version 18.20.3

## Integrated Development Environments

You are free to use an IDE of your choosing. Common IDEs that work with the solution are:

| IDE | Description |
|--|--|
| [Jetbrain Rider](https://www.jetbrains.com/rider/) | The world's most loved .NET and game dev IDE. |
| [Visual Studio](https://visualstudio.microsoft.com/) | The Visual Studio IDE is a creative launching pad that you can use to edit, debug, and build code and then publish an app. |
| [Visual Studio Code](https://code.visualstudio.com/) | Visual Studio Code is a free source code editor that runs on your desktop and supports various languages and runtimes. |

### Tools

For the best local development and quality assurance experience, I recommended the following tools:

| Tool | Description |
|--|--|
| [Node Version Manager](https://github.com/nvm-sh/nvm) | Install, manage, and switch between multiple versions of Node.js on your system. |
| [Redis Insights](https://redis.io/insight/) | Redis Insight lets you visually interact with a Redis Cache. |
| [SSMS](https://redis.io/insight/) | SQL Server Management Studio (SSMS) is an integrated environment for managing any SQL infrastructure. |

### Online Tools

Other tools that are useful when working with some of the artifacts in this repository:

| Tool | Description |
|--|--|
| [Swagger Editor](https://editor.swagger.io/) | Design, describe, and document your API on the first open source editor supporting multiple API specifications and serialization formats. |

## Cloning the repository

You can clone the repository from https://github.com/DanielNieuwoudt/developer-assessment.git

## Repository Structure

We use a mono repo structure for our repository, allowing us to consolidate multiple projects into one repository. This approach simplifies dependency management, code sharing, and collaboration while maintaining consistency across projects. 
It also facilitates unified version control, streamlined builds, and easier refactoring and testing.

Folders in our repository have been structured in the following way:

|Folder| Decription |
|--|--|
| **specs** | Open API specification that is used to generate the controller and clients. |
| **src** | Source code for the front and back end.  |
| **tests** | Contract, end-to-end and performance automated tests. |

### Docker compose files

| File | Description |
|--|--|
| docker-compose-deps.yaml | The separated definition and configuration of development and solution dependencies. |
| docker-compose-tests.yaml | The full compose which includes and executes the contract tests. |
| docker-compose.yaml | The full compose includes the authentication server, dependencies, and applications. |

### Port Mappings

Port mappings allow us to access the running containers and for the running containers to access dependencies using our local development machines as hosts.

> We try to let the port selection match the original dependency port.

| Container       | Host Port | Container Port |
|-----------------|-----------|----------------|
| Back End        | 5000      | 5000           |
| Front End       | 3000      | 3000           |
| Redis           | 6379      | 6379           |
| SQL Server      | 1433      | 1433           |

> We recommend stopping your local Microsoft SQL Server installation to avoid port conflicts. You can do this by executing the following command from a command prompt with elevated privileges:

`NET STOP mssqlserver`

## Building the containers

To build or rebuild all the containers, you can use:

 `docker compose build`

To force a rebuild of all containers without using cache:

`docker compose build --no-cache`

## Running the dependencies for local development

`docker compose -f .\docker-compose-deps.yaml up --build --detach --remove-orphans`

## Running the solution and its dependencies

`docker compose up --build --remove-orphans`

## Running the solution and the automated tests

`docker compose -f .\docker-compose-tests.yaml up --build --remove-orphans`

## Accessing the applications

To access the Todo List applications, we use the following links:

| Application | Url |
|--|--|
| Front End | http://localhost:3000 |
| Back End | http://localhost:5000/swagger |

> If you are getting redirected for HTTPS you can clear your local HSTS cache by following the instructions below:

In your browser of choice, type the following URL in the address bar:

- chrome://net-internals/#hsts
- edge://net-internals/#hsts

Once there, go to:

- Delete domain security policies
- Enter in  `localhost` 
- Press the  **Delete** button

## Checking the health of the backend

You can check the health of the backend by using the following endpoints.

The Todo List API provides two health check endponts:

| Endpoint | Description
|----------| -----------
| [/health](http://localhost:5000/health) | Startup health check|
| [/health/dependency](http://localhost:5000/health/dependency) | Dependency health check that validates the state of the Redis Cache and SQL Server

_Side Note_: The `/health/dependency` endpoint provides a convenient way for us to determine whether the API is ready for us to run contract tests during a `docker compose` locally or in the pipeline.

## See which containers are running after a docker compose

`docker compose ps`

## Stopping all the containers

`docker compose down`

### Parameters

The following parameters are used when using `docker compose`

| Parameter | Description |
|--|--|
| build | (Recommended) Build the containers to ensure they are up to date. |
| remove-orphans | (Optional) Clean up services that are no longer defined in the compose YAML. |
| detach | (Optional) Allows you to continue using the terminal for other tasks. |

### Development activities

The following activies takes place during the course of development:

#### Code Generation for the API Spesification

After updating the Open API specification, we must regenerate the controller and client code to ensure they are up to date and match the Specification.

##### C# Controller for the Todo List API

- Navigate to the `/specs/back-end` from the repository's root using a bash terminal.

- Execute the following bash script to regenerate the C# controller

    `./generate-controller.sh`

##### TypeScript Client for the Contract Tests

- Navigate to the `/specs/back-end` from the repository's root using a bash terminal.

- Execute the following bash script to regenerate the TypeScript Client

    `./generate-test-client.sh`

##### TypeScript Client for the Front End

- Navigate to the `/specs/front-end` from the repository's root using a bash terminal.

- Execute the following bash script to regenerate the TypeScript Client

    `./generate-frontend-client.sh`

#### Using EF Core Migrations

From `/src/back-end` directory:

- Update to the latest database

    <code>
    dotnet ef database update<br> 
    &emsp;--project TodoList.Infrastructure<br>
    &emsp;--startup-project TodoList.Api<br> 
    &emsp;--context TodoList Infrastructure.Persistence.TodoListDbContext<br>
    </code>

- List migrations

    <code>
    dotnet ef migrations list<br>
    &emsp;--project TodoList.Infrastructure<br>
    &emsp;--startup-project TodoList.Api<br>
    &emsp;--context TodoList.Infrastructure.Persistence.TodoListDbContext<br>
    </code>

- Create a new migration

    <code>
    dotnet ef migrations add <MIGRATIONNAME><br>
    &emsp;--project TodoList.Infrastructure<br>
    &emsp;--startup-project TodoList.Api<br>
    &emsp;--context TodoList.Infrastructure.Persistence.TodoListDbContext<br>
    &emsp;[MigrationName]
    </code>

- Remove the last migration

    <code>
    dotnet ef migrations remove<br> 
    &emsp;--project TodoList.Infrastructure<br>
    &emsp;--startup-project TodoList.Api<br> 
    &emsp;--context TodoList.Infrastructure.Persistence.TodoListDbContext<br>
    </code>

#### Running the Contract Tests

#### Ensure your dependencies are running 

From the repositories root:

- Start our development dependencies 

    `docker compose -f .\docker-compose-deps.yaml up --build --remove-orphans --detach`

- Start the Todo List API

    This can be done using your IDE or the command line e.g. `dotnet run` in `/src/back-end/TodoList.Api`

#### Run the contract tests

From the `/tests/contract` directory uinsg a bash terminal

- Ensure your NPM packages are up to date 

    `npm i`

- Run the contract tests

    `npm run test`

## Things that a real world application may need

These are things that I would still want to do for completeness:

| Item | Description
|----------| -----------
| Authentication and Authorisation | Use mock authentication server and implement authentication and authorisation for the front and backend|
| Caching | Implement Redis cache |
| Cypress Tests | Use Cypress to build a simple set of UI Automated tests for the Front End. |
| Mock Server | Provide Mockoon mocks or stateful Mockaco mocks for the Front End |
| Performance Tests | Use K6 to build a simple set of performance tests for the API |
| Paging | For those who have pages of things to do :) |
| Versioning | Decide on a versioning strategy and implement it. |


## Cool Finds

Along the way, we encounter tools, libraries, patterns, and practices that we may not have the opportunity to explore every day. 

Below is a table of the ones I came across that I found helpful or interesting.

## .NET

| Item | Description
|----------| -----------
| [Centralizing .NET Project Configurations](https://blog.ndepend.com/directory-build-props/) | Centralise your package and version management. Useful for solutions with a lot of projects. |
| [Amichai Mantinband](https://www.youtube.com/@amantinband) | Industry-level tutorials about coding, design patterns, architecture, and the latest and greatest libraries, tricks, and tips you should know about. |
| [Mockaco](https://natenho.github.io/Mockaco/) | Get your mock server up and running quickly! |
| [Error handling with Result Pattern](https://goatreview.com/improving-error-handling-result-pattern-mediatr/) | Improving Error Handling with the Result Pattern in MediatR |

## Documentation

| Item | Description
|----------| -----------
| [Mermaid](https://www.mermaidchart.com/) | Easily create complex diagrams from markdown-style code and collaborate with team members in real time. |