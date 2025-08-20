using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lmes.功能
{
    class 文件变更监视:IDisposable
    {
        public 文件变更监视(string path)
        {
            fw = new FileSystemWatcher(path);
            fw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fw.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            fw.Changed += OnChanged;
            fw.Created += OnCreated;
            fw.Deleted += OnDeleted;
            fw.Renamed += OnRenamed;
            fw.Error += OnError;

            fw.Created += (object o,FileSystemEventArgs e) =>
            {
                最后更新的文件 = e.FullPath + e.Name;
                OnFileChanged?.Invoke(o,e);
            };
        }
        public string 最后更新的文件 { get; set; }

        public FileSystemWatcher fw ;
        public event FileSystemEventHandler OnFileChanged;
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
           
        }
            
        public void Dispose()
        {
            fw.Dispose();
        }
    }
}
