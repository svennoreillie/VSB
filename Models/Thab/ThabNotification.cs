using System;
using System.Collections.Generic;
using VSBaseAngular.Models;

public class ThabNotification : ModelBase
{
    public string NextStep { get; set; }
    public IEnumerable<Notification> Notifications { get; set; }
}

public class Notification {
    public DateTime ReceivedDate { get; set; }
    public string Message { get; set; }
}