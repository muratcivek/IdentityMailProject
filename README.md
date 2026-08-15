# IdentityMail

IdentityMail, kurum içi iletişim ihtiyaçları için geliştirilmiş, rol tabanlı yetkilendirme ve yönetim özelliklerine sahip bir **ASP.NET Core MVC mesajlaşma uygulamasıdır**.

Uygulama; kullanıcıların kurum içerisinde mesaj gönderebilmesini, gelen ve gönderilen mesajlarını yönetebilmesini, taslak oluşturabilmesini, önemli mesajlarını takip edebilmesini ve uygunsuz içerikleri yöneticilere bildirebilmesini sağlar.

Yönetim paneli üzerinden kullanıcılar, roller, mesaj şikayetleri ve şifre sıfırlama talepleri merkezi olarak yönetilebilir.

---

## Özellikler

### Mesaj Yönetimi

- Gelen kutusu
- Mesaj gönderme
- Gönderilen mesajları görüntüleme
- Mesaj detaylarını görüntüleme
- Mesaj yanıtlama
- Taslak oluşturma ve düzenleme
- Mesajları önemli olarak işaretleme
- Mesajları çöp kutusuna taşıma
- Mesaj arama
- Tarihe göre filtreleme
- Kategoriye göre filtreleme
- Okunma durumuna göre filtreleme
- Önemli mesajlara göre filtreleme
- Mesaj sıralama

### Mesaj Şikayet Sistemi

Kullanıcılar şüpheli veya uygunsuz olduğunu düşündükleri mesajları sistem yöneticilerine bildirebilir.

Şikayet oluşturulurken:

- Şikayet nedeni seçilebilir
- Ek açıklama girilebilir
- Aynı mesaj için tekrar şikayet oluşturulması engellenebilir
- Kullanıcı kendi oluşturduğu şikayetleri görüntüleyebilir
- Şikayetin inceleme durumu takip edilebilir

Yöneticiler gelen şikayetleri yönetim panelinden inceleyebilir ve durumlarını güncelleyebilir.

Şikayet durumları örnek olarak:

- İnceleme Bekliyor
- İnceleniyor
- İşlem Yapıldı
- Reddedildi

---

## Yönetim Paneli

Admin rolüne sahip kullanıcılar için ayrı yönetim özellikleri bulunmaktadır.

Yönetim paneli üzerinden:

- Kullanıcılar görüntülenebilir
- Kullanıcı hesapları aktif/pasif hale getirilebilir
- Sistem rolleri yönetilebilir
- Kullanıcılara rol atanabilir
- Kullanıcılardan roller kaldırılabilir
- Yeni roller oluşturulabilir
- Roller silinebilir
- Mesaj şikayetleri incelenebilir
- Şikayet durumları güncellenebilir
- Şifre sıfırlama talepleri yönetilebilir

---

## Rol ve Yetkilendirme

IdentityMail, **ASP.NET Core Identity** altyapısını kullanmaktadır.

Sistemde rol tabanlı yetkilendirme uygulanmıştır.

Temel roller:

| Rol | Yetki |
|---|---|
| `User` | Mesajlaşma ve standart kullanıcı işlemleri |
| `Admin` | Yönetim paneli ve sistem yönetimi |

Admin kullanıcıları sistem içerisindeki kullanıcı ve rol yönetimi işlemlerini gerçekleştirebilir.

---

## Şifre Sıfırlama Sistemi

IdentityMail, kurum içi kullanım senaryosuna uygun yönetici kontrollü bir şifre sıfırlama mekanizmasına sahiptir.

Süreç aşağıdaki şekilde çalışır:

```text
Kullanıcı
   │
   ▼
Şifremi Unuttum
   │
   ▼
E-posta adresini girer
   │
   ▼
Şifre sıfırlama talebi oluşturulur
   │
   ▼
Admin Paneli
   │
   ▼
Yönetici talebi inceler
   │
   ▼
Geçici / yeni parola belirlenir
   │
   ▼
Kullanıcı yeni parolasıyla giriş yapar
```

Bu yapı sayesinde harici bir e-posta servisine ihtiyaç duyulmadan kurum içi şifre sıfırlama süreci yönetilebilir.

