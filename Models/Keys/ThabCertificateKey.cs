namespace VSBaseAngular.Models.Keys
{
    public class ThabCertificateKey : KeySet<ThabCertificate>
    {
        public ThabCertificateKey()
        {

        }
        public ThabCertificateKey(string certificateId, string insz, long siNumber)
        {
            this.CertificateId = certificateId;
            this.Insz = insz;
            this.SiNumber = siNumber;
        }

        public string CertificateId{ get; set; }
        public string Insz{ get; set; }
        public long SiNumber { get; set; }
    }
}