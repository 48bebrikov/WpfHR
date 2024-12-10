# Модуль “Управление адаптацией”

## Описание

Данный модуль предназначен для HR-специалистов и предоставляет удобные инструменты для создания программ адаптации, анализа мероприятий и управления адаптационными модулями.

### Основные блоки:

1. **Адаптационные модули**  
   Этот блок позволяет:
   - Отображать информацию по всем модулям (новые, черновики, принятые в работу).  
     - Данные: название, разработчики, согласованты, должности.
   - Создавать задания на разработку модуля.
   - Фильтровать модули по должности в реальном времени.

   **Процесс создания задания на разработку модуля:**
   - Формирование модуля с кодовым названием.
   - Добавление разработчиков и согласовантов.
   - Указание главного согласованта и срока реализации модуля.
   - Автоматическое уведомление заинтересованных лиц по почте с вложением PDF-приказа.

   **Пример текста письма:**
   > Добрый день, коллеги. Вы назначены разработчиком адаптационного модуля [Название]. Просим ознакомиться с приказом и приступить к работе.


2. **Конструктор программ адаптации**  
   Этот блок позволяет создать программу адаптации для нового сотрудника.

   **Процесс работы:**
   - Выбрать нового сотрудника из списка.
   - Указать отдел для трудоустройства.
   - Указать должность.
   - Автоматически отобразить адаптационные модули, подходящие для должности.  
     - Модули представлены в виде панелей с подробной информацией при наведении.
   - Выбрать необходимые модули для программы адаптации.
   - Назначить наставников.
   - Сформировать программу адаптации (xlsx файл).  

   **Результат:**
   - Программа отправляется на почту наставникам и сохраняется локально:
     ```
     Имя файла: [наименование_отдела]_[должность]_[ФИО]_[дата_начала_адаптации].xlsx
     ```

3. **Анализ адаптационных мероприятий**  
   Этот блок позволяет:
   - Формировать отчёты по:
     - Отделам
     - Должностям
   - Генерировать аналитические отчёты, включающие:
     - Количество ошибок по программе.
     - Процент выполнения программы.
     - Успешность трудоустройства после адаптации.  

   **Функции:**
   - Отчёты представлены в виде диаграммы.
   - Возможность фильтрации по отделам и должностям.

---

## Настройки

1. **Подключение базы данных**  
   Настройка SQL сервера осуществляется в файле: `ModuleDbContext.cs`.

   ```csharp
   optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AdaptationModulesDB;Trusted_Connection=True;");
   ```

2. **Почтовый сервер**  
   Настройки почтового сервера задаются в файле `.env`:

   ```env
   SMTP_SERVER=smtp.your-email-provider.com
   SMTP_PORT=587
   SMTP_EMAIL=your-email@example.com
   SMTP_PASSWORD=your-email-password
   ```

---

## Used Packages

- [ClosedXML](https://github.com/ClosedXML/ClosedXML) (v0.104.2)
- [Extended.Wpf.Toolkit](https://github.com/xceedsoftware/wpftoolkit) (v4.6.1)
- [iTextSharp](https://github.com/itext/itextsharp) (v5.5.13.4)
- [LiveCharts.Wpf](https://github.com/Live-Charts/Live-Charts) (v0.9.7)
- [MaterialDesignThemes](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) (v5.1.0)
- [Microsoft.EntityFrameworkCore](https://github.com/dotnet/efcore) (v7.0.0)
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/) (v7.0.0)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/) (v7.0.0)
- [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/) (v9.0.0)
- [dotenv.net](https://www.nuget.org/packages/dotenv.net) (v3.2.1)

### Install Packages

```bash
dotnet restore

