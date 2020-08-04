using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransitSample.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitSample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRequestClient<OrderRegistered> requestClient;

        public OrderController(IRequestClient<OrderRegistered> requestClient)
        {        
            this.requestClient = requestClient;
        }
        

        [HttpGet]
        public async Task<IActionResult> Get(int id = 1, string customerNumber = "12")
        {           
            var (accepted, rejected) = await requestClient.GetResponse<OrderAccepted, OrderRejected>(new OrderRegistered
            {
                CustomerNumber = customerNumber,
                OrderDate = DateTime.Now,
                OrderId = id
            });
            
            if (accepted.IsCompleted)
                return Ok(await accepted);
            return Ok(await rejected);
        }
    }
}