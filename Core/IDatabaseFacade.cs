using DatabaseModels;

namespace Core
{
    public interface IDatabaseFacade
    {
        Task AddVisitorAsync(Visitor visitor);
        Task<List<Visitor>> GetAllVisitorsAsync();
        Task UpdateVisitorAsync(Visitor visitor);
        Task DeleteVisitorAsync(int id);
        Task<Visitor?> GetVisitorByIdAsync(int id);

        Task AddExhibitionAsync(Exhibition exhibition);
        Task<List<Exhibition>> GetAllExhibitionsAsync();
        Task UpdateExhibitionAsync(Exhibition exhibition);
        Task DeleteExhibitionAsync(int id);
        Task<Exhibition?> GetExhibitionByIdAsync(int id);

        Task AddTicketAsync(Ticket ticket);
        Task<List<Ticket>> GetAllTicketsAsync();
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id);
        Task<Ticket?> GetTicketByIdAsync(int id);

        Task<int> GetSoldTicketsCountAsync(int exhibitionId);
        Task<int> GetUniqueExhibitionsCountAsync(int visitorId);
        Task<double> GetAverageDiscountForExhibitionAsync(int exhibitionId);
    }
}
