using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_CarnesDonFernando.Quartz
{
    public class ScheduledJobFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ScheduledJobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return serviceProvider.GetService(typeof(IJob)) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            if (job is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
