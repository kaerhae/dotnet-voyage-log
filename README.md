# dotnet-voyage-log

Travel logging service


## Configuration
Before starting, app must be configured correctly. For user management and admin controls, app must have following either on user-secrets:

```json
{
  "ConnectionStrings:PostgresConn": "Host=<DB_HOST>; 
  Database=<DB_NAME>; Username=<DB_USERNAME>; Password=<DB_PASSWORD>",
  "JwtSettings:SecretKey": "<SECRET_KEY>",
  "JwtSettings:Audience": "<JWT_AUDIENCE>",
  "JwtSettings:Issuer": "<JWT_ISSUER>",
  "InitAdmin:Username": "<ADMIN_USERNAME>",
  "InitAdmin:Email": "<ADMIN_EMAIL>",
  "InitAdmin:Password": "<ADMIN_PASSWORD>"
}
```

or on environment variables:
```bash
export POSTGRES_CONNECTION_STRING="Host=<DB_HOST>; \
    Database=<DB_NAME>; \
    Username=<DB_USERNAME> \
    Password=<DB_PASSWORD>"
export SECRET_KEY=<SECRET_KEY>
export JWT_AUDIENCE=<JWT_AUDIENCE>
export JWT_ISSUER=<JWT_ISSUER>
export INIT_ADMIN_USERNAME=<INIT_ADMIN_USERNAME>
export INIT_ADMIN_EMAIL=<INIT_ADMIN_EMAIL>
export INIT_ADMIN_PASSWORD=<INIT_ADMIN_PASSWORD>
```


## Getting started

App can be started easily as a release with docker-compose. Compose file contains declarations for dotnet-voyage-log, migration, Postgres database, and PGAdmin. Compose file uses arguments, which are passed to containers and then used as a environment variables. Before starting, it is recommended to change args from docker-compose.

Containers can be started with:
```bash
docker-compose up -d
```


<u>Note:</u> Migration container executes only database update command and then exits. 