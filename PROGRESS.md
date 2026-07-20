# Progress Log — Library Management System fixes + UI build

## სტატუსი: მუშაობა მიმდინარეობს

## ნაპოვნი პრობლემები (ჩამონათვალი, სანამ ფიქსებს დავიწყებდი)

### 🔴 კრიტიკული უსაფრთხოება
- [x] `Application/Implimentations/EmailService.cs` — hardcoded Gmail App Password `MailAddress`-ის მეორე პარამეტრში (არასწორი გამოყენებაც და secret leak-იც)
- [x] `management-ui-library/appsettings.json` — იგივე რეალური Gmail App Password plaintext-ში, **committed git history-ში** (commit b43c459, `origin/master`-თან sync-შია)
- [ ] (მომხმარებელმა ხელით უნდა გააკეთოს) მომხმარებელს სჭირდება: App Password-ის დაუყოვნებელი გაუქმება/გადაგენერირება Google-ის მხარეს

### 🟠 სტრუქტურული ბაგი
- [x] `Application/Interfaces/IBookService.cs` — `IBookService` interface თვითონ თავისი თავის შიგნით არის nested (ცარიელი outer interface). ეს დაბლოკავს Menu-ს, თუ `IBookService`-ს, როგორც DI ტიპს გამოვიყენებთ.

### 🟡 ფუნქციური ბაგები
- [x] `core/Models/Author.cs` — `FirstName` setter კვლავ ამოწმებს ძველ (`_firstname`) მნიშვნელობას, არა ახალს (`value`). LastName უკვე გასწორებულია, FirstName - არა.
- [x] `Infrastructure/Repositories/UserRepository.cs` — constructor-ში არ არის `User.SyncIdCounter(_users)` გამოძახება → ID collision restart-ის შემდეგ
- [x] `Infrastructure/Repositories/BookRepository.cs` — constructor-ში არ არის `Book.SyncIdCounter(_books)` → იგივე პრობლემა
- [x] `management-ui-library/Program.cs` — `"UserStorage.txt"` არ ემთხვევა რეალურ ფაილს `"UsersStorage.txt"`
- [x] `Ui.csproj` — `appsettings.json` არ აქვს `CopyToOutputDirectory` → runtime crash startup-ზე (`FileNotFoundException`)
- [x] `BorrowService`-ში რამდენიმე ადგილას `_userSession.CurrentUser.Id` null-check გარეშე — NullReferenceException-ის რისკი, თუ არავინაა დალოგინებული
- [x] `UserRepository.cs` — დუბლირებული `using Domain.Interfaces;`
- [x] `BorrowRecord.Fee` property არასდროს ივსება (`CalculateFee()` თვლის, მაგრამ არსად ინახება ჩანაწერზე)

### 🟢 ახალი ფუნქციონალი (მომხმარებლის მოთხოვნით, წინა საუბრიდან)
- [x] `CancelRequest` — client-მა შეძლოს საკუთარი Pending მოთხოვნის გაუქმება

### 🔵 UI აშენება (მთავარი დავალება)
- [ ] `ConsoleHelper.cs` — გაზიარებული input/retry ლოგიკა
- [ ] `Menu.cs` — root loop: რეგისტრაცია/ავტორიზაცია/გასვლა, როლის მიხედვით routing
- [ ] `UserMenu.cs` — client-ის ოპერაციები
- [ ] `AdminMenu.cs` — admin-ის ოპერაციები
- [ ] `Program.cs` — საბოლოო wiring, `Menu.Run(...)`-ის გამოძახება

## შემდეგი ნაბიჯი, თუ სესია გაწყდა
გააგრძელე ზემოთ მოცემული checklist-ის მიხედვით, ზემოდან ქვემოთ. ყოველი გასწორებული პუნქტი მონიშნე [x]-ად.

## განახლება — UI დასრულებულია (ინგლისურ ტექსტებზე გადავიდა)
- [x] ConsoleHelper.cs
- [x] Menu.cs (root loop, register/login/verify-email, role routing)
- [x] UserMenu.cs
- [x] AdminMenu.cs
- [x] Program.cs wiring + `using management_ui_library.Menus;` (აკლდა — build-blocking იყო)
- [x] დამატებით ნაპოვნი: UserMenu.cs/AdminMenu.cs-ს აკლდა `using Domain.Interfaces;` (IBookService-სთვის) — build-blocking, გასწორებულია

ყველა checklist პუნქტი დასრულებულია.
