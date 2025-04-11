# TestApi

## Setup

### 1. Start Oracle DB with Docker

Create a `docker-compose.yml` file with the following content:

```yaml
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
```

Then run:

```bash
docker-compose up -d
```

---

### 2. Clone and Run the API

```bash
git clone https://github.com/fhimaj/TestApi.git
cd TestApi
```

Open the project in **Visual Studio** and run it.

---

### 3. Sign Up & Authenticate
- First make sure db on docker is up and running.
- Run the api.
- Open the Swagger at http://localhost:{launchedPort}/index.html (e.g: http://localhost:5169/index.html)
- Use the `/api/user/` endpoint in **Swagger** to create a user or manually call the endpoint.
- Login with the username and password you just created.
- Copy the returned **JWT token**.

You can now authorize requests in two ways:

#### a. Using Swagger UI:
Click the **Authorize** button and paste the token:

```
Bearer <your_token>
```

#### b. Manually (e.g., Postman or custom client):

Add this header to your requests:

```
Authorization: Bearer <your_token>
```

> Token is valid for 30 minutes.

