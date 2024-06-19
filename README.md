
This is a testing of ASP.NET Core in web application that securely stores encrypted data provided by hypothetical customers.
There are 2 crazy admin roles that have all the privileges over databases, which means admins can steal and modify data.
If you are lucky you can find in one table the data stolen by admin Julius Caesar which he replaced to: "CAESAR STOLE YOUR PASSWORD!"

1. delete migrations


dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

dotnet ef migrations add InitialDbCreate
dotnet ef database drop       // delete old db
dotnet ef database update    



dotnet tool install -g dotnet-aspnet-codegenerator
dotnet tool update -g dotnet-aspnet-codegenerator
packages to install: Microsoft.VisualStudio.Web.CodeGeneration.Design
                     Microsoft.EntityFrameworkCore.SqlServer
                     Microsoft.EntityFrameworkCore.Design

dotnet aspnet-codegenerator controller --model Caesar --controllerName CaesarsController --dataContext ApplicationDbContext --referenceScriptLibraries --useDefaultLayout --force --relativeFolderPath Controllers --useAsyncActions
dotnet aspnet-codegenerator controller --model Vigenere --controllerName VigeneresController --dataContext ApplicationDbContext --referenceScriptLibraries --useDefaultLayout --force --relativeFolderPath Controllers --useAsyncActions

[//]: # (Admin Caesar and Admin Vigenere)
dotnet aspnet-codegenerator controller --model Caesar --controllerName AdminCaesarsController --dataContext ApplicationDbContext --referenceScriptLibraries --useDefaultLayout --force --relativeFolderPath Controllers --useAsyncActions
dotnet aspnet-codegenerator controller --model Vigenere --controllerName AdminVigeneresController --dataContext ApplicationDbContext --referenceScriptLibraries --useDefaultLayout --force --relativeFolderPath Controllers --useAsyncActions




-> If you add a new table or column in Models (eg. after setting up User)
   - Redo all of the above steps



Users can only add data and therefore keep track of their cyphers(it fulfills the business logic requirements)
Only these 2 guys can actually delete or modify their own parts of the data:

1. admin_julius_caesar 
   email: julius.caesar@empire.spqr
password: Veni,Vidi,Vici!1
authorized to modify: Caesar Table

2. admin_blaise_de_vigenere
   email: blaise.vigenere@kingdom.fr
password: Montjoie_Saint_Denis!1
authorized to modify: Vigenere Table










