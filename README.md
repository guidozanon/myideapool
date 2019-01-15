# myideapool
A codementor test

## Environment
The application is using SQL Server as Database. It can run on Windows (IIS) or Linux.

You can Clone the repo or download as a zip

## Run
- Open the project with *Visual Studio* or *Visual Studio Code*.
- Restore Nuget Packages
- Build

## Configure
In order to run the WebApi you will need to change the following values on appsettings.json:

- *JwtAuthentication.SecurityKey*: A base 64 string with at least 16 characters.
- *JwtAuthentication.ValidIssuer*: The host name that will generate the token (ie: https://localhost:433)
- *ConnectionStrings.IdeasDatabase*: Sql Connection string to your database.
