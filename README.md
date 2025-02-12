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

## Web Api Uygulamasında KeyCloak Kullanımı

Repoda yapılan son değişiklikle Minimal Web API fonksiyonlarına yetkilendirme özelliği de eklendi. Buna göre bazı servis çağrıları deneyimlemek amacıyla farklı roller tarafından çalıştırılabilir durumda. Tabii Authorization mekanizması için kullanıcıların bir Authority tarafından doğrulanması ve sahip oldukları yetkilerin alınabilmesi de gerekiyor. Yetkili merci olarak **[Key Cloak](https://www.keycloak.org/)** ürünü kullanılmakta. Bu ürünün docker imajı da mevcut dolayısıyla docker-compose aracılığıyla çalıştırılabilir _(Detaylar için docker-compose dosyasına bakılabilir)_ Örnekte iki rol grubu söz konusu. **SuperRole** ve **CustomersRole**. **Key Cloak** tarafında yapacağımız işleri aşağıdaki gibi özetleyebilir.

- Docker imajı başarılı bir şekilde başlatıldıktan sonra **localhost:8380/** adresinden sisteme admin/admin bilgileri ile girilir
- Organizasyonumuzu veya etkil alanını belirten **Realm** oluşturulur. Bu senaryoda **GamersWorld** olarak isimlendirilmiştir.
- Key Cloak'a gelecek uygulamalar birer istemcidir. Bu nedenle Clients sekmesinden yola çıkılarak bir client oluşturulur. **Client Id** için **gamersworld-api**, **client type** için **OpenID Connect** seçilebilir. Bu senaryoda **Client Authentication** seçeneği kapatılabilir(off). **Direct Grants Access Enabled** seçeneği **Enabled**'a çekilir (On), **Access Type** değeri ise **public** yapılarak client ayarları kaydedilir.
- Bu işlemlerin ardından gerekli rol tanımlamaları yapılır. **Create Role** ile bu örneklerde kullanılan **SuperRole** ve **CustomersRole** isimli roller tanımlanır.
- **Users** sekmesine geçilerek örnek kullanıcılar tanımlanır. Örnekte Scoth Tiger ve benzeri kullanıcılar tanımlanabilir. Kullanıcılar tanımlanırken **Role Mappings** sekmesinden de örnek rollere atama işlemleri gerçekleştirilir.

Web Api hizmetlerinin kullanılabilmesi için kullanıcıların öncelikle doğrulanması _(Authenticate)_ ve rollerinin yüklendiği **token** bilgilerinin oluşturulması gerekir. Bunun için öncelikle **localhost:8380/realms/GamersWorld/protocol/openid-connect/token** adresine aşağıdaki örnek bilgilerle **Http POST** talebi gönderilir. Bu bilgiler Body kısmında **x-www-form-urlencoded**  formatında olacak şekilde girilir.

```text
Client Id     : gamersworld-api
Grant Type    : password
Username      : scoth.tiger
Password      : 123456
```

Bir kullanıcının web api'den talepte bulunması hem doğrulamaya hem de yetkiye bağlıdır. Dolayısıyla kullanıcı öncelikle **KeyCloak** sistemine uğrayıp kullanıcı adı ve şifre girerek **gamersworld-api** isimli **client** için yetkilerini talep eder. **Realm** kullanılmasının bir sebebi de budur. Aynı kullanıcı farklı realm alanlarında farklı yetkilere sahip olacak şekilde tesis edilebilir ya da sadece tanımlı olduğu Realm içinde geçerli olabilir. Kullanıcı başarılı bir şekilde doğrulanmışsa bir **JWT** değeri üretilir. Çok doğal olarak servis çağrılarında bu token bilgisinin **Bearer Token** halinde pakete eklenmesi gerekir.

## Kaynaklar

- Minimal Web API tasarımı için [şu öğretiye](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio-code) bakılabilir.
- Grpc tasarımı içinse [bu öğretiye](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-8.0&tabs=visual-studio-code) bakılabilir.
- OData örneği için [şu adrese](https://github.com/buraksenyurt/odata-challenge/tree/main) bakılabilir.
