# IoT‑Bridge

## Açıklama

IoT‑Bridge projesi **MQTT protokolü** ile aktarılan verileri dinleyip **REST API** aracılığıyla erişilebilir hale getiren .NET tabanlı bir köprü uygulamasıdır. IoT sistem entegratörleri ve bu alanda kendini geliştirmek isteyen geliştiriciler için öğretici ve kullanılabilir bir temel oluşturur.

## Mimarî Bileşenler

| Bileşen                        | Açıklama                                                                                                             |
| ------------------------------ | -------------------------------------------------------------------------------------------------------------------- |
| `MqttSubscriber`               | Belirlenen topic’leri dinleyen MQTT konsol uygulamasıdır.                                                            |
| `MqttSubscriber_WorkerService` | Konsol uygulamasının Windows arkaplan servisi (Worker Service) olarak yapılandırılmış versiyonudur.                  |
| `RestAPI`                      | Gelen verileri PostgreSQL veritabanına yazar ve ASP.NET Core Web API ile REST endpoint’leri üzerinden erişim sağlar. |

## Kullanılan Teknolojiler

* **.NET 8**
* **ASP.NET Core Web API** (REST servisleri için)
* **MQTT** (veri iletimi – HiveMQ Cloud kullanılabilir)
* **PostgreSQL** (veri depolama)

## Gereksinimler

* .NET 8 SDK
* PostgreSQL (yerel veya uzak)
* Bir MQTT broker (örneğin HiveMQ Cloud / Mosquitto)

## Kurulum

```bash
git clone https://github.com/muharremdemirr/IoT-Bridge.git
cd IoT-Bridge
dotnet restore
dotnet build
```

## Konfigürasyon

`appsettings.json` dosyalarındaki aşağıdaki alanlar kullanıcıya göre düzenlenmelidir:

```
"Mqtt": {
  "BrokerAddress": "broker.hivemq.com",
  "Port": 1883,
  "Topic": "iot/sensor/#"
},
"ConnectionStrings": {
  "DefaultConnection": "Host=your_db_host;Port=5432;Database=your_db_name;Username=your_db_user;Password=your_db_password"
}
```

## Çalıştırma

```bash
# MqttSubscriber
cd MqttSubscriber
dotnet run

# MqttSubscriber_WorkerService (Windows Service olarak konfigüre edilebilir)
cd MqttSubscriber_WorkerService
dotnet run

# RestAPI
cd RestAPI
dotnet run
```

## Kullanım Örnekleri

### MQTT üzerinden veri yayınlama

```bash
mosquitto_pub -h broker.hivemq.com -t "iot/sensor/temperature" -m "25.3"
```

### REST API ile veri çekme

```bash
curl http://localhost:5000/api/sensors/latest
```

## Katkıda Bulunma

* Repository’yi fork’layın
* Yeni bir branch oluşturun
* Değişikliklerinizi yapın ve commit edin
* Pull request gönderin

## Lisans

Bu proje MIT lisansı ile yayınlanmıştır.
