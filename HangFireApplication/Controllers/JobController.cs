using Hangfire;
using HangFireApplication.Class;
using Microsoft.AspNetCore.Mvc;

namespace HangFireApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost]
        [Route("CreateBackgroundJob")]
        public IActionResult CreateBackgroundJob()
        {
            BackgroundJob.Enqueue<BackgroundJobs>(job => job.ListInteiros());

            return Ok();
        }

        [HttpPost]
        [Route("CreateScheduledJob")]
        public IActionResult CreateScheduleJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);

            BackgroundJob.Schedule<ScheduledJobs>(job => job.ExecuteTask(), dateTimeOffset);

            return Ok();
        }

        [HttpPost]
        [Route("CreateContinuationJob")]
        public IActionResult CreateContinuationJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);

            var job1 = BackgroundJob.Schedule<ContinuationJobs>(job => job.Job1(), dateTimeOffset);

            var job2 = BackgroundJob.ContinueJobWith<ContinuationJobs>(job1, job => job.Job2());

            var job3 = BackgroundJob.ContinueJobWith<ContinuationJobs>(job2, job => job.Job3());

            var job4 = BackgroundJob.ContinueJobWith<ContinuationJobs>(job3, job => job.Job4());

            return Ok();
        }


        [HttpPost]
        [Route("CreateRecurringJob")]
        public IActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate<RecurringJobs>("RecurringJob1", job => job.ExecutarTarefa(), Cron.Minutely);

            return Ok();
        }
    }
}
