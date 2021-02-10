using System;

namespace AMT.Data.Entities
{
    public class UserDim
    {
        public string UserKey { get; set; }
        public string UserEmail { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
