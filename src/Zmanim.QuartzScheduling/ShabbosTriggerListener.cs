using System;
using Quartz;
using Zmanim.Extensions;
using Zmanim.QuartzScheduling.Configuration;
using Zmanim.QuartzScheduling.Jobs;

namespace Zmanim.QuartzScheduling
{
    public class ShabbosTriggerListener : ITriggerListener
    {
        public void TriggerFired(Trigger trigger, JobExecutionContext context) { }
        public void TriggerMisfired(Trigger trigger) { }

        public bool VetoJobExecution(Trigger trigger, JobExecutionContext context)
        {
            var reminderServiceJobDetail = context.JobDetail as ReminderServiceJobDetail;
            if (reminderServiceJobDetail != null)
                if (IsShabbos(DateTime.UtcNow, reminderServiceJobDetail.ReminderService.LocationProperties))
                {
                    ReSchedule(context, trigger, reminderServiceJobDetail);
                    return true;
                }

            return false;
        }

        private bool IsShabbos(DateTime timeUtc, ZmanimLocationProperties locationProperties)
        {
            var calendar = SchedulerHelper.GetComplexZmanimCalendar(locationProperties, timeUtc);
            bool isShabbos = false;

            if (timeUtc.DayOfWeek == DayOfWeek.Friday)
                isShabbos = timeUtc > calendar.getCandelLighting().ToDateTime().ToUniversalTime();
            if (timeUtc.DayOfWeek == DayOfWeek.Saturday)
                isShabbos = timeUtc <= calendar.getTzais().ToDateTime().ToUniversalTime();

            return isShabbos;
        }

        public void TriggerComplete(Trigger trigger, JobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            if (triggerInstructionCode == SchedulerInstruction.DeleteTrigger && context.JobDetail is ReminderServiceJobDetail)
            {
                var reminderServiceJobDetail = context.JobDetail as ReminderServiceJobDetail;

                ReSchedule(context, trigger, reminderServiceJobDetail);
            }
        }

        private void ReSchedule(JobExecutionContext context, Trigger trigger, ReminderServiceJobDetail reminderServiceJobDetail)
        {
            var job = context.JobInstance as IZmanJob;
            if (job != null)
                SchedulerHelper.ReScheduleZmanJob(trigger.Name, job.RunNextJobAt(), context.Scheduler, reminderServiceJobDetail.ReminderService, reminderServiceJobDetail.Account);
        }

        public string Name { get { return "ShabbosTriggerListener"; } }
    }
}