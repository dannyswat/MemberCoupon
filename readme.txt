dotnet tool install -g dotnet-aspnet-codegenerator
Generate Razor pages
dotnet aspnet-codegenerator razorpage -m [Entity class] -dc [Db context] -outDir [Pages folder]

dotnet aspnet-codegenerator razorpage -m MemberGroup -dc ApplicationDbContext -outDir Pages\MemberGroups