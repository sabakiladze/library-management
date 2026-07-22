# Library Management System

C# (.NET 8) კონსოლური აპლიკაცია ბიბლიოთეკის მართვისთვის — Clean Architecture-ის პრინციპებზე აგებული, როლებზე დაფუძნებული წვდომით (Client / Admin).

## შესაძლებლობები

**ავთენტიფიკაცია**
- რეგისტრაცია, პაროლი ინახება BCrypt hash-ის სახით
- ელ-ფოსტის ვერიფიკაცია კოდით (იგზავნება Gmail SMTP-ით)
- Login/Logout
- ანგარიშის წაშლა (`DELETE` — მოითხოვს ტიპირებულ "YES" დადასტურებას)

**Client-ის ფუნქციები**
- წიგნების კატალოგის დათვალიერება
- ძებნა სათაურით, ავტორით, გამოცემის წლით
- წიგნის გამოთხოვა (borrow request → Pending)
- საკუთარი მოთხოვნების/ჩანაწერების ნახვა
- Pending მოთხოვნის გაუქმება
- წიგნის დაბრუნება (ავტომატური ჯარიმის დათვლა ვადაგადაცილი
- **Admin-ის ფუნქციები**
- წიგნების სრული კატალოგის ნახვა (ავტორის, გამოცემის წლისა და ასლების სტატუსის ჩათვლით)
- ახალი წიგნის დამატება
- არსებულ წიგნზე ახალი ასლის (copy) დამატება
- წიგნის/ასლის წაშლა
- Pending მოთხოვნების ნახვა
- მოთხოვნის დადასტურება (Approve → ავტომატურად იქმნება BorrowRecord და ასლის სტატუსი იცვლება Borrowed-ზე)
- მოთხოვნის უარყოფა (Reject)
- მომხმარებლის დაწინაურება Admin-ად
- სტატისტიკა (წიგნების/ასლების საერთო რაოდენობა)
- საკუთარი ჯარიმის ნახვა
- ანგარიშის წაშლა
- - **`core/` (Domain)** — ბიზნეს-ცნებები, არაფერზე არ არის დამოკიდებული: `Models/` (`User`, `Book`, `BookCopy`, `Author`, `BorrowRecord`, `BorrowRequest`), `Enums/`, `Exceptions/`.
- **`Application/`** — Use-case-ების იმპლემენტაცია: `Interfaces/Services/` და `Interfaces/Repositories/` (abstraction-ები), `Implimentations/` (`AuthService`, `BookService`, `BorrowService`, `UserService`), `Validations/`.
- **`Infrastructure/`** — გარეთა რესურსებთან საუბარი: `Repositories/` (JSON-ფაილურ შენახვაზე მომუშავე repository-ები) და `Services/` (`EmailService` — Gmail SMTP-სთან საუბარი).
- **`management-ui-library/`** — კონსოლური UI: `Menus/` (`Menu`, `UserMenu`, `AdminMenu`), `Utils/ConsoleHelper.cs`, `Program.cs` (composition root — ერთადერთი ადგილი, სადაც ყველა კონკრეტული კლასია ცნობილი).

## Tech stack

- **.NET 8 / C# 12**
- **BCrypt.Net-Next** — პაროლების ჰეშირება
- **System.Text.Json** — მონაცემთა (de)serialization
- **System.Net.Mail (SmtpClient)** — ვერიფიკაციის კოდების გაგზავნა Gmail SMTP-ით
- **Microsoft.Extensions.Configuration.Json** — `appsettings.json`-ის წაკითხვა

## მონაცემთა შენახვა

მონაცემები ინახება `.txt` ფაილებში (JSON ფორმატში), `Files/` საქაღალდეში, აპლიკაციის output დირექტორიის მიმართ (`AppDomain.CurrentDomain.BaseDirectory`):

| ფაილი | შიგთავსი |
|---|---|
| `UsersBase.txt` | მომხმარებლები |
| `BookStorage.txt` | წიგნები + ასლები |
| `BorrowRecordInfo.txt` | დამტკიცებული გამოწერების ჩანაწერები |
| `BorrowRequestInfo.txt` | ყველა მოთხოვნა (Pending/Approved/Rejected/Cancelled) |
| `LogginInfo.txt` | ავტორიზაციის ისტორია |

I/O ოპერაციები (ჩატვირთვა/შენახვა) async-ია (`GetAllLineAsync`/`SaveAllAsync`); repository-ების in-memory query-ები (`GetById`, `GetByName` და ა.შ.) — sync, რადგან უკვე ჩატვირთულ სიაზე მუშაობენ.

## გაშვება

```bash
cd management-ui-library
dotnet build
dotnet run
```

**Email-ვერიფიკაციისთვის** (არასავალდებულო, მაგრამ საჭირო registration-flow-ის სრულად სამუშაოდ): `appsettings.json`-ში ჩაწერე რეალური Gmail მისამართი და App Password (არა ჩვეულებრივი პაროლი — საჭიროა 2-Step Verification და App Password Google-ის Security პარამეტრებში):

```json
"EmailSettings": {
  "Email": "your.email@gmail.com",
  "AppPassword": "abcd efgh ijkl mnop",
  "SmtpServer": "smtp.gmail.com",
  "Port": 587
}
```

ეს ფაილი `.gitignore`-შია — რეალური მონაცემები არასდროს unda აღმოჩნდეს commit-ში.

## UI-ის ქცევა

- ნებისმიერ input-ზე შეგიძლია დაწერო **`cancel`**, რომ გამოხვიდე მიმდინარე ოპერაციიდან.
- მრავალ-ველიან ოპერაციებზე (მაგ. წიგნის დამატება) ბოლოს გეკითხება დადასტურება, სანამ რეალურად submit მოხდება.
- თუ ოპერაცია ჩავარდება (validation/ბიზნეს-წესი), გეკითხება **"1. Try again / 2. Cancel"** — არაფერი არ იკარგება უსაფუძვლოდ.

## ცნობილი შეზღუდვები

- Email-ვერიფიკაცია მთლიანად რეალურ SMTP-ზეა დამოკიდებული — placeholder-ებით registration "ჩავარდნილად" გამოჩნდება (თუმცა user მაინც შეიქმნება ფაილში, unverified სტატუსით).
- `IAuthService`-ს არ აქვს "resend code" მექანიზმი.
- ერთდროული (concurrent) წვდომა ფაილებზე არ არის დამუშავებული — ეს single-user კონსოლური აპლიკაციაა.

## არქიტექტურა

პროექტი აგებულია Clean Architecture-ის ოთხფენიან მოდელზე — დამოკიდებულება ყოველთვის შიგნისკენ მიდის:
