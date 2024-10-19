using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Techne.Compat
{
    internal abstract class ApplicationDeployment
    {
        public static ApplicationDeployment CurrentDeployment { get; } = new DummyApplicationDeployment();

        public abstract bool IsNetworkDeployed { get; }

        public abstract string DataDirectory { get; }

        public abstract Version CurrentVersion { get; }
    }

    internal class DummyApplicationDeployment : ApplicationDeployment
    {
        public override bool IsNetworkDeployed => false;
        
        public override string DataDirectory => Assembly.GetExecutingAssembly().Location;

        public override Version CurrentVersion => null;
    }
}
