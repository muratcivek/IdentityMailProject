# 📧 IdentityMail

IdentityMail, **ASP.NET Core MVC**, **Entity Framework Core** ve **ASP.NET Core Identity** kullanılarak geliştirilmiş, kullanıcıların sistem içerisinde birbirleriyle mesajlaşmasını sağlayan web tabanlı bir mesajlaşma uygulamasıdır.

Proje; kullanıcı yönetimi, rol ve yetkilendirme, gelen/gönderilen mesajlar, taslaklar, önemli mesajlar, çöp kutusu, gelişmiş filtreleme, mesaj şikayet sistemi ve yönetici paneli gibi özellikler içermektedir.

---

## 🚀 Özellikler

### 👤 Kullanıcı İşlemleri

- Kullanıcı kayıt olma
- Kullanıcı giriş / çıkış işlemleri
- ASP.NET Core Identity tabanlı kimlik doğrulama
- Profil bilgilerini görüntüleme
- Profil bilgilerini güncelleme
- Şifre değiştirme
- Aktif / pasif kullanıcı sistemi
- Pasif kullanıcıların sisteme girişinin engellenmesi

---

### 📩 Mesajlaşma Sistemi

Kullanıcılar sistemde kayıtlı diğer kullanıcılara mesaj gönderebilir.

Desteklenen işlemler:

- Yeni mesaj gönderme
- Gelen kutusu
- Gönderilen mesajlar
- Mesaj detayını görüntüleme
- Mesaja yanıt verme
- Okundu / okunmadı takibi
- Mesaj kategorileri
- Mesaj gönderim tarihi
- Mesaj içeriği ve konu bilgisi

---

### ⭐ Önemli Mesajlar

Kullanıcılar gelen mesajları önemli olarak işaretleyebilir.

- Mesajı önemli olarak işaretleme
- Önemli işaretini kaldırma
- Önemli mesajları ayrı ekranda görüntüleme

---

### 📝 Taslak Sistemi

Gönderilmeye hazır olmayan mesajlar taslak olarak saklanabilir.

- Taslak kaydetme
- Taslakları listeleme
- Taslak düzenleme
- Taslağı gönderme
- Taslak silme

---

### 🗑️ Çöp Kutusu

Mesajlar doğrudan kalıcı olarak silinmek yerine çöp kutusuna taşınabilir.

- Gelen mesajı çöp kutusuna taşıma
- Gönderilen mesajı çöp kutusuna taşıma
- Çöp kutusundaki mesajları görüntüleme
- Mesajı geri yükleme
- Gönderen ve alıcı için bağımsız silme durumu

---

### 🔎 Arama, Filtreleme ve Sayfalama

Gelen kutusunda gelişmiş filtreleme desteği bulunmaktadır.

Mesajlar;

- Gönderen adına
- Mesaj konusuna
- Kategoriye
- Tarih aralığına
- Okundu durumuna
- Önemli durumuna

göre filtrelenebilir.

Ayrıca:

- Yeni → Eski sıralama
- Eski → Yeni sıralama
- Sayfalama
- Birden fazla filtrenin aynı anda kullanılması

desteklenmektedir.

Filtreleme işlemleri **Entity Framework Core sorguları üzerinden backend tarafında** gerçekleştirilmektedir.

---

## 🚩 Mesaj Şikayet Sistemi

Kullanıcılar uygunsuz olduğunu düşündükleri mesajları yöneticilere bildirebilir.

Şikayet nedenleri:

- Spam / Gereksiz Mesaj
- Taciz veya Rahatsız Edici İçerik
- Uygunsuz İçerik
- Dolandırıcılık / Şüpheli İçerik
- Diğer

Bir kullanıcı aynı mesaj için yalnızca bir kez şikayet oluşturabilir.

### Şikayet Durumları

Şikayetler yönetici tarafından incelenerek aşağıdaki durumlara getirilebilir:

- 🟠 İnceleme Bekliyor
- 🔵 İncelendi
- 🟢 İşlem Yapıldı
- 🔴 Reddedildi

Kullanıcılar **Şikayetlerim** ekranından oluşturdukları şikayetlerin güncel durumlarını takip edebilir.

---

# 🛡️ Admin Paneli

Sistem yöneticileri için ayrı bir yönetim paneli bulunmaktadır.

Admin paneline yalnızca gerekli role sahip kullanıcılar erişebilir.

## 📊 Dashboard

Admin dashboard üzerinde sistem genelindeki istatistikler görüntülenir.

- Toplam kullanıcı sayısı
- Aktif kullanıcı sayısı
- Toplam mesaj sayısı
- Bugün gönderilen mesaj sayısı
- Okunmamış mesaj sayısı
- Çöp kutusundaki mesaj sayısı
- En fazla mesaj gönderen kullanıcılar
- En çok kullanılan mesaj kategorileri

---

## 👥 Kullanıcı Yönetimi

Yöneticiler sistemdeki kullanıcıları görüntüleyebilir ve yönetebilir.

