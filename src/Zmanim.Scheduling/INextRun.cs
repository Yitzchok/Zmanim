using System;

namespace Zmanim.Scheduling
{
    public interface INextRun 
    {
        DateTime RunNextJobAt();
    }
}