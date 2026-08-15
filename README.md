# 📬 IdentityMail

**IdentityMail**, kurum içi iletişim ihtiyaçları için geliştirilmiş, rol tabanlı yetkilendirme ve yönetim özelliklerine sahip bir **ASP.NET Core MVC mesajlaşma uygulamasıdır**.

Uygulama; kullanıcıların kurum içerisinde mesaj göndermesine, gelen ve gönderilen mesajlarını yönetmesine, taslak oluşturmasına, önemli mesajları takip etmesine, mesajları filtrelemesine ve uygunsuz veya şüpheli içerikleri yöneticilere bildirmesine olanak sağlar.

Yönetim paneli üzerinden kullanıcılar, roller, mesaj şikayetleri ve şifre sıfırlama talepleri merkezi olarak yönetilebilir.

---

## ✨ Özellikler

### 📩 Mesaj Yönetimi

- Gelen kutusu
- Yeni mesaj gönderme
- Gönderilen mesajları görüntüleme
- Mesaj detaylarını görüntüleme
- Mesaj yanıtlama
- Taslak oluşturma ve düzenleme
- Mesajları önemli olarak işaretleme
- Mesajları çöp kutusuna taşıma
- Gelen mesaj sayısını görüntüleme
- Mesaj arama
- Tarihe göre filtreleme
- Kategoriye göre filtreleme
- Okunma durumuna göre filtreleme
- Önemli mesajlara göre filtreleme
- Mesaj sıralama

### 🚩 Mesaj Şikayet Sistemi

Kullanıcılar şüpheli veya uygunsuz olduğunu düşündükleri mesajları sistem yöneticilerine bildirebilir.

Şikayet sistemi kapsamında:

- Şikayet nedeni seçilebilir
- Şikayete açıklama eklenebilir
- Aynı mesaj için tekrar şikayet oluşturulması engellenebilir
- Kullanıcı kendi oluşturduğu şikayetleri görüntüleyebilir
- Kullanıcı şikayetinin güncel durumunu takip edebilir
- Yönetici gelen şikayetleri inceleyebilir
- Yönetici şikayet durumunu güncelleyebilir

Şikayet durumları:

- İnceleme Bekliyor
- İncelendi
- İşlem Yapıldı
- Reddedildi

---

## 🛡️ Yönetim Paneli

`Admin` rolüne sahip kullanıcılar için ayrı bir yönetim paneli bulunmaktadır.

Yönetim paneli üzerinden:

- Kullanıcılar görüntülenebilir
- Kullanıcı hesapları aktif/pasif hale getirilebilir
- Sistem rolleri görüntülenebilir
- Yeni roller oluşturulabilir
- Roller silinebilir
- Kullanıcılara rol atanabilir
- Kullanıcılardan roller kaldırılabilir
- Mesaj şikayetleri incelenebilir
- Şikayet durumları güncellenebilir
- Şifre sıfırlama talepleri görüntülenebilir
- Kullanıcı şifreleri yönetici tarafından sıfırlanabilir

---

## 🔐 Kimlik Doğrulama ve Yetkilendirme

IdentityMail, kullanıcı yönetimi için **ASP.NET Core Identity** altyapısını kullanmaktadır.

Sistemde rol tabanlı yetkilendirme uygulanmıştır.

| Rol | Yetki |
|---|---|
| `User` | Mesajlaşma ve standart kullanıcı işlemleri |
| `Admin` | Standart işlemler + yönetim paneli ve sistem yönetimi |

Kullanıcı hesaplarının aktiflik durumu sistem tarafından kontrol edilir. Pasif hale getirilen kullanıcıların sisteme erişimi engellenebilir.

---

## 🔑 Şifre Sıfırlama Sistemi

IdentityMail, kurum içi kullanım senaryosuna uygun **yönetici kontrollü şifre sıfırlama mekanizmasına** sahiptir.

Süreç:

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
Yönetici talebi görüntüler
   │
   ▼
Yeni / geçici parola belirler
   │
   ▼
Talep tamamlandı olarak işaretlenir
   │
   ▼
