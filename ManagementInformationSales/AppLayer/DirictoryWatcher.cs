using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppLayer
{
    public class DirictoryWatcher : IDisposable
    {
        public string WatcherDirectory { get; set; }
        public string ViewedFilesDirectory{ get; set; }

        private BlockingCollection<Administrator> _blockingCollection;
       

        private FileSystemWatcher _catalogWatcher;
        
        public DirictoryWatcher(string path)
        {
          Initialize(path);
        }

        private void Initialize(string watcherDirectory)
        {
            _blockingCollection  = new  BlockingCollection<Administrator>(3);
             if (watcherDirectory != null)
            {
                WatcherDirectory = watcherDirectory;
            }
            else
            {
                if (ReadSetting("WatcherDirectory") != null)
                    WatcherDirectory = ReadSetting("WatcherDirectory");
            }
            
            if (ReadSetting("ViewedFilesDirectory") != null)
            {
                ViewedFilesDirectory = ReadSetting("ViewedFilesDirectory");
            }

            _catalogWatcher = new FileSystemWatcher(WatcherDirectory);
            _catalogWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
             _catalogWatcher.Filter = "*.csv";
            //_factory = new TaskFactory();

        }

        public void Run()
        {
            _catalogWatcher.Created += new FileSystemEventHandler(OnCreated);
            _catalogWatcher.EnableRaisingEvents = true;

            IEnumerable<string> currentFiles = GetFiles();

            foreach (string currentFile in currentFiles)
            {
                Administrator administrator = new Administrator(currentFile, WatcherDirectory)
                {
                    ViewedDirictory = ViewedFilesDirectory
                };
                if (!_blockingCollection.IsAddingCompleted)
               _blockingCollection.Add(administrator);
               
               Task.Factory.StartNew(() => _blockingCollection.Take().RegistrationSale());
            }
        }
 
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
           Administrator administrator = new Administrator(e.Name, WatcherDirectory)
           {
               ViewedDirictory =  ViewedFilesDirectory
           };
           
            _blockingCollection.Add(administrator);

            Task.Factory.StartNew(() => _blockingCollection.Take().RegistrationSale());
        }

        public void Stop()
        {
            _catalogWatcher.EnableRaisingEvents = false;
            _catalogWatcher.Dispose();
        }

        private IEnumerable<String> GetFiles()
        {
            DirectoryInfo dirictory = new DirectoryInfo(WatcherDirectory);
            IList<string> nameFileList =new List<string>();

            foreach (var info in dirictory.GetFiles("*.csv"))
            {
                nameFileList.Add(info.Name);
            }

            return nameFileList;
        }

        public void Dispose()
        {
            _catalogWatcher.Created -= OnCreated;
            _catalogWatcher.Dispose();
            _blockingCollection.CompleteAdding();
        }

        private static string ReadSetting(string key)
        {
            string result = null;
            string sectionName = "appSettings";

            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(
            ConfigurationUserLevel.None);

                AppSettingsSection appSettingSection =
        (AppSettingsSection)config.GetSection(sectionName);

                if (appSettingSection.Settings[key]  != null)
                {
                    result = appSettingSection.Settings[key].Value;
                }
            }
            catch (ConfigurationErrorsException)
            {
                throw new Exception("Error reading app settings");
            }

            return result;
        }
     
    }
}
