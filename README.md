# Eshop

# Eshop.WebApi

Create `appsettings.Development.json` file with this content. This file is excluded from the Git:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.;Initial catalog=Eshop;Integrated Security=True;TrustServerCertificate=True;"
  },
  "Auth": {
    "SecretKey": "QJgILc4m267gH0lIxBpgXzfiByLx0wjMWcleR79nVXc=",
    "Issuer": "https://localhost:7203",
    "Audience": "Eshop"
  },
  "Client": {
    "Url": "http://localhost:3000"
  }
}
```

`SecreatKey` should be long enough(more then bits/bytes 256/32).
You can use powershell to generate some:

```powershell
PS> [Convert]::ToBase64String((1..32 | ForEach-Object { Get-Random -Maximum 256 }))
```

E.g.: QJgILc4m267gH0lIxBpgXzfiByLx0wjMWcleR79nVXc=
It should be hard to guess. Keep it safe and do not share.
Later, this key will be stored in the secret vault for particular provider e.g. Google, Azure, AWS.

# Eshop.Client

[Client setup is here](Eshop.Client/README.md)

# Deployment

We are using Azure. After you register on Azure you need to create:
1. 1x Resource Group - North Europe ocation because of free Azure SQL
2. 1x Service Plan - Windows(WebApi)
3. 1x Service Plan - Linux(Client) 
4. 1x Sql Server
5. 1x Azure SQL Database - currently offering free for North Europe location
6. 1x Web App Node.js 22 LTS Client
7. 1x Web App .NET 8 WebApi

Create `appsettings.Production.json` file with this content. This file is excluded from the Git:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:eshop-sqlserver.database.windows.net,1433;Initial Catalog=eshop-db;Persist Security Info=False;User ID=eshop-sqladmin;Password=HERE-PASTE-YOUR-PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  },
  "Auth": {
    "SecretKey": "QJgILc4m267gH0lIxBpgXzfiByLx0wjMWcleR79nVXc=",
    "Issuer": "",
    "Audience": "Eshop"
  },
  "Client": {
    "Url": ""
  }
}
```

ConnectionStrings - `HERE-PASTE-YOUR-PASSWORD` - change this to you password from Azure
Auth - check above section Eshop.WebApi how to generate SecretKey. Issuer will be url of created App Service for WebApi.
Client - will be url of created App Service for Client