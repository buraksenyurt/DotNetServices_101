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

## OData ve GraphQL Servisleri için Örnek Sorgular

OData servisleri için aşağıdaki örnek sorgular kullanılabilir.

```text
http://localhost:5073/gamebox/v1/games?$select=title,point,releaseYear&orderby=point desc

http://localhost:5073/gamebox/v1/games?$apply=groupby((releaseYear),aggregate(point with average as AveragePoint))

http://localhost:5073/gamebox/v1/games/?$apply=aggregate(Point with average as AveragePoint)

http://localhost:5073/gamebox/v1/games/?$orderby=title&$select=title,onSale&top=10
```

GraphQL servisinde de şu örnek sorgular ele alınabilir.

Oyun listesi;

```graphql
query getGames{
        games{
            title,
            point,
            releaseYear,
            onSale
        }
    }
```

Belli bir oyuna ait bilgiler;

```graphql
query find {
        game(gameId: 7) {
            title
            onSale
            releaseYear
            point
        }
    }
```

Yeni bir oyun eklenmesi;

```graphql
mutation {
        addGame(
            payload: {
            title: "Tower Defense"
            point: 5.4
            releaseYear: 2005
            onSale: true
            }
        ) {
            game {
            gameId
            title
            onSale
            releaseYear
            point
            }
        }
    }
```

## Kaynaklar

- Minimal Web API tasarımı için [şu öğretiye](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio-code) bakılabilir.
- Grpc tasarımı içinse [bu öğretiye](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-8.0&tabs=visual-studio-code) bakılabilir.
- OData örneği için [şu adrese](https://github.com/buraksenyurt/odata-challenge/tree/main) bakılabilir.
