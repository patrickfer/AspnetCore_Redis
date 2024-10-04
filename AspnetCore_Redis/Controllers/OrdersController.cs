using AspnetCore_Redis.Data;
using AspnetCore_Redis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace AspnetCore_Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly NorthwindContext _context;
        private readonly IDistributedCache _distributedCache;

        public OrdersController(NorthwindContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllOrdersUsingRedisCache()
        {
            var cacheKey = "orderList";
            string serializedOrderList;
            var orderList = new List<Order>();

            var redisOrderList = await _distributedCache.GetAsync(cacheKey);

            if (redisOrderList != null) 
            {
                serializedOrderList = Encoding.UTF8.GetString(redisOrderList);
                orderList = JsonConvert.DeserializeObject<List<Order>>(serializedOrderList);
            }
            else
            {
                orderList = await _context.Orders.ToListAsync();
                serializedOrderList = JsonConvert.SerializeObject(orderList);
                redisOrderList = Encoding.UTF8.GetBytes(serializedOrderList);

                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                await _distributedCache.SetAsync(cacheKey, redisOrderList, options);
            }
            return Ok(orderList);
        }
    }
}
