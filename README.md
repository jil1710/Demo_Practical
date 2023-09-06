# Deixar

Leave management project using ASP.NET 7 API + MVC + Sentry + Unit Testing+ Serilog + Health check

## Setup

- Add connection string in the user secrets file of Deixar.API Project

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=[DBSOURCENAME];Initial Catalog=DeixarDB;Persist Security Info=True;User ID=[YOURUSERID];Password=[******];TrustServerCertificate=True"
  },
  "MailSettings": {
    "From": "[EMAIL]",
    "SmtpServer": "smtp.gmail.com",
    "Port": 465,
    "UserName": "[USERNAME]",
    "Password": "[Password]" // This is generated password from security section of google account
  },
  "Jwt": {
    "Audience": "Deixar.MVC",
    "Issuer": "Deixar.API",
    "Key": "[JWTKEY]"
  }
}
```

> **_NOTE:_** This configuration only works under "Development" enviorment.

## Migration

- Make sure you add connection string in secrets and then follow the following steps
- In the package manager console select "Deixar.Data" project and then run the given command

```bash
Update-Database
```
