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

```
