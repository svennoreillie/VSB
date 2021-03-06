using System;

public class ZvzPayment
{
    public string AccountNb { get; set; }
    public decimal Amount { get; set; }
    public DateTime BeginDate { get; set; }
    public string Currency { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? SendDate { get; set; }
    public int UnCode { get; set; }
    public string CertificateId { get; set; }
}