# .NET 8 WebApi Placeholder

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

A generic REST API service for testing CRUD endpoints.

## Core Features

- ✅ **Standard CRUD Endpoints** (GET, POST, PUT, PATCH, DELETE)
- ✅ **Partial Updates** (PATCH support)
- ✅ Auto-Generated IDs (GUID)
- ✅ Basic Data Validation
- ✅ Interactive Documentation with **Scalar**
- ✅ In-Memory Storage (non-persistent)

## 🎯Live demo

- API URL: [**rest-placeholder.dangos.dev**](https://rest-placeholder.dangos.dev/)
- Scalar UI: [**rest-placeholder.dangos.dev/scalar/v1**](https://rest-placeholder.dangos.dev/scalar/v1)

## Getting Started

Follow these steps to run the Web API example on your local machine.

### Prerequisites

- **[.NET SDK 8.0](https://dotnet.microsoft.com/download)** - Make sure you have the **.NET SDK 8.0** installed to build
  and run ASP.NET Core applications. This project is built and tested using **.NET 8**.

### Running the Application

1. **Clone the repository:** If you haven't already, clone this GitHub repository to your local machine.

``` bash
git clone https://github.com/dangos-dev/DotNet8.WebApiPlaceholder.git
cd DotNet8.WebApiPlaceholder
```

2. **Run the application:** Execute the following dotnet CLI command to start the Web API:

``` bash
 dotnet run
```

3. **Access API Documentation:** Once the application is running, you can access the API documentation through your web
   browser:
    - **Scalar UI:** Open your browser and navigate to the `/scalar` endpoint. For example:
      `https://localhost:7242/scalar/v1` or `http://localhost:5290/scalar/v1`. The specific port will be shown in the
      console output when you run the application.

    - **OpenAPI JSON:** The raw OpenAPI document in JSON format is available at `/openapi/v1.json`. For example:
      `https://localhost:7242/openapi/v1.json`