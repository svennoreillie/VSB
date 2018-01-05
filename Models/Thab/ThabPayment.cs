using System;

public class ThabPayment
{
    public string CertificateId { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? SendDate { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string AccountNb { get; set; }
    public int UnCode { get; set; }
}