---

## Kullanıcı Hesap Yönetimi

Kullanıcı hesapları yöneticiler tarafından kontrol edilebilir.

Bir hesap pasif duruma getirildiğinde kullanıcı sisteme giriş yapamaz.

Bu özellik özellikle:

- Kurumdan ayrılan çalışanların hesaplarının kapatılması
- Geçici hesap engelleme
- Yönetici kontrollü kullanıcı erişimi

gibi senaryolar için kullanılabilir.

---

## Kullanılan Teknolojiler

| Teknoloji | Kullanım |
|---|---|
| ASP.NET Core MVC | Web uygulama altyapısı |
| C# | Backend geliştirme |
| Entity Framework Core | ORM ve veri erişimi |
| SQL Server | Veritabanı |
| ASP.NET Core Identity | Authentication ve Authorization |
| Razor Views | Kullanıcı arayüzü |
| HTML5 | Sayfa yapısı |
| CSS3 | Arayüz tasarımı |
| Bootstrap Icons | İkon sistemi |

---

## Proje Yapısı

Projenin temel klasör yapısı aşağıdaki gibidir:

```text
IdentityMail.Web/
│
├── Context/
│   └── Veritabanı ve DbContext yapılandırmaları
│
├── Controllers/
│   └── MVC controller sınıfları
│
├── CustomValidation/
│   └── Identity doğrulama ve özel hata mesajları
│
├── DTOs/
│   └── Veri transfer modelleri
│
├── Entities/
│   └── Veritabanı entity sınıfları
│
├── Helpers/
│   └── Yardımcı sınıflar ve extension metotları
│
├── Migrations/
│   └── Entity Framework Core migration dosyaları
│
├── Models/
│   └── Uygulama modelleri
│
├── Views/
│   ├── Admin/
│   ├── Auth/
│   ├── Message/
│   ├── Profile/
│   └── Shared/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
│
├── appsettings.json
├── Program.cs
└── REQUIREMENTS.txt
```

---

## Mimari

Uygulama ASP.NET Core MVC mimarisi üzerine geliştirilmiştir.

```text
                    ┌─────────────────┐
                    │      User       │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │   Razor Views   │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │   Controllers   │
                    └────────┬────────┘
                             │
                 ┌───────────┴───────────┐
                 ▼                       ▼
        ┌─────────────────┐     ┌─────────────────┐
        │      DTOs       │     │ ASP.NET Identity│
        └────────┬────────┘     └────────┬────────┘
                 │                       │
                 └───────────┬───────────┘
                             ▼
                    ┌─────────────────┐
                    │ Entity Framework│
                    │      Core       │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │   SQL Server    │
                    └─────────────────┘
```

---

## Veritabanı Yapısı

Uygulamada Entity Framework Core Code First yaklaşımı kullanılmaktadır.

Temel veri yapıları arasında:

- Kullanıcılar
- Roller
- Kullanıcı-Rol ilişkileri
- Kullanıcı mesajları
- Mesaj şikayetleri
- Şifre sıfırlama talepleri

bulunmaktadır.

ASP.NET Core Identity tabloları kullanıcı kimlik doğrulama ve yetkilendirme işlemleri için kullanılmaktadır.

---

## Kurulum

### 1. Repository'yi Klonlayın

```bash
git clone <repository-url>
```

Ardından proje dizinine geçin:

```bash
cd IdentityMailProject/IdentityMail.Web
```

### 2. NuGet Paketlerini Yükleyin

```bash
dotnet restore
```

### 3. Veritabanı Bağlantısını Yapılandırın

`appsettings.json` içerisindeki connection string değerini kendi SQL Server ortamınıza göre yapılandırın.

