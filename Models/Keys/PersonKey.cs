namespace VSBaseAngular.Models.Keys
{
    public class PersonKey : KeySet<Person>
    {
        public PersonKey()
        {
            
        }
        public PersonKey(string SiNumber)
        {
            this.SiNumber = SiNumber;
        }

        public string SiNumber { get; set; }
    }
}