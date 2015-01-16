using System.Configuration.Install;
using System.ServiceProcess;

namespace WindowsService
{
    [System.ComponentModel.RunInstaller(true)]
    [System.ComponentModel.DesignerCategory("")]
    public class ServiceInstaller : Installer
    {
        public ServiceInstaller()
        {
            Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            });
            Installers.Add(new System.ServiceProcess.ServiceInstaller
            {
                ServiceName = "TaskService",
                StartType = ServiceStartMode.Automatic
            });
        }
    }
}