- Kullanıcıları listeleme
- Kullanıcı arama
- Kullanıcıyı aktif hale getirme
- Kullanıcıyı pasif hale getirme
- Kullanıcının rollerini görüntüleme
- Kullanıcıya Admin rolü verme
- Admin rolünü kaldırma

Kullanıcı arama işlemleri backend tarafında **Entity Framework Core** ile gerçekleştirilmektedir.

---

## 🔐 Rol ve Yetki Yönetimi

Proje ASP.NET Core Identity rol sistemi kullanmaktadır.

Yönetici tarafından:

- Yeni rol oluşturulabilir
- Mevcut roller görüntülenebilir
- Rol silinebilir
- Kullanıcıya rol atanabilir
- Kullanıcıdan rol kaldırılabilir

Örnek roller:

```text
Admin
User
```

Controller seviyesinde yetkilendirme uygulanabilir:

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
}
```

View tarafında da role göre içerik gösterilebilir:

```cshtml
@if (User.IsInRole("Admin"))
{
    <a asp-controller="Admin"
       asp-action="Index">
        Admin Paneli
    </a>
}
```

---

## 🚨 Şikayet Yönetimi

Admin paneli üzerinden kullanıcıların oluşturduğu mesaj şikayetleri incelenebilir.

Yönetici;

- Şikayetleri görüntüleyebilir
- Şikayet nedenini görebilir
- Şikayet edilen mesajı inceleyebilir
- Mesajın gönderenini görüntüleyebilir
- Mesajın alıcısını görüntüleyebilir
- Şikayet durumunu değiştirebilir

Şikayet durumu güncellendiğinde kullanıcı bu değişikliği **Şikayetlerim** ekranından görebilir.

---

# 🧰 Kullanılan Teknolojiler

| Teknoloji | Kullanım |
|---|---|
| ASP.NET Core MVC | Web uygulama mimarisi |
| C# | Backend geliştirme |
| Entity Framework Core | ORM / veri erişimi |
| Microsoft SQL Server | Veritabanı |
| ASP.NET Core Identity | Authentication & Authorization |
| Razor Views | Dinamik kullanıcı arayüzleri |
| LINQ | Veri sorgulama |
| Bootstrap Icons | Arayüz ikonları |
| HTML5 | Sayfa yapısı |
| CSS3 | Tasarım |
| JavaScript | İstemci tarafı işlemleri |

---

# 🏗️ Proje Yapısı

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
│   └── ...
│
├── Context
│   └── AppDbContext.cs
│
├── Controllers
│   ├── AuthController.cs
│   ├── MessageController.cs
│   ├── AdminController.cs
│   └── ProfileController.cs
│
├── CustomValidation
│
├── DTOs
│   ├── AdminDtos
│   ├── UserDtos
│   └── UserMessageDtos
│
├── Entities
│   ├── AppUser.cs
│   ├── UserMessage.cs
│   ├── MessageReport.cs
│   └── ...
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
│   ├── Profile
│   └── Shared
│
├── appsettings.json
└── Program.cs
```

---

# 📁 Mesaj View Yapısı

Mesaj sistemi farklı işlemler için ayrı Razor View dosyalarına ayrılmıştır.

```text
Views
└── Message
    ├── Index.cshtml
    ├── Sent.cshtml
    ├── Detail.cshtml
    ├── SendMail.cshtml
    ├── Important.cshtml
    ├── Drafts.cshtml
    ├── Trash.cshtml
    ├── Report.cshtml
    └── MyReports.cshtml
```

### View Görevleri

| View | Açıklama |
|---|---|
| `Index.cshtml` | Gelen mesajları görüntüler |
| `Sent.cshtml` | Gönderilen mesajları görüntüler |
| `Detail.cshtml` | Mesaj detayını gösterir |
| `SendMail.cshtml` | Yeni mesaj oluşturur |
| `Important.cshtml` | Önemli mesajları listeler |
| `Drafts.cshtml` | Taslak mesajları listeler |
| `Trash.cshtml` | Çöp kutusundaki mesajları gösterir |
| `Report.cshtml` | Mesaj şikayeti oluşturur |
| `MyReports.cshtml` | Kullanıcının şikayetlerini ve durumlarını gösterir |

---

# 🧩 Ortak Sidebar Yapısı

Mesaj sayfalarında sidebar kodunun tekrar edilmesini önlemek için Partial View kullanılmaktadır.

```text
Views
└── Shared
    └── _MessageSidebar.cshtml
```

Sayfalarda:

```cshtml
<partial name="_MessageSidebar" />
```

kullanılarak sidebar tek noktadan yönetilmektedir.

Bu sayede menüde yapılan değişikliklerin her `.cshtml` dosyasında ayrı ayrı uygulanmasına gerek kalmaz.

---

# 🗃️ Veritabanı

Proje **Microsoft SQL Server** kullanmaktadır.

