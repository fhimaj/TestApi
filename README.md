TestApi

To test first setup Oracle db using this docker-compose yaml config:
"
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
  "
then download the repo or fork it. To test it run it on Visual Studio, where first you need to sign up using the endpoint below:
![image](https://github.com/user-attachments/assets/27a7e59b-b671-4a10-a4df-9f346e0e450b)

 Use the generated token either by authorizing in Swagger or by manually sending the Bearer token in requests. A token is valid for 30mins.

