using System;
using Quartz;

namespace Zmanim.QuartzScheduling.Jobs
{
    public interface IZmanJob : IJob
    {
        DateTime RunNextJobAt();
    }
}