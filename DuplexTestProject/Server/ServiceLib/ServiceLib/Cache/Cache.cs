using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLib.Cache
{
    public class Cache
    {
        private static readonly Cache instance = new Cache();
        public static Cache Instance
        {
            get { return instance; }
        }

        [DataMember]
        public List<string> Data = new List<string>() { "Test1", "Test2", "Test3" };
    }
}
