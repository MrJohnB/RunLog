![.NET Core](https://github.com/MrJohnB/RunLog/workflows/.NET%20Core/badge.svg)

# Demo

[![RunLog Website](https://raw.githubusercontent.com/MrJohnB/RunLog/master/src/LiteBulb.RunLog.Web/wwwroot/runlog-logo.png)](https://runlogstorageaccount.z13.web.core.windows.net/)

[RunLog REST API](https://runlog.azurewebsites.net/index.html)

# Introduction
RunLog is an activity tracking application.  Users can create/edit/delete activities.  And activity has a start/stop button and while the activity is running, the device's geolocation coordinates are tracked and stored in the RunLog system.

The solution is comprised of 3 components:
1.	RunLog.API is a REST API that allows for client applications to interact with the RunLog system.  Provides data access and back-end processing.
2.	RunLog.Web is a client-side SPA website to interface with the system.
3.	RunLog Tests is a set of unit tests and integration tests to automate testing of the system components.

Additional components:
- LiteBulb.Database to store all the data used for the system.

#Tech Stack
1.	.NET Core 3.1 (for Web API, Blazor WebAssembly and libraries)
2.	.NET Standard 2.1 (for libraries)
3.	LiteBulb MemoryDb 0.1 (for database)
4.	Swagger (API documentation)
5.	Serilog (logging)

# Problem Statement

The RunLog application needs a REST API to provide data storage and retrieval functionality.  The API needs to perform some back-end processing.  The API needs to write log messages to an AMQP message broker.  An interactive UI is needed to provide user access to all the CRUD operations supported by the application.

A solution called RunLog will be developed to perform the required operations.

# Requirements

1.	RunLog application needs a logical data model that includes two entities with a cardinality relationship of one-to-many.
2.	REST API that implements all the CRUD operations using a resource-based modeling approach for each data model entity.
3.	REST API requires HTTPS security.
4.	Each invocation of the REST API should write a small log message describing the given interaction to an AMQP message broker.

Note: See below for links to code repositories.

# Documentation
- [README.md](https://github.com/MrJohnB/RunLog/blob/master/README.md)
- [Solution Document](https://github.com/MrJohnB/RunLog/blob/master/docs/guides/runlog-solution-document.docx)
- [RunLog Rest API](https://github.com/MrJohnB/RunLog/blob/master/docs/guides/runlog-rest-api-guide.docx)
- [Diagrams](https://github.com/MrJohnB/RunLog/blob/master/docs/diagrams/runlog-diagrams.drawio)
- [Installation](https://github.com/MrJohnB/RunLog/blob/master/docs/guides/RunLog%20API%20Installation%20Guide.docx)

# GitHub
- [RunLog Project](https://github.com/users/MrJohnB/projects/1)
- [RunLog Repository](https://github.com/MrJohnB/RunLog)

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test
- Build the solution in Visual Studio 2019 and run.
TODO: Describe and show how to build your code and run the tests.

# Contribute
- [RunLog Repository](https://github.com/MrJohnB/RunLog)

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)