**Requirements**
- Docker Desktop
- Visual Studio

**Run the App**
- Option 1 (Docker Containers): 
    - Open VS -> Package Manager Console
    - Type "docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
" to Package Manager Console

- Option 2 (VS): 
    - Run from VS
    - Set FlowerSpot.Api as the start-up project
    - Run as FlowerSpot.Api

**Populate Database**
- Open VS -> Package Manager Console
- Type "update-database" to Package Manager Console

**Check Database update**
- Open browser and navigate to http://localhost:5050/
- Email: pgadmin4@pgadmin.org
- Password: admin

**Add New PostgreSQL Server**
- Host: postgres_flowerspotdb
- Port: 5432
- Username: flowerSpot
- Password: flowerSpot123
- Database: FlowerSpot

**Notes**
- Use either swagger or any other software for creating requests (navigate to http://localhost:15000/swagger)
- Register User with the register endpoint

**Login**
- After successful registration, type the credentials in the body of Login method
- Wait for the bearer token in response, and us it to authorize future request