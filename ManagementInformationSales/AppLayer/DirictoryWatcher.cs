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
        public string WatcherDirectory { get; set; }
        public string ViewedFilesDirectory{ get; set; }


        private FileSystemWatcher _catalogWatcher;
        
        public DirictoryWatcher(string path)
        {
          Initialize(path);
        }

        private void Initialize(string watcherDirectory)
        {
            if (watcherDirectory != null)
            {
                WatcherDirectory = watcherDirectory;
            }
            else
            {
                if (ReadSetting("WatcherDirectory") != null)
                    WatcherDirectory = ReadSetting("WatcherDirectory");
            }


            _catalogWatcher = new FileSystemWatcher(WatcherDirectory);
            _catalogWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
             _catalogWatcher.Filter = "*.csv";
        }

        public void Run()
        {
            _catalogWatcher.Created += new FileSystemEventHandler(OnCreated);
            _catalogWatcher.EnableRaisingEvents = true;

            IEnumerable<string> currentFiles = GetFiles();

            foreach (string currentFile in currentFiles)
            {
                Administrator administrator = new Administrator(currentFile, WatcherDirectory);
                administrator.RegistrationSale();
                MoveFile(currentFile);
            }
        }
       
        //создание
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
           Administrator administrator = new Administrator(e.Name, WatcherDirectory);
           administrator.RegistrationSale();
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
        }

        private static string ReadSetting(string key)
        {
            string result = null;

            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings[key] != null)
                {
                    result = appSettings[key];
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }

            return result;
        }


        private  void MoveFile(string fileName)
        {

            string sourceFile = Path.Combine(WatcherDirectory, fileName);
            string destinationFile = Path.Combine(ViewedFilesDirectory, fileName);

            if (!Directory.Exists(ViewedFilesDirectory))
            {
                Directory.CreateDirectory(ViewedFilesDirectory);
            }

            File.Move(sourceFile, destinationFile);
        }
    }
}
