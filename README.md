# Dot Net Services 101 Eğitimi

Dot Net Servislerinin kullanımının ele alındığı eğitim için taban repodur. Eğitim başlangıcında gerekli solution altyapısını içermektedir. Eğitim Ubuntu sistemi üzerinde anlatılmaktadır. IDE olarak VS Code kullanılır.

## Başlangıç

```bash
# Postgresql ve pgadmin arabirimini ayağa kaldırmak için
sudo docker-compose up -d

# pgadmin arabirimine login olmak için
# http://localhost:5050/ adresine gidilir
# arabirimde scoth.tiger@email.com ve 123456 ile login olunur
# Aşağıdaki Connection bilgileri ile yeni sunucu tanımı eklenir
# Host name/address -> postgresdb
# Port -> 5432
# Maintanance Database -> postgres
# Username -> scoth
# Password -> tiger

# gamersworld veritabanının oluşturulması için
# GamersWorld.DataContext projesi içerisindeyken
# aşağıdaki komut ile migration planı hazırlanır
dotnet ef migrations add InitialCreate
# sonrasında plan işletilir ve veri tabanının oluştuğu gözlemlenir
dotnet ef database update

# Minimal web api projesini oluşturmak için
dotnet new web -o GamersWorld.WebApi

# Grpc Servis projesini oluşturmak için
dotnet new grpc -o GamersWorld.GrpcApi

# Grpc Client projesi için
dotnet new console -o GamersWorld.Grpc.Client
```

## Gerekli Paketler

Minimal Api projesinde aşağıdaki paketlerin olmasında yarar var

```text
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.OpenApi
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
```

Grpc servis projesinde ise ekstra bir paket gerekli değil ancak grpc istemcisi rolünü üstlenen Console uygulamasında aşağıdaki paketlerin olması gerekiyor.

```text
dotnet add package Grpc.Net.Client
dotnet add package Google.Protobuf
dotnet add package Grpc.Tools
```

## Yardımcı Bilgiler

- Unutursan eğer swagger adresi genellikle şöyle olacaktır -> http://localhost:5199/swagger/index.html
- Grpc proto dosyalarını ekledikten sonra bunları proje dosyasındaki ItemGroup içerisine koymayı da unutma. Ki derleme sonrası gerekli proto proxy servis içeriği otomatik olarak oluşturulsun.
- Projenin eğitim sonunda bitirilmiş hali [final isimli branch](https://github.com/buraksenyurt/DotNetServices_101/tree/final) üstünde tutulmaktadır.

## Kaynaklar

- Minimal Web API tasarımı için [şu öğretiye](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio-code) bakılabilir.
- Grpc tasarımı içinse [bu öğretiye](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-8.0&tabs=visual-studio-code) bakılabilir.
- OData örneği için [şu adrese](https://github.com/buraksenyurt/odata-challenge/tree/main) bakılabilir.
