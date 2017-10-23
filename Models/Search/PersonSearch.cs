namespace VSBaseAngular.Models.Search
{
    public class PersonSearch : SearchBaseModel<Person>
    {
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string Insz { get; set; }
        public string MemberNr { get; set; }
    }
}