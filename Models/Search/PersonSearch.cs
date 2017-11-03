namespace VSBaseAngular.Models.Search
{
    public class PersonSearch : SearchBaseModel<Person>
    {
        public int Federation { get; set; }
        public string MemberNr { get; set; }
        public string Insz { get; set; }
        public long SiNumber { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
    }
}