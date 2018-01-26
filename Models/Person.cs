using System;

namespace VSBaseAngular.Models
{
    public class Person : ModelBase
    {
        public long SiNumber { get; set; }
        public string Insz { get; set; }

        public int FederationNumber { get; set; }
        public string MemberNumber { get; set; }

        public string FirstName { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DeceaseDate { get; set; }
        public int Sex { get; set; }

        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string NumberBox { get; set; }
        public string ZipCode { get; set; }
        public string Locality { get; set; }
    }
}