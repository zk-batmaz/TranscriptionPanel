# TranscriptionPanel
Bu web uygulaması, kullanıcıların yüklediği ses dosyalarına ait transkriptleri yönetmesini sağlayan bir Transkripsiyon Yönetim Panelidir.
Kullanıcılar sisteme ses dosyalarını ve karşılık gelen transkript metinlerini birlikte yükleyebilir. Uygulama içerisinde bu kayıtlar listelenebilir, düzenlenebilir veya silinebilir.
Uygulama, rol tabanlı erişim kontrolüyle çalışır:
 > Adminler, sistemde kullanıcı yönetimi (ekleme, silme) gibi yetkilere sahiptir.
 > Editörler, transkript girişleri ekleyip düzenleyebilir.
Modern Angular arayüzü ve güçlü .NET Core API altyapısıyla geliştirilen bu sistem, içerik yönetimini kolaylaştırır ve sade bir kullanıcı deneyimi sunar.

- Frontend: Angular + Angular Material
- Backend: ASP.NET Core Web API
- Özellikler:
  - Giriş/Çıkış (JWT Auth)
  
  ![](/Images/Login.png)

  - Kullanıcı yönetimi (sadece admin)
  
  ![](/Images/userControlPanel.png)

  - Transkripsiyon yükleme (editör ve admin)
  
  ![](/Images/addingTranscription.png)

  - Aktivite geçmişi görüntüleme (sadece admin)
  
  ![](/Images/Activities.png)
 
## Gereksinimler

| Araç               | Sürüm                       |
| ------------------ | --------------------------- |
| Node.js            | v22.15.1                    |
| npm                | v10.9.2                     |
| Angular CLI        | v19.2.12                    |
| .NET SDK           | v8.0.409                    |
| Angular Framework  | v19.2.11                    |

API: https://localhost:5001


Projeyi Çalıştırmak için split terminalde hem backend hem frontend uygulamasının başlatılması gerekmektedir.

![Split terminalde projeyi çalıştırma](/Images/baslatma.png)

## Kayıtlı Kullanıcılar
> Kullanıcı adı: `admin`  
> Şifre: `admin123`

> Kullanıcı adı: `editor`  
> Şifre: `editor123`
