using System;

public class ThabCertificate
{
    public DateTime? BeginDate { get; set; }
    public string CertificateId { get; set; }
    public DateTime? DecisionDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsMigrated { get; }
    public DateTime? MigrateDate { get; set; }
    public DateTime ReferenceDate { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public string Remark { get; set; }
    public string State { get; set; }
    public string TerminationReason { get; set; }
}