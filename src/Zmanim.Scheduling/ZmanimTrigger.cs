using System;
using System.Linq.Expressions;
using java.util;
using net.sourceforge.zmanim;
using Quartz;
using Quartz.Util;
using Zmanim.Extensions;

namespace Zmanim.Scheduling
{
    /// <summary> 
    /// A concrete <see cref="Trigger" /> that is used to fire a <see cref="JobDetail" />
    /// at a given moment in time, and optionally repeated at a specified interval.
    /// </summary>
    /// <seealso cref="Trigger" />
    /// <seealso cref="CronTrigger" />
    /// <seealso cref="TriggerUtils" />
    /// 
    /// <author>James House</author>
    /// <author>Contributions by Lieven Govaerts of Ebitec Nv, Belgium.</author>
    /// <author>Marko Lahma (.NET)</author>
    [Serializable]
    public class ZmanimTrigger : Trigger
    {
        /// <summary>
        /// Used to indicate the 'repeat count' of the trigger is indefinite. Or in
        /// other words, the trigger should repeat continually until the trigger's
        /// ending timestamp.
        /// </summary>
        public const int RepeatIndefinitely = -1;
        private const int YearToGiveupSchedulingAt = 2299;

        private DateTime? nextFireTimeUtc = null;
        private DateTime? previousFireTimeUtc = null;

        private int repeatCount = 0;
        private TimeSpan repeatInterval = TimeSpan.Zero;
        private int timesTriggered = 0;
        private bool complete = false;


        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Location Location { get; set; }

        /// <summary>
        /// Gets or sets the execute on zman.
        /// </summary>
        /// <value>The execute on zman.</value>
        public Func<ComplexZmanimCalendar, Date> ExecuteOnZman { get; set; }

