using DatabaseModels;
using DatabaseContext;
using System.Diagnostics;
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
        public void AddVisitor(Visitor visitor)
        {
            _context.Visitors.Add(visitor);
            _context.SaveChanges();
        }

        public void AddExhibition(Exhibition exhibition)
        {
            _context.Exhibitions.Add(exhibition);
            _context.SaveChanges();
        }

        public void AddTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        public Visitor GetVisitorById(int id)
        {
            return _context.Visitors.Find(id);
        }

        public Exhibition GetExhibitionById(int id)
        {
            return _context.Exhibitions.Find(id);
        }

        public Ticket GetTicketById(int id)
        {
            return _context.Tickets.Include(t => t.VisitorId).Include(t => t.ExhibitionId).FirstOrDefault(t => t.Id == id);
        }

        public List<Visitor> GetAllVisitors() => _context.Visitors.ToList();
        public List<Exhibition> GetAllExhibitions() => _context.Exhibitions.ToList();
        public List<Ticket> GetAllTickets() => _context.Tickets.ToList();

        public void UpdateVisitor(Visitor visitor)
        {
            _context.Visitors.Update(visitor);
            _context.SaveChanges();
        }

        public void UpdateExhibition(Exhibition exhibition)
        {
            _context.Exhibitions.Update(exhibition);
            _context.SaveChanges();
        }

        public void UpdateTicket(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }

        public void DeleteVisitor(int id)
        {
            var visitor = _context.Visitors.Find(id);
            if (visitor != null)
            {
                _context.Visitors.Remove(visitor);
                _context.SaveChanges();
            }
        }

        public void DeleteExhibition(int id)
        {
            var exhibition = _context.Exhibitions.Find(id);
            if (exhibition != null)
            {
                _context.Exhibitions.Remove(exhibition);
                _context.SaveChanges();
            }
        }

        public void DeleteTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                _context.SaveChanges();
            }
        }

        public int GetSoldTicketsCount(int exhibitionId)
        {
            return _context.Tickets.Count(tikcet => tikcet.ExhibitionId == exhibitionId);
        }

        public int GetUniqueExhibitionsCount(int visitorId)
        {
            return _context.Tickets.Where(tikcet => tikcet.VisitorId == visitorId).Select(tikcet => tikcet.ExhibitionId).Distinct().Count();
        }

        public double GetAverageDiscountForExhibition(int exhibitionId)
        {
            var discounts = _context.Tickets
                .Where(tikcet => tikcet.ExhibitionId == exhibitionId)
                .Join(_context.Visitors,ticket => ticket.VisitorId,visitor => visitor.Id,(ticket, visitor) => visitor.Discount)
                .ToList();

            return discounts.Any() ? discounts.Average() : 0;
        }
    }
}
