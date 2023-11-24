- # JetDev - .NET Test 7 WEB\*

# TOOLS:

1. Visual Studio 2022
2. MS SQL Management Studio (Latest)
3. MS SQL Local DB should be installed with VS 22

# Project Structure:
1. UserJourney
	Main project, API or you can say presentation layer which holds controllers, entity (table) & database context. We have used Entity Framework Core & Code first migration. All migration files reside inside this project as well. Keep this project as startup project in order to run the app.

2.UserJourney.Common
	The common project which holds common service and DTOs.

## Run project

Right click on UserJourney project then select Set as a Startup Project and then after click on IIS Express button on the top menu or you can use F5 shortcut key.

## Change Connection String

If you need to change the connection string, so you must change the below project file's contents.

1. UserJourney / appsettings.json


## Create a Database and table

1. Goto the top bar menu and select Tools > Nuget Package Manager > Package Manager Console
2. Use the command to update/create a database and table in the database
   -> Update-Database