        /// <summary>
        /// Create a <see cref="ZmanimTrigger" /> with no settings.
        /// </summary>
        public ZmanimTrigger()
        {
        }

        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at the next given zman,
        /// and not repeat.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        public ZmanimTrigger(string name, Location location, Func<ComplexZmanimCalendar, Date> executeOnZman)
            : this(name, null, location, executeOnZman)
        {
        }

        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at the given zman after the given time,
        /// and not repeat.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="startTimeUtc">The start time UTC.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        public ZmanimTrigger(string name, Location location, DateTime startTimeUtc, Func<ComplexZmanimCalendar, Date> executeOnZman)
            : this(name, null, location, startTimeUtc, executeOnZman)
        {
        }

        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at the zman after the given time,
        /// and not repeat.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="group">The group.</param>
        /// <param name="location">The location.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        public ZmanimTrigger(string name, string group, Location location, Func<ComplexZmanimCalendar, Date> executeOnZman)
            : this(name, group, location, executeOnZman, null, 0, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at zman after the given time,
        /// and not repeat.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="group">The group.</param>
        /// <param name="location">The location.</param>
        /// <param name="startTimeUtc">The start time UTC.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        public ZmanimTrigger(string name, string group, Location location, DateTime startTimeUtc, Func<ComplexZmanimCalendar, Date> executeOnZman)
            : this(name, group, location, startTimeUtc, executeOnZman, null, 0, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at the zman after the given time,
        /// and repeat at the the given interval the given number of times, or until
        /// the given end time.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        /// <param name="endTimeUtc">A UTC <see cref="DateTime"/> set to the time for the <see cref="Trigger"/>
        /// to quit repeat firing.</param>
        /// <param name="repeatCount">The number of times for the <see cref="Trigger"/> to repeat
        /// firing, use <see cref="RepeatIndefinitely "/> for unlimited times.</param>
        /// <param name="repeatInterval">The time span to pause between the repeat firing.</param>
        public ZmanimTrigger(string name, Location location,
            Func<ComplexZmanimCalendar, Date> executeOnZman,
            DateTime? endTimeUtc, int repeatCount, TimeSpan repeatInterval)
            : this(name, null, location, executeOnZman, endTimeUtc, repeatCount, repeatInterval)
        {
        }

        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at the zman after the given time,
        /// and repeat at the the given interval the given number of times, or until
        /// the given end time.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="location">The location.</param>
        /// <param name="startTimeUtc">A UTC <see cref="DateTime"/> set to the time for the <see cref="Trigger"/> to fire.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        /// <param name="endTimeUtc">A UTC <see cref="DateTime"/> set to the time for the <see cref="Trigger"/>
        /// to quit repeat firing.</param>
        /// <param name="repeatCount">The number of times for the <see cref="Trigger"/> to repeat
        /// firing, use <see cref="RepeatIndefinitely "/> for unlimited times.</param>
        /// <param name="repeatInterval">The time span to pause between the repeat firing.</param>
        public ZmanimTrigger(string name, Location location, DateTime startTimeUtc,
            Func<ComplexZmanimCalendar, Date> executeOnZman,
            DateTime? endTimeUtc, int repeatCount, TimeSpan repeatInterval)
            : this(name, null, location, startTimeUtc, executeOnZman, endTimeUtc, repeatCount, repeatInterval)
        {
        }


        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at the zman after the given time,
        /// and repeat at the the given interval the given number of times, or until
        /// the given end time.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="group">The group.</param>
        /// <param name="location">The location.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        /// <param name="endTimeUtc">A UTC <see cref="DateTime"/> set to the time for the <see cref="Trigger"/>
        /// to quit repeat firing.</param>
        /// <param name="repeatCount">The number of times for the <see cref="Trigger"/> to repeat
        /// firing, use <see cref="RepeatIndefinitely "/> for unlimited times.</param>
        /// <param name="repeatInterval">The time span to pause between the repeat firing.</param>
        public ZmanimTrigger(string name, string group, Location location,
            Func<ComplexZmanimCalendar, Date> executeOnZman,
            DateTime? endTimeUtc,
            int repeatCount, TimeSpan repeatInterval)
            : base(name, group)
        {
            StartTimeUtc = DateTime.UtcNow;
            EndTimeUtc = endTimeUtc;
            RepeatCount = repeatCount;
            RepeatInterval = repeatInterval;
        }


        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at the zman after the given time,
        /// and repeat at the the given interval the given number of times, or until
        /// the given end time.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="group">The group.</param>
        /// <param name="location">The location.</param>
        /// <param name="startTimeUtc">A UTC <see cref="DateTime"/> set to the time for the <see cref="Trigger"/> to fire.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        /// <param name="endTimeUtc">A UTC <see cref="DateTime"/> set to the time for the <see cref="Trigger"/>
        /// to quit repeat firing.</param>
        /// <param name="repeatCount">The number of times for the <see cref="Trigger"/> to repeat
        /// firing, use <see cref="RepeatIndefinitely "/> for unlimited times.</param>
        /// <param name="repeatInterval">The time span to pause between the repeat firing.</param>
        public ZmanimTrigger(string name, string group, Location location, DateTime startTimeUtc,
            Func<ComplexZmanimCalendar, Date> executeOnZman,
            DateTime? endTimeUtc,
            int repeatCount, TimeSpan repeatInterval)
            : base(name, group)
        {
            StartTimeUtc = startTimeUtc;
            EndTimeUtc = endTimeUtc;
            RepeatCount = repeatCount;
            RepeatInterval = repeatInterval;
        }

        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at the given time,
        /// fire the identified <see cref="IJob"/> and repeat at the the given
        /// interval the given number of times, or until the given end time.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="group">The group.</param>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="jobGroup">The job group.</param>
        /// <param name="location">The location.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        /// <param name="endTimeUtc">A <see cref="DateTime"/> set to the time for the <see cref="Trigger"/>
        /// to quit repeat firing.</param>
        /// <param name="repeatCount">The number of times for the <see cref="Trigger"/> to repeat
        /// firing, use RepeatIndefinitely for unlimited times.</param>
        /// <param name="repeatInterval">The time span to pause between the repeat firing.</param>
        public ZmanimTrigger(string name, string group, string jobName, string jobGroup, Location location,
                 Func<ComplexZmanimCalendar, Date> executeOnZman,
                 DateTime? endTimeUtc,
                 int repeatCount, TimeSpan repeatInterval)
            : base(name, group, jobName, jobGroup)
        {
            StartTimeUtc = DateTime.UtcNow;
            EndTimeUtc = endTimeUtc;
            RepeatCount = repeatCount;
            RepeatInterval = repeatInterval;
            Location = location;
            ExecuteOnZman = executeOnZman;
        }

        /// <summary>
        /// Create a <see cref="ZmanimTrigger"/> that will occur at the given time,
        /// fire the identified <see cref="IJob"/> and repeat at the the given
        /// interval the given number of times, or until the given end time.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="group">The group.</param>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="jobGroup">The job group.</param>
        /// <param name="location">The location.</param>
        /// <param name="startTimeUtc">A <see cref="DateTime"/> set to the time for the <see cref="Trigger"/>
        /// to fire.</param>
        /// <param name="executeOnZman">The execute on zman.</param>
        /// <param name="endTimeUtc">A <see cref="DateTime"/> set to the time for the <see cref="Trigger"/>
        /// to quit repeat firing.</param>
        /// <param name="repeatCount">The number of times for the <see cref="Trigger"/> to repeat
        /// firing, use RepeatIndefinitely for unlimited times.</param>
        /// <param name="repeatInterval">The time span to pause between the repeat firing.</param>
        public ZmanimTrigger(string name, string group, string jobName, string jobGroup, Location location, DateTime startTimeUtc,
                 Func<ComplexZmanimCalendar, Date> executeOnZman,
                 DateTime? endTimeUtc,
                 int repeatCount, TimeSpan repeatInterval)
            : base(name, group, jobName, jobGroup)
        {
            StartTimeUtc = startTimeUtc;
            EndTimeUtc = endTimeUtc;
            RepeatCount = repeatCount;
            RepeatInterval = repeatInterval;
            Location = location;
            ExecuteOnZman = executeOnZman;
        }

        /// <summary>
        /// Get or set thhe number of times the <see cref="ZmanimTrigger" /> should
        /// repeat, after which it will be automatically deleted.
        /// </summary>
        /// <seealso cref="RepeatIndefinitely" />
        public int RepeatCount
        {
            get { return repeatCount; }

            set
            {
                if (value < 0 && value != RepeatIndefinitely)
                {
                    throw new ArgumentException("Repeat count must be >= 0, use the constant RepeatIndefinitely for infinite.");
                }

                repeatCount = value;
            }
        }

        /// <summary>
        /// Get or set the the time interval at which the <see cref="ZmanimTrigger" /> should repeat.
        /// </summary>
        public TimeSpan RepeatInterval
        {
            get { return repeatInterval; }

            set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentException("Repeat interval must be >= 0");
                }

                repeatInterval = value;
            }
        }

