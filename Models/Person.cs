using System;

namespace VSBaseAngular.Models
{
    public class Person : ModelBase
    {
        public string FirstName { get; set; }
        public string Name { get; set; }
        public DateTime BirthDateTime { get; set; }
        public DateTime DeceaseDateTime { get; set; }
        public int Sex { get; set; }
        public int SiNumber { get; set; }
        public int FederationNumber { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string NumberBox { get; set; }
        public string ZipCode { get; set; }
        public string Locality { get; set; }
        public string NissNb { get; set; }
        public string MemberNumber { get; set; }
    }
}