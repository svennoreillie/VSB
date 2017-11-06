using System;

namespace VSBaseAngular.Models
{
    public class Person : ModelBase
    {
        public int SiNumber { get; set; }
        public string Insz { get; set; }

        public int FederationNumber { get; set; }
        public string MemberNumber { get; set; }

        public string FirstName { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DeceaseDate { get; set; }
        public int Sex { get; set; }
    }
}