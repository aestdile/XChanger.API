# 🔄 XChanger API

> **Format konvertatsiya qilish uchun mo'ljallangan API**
> 
> Excel → SQL Server Database → XML

---

## 📋 Loyiha haqida

XChanger API - bu Excel fayllardan ma'lumotlarni o'qib, ularni SQL Server ma'lumotlar bazasiga saqlash va XML formatida qaytarish uchun mo'ljallangan RESTful API.

### ✨ Asosiy imkoniyatlar

- 📊 **Excel fayllarni o'qish** - SheetJS kutubxonasi orqali
- 💾 **SQL Server integratsiyasi** - Entity Framework Core orqali
- 📄 **XML formatda eksport** - System.Xml.Linq orqali
- 🔍 **Ma'lumotlarni filtrlash** - Minus qiymatli petlarni avtomatik filtrlash

---

---

## 📊 Ma'lumotlar Modeli

### 👤 Person Model
```csharp
public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public List<Pet> Pets { get; set; }
}
```

### 🐾 Pet Model
```csharp
public class Pet
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public PetType Type { get; set; }
    public Guid PersonId { get; set; }
    public Person Person { get; set; }
}
```

### 🏷️ Pet Type Enum
```csharp
public enum PetType
{
    Cat = 1,
    Dog = 2,
    Parrot = 3
}
```

### 📥 External Person Model (Excel import uchun)
```csharp
public class ExternalPersonModel
{
    public string PersonName { get; set; }
    public int Age { get; set; }
    public string PetOne { get; set; }
    public string PetOneType { get; set; }
    public string PetTwo { get; set; }
    public string PetTwoType { get; set; }
    public string PetThree { get; set; }
    public string PetThreeType { get; set; }
}
```

---

## 🔌 API Endpointlar

### 📋 Ma'lumotlarni olish

| Method | Endpoint | Tavsif |
|--------|----------|--------|
| `GET` | `/api/persons` | Barcha personlarni olish |
| `GET` | `/api/pets` | Barcha petlarni olish |
| `GET` | `/api/persons/with-pets` | Personlar va ularning petlari |

### 📤 Ma'lumotlarni yuklash

| Method | Endpoint | Tavsif |
|--------|----------|--------|
| `POST` | `/api/import/excel` | Excel faylni yuklash va import qilish |
| `GET` | `/api/export/xml` | Ma'lumotlarni XML formatida olish |

---

## 📋 Excel Fayl Formati

Excel faylda quyidagi ustunlar bo'lishi kerak:

| Ustun | Tavsif |
|-------|--------|
| `PersonName` | Shaxs ismi |
| `Age` | Yoshi |
| `Pet1` | Birinchi pet nomi |
| `Pet1 Type` | Birinchi pet turi |
| `Pet2` | Ikkinchi pet nomi |
| `Pet2 Type` | Ikkinchi pet turi |
| `Pet3` | Uchinchi pet nomi |
| `Pet3 Type` | Uchinchi pet turi |

### 📝 Misol:

| PersonName | Age | Pet1 | Pet1 Type | Pet2 | Pet2 Type | Pet3 | Pet3 Type |
|------------|-----|------|-----------|------|-----------|------|-----------|
| Mary | 32 | Markiza | Cat | Pushistik | Cat | Belka | Dog |
| Gary | 18 | Norm | Dog | - | - | - | - |
| Mike | 24 | Bert | Parrot | Gosha | Parrot | Milka | Cat |
| Alex | 25 | Casper | Cat | Charli | Dog | - | - |

---

## ⚙️ Sozlash va Ishga Tushirish

### 🔧 Talablar

- .NET 8.0 SDK
- SQL Server (LocalDB yoki to'liq versiya)
- Visual Studio 2022 yoki VS Code

### 📥 O'rnatish

1. **Repositoryni clone qiling:**
```bash
git clone https://github.com/yourusername/XChanger.git
cd XChanger
```

2. **Connection stringni sozlang:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=XChangerDB;Trusted_Connection=true;"
  }
}
```

3. **Migration yarating va ishga tushiring:**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. **Loyihani ishga tushiring:**
```bash
dotnet run
```

---

## 🚀 Foydalanish

### 📊 Excel faylni yuklash

```bash
curl -X POST "https://localhost:7000/api/import/excel" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@persons.xlsx"
```

### 📄 XML formatida eksport

```bash
curl -X GET "https://localhost:7000/api/export/xml" \
  -H "Accept: application/xml"
