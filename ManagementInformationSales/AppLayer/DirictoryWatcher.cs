using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer
{
    public class DirictoryWatcher : IDisposable
    {
        public string CatalogName { get; set; }

        private FileSystemWatcher _catalogWatcher;
        
        private bool disposed = false;

        public DirictoryWatcher(string path)
        {
          Init(path);
        }

        private void Init(string catalogName)
        {
            if (catalogName != null)
            {
                CatalogName = catalogName;
            }
            else
            {
                try
                {
                    var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    var settings = configFile.AppSettings.Settings;

                    if (settings["Catalog"] != null)
                    {
                        CatalogName = settings["Catalog"].Value;
                    }
                }
                catch (ConfigurationErrorsException)
                {
                    throw new Exception("Error reading app settings");
                }     
            }

            using (_catalogWatcher = new FileSystemWatcher(CatalogName))
            {
                _catalogWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                _catalogWatcher.Filter = "*.csv";
                _catalogWatcher.Path = CatalogName;
            }
        }

        public void Run()
        {
            _catalogWatcher.Changed += new FileSystemEventHandler(OnChanged);
            _catalogWatcher.Created += new FileSystemEventHandler(OnCreated);
            _catalogWatcher.Deleted += new FileSystemEventHandler(OnDeleted);
            _catalogWatcher.Renamed += new RenamedEventHandler(OnRenamed);
            _catalogWatcher.EnableRaisingEvents = true;
        }

        //удаление
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
           
        }

        //создание
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
          
        }

        //изменение
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            
        }

        public void Stop()
        {
            _catalogWatcher.EnableRaisingEvents = false;
            _catalogWatcher.Dispose();
        }

        public IList<String> GetFiles()
        {
            return Directory.GetFiles(CatalogName, "*.csv");
        }

        public void Dispose()
        {
            _catalogWatcher.Changed -= OnChanged;
            _catalogWatcher.Created -= OnCreated;
            _catalogWatcher.Deleted -= OnDeleted;
            _catalogWatcher.Renamed -= OnRenamed;
            _catalogWatcher.Dispose();
        }

    }
}
