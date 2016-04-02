using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLib.Client
{
    public class ClientInfo
    {
        public Guid ID { get; set; }

        public string MachineName { get; set; }

        public ClientInfo(string machineName)
        {
            MachineName = machineName;
            ID = Guid.NewGuid();
        }

        public ClientInfo(Guid id)
        {
            ID = id;
        }

        #region Methods

        protected bool Equals(ClientInfo other)
        {
            return ID.Equals(other.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClientInfo)obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        #endregion Methods
    }
}
