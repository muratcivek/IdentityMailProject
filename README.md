# 📬 IdentityMail

**IdentityMail**, ASP.NET Core MVC ve ASP.NET Core Identity kullanılarak geliştirilmiş, rol tabanlı yetkilendirme ve yönetim özelliklerine sahip bir **şirket içi mesajlaşma sistemi** projesidir.

Sistem; çalışanların kendi aralarında mesaj göndermesine, gelen ve gönderilen mesajlarını yönetmesine, mesajları kategorilere ayırmasına ve gerektiğinde mesajları yöneticilere şikayet etmesine olanak sağlar.

Yönetici tarafında ise kullanıcı, rol, şikayet ve şifre sıfırlama taleplerinin yönetilebildiği kapsamlı bir admin paneli bulunmaktadır.

---

## 🚀 Özellikler

### 👤 Kimlik Doğrulama ve Kullanıcı Sistemi

- Kullanıcı kayıt sistemi
- Kullanıcı giriş / çıkış işlemleri
- ASP.NET Core Identity altyapısı
- Güvenli parola hashleme
- Benzersiz e-posta kontrolü
- Kullanıcı profil yönetimi
- Profil fotoğrafı desteği
- Aktif / pasif kullanıcı sistemi
- Rol tabanlı yetkilendirme
- Admin ve User rolleri
- Özelleştirilmiş Identity doğrulama mesajları

Parola politikası:

- Minimum 8 karakter
- En az bir büyük harf
- En az bir küçük harf
- En az bir rakam
- En az bir özel karakter

---

## ✉️ Mesajlaşma Sistemi

Kullanıcılar sistem içerisindeki diğer kullanıcılara mesaj gönderebilir.

Desteklenen temel işlemler:

- Yeni mesaj oluşturma
- Gelen kutusu
- Gönderilen mesajlar
- Mesaj detay ekranı
- Mesaja yanıt verme
- Mesaj kategorileri
- Gönderen ve alıcı bilgileri
- Mesaj gönderim tarihi
- Okundu / okunmadı durumu
- Mesaj önizleme
- Mesaj arama

---

## 📝 Taslak Sistemi

Tamamlanmamış mesajlar taslak olarak saklanabilir.

- Taslak oluşturma
- Taslakları listeleme
- Taslağı tekrar düzenleme
- Taslağı gönderme
- Taslak güncelleme tarihi
- Taslakların normal mesaj istatistiklerinden ayrılması

---

## ⭐ Önemli Mesajlar

Kullanıcılar gelen mesajlarını önemli olarak işaretleyebilir.

- Mesajı önemli olarak işaretleme
- Önemli işaretini kaldırma
- Önemli mesajları ayrı ekranda görüntüleme
- Yıldız göstergesi

---

## 📥 Gelen Kutusu

Gelen kutusunda mesajlar farklı kriterlere göre yönetilebilir.

### Filtreleme

- Tüm mesajlar
- Okunan mesajlar
- Okunmayan mesajlar
- Önemli mesajlar
- Kategori
- Gönderen
- Konu
- Tarih aralığı

Birden fazla filtre birlikte kullanılabilir.

### Sıralama

- Yeniden eskiye
- Eskiden yeniye

### Sayfalama

Mesaj listeleri EF Core üzerinden backend tarafında sayfalanmaktadır.

```csharp
.Skip((page - 1) * pageSize)
.Take(pageSize)
```

Bu sayede büyük mesaj listelerinin tamamının tek seferde yüklenmesi engellenir.

---

## 🗑️ Çöp Kutusu

Mesajlar doğrudan fiziksel olarak veritabanından silinmek yerine kullanıcı bazlı olarak çöp kutusuna taşınabilir.

Desteklenen işlemler:

- Gelen mesajı çöp kutusuna taşıma
- Gönderilen mesajı çöp kutusuna taşıma
- Çöp kutusundaki mesajları görüntüleme
- Mesajı geri yükleme
- Gönderen ve alıcı için ayrı silinme durumları

Bu yapı sayesinde bir kullanıcının mesajı silmesi diğer kullanıcının mesajını otomatik olarak kaldırmaz.