```

### 👥 Personlarni olish

```bash
curl -X GET "https://localhost:7000/api/persons"
```

---

## 📋 Biznes Qoidalari

### 🚫 Filtrlash qoidalari

- **Minus qiymatli petlar** (`-`) import qilinmaydi
- **Bo'sh qiymatlar** ham import qilinmaydi
- **Noto'g'ri pet turlari** import qilinmaydi

### ✅ Qo'llab-quvvatlanadigan pet turlari

- `Cat` → PetType.Cat (1)
- `Dog` → PetType.Dog (2)
- `Parrot` → PetType.Parrot (3)

---

## 🔍 Xatoliklarni hal qilish

### 🐛 Keng tarqalgan xatoliklar

| Xatolik | Sabab | Yechim |
|---------|-------|--------|
| `Database connection failed` | SQL Server ishlamayapti | SQL Server xizmatini ishga tushiring |
| `Excel file format error` | Noto'g'ri fayl formati | Excel faylni tekshiring |
| `Invalid pet type` | Noto'g'ri pet turi | Cat, Dog, Parrot dan foydalaning |

---

## 📊 Monitoring va Logging

Loyiha `Serilog` orqali loggingni amalga oshiradi:

- **Information** - Asosiy operatsiyalar
- **Warning** - Filtrlangan ma'lumotlar
- **Error** - Xatoliklar va exceptionlar

---

## 🛠️ Texnologiyalar

| Texnologiya | Versiya | Maqsad |
|-------------|---------|--------|
| `.NET` | 8.0 | Backend framework |
| `Entity Framework Core` | 8.0 | ORM |
| `SQL Server` | 2022 | Ma'lumotlar bazasi |
| `SheetJS` | Latest | Excel fayllarni o'qish |
| `System.Xml.Linq` | Built-in | XML generation |
| `Serilog` | Latest | Logging |

---

## 🤝 Hissa qo'shish

1. Fork qiling
2. Feature branch yarating (`git checkout -b feature/amazing-feature`)
3. Commit qiling (`git commit -m 'Add amazing feature'`)
4. Push qiling (`git push origin feature/amazing-feature`)
5. Pull Request oching

---

## 📄 Litsenziya

Bu loyiha MIT litsenziyasi ostida tarqatiladi. Batafsil ma'lumot uchun `LICENSE` faylini ko'ring.

## ✍️ Muallif
👤 Mukhtor Eshboyev\
🔗 GitHub: [@aestdile](https://github.com/aestdile)

## 📞 Aloqa

Savollar yoki takliflar uchun:
- Issue yarating
- Telegram: [@aestdile](https://t.me/aestdile)
- Email: aestdile@gmail.com

---

⭐ Agar loyiha foydali bo'lsa, star bosishni unutmang!

## 🌐 Social Networks

<div align="center">
  <a href="https://t.me/aestdile"><img src="https://img.shields.io/badge/Telegram-2CA5E0?style=for-the-badge&logo=telegram&logoColor=white" /></a>
  <a href="https://github.com/aestdile"><img src="https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white" /></a>
  <a href="https://leetcode.com/aestdile"><img src="https://img.shields.io/badge/LeetCode-FFA116?style=for-the-badge&logo=leetcode&logoColor=black" /></a>
  <a href="https://linkedin.com/in/aestdile"><img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white" /></a>
  <a href="https://youtube.com/@aestdile"><img src="https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white" /></a>
  <a href="https://instagram.com/aestdile"><img src="https://img.shields.io/badge/Instagram-E4405F?style=for-the-badge&logo=instagram&logoColor=white" /></a>
  <a href="https://facebook.com/aestdile"><img src="https://img.shields.io/badge/Facebook-1877F2?style=for-the-badge&logo=facebook&logoColor=white" /></a>
  <a href="mailto:aestdile@gmail.com"><img src="https://img.shields.io/badge/Gmail-D14836?style=for-the-badge&logo=gmail&logoColor=white" /></a>
  <a href="https://twitter.com/aestdile"><img src="https://img.shields.io/badge/Twitter-1DA1F2?style=for-the-badge&logo=twitter&logoColor=white" /></a>
  <a href="tel:+998772672774"><img src="https://img.shields.io/badge/Phone:+998772672774-25D366?style=for-the-badge&logo=whatsapp&logoColor=white" /></a>
</div>

