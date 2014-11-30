using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AppLayer;

namespace ObserverService
{
    public partial class Service1 : ServiceBase
    {
        DirictoryWatcher dir = new DirictoryWatcher(null);

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            dir.Run();
        }

        protected override void OnStop()
        {
            dir.Stop();
        }
    }
}
