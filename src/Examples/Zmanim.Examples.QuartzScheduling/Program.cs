using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Topshelf;
using Topshelf.Configuration.Dsl;

namespace Zmanim.Examples.QuartzScheduling
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = RunnerConfigurator.New(x =>
            {
                x.AfterStoppingTheHost(h => { });

                x.ConfigureService<Scheduler>(s =>
                {
                    s.Named("ZmanimScheduler");
                    s.HowToBuildService(name => new Scheduler());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                    s.WhenPaused(tc => tc.Pause());
                });

                x.RunAsLocalSystem();
                //x.RunAsFromInteractive();

                x.SetDescription("ZmanimScheduler Host");
                x.SetDisplayName("ZmanimScheduler");
                x.SetServiceName("ZmanimScheduler");

            });

            Runner.Host(cfg, args);
        }
    }
}
