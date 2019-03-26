using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Threading;

namespace San.Base.Plugin
{
    public class PluginInstaller
    {
        public string TargetFolder = "";
        private string FileToInstall = "";
        private string PlugInfoPath = "";
        private string InfoFileName = "";
        public event EventHandler ProgressUpdated;
        public event EventHandler ExtractedInfoFile;

        public PluginInstaller(string TargetFolder)
        {
            this.TargetFolder = TargetFolder;
        }

        public void AsynchronInstall(string FileToInstall, string PlugInfoPath, string InfoFileName = "plugData.txt")
        {
            this.FileToInstall = FileToInstall;
            this.PlugInfoPath = PlugInfoPath;
            this.InfoFileName = InfoFileName;
            AutoResetEvent ready = new AutoResetEvent(true);
            ThreadPool.QueueUserWorkItem(new WaitCallback(asyncInstall), ready);         
        }

        private void asyncInstall(object state)
        {
            Install(FileToInstall, PlugInfoPath, InfoFileName);
        }


        public void Install(string FileToInstall, string PlugInfoPath, string InfoFileName = "plugData.txt")
        {
            using (ZipArchive archive = ZipFile.OpenRead(FileToInstall))
            {
                var infoEntry = archive.GetEntry(InfoFileName);

                if(ExtractedInfoFile != null && infoEntry != null)
                {
                    PlugInfoPath = Path.Combine(PlugInfoPath, InfoFileName);
                    infoEntry?.ExtractToFile(PlugInfoPath,true);
                    ExtractedInfoFile(this, new InstallationEventArgs(PlugInfoPath));
                }

                int i = 1;
                foreach(ZipArchiveEntry entry in archive.Entries)
                {
                    try
                    {
                        string destinationPath = Path.GetFullPath(Path.Combine(TargetFolder, entry.FullName));            //Provisorische Lösung mit try/catch
                        entry.ExtractToFile(destinationPath, true);
                    }
                    catch
                    {
                    }
                    

                    if(ProgressUpdated != null)
                        ProgressUpdated(this, new InstallationEventArgs(archive.Entries.Count, i));
                    i++;
                    Thread.Sleep(100);
                }
            }
        }
        
    }

    public class InstallationEventArgs : EventArgs
    {
        public int EntrieCount;
        public int ActualEntryNumber;
        public string PlugInfoPath;
        public InstallationEventArgs(int EntrieCount, int ActualEntryNumber)
        {
            this.EntrieCount = EntrieCount;
            this.ActualEntryNumber = ActualEntryNumber;
        }

        public InstallationEventArgs(string PlugInfoPath)
        {
            this.PlugInfoPath = PlugInfoPath;
        }
    }
}
