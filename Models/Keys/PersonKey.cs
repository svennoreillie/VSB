namespace VSBaseAngular.Models.Keys
{
    public class PersonKey : KeySet<Person>
    {
        public PersonKey()
        {
            
        }
        public PersonKey(long SiNumber)
        {
            this.SiNumber = SiNumber;
        }

        public long SiNumber { get; set; }
    }
}