        /// <summary>
        /// Get or set the number of times the <see cref="ZmanimTrigger" /> has already
        /// fired.
        /// </summary>
        public virtual int TimesTriggered
        {
            get { return timesTriggered; }
            set { timesTriggered = value; }
        }

        /// <summary> 
        /// Returns the final UTC time at which the <see cref="ZmanimTrigger" /> will
        /// fire, if repeatCount is RepeatIndefinitely, null will be returned.
        /// <p>
        /// Note that the return time may be in the past.
        /// </p>
        /// </summary>
        public override DateTime? FinalFireTimeUtc
        {
            get
            {
                if (repeatCount == 0)
                {
                    return StartTimeUtc;
                }

                if (repeatCount == RepeatIndefinitely && !EndTimeUtc.HasValue)
                {
                    return null;
                }

                if (repeatCount == RepeatIndefinitely && !EndTimeUtc.HasValue)
                {
                    return null;
                }
                else if (repeatCount == RepeatIndefinitely)
                {
                    return GetFireTimeBefore(EndTimeUtc);
                }

                DateTime lastTrigger = StartTimeUtc.AddMilliseconds(repeatCount * repeatInterval.TotalMilliseconds);

                if (!EndTimeUtc.HasValue || lastTrigger < EndTimeUtc.Value)
                {
                    return lastTrigger;
                }
                else
                {
                    return GetFireTimeBefore(EndTimeUtc);
                }
            }
        }

        /// <summary>
        /// Tells whether this Trigger instance can handle events
        /// in millisecond precision.
        /// </summary>
        /// <value></value>
        public override bool HasMillisecondPrecision
        {
            get { return true; }
        }


