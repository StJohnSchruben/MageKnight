using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MKService.ProxyFactories
{
    internal interface IUserModelProxyFactory
    {
        IUserModel Create();
    }
}