Kullanıcı yeni parolasıyla giriş yapar
```

Bu yaklaşım sayesinde şifre sıfırlama işlemleri kurum yöneticisinin kontrolünde gerçekleştirilebilir.

---

## 👤 Kullanıcı Hesap Yönetimi

Yöneticiler kullanıcı hesaplarını merkezi olarak yönetebilir.

Kullanıcı hesabı pasif duruma getirildiğinde kullanıcının sisteme erişimi engellenebilir.

Bu yapı özellikle:

- Kurumdan ayrılan kullanıcıların erişiminin kapatılması
- Geçici hesap engelleme
- Yönetici kontrollü kullanıcı erişimi
- Kurum içi hesap yönetimi

gibi senaryolarda kullanılabilir.

---

# 🛠️ Kullanılan Teknolojiler

| Teknoloji | Kullanım |
|---|---|
| ASP.NET Core MVC | Web uygulama altyapısı |
| C# | Backend geliştirme |
| Entity Framework Core | ORM ve veri erişimi |
| SQL Server | İlişkisel veritabanı |
| ASP.NET Core Identity | Authentication ve Authorization |
| Razor Views | Dinamik kullanıcı arayüzleri |
| HTML5 | Sayfa yapısı |
| CSS3 | Arayüz tasarımı |
| Bootstrap Icons | İkon sistemi |

---

# 📁 Proje Yapısı

```text
IdentityMailProject/
│
├── IdentityMail.Web/
│   │
│   ├── Context/
│   │   └── DbContext ve veritabanı yapılandırmaları
│   │
│   ├── Controllers/
│   │   └── MVC Controller sınıfları
│   │
│   ├── CustomValidation/
│   │   └── Identity doğrulama ve özel hata mesajları
│   │
│   ├── DTOs/
│   │   └── Veri transfer modelleri
│   │
│   ├── Entities/
│   │   └── Veritabanı entity sınıfları
│   │
│   ├── Helpers/
│   │   └── Yardımcı sınıflar ve extension metotları
│   │
│   ├── Migrations/
│   │   └── Entity Framework Core migration dosyaları
│   │
│   ├── Models/
│   │   └── Uygulama modelleri
│   │
│   ├── Views/
│   │   ├── Admin/
│   │   ├── Auth/
│   │   ├── Message/
│   │   ├── Profile/
│   │   └── Shared/
│   │
│   ├── wwwroot/
│   │   ├── css/
│   │   ├── js/
│   │   └── images/
│   │
│   ├── appsettings.json
│   ├── Program.cs
│   └── REQUIREMENTS.txt
│
├── screenshots/
│   └── Uygulama ekran görüntüleri
│
├── IdentityMailProject.sln
├── .gitignore
└── README.md
```

---

# 🏗️ Mimari

Uygulama **ASP.NET Core MVC** mimarisi üzerine geliştirilmiştir.

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
        ┌─────────────────┐     ┌──────────────────┐
        │      DTOs       │     │ ASP.NET Identity │
        └────────┬────────┘     └────────┬─────────┘
                 │                       │
                 └───────────┬───────────┘
                             │
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

# 🗄️ Veritabanı

Projede **Entity Framework Core Code First** yaklaşımı kullanılmaktadır.

Temel veri yapıları:

- Kullanıcılar
- Roller
- Kullanıcı-Rol ilişkileri
- Kullanıcı mesajları
- Mesaj şikayetleri
- Şifre sıfırlama talepleri

ASP.NET Core Identity tabloları kullanıcı kimlik doğrulama ve yetkilendirme işlemlerinin yönetilmesinde kullanılmaktadır.

Veritabanı şemasındaki değişiklikler Entity Framework Core migration sistemi ile takip edilmektedir.

---

# 🚀 Kurulum

## 1. Repository'yi Klonlayın

```bash
git clone <repository-url>
```

Proje dizinine geçin:

```bash
cd IdentityMailProject/IdentityMail.Web
```

## 2. Bağımlılıkları Yükleyin

```bash
dotnet restore
```

## 3. Veritabanı Bağlantısını Yapılandırın

`appsettings.json` içerisindeki connection string değerini kendi SQL Server ortamınıza göre yapılandırın.

Örnek:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_SQL_SERVER_CONNECTION_STRING"
  }
}
```

## 4. Migration'ları Uygulayın

```bash
dotnet ef database update
```

## 5. Projeyi Derleyin

```bash
dotnet build
```

## 6. Uygulamayı Çalıştırın

```bash
dotnet run
```

