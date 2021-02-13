using FreeDownloaderDotNet.Data;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDownloaderDotNet.Ninject
{
    public class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(ApplicationDbContext)).ToSelf().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            Bind(typeof(OptionsForm)).ToSelf();
        }
    }
}
