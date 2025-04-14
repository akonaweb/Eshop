# Eshop

# Eshop.WebApi

Create `appsettings.Development.json` file with this content. This file is excluded from the Git:

```json
{
  "Auth": {
    "SecretKey": "QJgILc4m267gH0lIxBpgXzfiByLx0wjMWcleR79nVXc=",
    "Issuer": "https://localhost:7203",
    "Audience": "Eshop"
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

[a relative link](Eshop.Client/README.md)