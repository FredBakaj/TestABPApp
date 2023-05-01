Виконання тестового завдання для ABP

Опис
Реалізація REST API для проведення АБ - тестування.

<h1>Опис класів</h1>
<h2>ExperementController</h2>
Клас ExperementController відповідає за обробку запитів, що стосуються експериментів. Конструктор класу приймає різні сервіси та залежності, такі як IRegistrationDeviceTokenService та IExperementService, що дозволяють отримувати та зберігати дані про реєстрацію та експерименти.

Функція ButtonCollor приймає параметр device_token та повертає колір кнопки на основі налаштувань експерименту та ідентифікатора пристрою.

Функція Price також приймає параметр device_token та повертає ціну на основі налаштувань експерименту та ідентифікатора пристрою.

Метод GetExperementStats повертає статистику експерименту.

Приватна функція GenereateResponce приймає делегат та повертає результат функції як Ok-відповідь або помилку BadRequest, якщо виникла помилка виконання функції.

Приватна функція GetExperementValue приймає назву експерименту, ідентифікатор пристрою та значення за замовчуванням та повертає значення експерименту на основі цих параметрів. Якщо ідентифікатор пристрою не є валідним, функція генерує виключення ArgumentException. Якщо пристрій не зареєстрований, функція реєструє його. Якщо ідентифікатор пристрою не взят участь в даному експерименті, функція генерує AB-групу для пристрою та додає запис. Нарешті, функція повертає значення експерименту на основі інформації, збереженої в базі даних.

<h2>ExperementService</h2>
Цей код є реалізацією сервісу для проведення A/B експериментів з використанням Entity Framework Core ORM. Сервіс надає методи для створення A/B тестових груп, запису дій користувачів та отримання статистики експерименту. 

Клас `ExperementService` реалізує інтерфейс `IExperementService`, який визначає контракт для сервісу A/B експерименту. 

Конструктор отримує екземпляр `AppDBContext` та `IRegistrationDeviceTokenService`. AppDBContext використовується для доступу до бази даних, в той час як IRegistrationDeviceTokenService використовується для перевірки того, чи зареєстровано токен пристрою. 

Метод `GetValue` повертає значення A/B експерименту для заданого токену пристрою та назви експерименту. Він перевіряє, чи зареєстровано токен пристрою і чи розпочато експеримент. Якщо експеримент розпочато, він отримує значення A/B експерименту з бази даних. В іншому випадку повертається значення за замовчуванням.

Метод `GetDateTimeExperimentStart` повертає дату початку експерименту за назвою експерименту.

Метод `GenerateABGroup` генерує випадкову групу A/B для заданої назви експерименту. Він отримує значення A/B експерименту з бази даних, обчислює суму коефіцієнтів ефективності та вибирає A/B групу на основі коефіцієнтів ефективності.

Метод `AddRecord` записує дію для заданого токену пристрою та назви експерименту. Він отримує ідентифікатор експерименту та ідентифікатор значення експерименту A/B з бази даних і створює новий об'єкт `ABExperimentRecord` з набором властивостей `Action`, `UserId`, `ABExperimentValueId` і `ABExperimentId`. Потім він додає новий запис до таблиці `ABExperimentRecords` у базі даних.

Метод `IsDeviceTokenInExperiment` перевіряє, чи знаходиться токен пристрою в експерименті.

Метод `GetExperimentStats` отримує статистику для всіх експериментів. Він отримує всі експерименти з бази даних, групує записи експериментів за групами A/B і підраховує кількість показів і дій для кожної групи.

Метод `SelectElementWithProbability` вибирає елемент зі списку із заданою ймовірністю. Він перевіряє, чи списки елементів і ймовірностей однакового розміру, і обчислює загальну ймовірність. Потім він генерує випадкове значення між 0 і сумарною ймовірністю і вибирає відповідний елемент. 

Загалом, реалізація сервісу A/B експерименту виглядає добре структурованою і відповідає хорошим практикам програмування. Однак без деталей інтерфейсу `IExperimentService` та сутностей `ABExperimentRecord`, `ABExperimentValue` і `ABExperiments` важко надати більш детальну оцінку реалізації.

<h2>RegistrationDeviceTokenService</h2>
Даний класс  використовує фреймворк ASP.NET Core та Entity Framework Core для роботи з базою даних. Клас `RegistrationDeviceTokenService` реалізує інтерфейс `IRegistrationDeviceTokenService` та містить три методи:

- `IsRegistered(int deviceToken)` - перевіряє, чи є користувач з переданим `deviceToken` зареєстрованим у базі даних. Повертає `true`, якщо користувач існує, інакше - `false`.
- `RegistrationUser(int deviceToken)` - реєструє нового користувача у базі даних з переданим `deviceToken` та поточним часом.
- `GetDateTimeRegistered(int deviceToken)` - повертає дату та час реєстрації користувача з переданим `deviceToken`.

Клас використовує контекст бази даних `AppDBContext`, який унаслідував від класу `DbContext` з бібліотеки Entity Framework Core. Також він використовує модель даних `User`, яка містить властивості `DeviceToken` та `DateRegistration`, для взаємодії з таблицею "Users" у базі даних.

<h1>Опис Бази даних</h1>
<h2> User </h2>
Ця модель найбільш відображає таблицю "Users" у базі даних і містить два поля:

- "DeviceToken" - унікальний ідентифікатор пристрою, який є ключовим полем моделі і зазвичай використовується для ідентифікації користувачів у системі.
- "DateRegistration" - дата реєстрації користувача у системі.

<h2> ABExperiment </h2>
Це модельний клас на C#, який представляє AB (A/B) експеримент. Він має три властивості:
- Id: Цілочисельна властивість, яка унікально ідентифікує об'єкт ABExperiment у базі даних.
- Name: Рядкова властивість, яка зберігає назву експерименту.
- DateStart: Властивість типу DateTime, яка представляє дату і час початку експерименту.

<h2>ABExperimentValue</h2>
Ця модель представляє сутність значень для експерименту A/B. Вона містить такі властивості:
- `Id` - унікальний ідентифікатор значення.
- `Value` - текстове значення для групи, до якої належить це значення.
- `GroupName` - назва групи, до якої належить це значення.
- `СoefficientPerformance` - коефіцієнт ефективності для цього значення.
- `ABExperiment` - зв'язок з експериментом A/B, до якого належить це значення.

<h2>ABExperimentRecord</h2>
Модель `ABExperimentRecord` описує записи експерименту з розбиттям на групи. Вона містить наступні поля:
- `Id` - унікальний ідентифікатор запису експерименту;
- `Action` - дія користувача, яка спричинила внесення запису;
- `UserId` - ідентифікатор користувача, який виконав дію;
- `ABExperimentId` - ідентифікатор експерименту;
- `ABExperimentValueId` - ідентифікатор значення, яке було вибране для даного користувача;
- `User` - об'єкт, що представляє користувача;
- `ABExperiment` - об'єкт, що представляє експеримент;
- `ABExperimentValue` - об'єкт, що представляє значення, яке було вибране для даного користувача в межах експерименту.