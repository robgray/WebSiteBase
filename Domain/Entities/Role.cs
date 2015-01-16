using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role : IHasId
    {
        private IList<User> _users;

        public Role()
        {
            _users = new List<User>();
        }

        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual bool IsAdministrator { get; set; }

        public virtual IEnumerable<User> Users { get { return _users; } }
    }
}
