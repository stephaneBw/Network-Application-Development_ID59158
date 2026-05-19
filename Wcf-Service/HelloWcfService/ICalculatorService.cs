using System.ServiceModel;

namespace HelloWcfService
{
    [ServiceContract]
    public interface ICalculatorService
    {
        [OperationContract]
        int Add(int a, int b);
    }
}