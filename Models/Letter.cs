using System;

public class Letter
{
    public DateTime LetterDate { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
}

public class ThabLetter : Letter {
    public string CertificateId { get; set; }
}