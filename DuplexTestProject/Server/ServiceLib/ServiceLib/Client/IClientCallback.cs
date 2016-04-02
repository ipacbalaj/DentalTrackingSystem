using System.Collections.Generic;
using System.ServiceModel;

namespace ServiceLib.Client
{
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void TestCallback(List<string> testString);
    }
}
