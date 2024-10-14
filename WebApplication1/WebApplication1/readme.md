
Migration: 
Add-Migration "1stMigration"
Update-Database

nếu có thay đổi thì phải chạy lệnh add 1 tên mới và chạy lại update 
---------------------------------------

check install DotEnv: 
Install-Package DotNetEnv

---------------------------------------
FluebtValidation
AutoMapper
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer

Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.IdentityModel.Tokens
System.IdentityModel.Tokens.Jwt
Microsoft.AspNetCore.Identity.EntityFrameworkCore


-------------------------------------------

Add-Migration "CreatingAuthDatabaseCompany" -Context "AuthWalksDbContext" 
Update-Database -Context "AuthWalksDbContext"


Add-Migration "CreatingWalkDatabaseCompany" -Context "WalksDbContext" 
Update-Database -Context "WalksDbContext"

 Add-Migration "CreatTbl Image of DB Walk" -Context "WalksDbContext"