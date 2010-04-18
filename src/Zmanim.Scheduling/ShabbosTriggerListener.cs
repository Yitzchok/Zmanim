using System;
using Quartz;

namespace Zmanim.Scheduling
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

        private bool IsShabbos(DateTime timeUtc, Location location)
        {
            return timeUtc.IsShabbos(location);
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