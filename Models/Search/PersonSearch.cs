using System;
using System.Collections.Generic;

namespace VSBaseAngular.Models.Search
{
    public class PersonSearch : SearchBaseModel<Person>
    {
        public int? Federation { get; set; }
        public string MemberNr { get; set; }
        public string Insz { get; set; }
        public long? SiNumber { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }

        public string Pillar { get; set; }

        public bool? StateInitiated { get; set; }
        public bool? StateCompleted { get; set; }
        public DateTime? StateCompletedDate { get; set; }
        public bool? StateRejected { get; set; }
        public DateTime? StateRejectedDate { get; set; }

        public IEnumerable<long> SiNumbers { get; set; }
    }
}