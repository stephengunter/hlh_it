# hlh_it
dotnet run --project ./IdentityWeb 

dotnet run --launch-profile https --project ./IdentityWeb

dotnet build ./hlh_it.sln && dotnet run --project ./IdentityWeb 

dotnet ef migrations add Event-test -s ../IdentityWeb -o PostgreSqlMigrations/

dotnet ef migrations add init -s ../IdentityWeb --context IdentityContext --output-dir Migrations/Identity
dotnet ef migrations add init -s ../IdentityWeb
dotnet ef migrations remove -s ../IdentityWeb
dotnet ef database update -s ../IdentityWeb --context IdentityContext

