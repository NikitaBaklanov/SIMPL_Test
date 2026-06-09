namespace PaymentService.DataAccess.Postgres.Models;

public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public decimal Price { get; set; }
    public bool Status { get; set; }   // false = не оплачен, true = оплачен
    public DateTime DateCreate { get; set; }
    public DateTime? DateUpdate { get; set; }
    public string EmailClient { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}