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
            var location = context.Get("Location") as Location;

            return location == null ? false : IsShabbos(DateTime.UtcNow, location);
        }

        private bool IsShabbos(DateTime timeUtc, Location location)
        {
            return timeUtc.IsShabbos(location);
        }

        public void TriggerComplete(Trigger trigger, JobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
        }

        public string Name { get { return "ShabbosTriggerListener"; } }
    }
}