Uygulama başlatıldıktan sonra terminalde gösterilen localhost adresi üzerinden erişilebilir.

---

# ⚙️ Gereksinimler

Projeyi yerel geliştirme ortamında çalıştırmak için:

- .NET SDK
- SQL Server
- Entity Framework Core CLI
- Visual Studio, Visual Studio Code veya Rider
- Modern bir web tarayıcısı

Entity Framework Core CLI kurulu değilse:

```bash
dotnet tool install --global dotnet-ef
```

Kurulumu kontrol etmek için:

```bash
dotnet ef
```

---

# 🖼️ Uygulama Ekran Görüntüleri

## 🔐 Giriş

Kullanıcıların e-posta adresi ve parolalarıyla sisteme giriş yapabildiği ekran.

![Giriş Ekranı](screenshots/login.png)

---

## 📝 Kayıt Ol

Yeni kullanıcı hesabı oluşturma ekranı.

![Kayıt Ol](screenshots/register.png)

---

## 🔑 Şifremi Unuttum

Kullanıcının yöneticiye şifre sıfırlama talebi oluşturabildiği ekran.

![Şifremi Unuttum](screenshots/forgot_password.png)

---

## 📥 Gelen Kutusu

Kullanıcının aldığı mesajları görüntülediği; arama, filtreleme ve sıralama işlemlerini gerçekleştirebildiği ana mesaj ekranı.

![Gelen Kutusu](screenshots/gelen_kutusu.png)

---

## ✉️ Yeni Mesaj

Kurum içerisindeki diğer kullanıcılara yeni mesaj gönderme ekranı.

![Yeni Mesaj](screenshots/yeni_mesaj.png)

---

## 📤 Gönderilenler

Kullanıcının daha önce gönderdiği mesajları görüntülediği ekran.

![Gönderilenler](screenshots/gonderilenler.png)

---

## 📝 Taslaklar

Henüz gönderilmemiş ve taslak olarak kaydedilmiş mesajların yönetildiği ekran.

![Taslaklar](screenshots/taslaklar.png)

---

## ⭐ Önemli Mesajlar

Kullanıcı tarafından önemli olarak işaretlenen mesajların görüntülendiği ekran.

![Önemli Mesajlar](screenshots/onemliler.png)

---

## 🗑️ Çöp Kutusu

Kullanıcının çöp kutusuna taşıdığı mesajları görüntülediği ekran.

![Çöp Kutusu](screenshots/cop_kutusu.png)

---

## 📄 Mesaj Detayı

Seçilen mesajın gönderen, alıcı, kategori, okunma durumu ve içerik bilgilerinin görüntülendiği ekran.

![Mesaj Detayı](screenshots/mesaj_detayi.png)

---

## ↩️ Mesaj Yanıtlama

Gelen bir mesaja doğrudan yanıt oluşturma ekranı.

![Mesaj Yanıtlama](screenshots/yanitla.png)

---

## 🚩 Şikayetlerim

Kullanıcının bildirdiği mesaj şikayetlerini ve bu şikayetlerin inceleme durumlarını takip edebildiği ekran.

![Şikayetlerim](screenshots/sikayetlerim.png)

---

## 👤 Profil Bilgileri

Kullanıcının hesap ve profil bilgilerini görüntüleyebildiği profil ekranı.

![Profil Bilgileri](screenshots/profil_bilgileri.png)

---

## 🔒 Şifre ve Güvenlik

Kullanıcının hesap güvenliği ve parola işlemlerini yönetebildiği ekran.

![Şifre ve Güvenlik](screenshots/sifre_ve_guvenlik.png)

---

# 🛡️ Yönetim Paneli Ekranları

## 📊 Admin Dashboard

Sisteme ait genel yönetim bilgilerinin görüntülendiği yönetici kontrol paneli.

![Admin Dashboard](screenshots/admin_dashboard.png)

---

## 👥 Kullanıcı Yönetimi

Yöneticilerin sistem kullanıcılarını görüntüleyebildiği ve kullanıcı hesaplarını yönetebildiği ekran.

![Kullanıcı Yönetimi](screenshots/admin_kullanici_yonetimi.png)

---

## 🛡️ Rol Yönetimi

Sistem rollerinin oluşturulması, silinmesi ve kullanıcı rollerinin yönetilmesi için kullanılan ekran.

