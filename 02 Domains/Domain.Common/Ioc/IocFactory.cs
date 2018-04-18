using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Domain.Common.Ioc
{
    public class IocFactory
    {
        private static IUnityContainer iocContainer = new UnityContainer();
        public static IUnityContainer Default { get { return iocContainer; } }

        public static IUnityContainer CreateContainer()
        {
            return null;
        }
        public static IUnityContainer CreateContainer(string containerName)
        {
            return null;
        }
    }
}