---

# 🚩 Mesaj Şikayet Sistemi

Kullanıcılar uygunsuz veya şüpheli mesajları yöneticilere bildirebilir.

### Şikayet nedenleri

- Spam
- Taciz
- Uygunsuz içerik
- Dolandırıcılık / şüpheli içerik
- Diğer

Bir kullanıcı aynı mesaj için yalnızca bir kez şikayet oluşturabilir.

Şikayet içerisinde:

- Şikayet eden kullanıcı
- Mesaj
- Şikayet nedeni
- Açıklama
- Oluşturulma tarihi
- İnceleme durumu

saklanmaktadır.

### Şikayet Durumları

Şikayetler yönetici tarafından farklı durumlara alınabilir:

- Bekliyor
- İnceleniyor
- İşlem Yapıldı
- Reddedildi

Kullanıcı kendi oluşturduğu şikayetlerin güncel durumunu **Şikayetlerim** ekranından takip edebilir.

---

# 🔑 Şifre Sıfırlama Talep Sistemi

IdentityMail, şirket içi kullanım senaryosuna uygun olarak yönetici kontrollü bir şifre sıfırlama sistemi içerir.

Sistem harici bir e-posta servisine bağımlı olmak zorunda değildir.

### Kullanıcı Akışı

```text
Kullanıcı
   ↓
Şifremi Unuttum
   ↓
E-posta adresini girer
   ↓
Şifre sıfırlama talebi oluşturulur
   ↓
Talep Admin Paneline gönderilir
```

### Yönetici Akışı

```text
Admin Paneli
   ↓
Şifre Talepleri
   ↓
Kullanıcı talebini görüntüler
   ↓
Şifreyi Sıfırla
   ↓
Geçici / yeni şifre belirler
   ↓
ASP.NET Core Identity şifreyi günceller
   ↓
Talep tamamlandı olarak işaretlenir
```

Şifre değiştirme işlemi ASP.NET Core Identity'nin token mekanizması üzerinden gerçekleştirilir.

```csharp
var token =
    await _userManager
        .GeneratePasswordResetTokenAsync(user);

var result =
    await _userManager.ResetPasswordAsync(
        user,
        token,
        newPassword);
```

---

# 🛡️ Admin Paneli

Admin rolüne sahip kullanıcılar sistem yönetim paneline erişebilir.

Admin paneli normal kullanıcılardan rol bazlı olarak ayrılmıştır.

```csharp
[Authorize(Roles = "Admin")]
```

## 📊 Dashboard

Dashboard üzerinden sistem genelindeki istatistikler görüntülenebilir.

- Toplam kullanıcı sayısı
- Aktif kullanıcı sayısı
- Toplam gönderilen mesaj
- Bugün gönderilen mesaj
- Okunmamış mesaj sayısı
- Çöp kutusundaki mesaj sayısı
- En fazla mesaj gönderen kullanıcılar
- En çok kullanılan kategoriler

---

# 👥 Kullanıcı Yönetimi

Yöneticiler sistemde kayıtlı kullanıcıları yönetebilir.

### Özellikler

- Kullanıcıları listeleme
- Kullanıcı adına göre arama
- Ad / soyada göre arama
- E-posta adresine göre arama
- Kullanıcıyı aktif yapma
- Kullanıcıyı pasif yapma
- Kullanıcının rollerini görüntüleme
- Kullanıcıya Admin rolü verme
- Admin rolünü kaldırma

Pasif hale getirilen kullanıcıların sisteme erişimi engellenebilir.

Yönetici güvenliği açısından admin kullanıcısının kendi hesabını yanlışlıkla pasif hale getirmesi engellenmiştir.

---

# 🛡️ Rol Yönetimi

Sistem dinamik rol yönetimini desteklemektedir.

Yönetici:

- Yeni rol oluşturabilir
- Rol silebilir
- Kullanıcıya rol atayabilir
- Kullanıcıdan rol kaldırabilir
- Kullanıcının mevcut rollerini görüntüleyebilir

Sistemin temel rolleri:

```text
Admin
User
```

