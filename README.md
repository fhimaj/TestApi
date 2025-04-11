TestApi
Setup
Start Oracle DB with Docker
Create a docker-compose.yml file with the following content:

yaml
Copy
Edit
version: "3.8"

services:
  oracle-db:
    image: gvenzl/oracle-xe:21-slim
    container_name: oracle-db
    environment:
      ORACLE_PASSWORD: testapidb123sys      
      APP_USER: testapidbuser                
      APP_USER_PASSWORD: testapidb123      
    ports:
      - "1521:1521"
      - "5500:5500"
    volumes:
      - oracle-data:/opt/oracle/oradata
    restart: unless-stopped

volumes:
  oracle-data:
Run it with:

bash
Copy
Edit
docker-compose up -d
Clone and Run the API

bash
Copy
Edit
git clone https://github.com/fhimaj/TestApi.git
cd TestApi
Open in Visual Studio and run the project.

Sign Up & Authenticate
Use the /api/user/ endpoint in Swagger to create a user.
Login with the given username and password.
Copy the returned token and use the Authorize button in Swagger, or add it to requests manually:

makefile
Copy
Edit
Authorization: Bearer <your_token>
Token is valid for 30 minutes.