        /// <summary>
        /// Validates the misfire instruction.
        /// </summary>
        /// <param name="misfireInstruction">The misfire instruction.</param>
        /// <returns></returns>
        protected override bool ValidateMisfireInstruction(int misfireInstruction)
        {
            return (misfireInstruction == Quartz.MisfireInstruction.SimpleTrigger.FireNow)
                || (misfireInstruction == Quartz.MisfireInstruction.SimpleTrigger.RescheduleNextWithExistingCount)
                || (misfireInstruction == Quartz.MisfireInstruction.SimpleTrigger.RescheduleNextWithRemainingCount)
                || (misfireInstruction == Quartz.MisfireInstruction.SimpleTrigger.RescheduleNowWithExistingRepeatCount)
                || (misfireInstruction == Quartz.MisfireInstruction.SimpleTrigger.RescheduleNowWithRemainingRepeatCount)
                || (misfireInstruction == Quartz.MisfireInstruction.SmartPolicy);

        }

        /// <summary>
        /// Updates the <see cref="ZmanimTrigger" />'s state based on the
        /// MisfireInstruction value that was selected when the <see cref="ZmanimTrigger" />
        /// was created.
        /// </summary>
        /// <remarks>
        /// If MisfireSmartPolicyEnabled is set to true,
        /// then the following scheme will be used: <br />
        /// <ul>
        /// <li>If the Repeat Count is 0, then the instruction will
        /// be interpreted as <see cref="MisfireInstruction.SimpleTrigger.FireNow" />.</li>
        /// <li>If the Repeat Count is <see cref="RepeatIndefinitely" />, then
        /// the instruction will be interpreted as <see cref="MisfireInstruction.SimpleTrigger.RescheduleNowWithRemainingRepeatCount" />.
        /// <b>WARNING:</b> using MisfirePolicy.ZmanimTrigger.RescheduleNowWithRemainingRepeatCount 
        /// with a trigger that has a non-null end-time may cause the trigger to 
        /// never fire again if the end-time arrived during the misfire time span. 
        /// </li>
        /// <li>If the Repeat Count is > 0, then the instruction
        /// will be interpreted as <see cref="MisfireInstruction.SimpleTrigger.RescheduleNowWithExistingRepeatCount" />.
        /// </li>
        /// </ul>
        /// </remarks>
        public override void UpdateAfterMisfire(ICalendar cal)
        {
            int instr = MisfireInstruction;
            if (instr == Quartz.MisfireInstruction.SmartPolicy)
            {
                if (RepeatCount == 0)
                {
                    instr = Quartz.MisfireInstruction.SimpleTrigger.FireNow;
                }
                else if (RepeatCount == RepeatIndefinitely)
                {
                    instr = Quartz.MisfireInstruction.SimpleTrigger.RescheduleNextWithRemainingCount;

                }
                else
                {
                    instr = Quartz.MisfireInstruction.SimpleTrigger.RescheduleNowWithExistingRepeatCount;
                }
            }
            else if (instr == Quartz.MisfireInstruction.SimpleTrigger.FireNow && RepeatCount != 0)
            {
                instr = Quartz.MisfireInstruction.SimpleTrigger.RescheduleNowWithRemainingRepeatCount;
            }

            if (instr == Quartz.MisfireInstruction.SimpleTrigger.FireNow)
            {
                SetNextFireTime(DateTime.UtcNow);
            }
            else if (instr == Quartz.MisfireInstruction.SimpleTrigger.RescheduleNextWithExistingCount)
            {
                DateTime? newFireTime = GetFireTimeAfter(DateTime.UtcNow);

                while (newFireTime.HasValue && cal != null && !cal.IsTimeIncluded(newFireTime.Value))
                {
                    newFireTime = GetFireTimeAfter(newFireTime);

                    if (!newFireTime.HasValue)
                    {
                        break;
                    }

                    //avoid infinite loop
                    if (newFireTime.Value.Year > YearToGiveupSchedulingAt)
                    {
                        newFireTime = null;
                    }
                }
                SetNextFireTime(newFireTime);
            }
            else if (instr == Quartz.MisfireInstruction.SimpleTrigger.RescheduleNextWithRemainingCount)
            {
                DateTime? newFireTime = GetFireTimeAfter(DateTime.UtcNow);

                while (newFireTime.HasValue && cal != null && !cal.IsTimeIncluded(newFireTime.Value))
                {
                    newFireTime = GetFireTimeAfter(newFireTime);

                    if (!newFireTime.HasValue)
                    {
                        break;
                    }

                    //avoid infinite loop
                    if (newFireTime.Value.Year > YearToGiveupSchedulingAt)
                    {
                        newFireTime = null;
                    }
                }

                if (newFireTime.HasValue)
                {
                    int timesMissed = ComputeNumTimesFiredBetween(nextFireTimeUtc, newFireTime);
                    TimesTriggered = TimesTriggered + timesMissed;
                }

                SetNextFireTime(newFireTime);
            }
            else if (instr == Quartz.MisfireInstruction.SimpleTrigger.RescheduleNowWithExistingRepeatCount)
            {
                DateTime newFireTime = DateTime.UtcNow;
                if (repeatCount != 0 && repeatCount != RepeatIndefinitely)
                {
                    RepeatCount = RepeatCount - TimesTriggered;
                    TimesTriggered = 0;
                }

                if (EndTimeUtc.HasValue && EndTimeUtc.Value < newFireTime)
                {
                    SetNextFireTime(null); // We are past the end time
                }
                else
                {
                    StartTimeUtc = newFireTime;
                    SetNextFireTime(newFireTime);
                }
            }
            else if (instr == Quartz.MisfireInstruction.SimpleTrigger.RescheduleNowWithRemainingRepeatCount)
            {
                DateTime newFireTime = DateTime.UtcNow;
                int timesMissed = ComputeNumTimesFiredBetween(nextFireTimeUtc, newFireTime);

                if (repeatCount != 0 && repeatCount != RepeatIndefinitely)
                {
                    int remainingCount = RepeatCount - (TimesTriggered + timesMissed);
                    if (remainingCount <= 0)
                    {
                        remainingCount = 0;
                    }
                    RepeatCount = remainingCount;
                    TimesTriggered = 0;
                }


                if (EndTimeUtc.HasValue && EndTimeUtc.Value < newFireTime)
                {
                    SetNextFireTime(null); // We are past the end time
                }
                else
                {
                    StartTimeUtc = newFireTime;
                    SetNextFireTime(newFireTime);
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="IScheduler" /> has decided to 'fire'
        /// the trigger (Execute the associated <see cref="IJob" />), in order to
        /// give the <see cref="Trigger" /> a chance to update itself for its next
        /// triggering (if any).
        /// </summary>
        /// <seealso cref="JobExecutionException" />
        public override void Triggered(ICalendar cal)
        {
            timesTriggered++;
            previousFireTimeUtc = nextFireTimeUtc;
            nextFireTimeUtc = GetFireTimeAfter(nextFireTimeUtc);

            while (nextFireTimeUtc.HasValue && cal != null && !cal.IsTimeIncluded(nextFireTimeUtc.Value))
            {
                nextFireTimeUtc = GetFireTimeAfter(nextFireTimeUtc);

                if (!nextFireTimeUtc.HasValue)
                {
                    break;
                }

                //avoid infinite loop
                if (nextFireTimeUtc.Value.Year > YearToGiveupSchedulingAt)
                {
                    nextFireTimeUtc = null;
                }
            }
        }


        /// <summary>
        /// Updates the instance with new calendar.
        /// </summary>
        /// <param name="calendar">The calendar.</param>
        /// <param name="misfireThreshold">The misfire threshold.</param>
        public override void UpdateWithNewCalendar(ICalendar calendar, TimeSpan misfireThreshold)
        {
            nextFireTimeUtc = GetFireTimeAfter(previousFireTimeUtc);

            if (nextFireTimeUtc == null || calendar == null)
            {
                return;
            }

            DateTime now = DateTime.UtcNow;
            while (nextFireTimeUtc.HasValue && !calendar.IsTimeIncluded(nextFireTimeUtc.Value))
            {
                nextFireTimeUtc = GetFireTimeAfter(nextFireTimeUtc);

                if (!nextFireTimeUtc.HasValue)
                {
                    break;
                }

                //avoid infinite loop
                if (nextFireTimeUtc.Value.Year > YearToGiveupSchedulingAt)
                {
                    nextFireTimeUtc = null;
                }

                if (nextFireTimeUtc != null && nextFireTimeUtc.Value < now)
                {
                    TimeSpan diff = now - nextFireTimeUtc.Value;
                    if (diff >= misfireThreshold)
                    {
                        nextFireTimeUtc = GetFireTimeAfter(nextFireTimeUtc);
                    }
                }
            }
        }

        /// <summary>
        /// Called by the scheduler at the time a <see cref="Trigger" /> is first
        /// added to the scheduler, in order to have the <see cref="Trigger" />
        /// compute its first fire time, based on any associated calendar.
        /// <p>
        /// After this method has been called, <see cref="GetNextFireTimeUtc" />
        /// should return a valid answer.
        /// </p>
        /// </summary>
        /// <returns> 
        /// The first time at which the <see cref="Trigger" /> will be fired
        /// by the scheduler, which is also the same value <see cref="GetNextFireTimeUtc" />
        /// will return (until after the first firing of the <see cref="Trigger" />).
        /// </returns>
        public override DateTime? ComputeFirstFireTimeUtc(ICalendar cal)
        {
            nextFireTimeUtc = ComputeNextZmanTime(StartTimeUtc);

            while (nextFireTimeUtc.HasValue && cal != null && !cal.IsTimeIncluded(nextFireTimeUtc.Value))
            {
                nextFireTimeUtc = GetFireTimeAfter(nextFireTimeUtc);

                if (!nextFireTimeUtc.HasValue)
                {
                    break;
                }

                //avoid infinite loop
                if (nextFireTimeUtc.Value.Year > YearToGiveupSchedulingAt)
                {
                    return null;
                }
            }

            return nextFireTimeUtc;
        }

        /// <summary>
        /// Computes the next zman time.
        /// </summary>
        /// <param name="nextTime">The next time.</param>
        /// <returns></returns>
        private DateTime? ComputeNextZmanTime(DateTime? nextTime)
        {
            if (!nextTime.HasValue)
                return null;

            var zmanTime = Location.GetZman(nextTime.Value, ExecuteOnZman);
            if (zmanTime < nextTime)
                zmanTime = Location.GetZman(nextTime.Value.AddDays(1), ExecuteOnZman);

            return zmanTime;
        }


        /// <summary>
        /// Computes the previous zman time.
        /// </summary>
        /// <param name="previousTime">The previous time.</param>
        /// <returns></returns>
        private DateTime? ComputePreviousZmanTime(DateTime? previousTime)
        {
            if (!previousTime.HasValue)
                return null;

            var zmanTime = Location.GetZman(previousTime.Value, ExecuteOnZman);
            if (zmanTime > previousTime)
                zmanTime = Location.GetZman(previousTime.Value.AddDays(-1), ExecuteOnZman);

            return zmanTime;
        }

        /// <summary>
        /// Returns the next time at which the <see cref="ZmanimTrigger" /> will
        /// fire. If the trigger will not fire again, <see langword="null" /> will be
        /// returned. The value returned is not guaranteed to be valid until after
        /// the <see cref="Trigger" /> has been added to the scheduler.
        /// </summary>
        public override DateTime? GetNextFireTimeUtc()
        {
            return nextFireTimeUtc;
        }

        /// <summary>
        /// Returns the previous time at which the <see cref="ZmanimTrigger" /> fired.
        /// If the trigger has not yet fired, <see langword="null" /> will be
        /// returned.
        /// </summary>
        public override DateTime? GetPreviousFireTimeUtc()
        {
            return previousFireTimeUtc;
        }

        /// <summary>
        /// Set the next UTC time at which the <see cref="ZmanimTrigger" /> should fire.
        /// <strong>This method should not be invoked by client code.</strong>
        /// </summary>
        public void SetNextFireTime(DateTime? fireTimeUtc)
        {
            nextFireTimeUtc = DateTimeUtil.AssumeUniversalTime(fireTimeUtc);
        }

        /// <summary>
        /// Set the previous UTC time at which the <see cref="ZmanimTrigger" /> fired.
        /// <strong>This method should not be invoked by client code.</strong>
        /// </summary>
        public virtual void SetPreviousFireTime(DateTime? fireTimeUtc)
        {
            previousFireTimeUtc = DateTimeUtil.AssumeUniversalTime(fireTimeUtc);
        }

        /// <summary> 
        /// Returns the next UTC time at which the <see cref="ZmanimTrigger" /> will
        /// fire, after the given UTC time. If the trigger will not fire after the given
        /// time, <see langword="null" /> will be returned.
        /// </summary>
        public override DateTime? GetFireTimeAfter(DateTime? afterTimeUtc)
        {
            afterTimeUtc = ComputeNextZmanTime(DateTimeUtil.AssumeUniversalTime(afterTimeUtc));

            if (complete)
            {
                return null;
            }

            if ((timesTriggered > repeatCount) && (repeatCount != RepeatIndefinitely))
            {
                return null;
            }

            if (!afterTimeUtc.HasValue)
            {
                afterTimeUtc = DateTime.UtcNow;
            }

            if (repeatCount == 0 && afterTimeUtc.Value.CompareTo(StartTimeUtc) >= 0)
            {
                return null;
            }

            DateTime startMillis = StartTimeUtc;
            DateTime afterMillis = afterTimeUtc.Value;
            DateTime endMillis = !EndTimeUtc.HasValue ? DateTime.MaxValue : EndTimeUtc.Value;


            if (endMillis <= afterMillis)
            {
                return null;
            }

            if (afterMillis < startMillis)
            {
                return startMillis;
            }

            long numberOfTimesExecuted = (long)(((long)(afterMillis - startMillis).TotalMilliseconds / repeatInterval.TotalMilliseconds) + 1);

            if ((numberOfTimesExecuted > repeatCount) &&
                (repeatCount != RepeatIndefinitely))
            {
                return null;
            }

            DateTime time = startMillis.AddMilliseconds(numberOfTimesExecuted * repeatInterval.TotalMilliseconds);

            if (endMillis <= time)
            {
                return null;
            }


            return time;
        }

        /// <summary>
        /// Returns the last UTC time at which the <see cref="ZmanimTrigger" /> will
        /// fire, before the given time. If the trigger will not fire before the
        /// given time, <see langword="null" /> will be returned.
        /// </summary>
        public virtual DateTime? GetFireTimeBefore(DateTime? endUtc)
        {
            endUtc = ComputePreviousZmanTime(DateTimeUtil.AssumeUniversalTime(endUtc));
            if (endUtc.Value < StartTimeUtc)
            {
                return null;
            }

            int numFires = ComputeNumTimesFiredBetween(StartTimeUtc, endUtc);
            return StartTimeUtc.AddMilliseconds(numFires * repeatInterval.TotalMilliseconds);
        }

        /// <summary>
        /// Computes the number of times fired between the two UTC date times.
        /// </summary>
        /// <param name="startTimeUtc">The UTC start date and time.</param>
        /// <param name="endTimeUtc">The UTC end date and time.</param>
        /// <returns></returns>
        public virtual int ComputeNumTimesFiredBetween(DateTime? startTimeUtc, DateTime? endTimeUtc)
        {
            startTimeUtc = ComputeNextZmanTime(DateTimeUtil.AssumeUniversalTime(startTimeUtc));
            endTimeUtc = ComputePreviousZmanTime(DateTimeUtil.AssumeUniversalTime(endTimeUtc));

            long time = (long)(endTimeUtc.Value - startTimeUtc.Value).TotalMilliseconds;
            return (int)(time / repeatInterval.TotalMilliseconds);
        }

        /// <summary> 
        /// Determines whether or not the <see cref="ZmanimTrigger" /> will occur
        /// again.
        /// </summary>
        public override bool GetMayFireAgain()
        {
            return GetNextFireTimeUtc().HasValue;
        }

        /// <summary>
        /// Validates whether the properties of the <see cref="JobDetail" /> are
        /// valid for submission into a <see cref="IScheduler" />.
        /// </summary>
        public override void Validate()
        {
            base.Validate();

            if (repeatCount != 0 && repeatInterval.TotalMilliseconds < 1)
            {
                throw new SchedulerException("Repeat Interval cannot be zero.", SchedulerException.ErrorClientError);
            }
        }
    }
}