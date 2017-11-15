using System;

public class BobCertificate
{
    public string CertificateId { get; set; }
    
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime DecisionDate { get; set; }

    public string State { get; set; }

    public DateTime? TerminationStartDate { get; set; }
    public DateTime? TerminationEndDate { get; set; }
    public string TerminationReason { get; set; }
}