namespace VSBaseAngular.Models.Search
{
    public class ThabCertificateSearch : SearchBaseModel<ThabCertificate>
    {
        public string Insz { get; set; }
        public long SiNumber { get; set; }
    }
}