`Admin` ve `User` rolleri uygulama başlatılırken kontrol edilir ve bulunmuyorsa otomatik olarak oluşturulur.

Temel rollerin yanlışlıkla silinmesine karşı koruma bulunmaktadır.

---

# 🚩 Admin Şikayet Yönetimi

Yöneticiler kullanıcılar tarafından gönderilen mesaj şikayetlerini merkezi bir ekrandan inceleyebilir.

Yönetici:

- Şikayet eden kullanıcıyı görüntüleyebilir
- Şikayet edilen mesajı görüntüleyebilir
- Mesaj gönderen kullanıcıyı görebilir
- Mesaj alıcısını görebilir
- Şikayet nedenini inceleyebilir
- Kullanıcının açıklamasını okuyabilir
- Şikayet durumunu değiştirebilir

İnceleme sırasında yönetici bilgisi ve inceleme tarihi sistem tarafından saklanabilir.

---

# 🔐 Güvenlik

Projede ASP.NET Core Identity kullanılmaktadır.

Uygulanan güvenlik mekanizmalarından bazıları:

- Password hashing
- Role-based authorization
- Authentication cookies
- Anti-forgery token kontrolü
- Unique e-mail kontrolü
- Identity password validation
- Admin endpoint koruması
- Kullanıcı aktif / pasif kontrolü
- Yetkisiz admin erişiminin engellenmesi
- Identity password reset token sistemi

POST işlemlerinde:

```csharp
[ValidateAntiForgeryToken]
```

kullanılarak CSRF saldırılarına karşı koruma sağlanmaktadır.

---

# 🛠️ Kullanılan Teknolojiler

| Teknoloji | Kullanım |
|---|---|
| ASP.NET Core MVC | Web uygulama altyapısı |
| C# | Backend geliştirme |
| ASP.NET Core Identity | Authentication ve Authorization |
| Entity Framework Core | ORM |
| SQL Server | Veritabanı |
| LINQ | Veri sorgulama |
| Razor Views | UI oluşturma |
| HTML5 | Sayfa yapısı |
| CSS3 | Arayüz tasarımı |
| JavaScript | İstemci tarafı işlemler |
| Bootstrap Icons | İkonlar |

---

# 📁 Proje Yapısı

```text
IdentityMail.Web
│
├── Bağımlılıklar
├── Connected Services
├── Properties
│
├── wwwroot
│   ├── css
│   ├── js
│   └── images
│
├── Context
│   └── AppDbContext.cs
│
├── Controllers
│   ├── AuthController.cs
│   ├── MessageController.cs
│   └── AdminController.cs
│
├── CustomValidation
│   └── CustomErrorDescriber.cs
│
├── DTOs
│   ├── AdminDtos
│   └── UserDtos
│
├── Entities
│   ├── AppUser.cs
│   ├── AppRole.cs
│   ├── UserMessage.cs
│   ├── MessageReport.cs
│   └── PasswordResetRequest.cs
│
├── Helpers
│
├── Migrations
│
├── Models
│
├── Views
│   ├── Admin
│   ├── Auth
│   ├── Message
│   ├── Shared
│   └── ...
│
├── appsettings.json
├── Program.cs
└── REQUIREMENTS.txt
```

---

# 🗄️ Veritabanı

Proje Entity Framework Core Code First yaklaşımını kullanmaktadır.

Başlıca veri yapıları:

```text
AspNetUsers
AspNetRoles
AspNetUserRoles

UserMessages
MessageReports
PasswordResetRequests
```

Identity tabloları kullanıcı ve rol yönetimini sağlarken uygulamaya özel tablolar mesajlaşma ve yönetim özelliklerini saklamaktadır.

---

# ⚙️ Kurulum

## 1. Repository'yi klonlayın

```bash
git clone <repository-url>
```

Proje klasörüne geçin:

```bash
cd IdentityMailProject/IdentityMail.Web
```

---

## 2. Connection String

`appsettings.json` içerisinde SQL Server bağlantınızı yapılandırın.

Örnek:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=IdentityMailProjectDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Gerçek kullanıcı adı, parola, production connection string veya diğer gizli bilgileri GitHub repository'sine eklemeyin.

