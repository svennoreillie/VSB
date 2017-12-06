using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSBaseAngular.Models
{
    public class User : ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public Guid Guid { get; set; }
    }
}
