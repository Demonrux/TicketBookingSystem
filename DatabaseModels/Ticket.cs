namespace DatabaseModels
{
    public class Ticket
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }
        public int ExhibitionId { get; set; }
        public double Price { get; set; }
    }
}
