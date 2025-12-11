using Core;
using DatabaseContext;
using DatabaseModels;

namespace Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Система управления выставками");
                Console.WriteLine("Загрузка конфигурации");

                var config = new DatabaseConfig();
                Console.WriteLine("Конфигурация загружена успешно");

                using var context = new ApplicationContext(config);
                var facade = new DatabaseFacade(context);

                Console.WriteLine("Подключение к базе данных установлено");
                Console.WriteLine();

                RunMainMenu(facade);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода.");
                Console.ReadKey();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nНЕОЖИДАННАЯ ОШИБКА: {ex.Message}");
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        static void RunMainMenu(IDatabaseFacade facade)
        {
            while (true)
            {
                Console.WriteLine("\n=== Главное меню ===");
                Console.WriteLine("1. Управление посетителями");
                Console.WriteLine("2. Управление выставками");
                Console.WriteLine("3. Управление билетами");
                Console.WriteLine("4. Аналитика");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ManageVisitors(facade);
                            break;
                        case "2":
                            ManageExhibitions(facade);
                            break;
                        case "3":
                            ManageTickets(facade);
                            break;
                        case "4":
                            ShowAnalytics(facade);
                            break;
                        case "0":
                            Console.WriteLine("Выход из программы.");
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static void ManageVisitors(IDatabaseFacade facade)
        {
            while (true)
            {
                Console.WriteLine("\n=== Управление посетителями ===");
                Console.WriteLine("1. Добавить посетителя");
                Console.WriteLine("2. Показать всех посетителей");
                Console.WriteLine("3. Обновить посетителя");
                Console.WriteLine("4. Удалить посетителя");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddVisitor(facade);
                        break;
                    case "2":
                        ShowAllVisitors(facade);
                        break;
                    case "3":
                        UpdateVisitor(facade);
                        break;
                    case "4":
                        DeleteVisitor(facade);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static void ManageExhibitions(IDatabaseFacade facade)
        {
            while (true)
            {
                Console.WriteLine("\n=== Управление выставками ===");
                Console.WriteLine("1. Добавить выставку");
                Console.WriteLine("2. Показать все выставки");
                Console.WriteLine("3. Обновить выставку");
                Console.WriteLine("4. Удалить выставку");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddExhibition(facade);
                        break;
                    case "2":
                        ShowAllExhibitions(facade);
                        break;
                    case "3":
                        UpdateExhibition(facade);
                        break;
                    case "4":
                        DeleteExhibition(facade);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static void ManageTickets(IDatabaseFacade facade)
        {
            while (true)
            {
                Console.WriteLine("\n=== Управление билетами ===");
                Console.WriteLine("1. Добавить билет");
                Console.WriteLine("2. Показать все билеты");
                Console.WriteLine("3. Обновить билет");
                Console.WriteLine("4. Удалить билет");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTicket(facade);
                        break;
                    case "2":
                        ShowAllTickets(facade);
                        break;
                    case "3":
                        UpdateTicket(facade);
                        break;
                    case "4":
                        DeleteTicket(facade);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static void AddVisitor(IDatabaseFacade facade)
        {
            Console.Write("Введите имя посетителя: ");
            var name = Console.ReadLine();

            Console.Write("Введите размер скидки (0-100%): ");
            if (!int.TryParse(Console.ReadLine(), out int discount) || discount < 0 || discount > 100)
            {
                Console.WriteLine("Некорректный ввод скидки. Должно быть число от 0 до 100.");
                return;
            }

            facade.AddVisitor(new Visitor { Name = name, Discount = discount });
            Console.WriteLine("Посетитель успешно добавлен.");
        }

        static void ShowAllVisitors(IDatabaseFacade facade)
        {
            var visitors = facade.GetAllVisitors();
            if (visitors.Count == 0)
            {
                Console.WriteLine("Список посетителей пуст.");
                return;
            }

            Console.WriteLine("\n=== Список посетителей ===");
            foreach (var visitor in visitors)
            {
                Console.WriteLine($"ID: {visitor.Id}, Имя: {visitor.Name}, Скидка: {visitor.Discount}%");
            }
        }

        static void UpdateVisitor(IDatabaseFacade facade)
        {
            ShowAllVisitors(facade);

            Console.Write("\nВведите ID посетителя для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            var visitor = facade.GetVisitorById(id);
            if (visitor == null)
            {
                Console.WriteLine("Посетитель с таким ID не найден.");
                return;
            }

            Console.Write($"Введите новое имя (текущее: {visitor.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                visitor.Name = name;

            Console.Write($"Введите новую скидку (текущая: {visitor.Discount}%): ");
            var discountInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(discountInput) && int.TryParse(discountInput, out int discount))
                visitor.Discount = discount;

            facade.UpdateVisitor(visitor);
            Console.WriteLine("Посетитель успешно обновлен.");
        }

        static void DeleteVisitor(IDatabaseFacade facade)
        {
            ShowAllVisitors(facade);

            Console.Write("\nВведите ID посетителя для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            facade.DeleteVisitor(id);
            Console.WriteLine("Посетитель успешно удален.");
        }

        static void AddExhibition(IDatabaseFacade facade)
        {
            Console.Write("Введите название выставки: ");
            var name = Console.ReadLine();

            Console.Write("Введите дату выставки (гггг-мм-дд): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("Некорректный формат даты.");
                return;
            }

            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            facade.AddExhibition(new Exhibition { Name = name, Date = date });
            Console.WriteLine("Выставка успешно добавлена.");
        }

        static void ShowAllExhibitions(IDatabaseFacade facade)
        {
            var exhibitions = facade.GetAllExhibitions();
            if (exhibitions.Count == 0)
            {
                Console.WriteLine("Список выставок пуст.");
                return;
            }

            Console.WriteLine("\n=== Список выставок ===");
            foreach (var exhibition in exhibitions)
            {
                Console.WriteLine($"ID: {exhibition.Id}, Название: {exhibition.Name}, Дата: {exhibition.Date:yyyy-MM-dd}");
            }
        }

        static void UpdateExhibition(IDatabaseFacade facade)
        {
            ShowAllExhibitions(facade);

            Console.Write("\nВведите ID выставки для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            var exhibition = facade.GetExhibitionById(id);
            if (exhibition == null)
            {
                Console.WriteLine("Выставка с таким ID не найден.");
                return;
            }

            Console.Write($"Введите новое название (текущее: {exhibition.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                exhibition.Name = name;

            Console.Write($"Введите новую дату (текущая: {exhibition.Date:yyyy-MM-dd}): ");
            var dateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParse(dateInput, out DateTime date))
                exhibition.Date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            facade.UpdateExhibition(exhibition);
            Console.WriteLine("Выставка успешно обновлена.");
        }

        static void DeleteExhibition(IDatabaseFacade facade)
        {
            ShowAllExhibitions(facade);

            Console.Write("\nВведите ID выставки для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            facade.DeleteExhibition(id);
            Console.WriteLine("Выставка успешно удалена.");
        }

        static void AddTicket(IDatabaseFacade facade)
        {
            ShowAllVisitors(facade);
            var visitors = facade.GetAllVisitors();
            if (visitors.Count == 0)
            {
                Console.WriteLine("Невозможно добавить билет: нет посетителей.");
                return;
            }

            Console.Write("\nВведите ID посетителя: ");
            if (!int.TryParse(Console.ReadLine(), out int visitorId) ||
                !visitors.Any(v => v.Id == visitorId))
            {
                Console.WriteLine("Некорректный ID посетителя.");
                return;
            }

            // Показываем список выставок
            ShowAllExhibitions(facade);
            var exhibitions = facade.GetAllExhibitions();
            if (exhibitions.Count == 0)
            {
                Console.WriteLine("Невозможно добавить билет: нет выставок.");
                return;
            }

            Console.Write("Введите ID выставки: ");
            if (!int.TryParse(Console.ReadLine(), out int exhibitionId) ||
                !exhibitions.Any(e => e.Id == exhibitionId))
            {
                Console.WriteLine("Некорректный ID выставки.");
                return;
            }

            Console.Write("Введите цену билета: ");
            if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
            {
                Console.WriteLine("Некорректная цена.");
                return;
            }

            facade.AddTicket(new Ticket
            {
                VisitorId = visitorId,
                ExhibitionId = exhibitionId,
                Price = price
            });
            Console.WriteLine("Билет успешно добавлен.");
        }

        static void ShowAllTickets(IDatabaseFacade facade)
        {
            var tickets = facade.GetAllTickets();
            if (tickets.Count == 0)
            {
                Console.WriteLine("Список билетов пуст.");
                return;
            }

            Console.WriteLine("\n=== Список билетов ===");
            foreach (var ticket in tickets)
            {
                Console.WriteLine($"ID: {ticket.Id}, Посетитель ID: {ticket.VisitorId}, Выставка ID: {ticket.ExhibitionId}, Цена: {ticket.Price:C}");
            }
        }

        static void UpdateTicket(IDatabaseFacade facade)
        {
            ShowAllTickets(facade);

            Console.Write("\nВведите ID билета для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            var ticket = facade.GetTicketById(id);
            if (ticket == null)
            {
                Console.WriteLine("Билет с таким ID не найден.");
                return;
            }

            ShowAllVisitors(facade);
            Console.Write($"Введите новый ID посетителя (текущий: {ticket.VisitorId}): ");
            var visitorIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(visitorIdInput) && int.TryParse(visitorIdInput, out int visitorId))
                ticket.VisitorId = visitorId;

            ShowAllExhibitions(facade);
            Console.Write($"Введите новый ID выставки (текущий: {ticket.ExhibitionId}): ");
            var exhibitionIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(exhibitionIdInput) && int.TryParse(exhibitionIdInput, out int exhibitionId))
                ticket.ExhibitionId = exhibitionId;

            Console.Write($"Введите новую цену (текущая: {ticket.Price:C}): ");
            var priceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priceInput) && double.TryParse(priceInput, out double price))
                ticket.Price = price;

            facade.UpdateTicket(ticket);
            Console.WriteLine("Билет успешно обновлен.");
        }

        static void DeleteTicket(IDatabaseFacade facade)
        {
            ShowAllTickets(facade);

            Console.Write("\nВведите ID билета для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            facade.DeleteTicket(id);
            Console.WriteLine("Билет успешно удален.");
        }

        static void ShowAnalytics(IDatabaseFacade facade)
        {
            while (true)
            {
                Console.WriteLine("\n=== Аналитика ===");
                Console.WriteLine("1. Количество проданных билетов на выставку");
                Console.WriteLine("2. Количество уникальных выставок у посетителя");
                Console.WriteLine("3. Средний процент скидки у посетителей выставки");
                Console.WriteLine("0. Назад");
                Console.Write("Выберите запрос: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllExhibitions(facade);
                        Console.Write("\nВведите ID выставки: ");
                        if (int.TryParse(Console.ReadLine(), out int exhibitionId1))
                        {
                            var count = facade.GetSoldTicketsCount(exhibitionId1);
                            Console.WriteLine($"На выставку с ID {exhibitionId1} продано {count} билетов.");
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ID.");
                        }
                        break;

                    case "2":
                        ShowAllVisitors(facade);
                        Console.Write("\nВведите ID посетителя: ");
                        if (int.TryParse(Console.ReadLine(), out int visitorId))
                        {
                            var count = facade.GetUniqueExhibitionsCount(visitorId);
                            Console.WriteLine($"Посетитель с ID {visitorId} посетил {count} уникальных выставок.");
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ID.");
                        }
                        break;

                    case "3":
                        ShowAllExhibitions(facade);
                        Console.Write("\nВведите ID выставки: ");
                        if (int.TryParse(Console.ReadLine(), out int exhibitionId2))
                        {
                            var avgDiscount = facade.GetAverageDiscountForExhibition(exhibitionId2);
                            Console.WriteLine($"Средний процент скидки у посетителей выставки с ID {exhibitionId2}: {avgDiscount:F2}%");
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ID.");
                        }
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }
    }
}