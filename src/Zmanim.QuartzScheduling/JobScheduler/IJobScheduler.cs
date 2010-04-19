using Quartz;

namespace Zmanim.QuartzScheduling.JobScheduler
{
    public interface IJobScheduler
    {
        void Schedule(IScheduler scheduler);
    }
}