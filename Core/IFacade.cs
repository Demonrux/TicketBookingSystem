using DatabaseModels;

namespace Core
{
    public interface IDatabaseFacade
    {
        void AddVisitor(Visitor visitor);
        List<Visitor> GetAllVisitors();
        void UpdateVisitor(Visitor visitor);
        void DeleteVisitor(int id);
        Visitor GetVisitorById(int id);

        void AddExhibition(Exhibition exhibition);
        List<Exhibition> GetAllExhibitions();
        void UpdateExhibition(Exhibition exhibition);
        void DeleteExhibition(int id);
        Exhibition GetExhibitionById(int id);

        void AddTicket(Ticket ticket);
        List<Ticket> GetAllTickets();
        void UpdateTicket(Ticket ticket);
        void DeleteTicket(int id);
        Ticket GetTicketById(int id);

        int GetSoldTicketsCount(int exhibitionId);
        int GetUniqueExhibitionsCount(int visitorId);
        double GetAverageDiscountForExhibition(int exhibitionId);
    }
}
