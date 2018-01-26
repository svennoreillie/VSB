using System;
using VSBaseAngular.Models;

public class ThabCertificate : ModelBase
{
    public DateTime? BeginDate { get; set; }
    public string CertificateId { get; set; }
    public DateTime? DecisionDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsMigrated
    {
        get
        {
            return this.MigrateDate.HasValue;
        }
    }
    public DateTime? MigrateDate { get; set; }
    public DateTime ReferenceDate { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public string Remark { get; set; }
    public string State { get; set; }
    public string TerminationReason { get; set; }
}