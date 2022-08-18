//using Store_TechniaclTask.Services.ModelServices.Abstraction;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace _Store_TechniaclTask.Web.Helper.BackGroundServices
//{
//    public class CalculateCustomersBillsService : BackgroundService
//    {
//        private readonly ILogger<CalculateCustomersBillsService> logger;
//        private Timer timer;
//        public ICustomerBillService customerBillService;
//        public ICustomerCardService customerCardService;
//        public CalculateCustomersBillsService(IServiceScopeFactory factory, ILogger<CalculateCustomersBillsService> logger)
//        {
//            this.logger = logger;
//            this.customerBillService = factory.CreateScope().ServiceProvider.GetRequiredService<ICustomerBillService>();
//            this.customerCardService = factory.CreateScope().ServiceProvider.GetRequiredService<ICustomerCardService>();


//        }


//        public Task StopAsync(CancellationToken cancellationToken)
//        {
//            timer = new Timer(o => logger.LogInformation("CalculateCustomersBillsService Stopping"), null, TimeSpan.Zero, TimeSpan.FromHours(5));
//            return Task.CompletedTask;
//        }

//        protected override Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            int hou = DateTime.Now.Hour;
//            var Delay = DateTime.Now.Date.AddHours(25) - DateTime.Now;
//            timer = new Timer(o => Dowork(), null, Delay, TimeSpan.FromDays(1));
//            //timer = new Timer(o => Dowork(), null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
//            //timer = new Timer(o => Cont.glob(), null, TimeSpan.Zero, TimeSpan.FromHours(5));
//            return Task.CompletedTask;
//        }
//       async Task Dowork()
//        {
//            logger.LogInformation("CalculateCustomersBillsService Started");
//            var Result = await customerCardService.UpdateRequestDateAndInsertBills(DateTime.Now.Date);
//            //var Bills =await customerBillService.CreateCustmerBill(CustomerCards, DateTime.Now.Date);
//            logger.LogInformation($"${Result.Message} Total Bills Is {Result.Data?.Count} ");
//        }
//    }
//}
