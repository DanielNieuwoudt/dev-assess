
# Welcome to the Todo List 

## Goal

### Statement

>**_We aim to create a platform that empowers team members to collaborate and perform at their best while balancing speed and quality to deliver value to our clients._**

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

> A workflow for GitHub Actions has been configured to build, run and test our application and API whenever a Oull Request is created.

### Documentation
- **Types** - Code comments, API documentation, user manuals, architecture diagrams.
- **Benefits** - Facilitates understanding, maintenance, and onboarding of new developers.

### Version Control
- **Purpose** - Enables tracking changes, collaborating with team members, and managing different code versions.

## Developer Experience

### Why is this important

Having a local development environment that closely resembles the production environment enhances our development process and improves the quality of our final product.

Here are some of the benefits:

#### Early Detection of Issues

By mirroring the production environment, we can identify and fix environment-specific issues early in the development cycle, reducing the likelihood of encountering critical problems later.

#### Consistency and Reliability

Consistent environments help ensure that code behaves the same way locally as it does in production. This consistency will reduce the number of environment-related bugs and discrepancies we encounter.

#### Improved Testing

A production-like local environment allows for more accurate and comprehensive testing, including performance and load testing, ensuring that our solution can handle real-world scenarios effectively.

#### Faster Feedback Loop

We can test changes and receive immediate feedback on our local machines, speeding up the development process and increasing productivity.

#### Reduced Deployment Risks

By validating changes in an environment that closely resembles production, we can be more confident that deployments will be smooth and error-free, reducing the risk of deployment failures.

#### Enhanced Debugging

Debugging issues locally in an environment similar to production makes it easier for us to reproduce and diagnose problems, leading to quicker resolution times.

#### Improved Developer Confidence

When tested and validated in an environment that closely resembles production, we have greater confidence in our code, leading to higher-quality software.


## Back End

The following section reminds us of the considerations that need to be made when designing, building, and operating APIs.

### Security

#### Encryption 

SSL will be offloaded to a load balancer or gateway provided by the cloud provider. The client will then securely communicate over HTTPS while the load balancer or gateway forwards the traffic to the backend container over HTTP. 

#### Authentication and Authorisation

JWTs will be used to secure the Todo List Web API by providing a stateless, compact, and self-contained method for authenticating and authorising users, ensuring only valid tokens can access protected resources. 

JWTs embed user information and permissions within the token, improving scalability, reducing server load, and enhancing security.

#### Scalability
We want to design an API that can handle increasing loads, consider the use of caching and load balancing, and ensure that it can run on scalable infrastructure.

#### Performance
Aim to optimise response times with efficient code, database indexing, and minimising payload sizes.

#### Reliability
Ensure high availability with redundant systems, failover mechanisms, retry mechanisms, and robust error handling.

#### Rate Limiting
Aim to prevent abuse and ensure fair usage by limiting the number of requests a client can make.

#### Monitoring
Implement logging and monitoring to track usage and detect issues.

#### Versioning
Consider using versioning to support changes without disrupting existing clients or ensure that changes are always backward compatible.

#### Compliance
Adhere to relevant legal and industry standards (e.g., GDPR, HIPAA).

### Health Checks

The Todo List API provides two health check endponts:

| Endpoint | Description
|----------| -----------
| `/health` | Startup health check|
| `/health/dependency` | Dependency health check that validates the state of the Redis Cache and SQL Server

### Application Architecture

We prefer the use of Clean Architecture and Domain Driver Design for our ASP.NET Web API, which organises our application into four main layers:

- Presentation - The API is the entry point for communication with the application.
- Application - Business logic specific to the use cases in our application.
- Domain - Entities, value objects and aggregates, and domain-specific services.
- Infrastructure - Implementation details and integration include repositories, database context and external service.

Benefits 

- Separation of concerns, with each layer having distinct responsibilities, making the code base easier to understand and maintain.
- Flexibility that allows us to make updates to the technology stack without affecting or retesting the core business logic.
- Improves testability as business logic can be tested independently from the UI and Infrastructure.

#### Visual Representation

