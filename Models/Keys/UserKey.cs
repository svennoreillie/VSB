namespace VSBaseAngular.Models.Keys
{
    public class UserKey : KeySet<User>
    {
        public UserKey()
        {
            
        }
        public UserKey(string Domain, string UserId)
        {
            this.Domain = Domain;
            this.UserId = UserId;
        }

        public string Domain { get; private set; }
        public string UserId { get; private set; }
    }
}