using Core;
using DatabaseContext;
using DatabaseModels;

namespace Presentation
{
    class Program
    {
        static async Task Main(string[] args)  
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

                await RunMainMenuAsync(facade);  
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
                Console.WriteLine($"\nОшибка: {ex.Message}");
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        static async Task RunMainMenuAsync(IDatabaseFacade facade)  
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
                            await ManageVisitorsAsync(facade);  
                            break;
                        case "2":
                            await ManageExhibitionsAsync(facade);  
                            break;
                        case "3":
                            await ManageTicketsAsync(facade);  
                            break;
                        case "4":
                            await ShowAnalyticsAsync(facade);  
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

        static async Task ManageVisitorsAsync(IDatabaseFacade facade)  
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
                        await AddVisitorAsync(facade); 
                        break;
                    case "2":
                        await ShowAllVisitorsAsync(facade);  
                        break;
                    case "3":
                        await UpdateVisitorAsync(facade);  
                        break;
                    case "4":
                        await DeleteVisitorAsync(facade);  
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static async Task AddVisitorAsync(IDatabaseFacade facade)  
        {
            Console.Write("Введите имя посетителя: ");
            var name = Console.ReadLine();

            Console.Write("Введите размер скидки (0-100%): ");
            if (!int.TryParse(Console.ReadLine(), out int discount) || discount < 0 || discount > 100)
            {
                Console.WriteLine("Некорректный ввод скидки. Должно быть число от 0 до 100.");
                return;
            }

            try
            {
                await facade.AddVisitorAsync(new Visitor { Name = name, Discount = discount });  
                Console.WriteLine("Посетитель успешно добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task ShowAllVisitorsAsync(IDatabaseFacade facade)  
        {
            try
            {
                var visitors = await facade.GetAllVisitorsAsync();  
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
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task UpdateVisitorAsync(IDatabaseFacade facade)  
        {
            await ShowAllVisitorsAsync(facade); 

            Console.Write("\nВведите ID посетителя для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            try
            {
                var visitor = await facade.GetVisitorByIdAsync(id); 
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

                await facade.UpdateVisitorAsync(visitor);  
                Console.WriteLine("Посетитель успешно обновлен");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task DeleteVisitorAsync(IDatabaseFacade facade)  
        {
            await ShowAllVisitorsAsync(facade);  

            Console.Write("\nВведите ID посетителя для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            try
            {
                await facade.DeleteVisitorAsync(id);  
                Console.WriteLine("Посетитель успешно удален.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        // Остальные методы аналогично...

        static async Task ManageExhibitionsAsync(IDatabaseFacade facade)  
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
                        await AddExhibitionAsync(facade); 
                        break;
                    case "2":
                        await ShowAllExhibitionsAsync(facade);  
                        break;
                    case "3":
                        await UpdateExhibitionAsync(facade); 
                        break;
                    case "4":
                        await DeleteExhibitionAsync(facade);  
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static async Task AddExhibitionAsync(IDatabaseFacade facade)  
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

            try
            {
                await facade.AddExhibitionAsync(new Exhibition { Name = name, Date = date }); 
                Console.WriteLine("Выставка успешно добавлена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task ShowAllExhibitionsAsync(IDatabaseFacade facade)  
        {
            try
            {
                var exhibitions = await facade.GetAllExhibitionsAsync();  
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
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task UpdateExhibitionAsync(IDatabaseFacade facade)  
        {
            await ShowAllExhibitionsAsync(facade);  

            Console.Write("\nВведите ID выставки для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            try
            {
                var exhibition = await facade.GetExhibitionByIdAsync(id);  
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

                await facade.UpdateExhibitionAsync(exhibition); 
                Console.WriteLine("Выставка успешно обновлена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task DeleteExhibitionAsync(IDatabaseFacade facade)  
        {
            await ShowAllExhibitionsAsync(facade);  

            Console.Write("\nВведите ID выставки для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            try
            {
                await facade.DeleteExhibitionAsync(id);  
                Console.WriteLine("Выставка успешно удалена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task ManageTicketsAsync(IDatabaseFacade facade)  
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
                        await AddTicketAsync(facade);  
                        break;
                    case "2":
                        await ShowAllTicketsAsync(facade); 
                        break;
                    case "3":
                        await UpdateTicketAsync(facade);  
                        break;
                    case "4":
                        await DeleteTicketAsync(facade); 
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static async Task AddTicketAsync(IDatabaseFacade facade)  
        {
            await ShowAllVisitorsAsync(facade);  
            var visitors = await facade.GetAllVisitorsAsync();  
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

            await ShowAllExhibitionsAsync(facade);  
            var exhibitions = await facade.GetAllExhibitionsAsync();  
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

            try
            {
                await facade.AddTicketAsync(new Ticket  
                {
                    VisitorId = visitorId,
                    ExhibitionId = exhibitionId,
                    Price = price
                });
                Console.WriteLine("Билет успешно добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task ShowAllTicketsAsync(IDatabaseFacade facade)  
        {
            try
            {
                var tickets = await facade.GetAllTicketsAsync();  
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
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task UpdateTicketAsync(IDatabaseFacade facade)  
        {
            await ShowAllTicketsAsync(facade);  

            Console.Write("\nВведите ID билета для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            try
            {
                var ticket = await facade.GetTicketByIdAsync(id);  
                if (ticket == null)
                {
                    Console.WriteLine("Билет с таким ID не найден.");
                    return;
                }

                await ShowAllVisitorsAsync(facade);  
                Console.Write($"Введите новый ID посетителя (текущий: {ticket.VisitorId}): ");
                var visitorIdInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(visitorIdInput) && int.TryParse(visitorIdInput, out int visitorId))
                    ticket.VisitorId = visitorId;

                await ShowAllExhibitionsAsync(facade);  
                Console.Write($"Введите новый ID выставки (текущий: {ticket.ExhibitionId}): ");
                var exhibitionIdInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(exhibitionIdInput) && int.TryParse(exhibitionIdInput, out int exhibitionId))
                    ticket.ExhibitionId = exhibitionId;

                Console.Write($"Введите новую цену (текущая: {ticket.Price:C}): ");
                var priceInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(priceInput) && double.TryParse(priceInput, out double price))
                    ticket.Price = price;

                await facade.UpdateTicketAsync(ticket);  
                Console.WriteLine("Билет успешно обновлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task DeleteTicketAsync(IDatabaseFacade facade)  
        {
            await ShowAllTicketsAsync(facade);  

            Console.Write("\nВведите ID билета для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            try
            {
                await facade.DeleteTicketAsync(id);  
                Console.WriteLine("Билет успешно удален.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static async Task ShowAnalyticsAsync(IDatabaseFacade facade)  
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
                        await ShowAllExhibitionsAsync(facade);  
                        Console.Write("\nВведите ID выставки: ");
                        if (int.TryParse(Console.ReadLine(), out int exhibitionId1))
                        {
                            try
                            {
                                var count = await facade.GetSoldTicketsCountAsync(exhibitionId1);  
                                Console.WriteLine($"На выставку с ID {exhibitionId1} продано {count} билетов.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ID.");
                        }
                        break;

                    case "2":
                        await ShowAllVisitorsAsync(facade);  
                        Console.Write("\nВведите ID посетителя: ");
                        if (int.TryParse(Console.ReadLine(), out int visitorId))
                        {
                            try
                            {
                                var count = await facade.GetUniqueExhibitionsCountAsync(visitorId);  
                                Console.WriteLine($"Посетитель с ID {visitorId} посетил {count} уникальных выставок.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ID.");
                        }
                        break;

                    case "3":
                        await ShowAllExhibitionsAsync(facade);  
                        Console.Write("\nВведите ID выставки: ");
                        if (int.TryParse(Console.ReadLine(), out int exhibitionId2))
                        {
                            try
                            {
                                var avgDiscount = await facade.GetAverageDiscountForExhibitionAsync(exhibitionId2);  
                                Console.WriteLine($"Средний процент скидки у посетителей выставки с ID {exhibitionId2}: {avgDiscount:F2}%");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка: {ex.Message}");
                            }
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
