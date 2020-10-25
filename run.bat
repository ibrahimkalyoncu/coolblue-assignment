@ECHO OFF 
:: This batch file will run the application and dependecies via docker containers and apply initial ef migrations.
TITLE Insurance.Api 
ECHO Please wait...
docker-compose down
docker-compose up -d
ECHO Waiting sql server
timeout 30
dotnet ef database update -s src/Insurance.Api -p src/Insurance.Data
start "" http://localhost:5002/swagger
start "" http://localhost:5001/swagger
ECHO Done.
PAUSE
