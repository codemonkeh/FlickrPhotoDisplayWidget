using Amazon.Lambda.Core;

namespace Lambda.Services
{
    public interface IFunctionHandler
    {
        void Handle(ILambdaContext context);
    }
}