Örnek yapı:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_SQL_SERVER_CONNECTION_STRING"
  }
}
```

### 4. Migration'ları Uygulayın

```bash
dotnet ef database update
```

### 5. Uygulamayı Çalıştırın

```bash
dotnet run
```

Uygulama başlatıldıktan sonra terminalde gösterilen localhost adresi üzerinden erişilebilir.

---

## Gereksinimler

Projeyi yerel geliştirme ortamında çalıştırmak için aşağıdaki bileşenler gereklidir:

- .NET SDK
- SQL Server
- Entity Framework Core CLI
- Visual Studio / Visual Studio Code / Rider
- Modern bir web tarayıcısı

Entity Framework Core CLI kurulu değilse:

```bash
dotnet tool install --global dotnet-ef
```

---

## Ekran Görüntüleri

Projenin temel ekranları bu bölüm altında gösterilebilir.

### Giriş

![Giriş Ekranı](docs/screenshots/login.png)

### Gelen Kutusu

![Gelen Kutusu](docs/screenshots/inbox.png)

### Mesaj Gönderme

![Mesaj Gönderme](docs/screenshots/send-message.png)

### Mesaj Detayı

![Mesaj Detayı](docs/screenshots/message-detail.png)

### Şikayetlerim

![Şikayetlerim](docs/screenshots/my-reports.png)

### Admin Paneli

![Admin Paneli](docs/screenshots/admin-dashboard.png)

### Şikayet Yönetimi

![Şikayet Yönetimi](docs/screenshots/admin-reports.png)

### Kullanıcı Yönetimi

![Kullanıcı Yönetimi](docs/screenshots/admin-users.png)

### Rol Yönetimi

![Rol Yönetimi](docs/screenshots/admin-roles.png)

### Şifre Sıfırlama Talepleri

![Şifre Talepleri](docs/screenshots/password-reset-requests.png)

---

## Ekran Görüntüsü Klasör Yapısı

Repository içerisinde ekran görüntülerini aşağıdaki şekilde saklayabilirsiniz:

```text
IdentityMailProject/
│
├── docs/
│   └── screenshots/
│       ├── login.png
│       ├── inbox.png
│       ├── send-message.png
│       ├── message-detail.png
│       ├── my-reports.png
│       ├── admin-dashboard.png
│       ├── admin-reports.png
│       ├── admin-users.png
│       ├── admin-roles.png
│       └── password-reset-requests.png
│
├── IdentityMail.Web/
│
└── README.md
```

---

## Temel İş Akışı

```text
                        IdentityMail
                             │
              ┌──────────────┴──────────────┐
              │                             │
            USER                          ADMIN
              │                             │
      ┌───────┼────────┐          ┌─────────┼──────────┐
      │       │        │          │         │          │
   Mesaj    Taslak  Şikayet   Kullanıcı   Roller   Şikayetler
      │       │        │       Yönetimi   Yönetimi     │
      │       │        │                              │
      └───────┴────────┘                              │
              │                                       │
              └──────────► SQL Server ◄───────────────┘
```

---

## Öne Çıkan Uygulama Özellikleri

IdentityMail yalnızca temel CRUD işlemlerinden oluşan bir uygulama değildir. Projede gerçek bir kurum içi mesajlaşma senaryosuna yönelik farklı iş süreçleri birlikte ele alınmıştır.

Bunlardan bazıları:

- Authentication ve Authorization
- Role-based Access Control
- Kullanıcı durum yönetimi
- Mesaj yaşam döngüsü yönetimi
- Taslak sistemi
- Çöp kutusu sistemi
- Önemli mesaj sistemi
- Gelişmiş mesaj filtreleme
- Şikayet ve moderasyon sistemi
- Kullanıcı şikayet durum takibi
- Yönetici kontrollü şifre sıfırlama
- Entity Framework Core migration yönetimi
- Responsive yönetim ve kullanıcı arayüzleri

---

## Geliştirme Durumu

IdentityMail aktif olarak geliştirilebilecek şekilde tasarlanmıştır.

İlerleyen sürümlerde aşağıdaki özellikler eklenebilir:

- Dosya ve belge ekleri
- Mesaj bildirim sistemi
- Departman bazlı kullanıcı grupları
- Toplu mesaj gönderimi
- Gelişmiş yönetim raporları
- Audit log sistemi
- Mesaj arşivleme
- Gerçek zamanlı bildirimler
- SignalR entegrasyonu
- Kurumsal LDAP / Active Directory entegrasyonu

---

## Lisans

Bu proje eğitim, geliştirme ve portföy amaçlı bir ASP.NET Core MVC uygulamasıdır.