Entity Framework Core Code First yaklaşımıyla veritabanı yapısı migration'lar üzerinden yönetilmektedir.

Başlıca veri yapıları:

```text
AspNetUsers
AspNetRoles
AspNetUserRoles
UserMessages
MessageReports
```

---

# ⚙️ Kurulum

Projeyi klonlayın:

```bash
git clone https://github.com/KULLANICI_ADIN/IdentityMailProject.git
```

Proje klasörüne geçin:

```bash
cd IdentityMailProject/IdentityMail.Web
```

NuGet paketlerini yükleyin:

```bash
dotnet restore
```

---

## 🔗 Connection String

`appsettings.json` içerisinde SQL Server bağlantınızı yapılandırın.

Örnek:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=IdentityMailProjectDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Connection string içerisindeki sunucu bilgilerini kendi SQL Server ortamınıza göre düzenleyin.

---

## 🗄️ Veritabanını Oluşturma

Migration'ları veritabanına uygulayın:

```bash
dotnet ef database update
```

Migration oluşturmak gerektiğinde:

```bash
dotnet ef migrations add MigrationName
```

---

## ▶️ Projeyi Çalıştırma

```bash
dotnet run
```

veya Visual Studio üzerinden:

```text
F5
```

ile projeyi çalıştırabilirsiniz.

---

# 🔐 Güvenlik

Projede temel güvenlik işlemleri ASP.NET Core Identity üzerinden gerçekleştirilmektedir.

- Authentication
- Role-based Authorization
- Anti-Forgery Token
- Password Hashing
- Kullanıcı bazlı veri kontrolü
- Admin yetkilendirmesi
- Aktif / pasif hesap kontrolü

POST işlemlerinde:

```csharp
[ValidateAntiForgeryToken]
```

kullanılarak CSRF saldırılarına karşı koruma sağlanmaktadır.

---

# 💡 Mimari Yaklaşım

Projede sorumlulukların ayrılması amacıyla aşağıdaki yapı kullanılmaktadır:

```text
Controller
    ↓
DTO
    ↓
Entity Framework Core
    ↓
SQL Server
    ↓
Razor View
```

Entity sınıfları veritabanı modellerini, DTO sınıfları ise View ve Controller arasında taşınması gereken verileri temsil etmektedir.

---

# 📌 Öne Çıkan Noktalar

Bu projede yalnızca temel CRUD işlemleri değil, gerçek bir mesajlaşma uygulamasında ihtiyaç duyulabilecek birçok özellik birlikte uygulanmıştır:

- ASP.NET Core Identity
- Role Based Authorization
- Admin Paneli
- Kullanıcı Yönetimi
- Rol Yönetimi
- Mesajlaşma Sistemi
- Yanıtlama Sistemi
- Okundu / Okunmadı Sistemi
- Önemli Mesajlar
- Taslak Sistemi
- Çöp Kutusu
- Gelişmiş Filtreleme
- Backend Arama
- Sayfalama
- Mesaj Şikayet Sistemi
- Şikayet Durum Takibi
- Dashboard İstatistikleri
- Partial View kullanımı
- Entity Framework Core Code First

---

# 🔮 Geliştirilebilecek Özellikler

Projenin ilerleyen sürümlerinde aşağıdaki özellikler eklenebilir:

- 📎 Dosya / görsel eki gönderme
- 🔔 Gerçek zamanlı bildirim sistemi
- 💬 SignalR ile anlık mesajlaşma
- 📧 E-posta bildirimleri
- 🔍 Admin tarafında gelişmiş log sistemi
- 📊 Grafik tabanlı dashboard
- 🗑️ Çöp kutusunu otomatik temizleme
- 📱 Mobil görünüm iyileştirmeleri
- 🔐 İki faktörlü kimlik doğrulama (2FA)
- 📝 Admin işlem kayıtları / Audit Log

---

# 📸 Ekran Görüntüleri

Projeye ait ekran görüntülerini repository içerisinde örneğin:

```text
screenshots/
```

klasörüne ekleyerek bu bölümde gösterebilirsiniz.

```markdown
![Gelen Kutusu](screenshots/inbox.png)
![Mesaj Detayı](screenshots/message-detail.png)
![Admin Dashboard](screenshots/admin-dashboard.png)
![Kullanıcı Yönetimi](screenshots/user-management.png)
![Şikayet Yönetimi](screenshots/reports.png)
```

---

# 👨‍💻 Geliştirici

Bu proje ASP.NET Core MVC ve Entity Framework Core teknolojileri üzerinde pratik yapmak ve kapsamlı bir mesajlaşma/yönetim sistemi geliştirmek amacıyla hazırlanmıştır.

---

## ⭐ Projeyi Beğendiyseniz

Projeyi faydalı bulduysanız GitHub üzerinden ⭐ vermeyi unutmayın.

---

**IdentityMail — ASP.NET Core MVC ile geliştirilmiş rol tabanlı mesajlaşma ve yönetim sistemi.**