```mermaid
graph TD
A[Presentation]
B[Infrastructure]
C[Application]
D[Domain]
E[Database]
F[Cache]

A --> C
B --> C
C --> D
B -.-> F
F -.-> E

style F stroke-dasharray: 5, 5
```
### Frameworks and Libraries

We have selected the the following libraries for our implementation:

|Framework / Library|Description|
|-|-|
| [AutoMapper](https://github.com/AutoMapper/AutoMapper) | A convention-based object-object mapper. |
| [EF Core](https://learn.microsoft.com/en-us/ef/core/) | Entity Framework (EF) Core is a lightweight, extensible, open source and cross-platform version of the popular Entity Framework data access technology. |
| [Fluent Validation](https://github.com/FluentValidation/FluentValidation) | FluentValidation is a .NET library for building strongly-typed validation rules. |
| [MediatR](https://github.com/jbogard/MediatR) | Simple mediator implementation in .NET. In-process messaging with no dependencies. |

### Contract first development

We use a contract-first approach when designing and developing the Todo List API. 

In collaboration with other members of the team, we ensure:

- A documented representation of what we will be building. 
- Allow for early validation that helps identify design issues.
- Contain the endpoints, methods, request and response formats, and expected error codes.
- Ensure we reduce inconsistencies between what we have documented and what we are implementing.
- Engineers in the same or different teams can work in parallel once the contract is defined.
- The front-end and quality assurance engineers can start development against mock servers.
- It provides safety when refactoring, knowing that internal changes do not affect the API interface.
- Improves visibility and the opportunity to identify breaking changes as part of the Pull Request process.
- Generate code for controllers and clients 

### Contract Tests

We aim to minimise duplication between contract and unit tests, keeping the test pyramid in mind. 

Contract testing ensures that interactions between the front end and back end adhere to our predefined contract, reducing the risks of integration issues. 

During development, it allows the team to detect breaking changes early in the development cycle, improving reliability and increasing the overall stability of our solution. 

>Our main goal is to catch breaking changes early and ensure that the implementation of our API adheres to the contract, as that is what consumers will expect. 

### Code Generation

We choose to generate code for the C# controller and contract test client ensuring we meet the consumer's expectations when returning responses containing the Todo Items or Problem Details.

> See the section __Code Generation for the API Spesification__ later in this document.

## Front End 

> **Disclaimer:** I have not spent a significant amount of time developing React Applications. In part, this section reflects my time spent learning for this assessment.

### Application Architecture

React Application architecture typically involves a component-based structure. The UI is divided into reusable, self-contained components, each managing its state and props. 

- **Provider** - We wrap the application in a provider so the components have a shared context and state regarding the items. 
- **App** - This component is also our landing page and contains the Item and Items components.
- **Item** - Allows us to add a new todo item. 
- **Items** - Allows us to view a list of todo items that have not been completed.
- **Alerts** - Provides feedback on the actions taken. Allows us to view the Problem Details received from the API.

#### Benefits

- Components are organised in a hierarchical tree, promoting separation of concerns and modularity.
- Data fetching and side effects are handled with hooks like useEffect and the Axios library. 
- Promotes maintainability, scalability, and ease of testing.

#### Visual Representation

```mermaid
graph TD
A[Provider]
B[App]
C[Add Item]
D[Items]
E[Alert]

A --> B
B --> C
B --> D
C --> E
```

### Why TypeScript

TypeScript offers numerous benefits that enhance the codebase's development process and quality. 

By introducing static typing, TypeScript catches errors at compile time, reducing runtime errors and making the code more robust. It provides improved code editor support with features like autocompletion, type checking, and intelligent refactoring, which boost developer productivity and efficiency.

TypeScript enhances code readability and maintainability by enforcing clear type definitions, making it easier to understand and modify the code. This is particularly beneficial in large codebases and collaborative projects, as it ensures consistent coding standards and provides better documentation through type annotations.

TypeScript supports modern JavaScript features and transpiles to plain JavaScript, allowing developers to use the latest language features while maintaining compatibility with older environments. 

It integrates seamlessly with existing JavaScript code, making it easy to adopt TypeScript in a project gradually.

### Code Generation

We choose to generate code for the client and models we will use when interacting with the API. 

A strongly typed client and its associated functions make it easy to meet API expectations when posting and getting data.

> See the section __Code Generation for the API Spesification__ later in this document.

### Frameworks and Libraries

We have selected the following libraries for our implementation:

|Framework / Library|Description|
|-|-|
| [Axios](https://axios-http.com/docs/intro) | Axios is a promise-based HTTP Client for node.js and the browser. |
| [TypeScipt](https://www.typescriptlang.org/) | TypeScript is a strongly typed programming language that builds on JavaScript, giving you better tooling at any scale. |
| [uuid](https://www.npmjs.com/package/uuid) | For the creation of RFC9562 (formally RFC4122) UUIDs |


### End-to-End Tests

# Getting started

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
| [OIDC Mock Server](https://github.com/Soluto/oidc-server-mock) | A project that allows you to run a configurable mock server with OpenId Connect functionality. |
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

For the best local development and quality assurance experience, we recommended the following tools:

| Tool | Description |
|--|--|
| [Mockoon](https://mockoon.com/) | Mockoon is the easiest and quickest way to design and run mock REST APIs. |
| [Node Version Manager](https://github.com/nvm-sh/nvm) | Install, manage, and switch between multiple versions of Node.js on your system. |
| [Redis Insights](https://redis.io/insight/) | Redis Insight lets you visually interact with a Redis Cache. |
| [SSMS](https://redis.io/insight/) | SQL Server Management Studio (SSMS) is an integrated environment for managing any SQL infrastructure. |

## Cloning the repository

You can clone the repository from https://github.com/DanielNieuwoudt/developer-assessment.git

## Repository Structure

We use a mono repo structure for our repository, allowing us to consolidate multiple projects into one repository. This approach simplifies dependency management, code sharing, and collaboration while maintaining consistency across projects. 
It also facilitates unified version control, streamlined builds, and easier refactoring and testing.

Folders in our repository have been structured in the following way:

|Folder| Decription |
|--|--|
| **mocks** | Mockoon JSON files representing the mocks used by the front. |
| **specs** | Open API specification that is used to generate the controller and clients. |
| **src** | Source code for the front and back end.  |
| **tests** | Contract, end-to-end and performance automated tests. |

### Docker compose files

| File | Description |
|--|--|
| docker-compose-auth.yaml | The separated definition and configuration for the mock authentication server.  |
| docker-compose-deps.yaml | The separated definition and configuration of development and solution dependencies. |
| docker-compose-tests.yaml | The full compose which includes and executes the contract tests. |
| docker-compose.yaml | The full compose includes the authentication server, dependencies, and applications. |

### Port Mappings

Port mappings allow us to access the running containers and for the running containers to access dependencies using our local development machines as hosts.

> We try to let the port selection match the original dependency port.

| Container       | Host Port | Container Port |
|-----------------|-----------|----------------|
| Auth Server     | 5010      | 8080           |
| Back End        | 5000      | 5000           |
| Front End       | 3000      | 3000           |
| Redis           | 6379      | 6379           |
| SQL Server      | 1433      | 1433           |
| Mockoon         | 4000      | 3000           |

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

| Endpoint | Description
|----------| -----------
| [/health](http://localhost:5000/health) | Startup health check|
| [/health/dependency](http://localhost:5000/health/dependency) | Dependency health check that validates the state of the Redis Cache and SQL Server

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

# TODO

| Item | Description
|----------| -----------
| Serilog | Implement Serilog  |
| Versioning | Decide on a versioning strategy.  |
| Authentication and Authorisation | Use mock authentication server and implement authetication and authorisation|

# Cool Finds

Along the way, we encounter tools, libraries, patterns, and practices that we may not have the opportunity to explore every day. 

Below is a table of the ones I came across that I found helpful or interesting.

| Item | Description
|----------| -----------
| [Centralizing .NET Project Configurations](https://blog.ndepend.com/directory-build-props/) | Centralise your package and version management. Useful for solutions with a lot of projects. |
| [Mermaid](https://www.mermaidchart.com/) | Easily create complex diagrams from markdown-style code and collaborate with team members in real-time. |