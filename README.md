### `.env Example`

```env
# Database Configuration
SQL_SERVER=example-sqlserver
SQL_DATABASE=ExampleDB
SQL_USER_ID=admin
SQL_PASSWORD=Str0ngP@ssw0rd123!

# Auth0 Configuration
AUTH0_SECRET=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
AUTH0_ISSUER_BASE_URL=auth.example.com
AUTH0_CLIENT_ID=abcd1234efgh5678ijkl9012mnop3456
AUTH0_CLIENT_SECRET=ZYXwvu9876tsrqPONMLkjiHGFedcba4321!@#$%
AUTH0_AUDIENCE=https://example.com
AUTH0_SCOPE=openid profile email
AUTH0_DOMAIN=https://dev-example1234.us.auth0.com/
APP_BASE_URL=https://example.com

# ASP.NET Core Configuration
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:8080
ASPNETCORE_LOGGING_LEVEL_DEFAULT=Information
ASPNETCORE_LOGGING_LEVEL=Information
ASPNETCORE_LOGGING_LEVEL_EFCORE=Information
ASPNETCORE_APPLY_MIGRATIONS_AT_STARTUP=true

# Frontend Configuration
NEXT_PUBLIC_API_URL=https://example.com/api
NEXT_PUBLIC_AUTH0_DOMAIN=auth.example.com
NEXT_PUBLIC_AUTH0_CLIENT_ID=abcd1234efgh5678ijkl9012mnop3456