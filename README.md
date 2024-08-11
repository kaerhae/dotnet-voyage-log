# dotnet-voyage-log

## Overview
Travel logging backend service powered by ASPNET core, PostgreSQL, and S3 storage. Service contains user-management and voyage-log management, which can be integrated to frontend or CLI. Service is mostly aimed to run with docker-compose, but it scalable enough to run on IIS server with S3 storage and external PostgreSQL database.

Service has policies for admin, user and anonymous access. Voyage-log supports following details:
 - Topic
 - Description
 - Notes
 - Country
 - Region
 - Locations longitude coordinate
 - Locations latitude coordinate
 - Multiple images
    * Images are received as IFormFile, and are uploaded to S3 storage. Voyage-log will be saved with image keys as identifiers, and can be retrieved to frontend by ImageController operation.

For further description of app CRUD operations, read controller documentation [here](dotnet-voyage-log/Controllers/README.md).


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
  "InitAdmin:Password": "<ADMIN_PASSWORD>",
  "AWS:S3BucketName": "<BUCKET_NAME>"
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
export S3_BUCKET_NAME=<BUCKET_NAME>
```


## Getting started

Before starting application on production, app must be configured with suitable AWS configuration. This may be achieved, with inserting "AWS" configuration to appsettings.json, in example:
```json
...
"AWS": {
    "Profile": "<AWS_S3_PROFILE",
    "Region": "<REGION>"
  },
...
```
App will get this configuration by default and initialize AmazonS3Client with these options.

App can be started easily as a release with `docker-compose.yml`. Compose file contains declarations for dotnet-voyage-log, migration, Postgres database, and PGAdmin. Compose file uses arguments, which are passed to containers and then used as a environment variables. Before starting, it is recommended to change args from docker-compose.

Containers can be started with:
```bash
docker-compose -f docker-compose.yml up -d
```


<u>Note:</u> Migration container executes only database update command and then exits. 

## Development
Project contains `docker-compose.dev.yml` for development. Compose file orchestrates following containers:
 - dotnet-voyage-log: Main app
 - dotnet-voyage-log-migration: Migrations for main app
 - postgres:12.3-alpine: Database for main app
 - dpage/pgadmin4:4.23: Control panel as webUI for database
 - localstack/localstack:3.6: Localstack container, which serves as a local development environment for S3 storage
 - amazon/aws-cli: AWS cli tool, for creating test bucket in S3 storage

Development environment in docker can be started with:
```bash
docker-compose -f docker-compose.dev.yml build --no-cache
docker-compose -f docker-compose.dev.yml up --force-recreate
```

`--no-cache` argument makes sure that possible changes have been noted, and docker-compose does not build image from earlier images. `--force-recreate` makes sure that localstack is created successfully.

## Known issues on development

Pre-signed image url may not work correctly, since image-url will be formatted `https://localstack:<PORT>/<BUCKET_NAME>/...` and by default localstack container is available on localhost for external connections. If it is desired, that image should be displayed on browser with pre-signed url, development environment must be configured to access localstack container by its name. This can be resolved in following way:

### Linux

Run docker-compose.dev.yml and check localstack container id:
`docker container list`

Use container id to retrieve localstack container IP address with:
`docker container inspect <ID> | grep 'IPAddress'

Then open /etc/hosts file with your favorite text editor tool, with sudo. Recommended way is with nano:
```sudo nano /etc/hosts```

Insert following line to end of the file:
```bash
<LOCALSTACK_IP_ADDRESS>   localstack
```

Save and exit. Now localstack can be connected with `localstack` hostname, and pre-signed image-URL will be displayed on browser.

### Windows

Localstack hostname issue has not been tested on Windows environment, but it is very likely to happen. Windows environment has a similar approach to assigning hostname to IP address on local development. This might be tested on future.

