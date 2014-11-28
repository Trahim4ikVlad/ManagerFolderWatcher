using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppLayer;

namespace StartingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            DirictoryWatcher watcher = new DirictoryWatcher(@"C:\Users\Vlad\Documents\GitHub\ManagerFolderWatcher");

           
            watcher.Run();
            Thread.Sleep(100000);
            watcher.Stop();
        }
    }
}
