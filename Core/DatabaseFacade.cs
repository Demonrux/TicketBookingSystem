using DatabaseModels;
using DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class DatabaseFacade : IDatabaseFacade
    {
        private readonly ApplicationContext _context;

        public DatabaseFacade(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddVisitorAsync(Visitor visitor)
        {
            try
            {
                await _context.Visitors.AddAsync(visitor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Ошибка при добавлении посетителя: {ex.Message}", ex);
            }
        }

        public async Task<List<Visitor>> GetAllVisitorsAsync()
        {
            try
            {
                return await _context.Visitors.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка посетителей", ex);
            }
        }

        public async Task UpdateVisitorAsync(Visitor visitor)
        {
            try
            {
                _context.Visitors.Update(visitor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при обновлении посетителя", ex);
            }
        }

        public async Task DeleteVisitorAsync(int id)
        {
            try
            {
                var visitor = await _context.Visitors.FindAsync(id);
                if (visitor != null)
                {
                    _context.Visitors.Remove(visitor);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при удалении посетителя", ex);
            }
        }

        public async Task<Visitor?> GetVisitorByIdAsync(int id)
        {
            try
            {
                return await _context.Visitors.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении посетителя", ex);
            }
        }

        public async Task AddExhibitionAsync(Exhibition exhibition)
        {
            try
            {
                await _context.Exhibitions.AddAsync(exhibition);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при добавлении выставки", ex);
            }
        }

        public async Task<List<Exhibition>> GetAllExhibitionsAsync()
        {
            try
            {
                return await _context.Exhibitions.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка выставок", ex);
            }
        }

        public async Task UpdateExhibitionAsync(Exhibition exhibition)
        {
            try
            {
                _context.Exhibitions.Update(exhibition);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при обновлении выставки", ex);
            }
        }

        public async Task DeleteExhibitionAsync(int id)
        {
            try
            {
                var exhibition = await _context.Exhibitions.FindAsync(id);
                if (exhibition != null)
                {
                    _context.Exhibitions.Remove(exhibition);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при удалении выставки", ex);
            }
        }

        public async Task<Exhibition?> GetExhibitionByIdAsync(int id)
        {
            try
            {
                return await _context.Exhibitions.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении выставки", ex);
            }
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            try
            {
                await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при добавлении билета", ex);
            }
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            try
            {
                return await _context.Tickets.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении списка билетов", ex);
            }
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Tickets.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при обновлении билета", ex);
            }
        }

        public async Task DeleteTicketAsync(int id)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket != null)
                {
                    _context.Tickets.Remove(ticket);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ошибка при удалении билета", ex);
            }
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            try
            {
                return await _context.Tickets.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении билета", ex);
            }
        }

        public async Task<int> GetSoldTicketsCountAsync(int exhibitionId)
        {
            try
            {
                return await _context.Tickets
                    .CountAsync(t => t.ExhibitionId == exhibitionId);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при подсчете билетов", ex);
            }
        }

        public async Task<int> GetUniqueExhibitionsCountAsync(int visitorId)
        {
            try
            {
                return await _context.Tickets
                    .Where(t => t.VisitorId == visitorId)
                    .Select(t => t.ExhibitionId)
                    .Distinct()
                    .CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при подсчете уникальных выставок", ex);
            }
        }

        public async Task<double> GetAverageDiscountForExhibitionAsync(int exhibitionId)
        {
            try
            {
                var discounts = await _context.Tickets
                    .Where(t => t.ExhibitionId == exhibitionId)
                    .Join(_context.Visitors,
                        ticket => ticket.VisitorId,
                        visitor => visitor.Id,
                        (ticket, visitor) => visitor.Discount)
                    .ToListAsync();

                return discounts.Any() ? discounts.Average() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при расчете средней скидки", ex);
            }
        }
    }
}