![Rol Yönetimi](screenshots/admin_rol_yonetimi.png)

---

## 🚩 Şikayet Yönetimi

Kullanıcılar tarafından bildirilen mesajların yönetici tarafından incelendiği ve şikayet durumlarının güncellendiği ekran.

![Admin Şikayet Yönetimi](screenshots/admin_sikayetler.png)

---

## 🔑 Şifre Sıfırlama Talepleri

Kullanıcıların oluşturduğu şifre sıfırlama taleplerinin yöneticiler tarafından görüntülendiği ekran.

![Şifre Sıfırlama Talepleri](screenshots/admin_sifre_talepleri.png)

---

## 🔐 Yönetici Şifre Sıfırlama

Yöneticinin seçilen kullanıcı için yeni veya geçici parola belirlediği ekran.

![Yönetici Şifre Sıfırlama](screenshots/admin_sifre_sifirla.png)

---

# 🔄 Temel İş Akışı

```text
                           IdentityMail
                                │
              ┌─────────────────┴─────────────────┐
              │                                   │
            USER                                ADMIN
              │                                   │
      ┌───────┼──────────┐             ┌──────────┼───────────┐
      │       │          │             │          │           │
    Mesaj   Taslak    Şikayet      Kullanıcı    Roller    Şikayetler
      │       │          │          Yönetimi    Yönetimi      │
      │       │          │                                  │
      │       │          └──────► Durum Takibi ◄────────────┤
      │       │                                             │
      │       │                              Şifre Talepleri │
      │       │                                     │       │
      └───────┴────────────────┬────────────────────┴───────┘
                               │
                               ▼
                         ┌────────────┐
                         │ SQL Server │
                         └────────────┘
```

---

# 💡 Öne Çıkan Uygulama Özellikleri

IdentityMail, standart bir mesaj CRUD uygulamasının ötesinde kurum içi mesajlaşma senaryosuna yönelik farklı iş süreçlerini bir araya getirir.

Projede bulunan başlıca uygulama özellikleri:

- Authentication
- Authorization
- Role-Based Access Control
- ASP.NET Core Identity entegrasyonu
- Kullanıcı aktif/pasif durum yönetimi
- Gelen ve gönderilen mesaj yönetimi
- Mesaj yanıtlama
- Taslak sistemi
- Çöp kutusu sistemi
- Önemli mesaj sistemi
- Mesaj okunma durumu
- Mesaj kategorileri
- Gelişmiş mesaj arama ve filtreleme
- Mesaj sıralama
- Şikayet ve moderasyon sistemi
- Kullanıcı şikayet durum takibi
- Admin şikayet yönetimi
- Yönetici kontrollü şifre sıfırlama
- Kullanıcı ve rol yönetimi
- Entity Framework Core migration yönetimi
- Responsive kullanıcı arayüzü

---

# 🔮 Geliştirilebilir Özellikler

Proje ilerleyen sürümlerde aşağıdaki özelliklerle genişletilebilir:

- 📎 Dosya ve belge ekleri
- 🔔 Mesaj bildirim sistemi
- 👥 Departman bazlı kullanıcı grupları
- 📢 Toplu mesaj gönderimi
- 📊 Gelişmiş yönetim raporları
- 🧾 Audit log sistemi
- 🗃️ Mesaj arşivleme
- ⚡ Gerçek zamanlı bildirimler
- 🔄 SignalR entegrasyonu
- 🏢 LDAP / Active Directory entegrasyonu

---

# 📌 Proje Amacı

IdentityMail; **ASP.NET Core MVC, Entity Framework Core, SQL Server ve ASP.NET Core Identity** teknolojilerinin gerçek bir kurum içi mesajlaşma senaryosunda birlikte kullanımını göstermek amacıyla geliştirilmiştir.

Proje içerisinde kullanıcı mesajlaşmasının yanında rol tabanlı yetkilendirme, yönetim paneli, moderasyon, kullanıcı hesap yönetimi ve şifre sıfırlama gibi farklı iş süreçleri tek bir uygulama içerisinde ele alınmaktadır.

---

# 📄 Lisans

Bu proje eğitim, geliştirme ve portföy amaçlı hazırlanmış bir ASP.NET Core MVC uygulamasıdır.
