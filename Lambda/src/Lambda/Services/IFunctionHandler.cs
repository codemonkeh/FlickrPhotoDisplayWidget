using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace Lambda.Services
{
    public interface IFunctionHandler
    {
        Task Handle(ILambdaContext context);
    }
}