Production ortamlarında Secret Manager veya environment variable kullanılması önerilir.

---

## 3. Veritabanını oluşturun

```bash
dotnet ef database update
```

EF Core mevcut migration'ları SQL Server veritabanına uygular.

---

## 4. Projeyi derleyin

```bash
dotnet build
```

---

## 5. Uygulamayı çalıştırın

```bash
dotnet run
```

---

# 🔑 Identity Yapılandırması

Uygulamada Identity şu temel yapı ile kullanılmaktadır:

```csharp
builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
```

`AddDefaultTokenProviders()` şifre sıfırlama gibi Identity token işlemlerinin kullanılmasını sağlar.

---

# 🧱 Mimari Yaklaşım

Proje klasik ASP.NET Core MVC yapısını kullanmaktadır.

```text
Request
   ↓
Controller
   ↓
DTO / Entity
   ↓
Entity Framework Core
   ↓
SQL Server
   ↓
Controller
   ↓
Razor View
   ↓
Response
```

Controller'lar HTTP isteklerini yönetirken Entity Framework Core veritabanı erişimini sağlar.

DTO yapısı özellikle kullanıcı ve admin ekranlarında Entity sınıflarının doğrudan View tarafına taşınmasını azaltmak amacıyla kullanılmaktadır.

---

# 📌 Temel Modüller

```text
IdentityMail
│
├── Authentication
│   ├── Register
│   ├── Login
│   ├── Logout
│   └── Forgot Password
│
├── Messaging
│   ├── Inbox
│   ├── Sent
│   ├── Drafts
│   ├── Important
│   ├── Reply
│   ├── Search / Filter
│   └── Trash
│
├── Reports
│   ├── Report Message
│   ├── My Reports
│   └── Report Status
│
└── Administration
    ├── Dashboard
    ├── User Management
    ├── Role Management
    ├── Report Management
    └── Password Reset Requests
```

---

# 🎯 Projenin Amacı

IdentityMail'in temel amacı yalnızca basit bir mesaj gönderme uygulaması oluşturmak değil; ASP.NET Core ekosistemindeki önemli backend geliştirme konularını gerçekçi bir **kurum içi mesajlaşma senaryosu** üzerinden uygulamaktır.

Proje özellikle aşağıdaki konular üzerine yoğunlaşmaktadır:

- ASP.NET Core MVC mimarisi
- ASP.NET Core Identity
- Authentication
- Authorization
- Role Management
- Entity Framework Core
- LINQ sorguları
- Code First yaklaşımı
- Migration yönetimi
- DTO kullanımı
- Admin panel geliştirme
- Kullanıcı yönetimi
- Mesaj yönetimi
- Soft delete yaklaşımı
- Arama, filtreleme ve sayfalama
- Şikayet / moderasyon sistemi
- Yönetici kontrollü şifre sıfırlama

---

# 🔮 Geliştirilebilecek Özellikler

Projenin ilerleyen sürümlerinde aşağıdaki özellikler eklenebilir:

- İlk girişte zorunlu şifre değiştirme
- Departman sistemi
- Kullanıcı grupları
- Toplu mesaj gönderme
- Dosya / belge ekleri
- Mesaj bildirim sistemi
- Gerçek zamanlı bildirimler
- SignalR entegrasyonu
- Gelişmiş audit log sistemi
- Admin işlem geçmişi
- Mesaj arşivleme
- Dashboard grafiklerinin genişletilmesi
- Kullanıcı oturum geçmişi
- İki faktörlü kimlik doğrulama
- Kurumsal LDAP / Active Directory entegrasyonu

---

# 📄 Lisans

Bu proje eğitim, portföy ve geliştirme amaçlı hazırlanmıştır.

---

## 📬 IdentityMail

**ASP.NET Core MVC • Identity • Entity Framework Core • SQL Server**

Rol tabanlı yetkilendirme, mesaj yönetimi, kullanıcı yönetimi, şikayet sistemi ve yönetici kontrollü şifre sıfırlama özelliklerine sahip şirket içi mesajlaşma uygulaması.
