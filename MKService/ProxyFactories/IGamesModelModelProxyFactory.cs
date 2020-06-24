using MKModel;
namespace MKService.ProxyFactories
{
    internal interface IGameModelProxyFactory
    {
        IGameModel Create();
    }
    internal interface IGamesModelProxyFactory
    {
        IGameModels Create();
    }
}
