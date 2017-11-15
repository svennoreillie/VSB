using System;

public class ThabPayment
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime? SendDate { get; set; }
    public string Iban { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int UnCode { get; set; }
}