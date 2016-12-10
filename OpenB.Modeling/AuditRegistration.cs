using System;
using OpenB.Core.ACL;

namespace OpenB.Modeling
{
    public class AuditRegistration
    {
        public User User { get; set; }
        public DateTime Moment { get; set; }

        public AuditRegistration(User user, DateTime moment)
        {
            if (user == null) throw new ArgumentNullException("user");

            User = user;
            Moment = moment;
        }
    }
}