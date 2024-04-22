using FS20240422.Data;
using FS20240422.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FS20240422.Controllers
{
    public class OrdersController : ODataController
    {
        private readonly FsDbContext db;

        public OrdersController(FsDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public ActionResult<IQueryable<Order>> Get()
        {
            return this.db.Orders;
        }

        [EnableQuery]
        public SingleResult<Order> Get(int key)
        {
            return SingleResult.Create(this.db.Orders.Where(d => d.Id == key));
        }

        public ActionResult Post([FromBody] Order order)
        {
            if (order.Id == 0)
            {
                return BadRequest();
            }

            var dbOrder = this.db.Orders.FirstOrDefault(d => d.Id == order.Id);

            if (dbOrder != null)
            {
                return Conflict();
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return Created(order);
        }

        public ActionResult Patch(int key, [FromBody] Delta<Order> delta)
        {
            if (delta == null)
            {
                return BadRequest();
            }

            var order = this.db.Orders.FirstOrDefault(d => d.Id == key);

            if (order == null)
            {
                return BadRequest();
            }

            delta.Patch(order);

            db.SaveChanges();

            return Ok();
        }

        public ActionResult Put(int key, [FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }   
            
            var dbOrder = this.db.Orders.FirstOrDefault(d => d.Id == key);

            if (dbOrder == null)
            {
                return BadRequest();
            }

            dbOrder.OrderDate = order.OrderDate;
            dbOrder.Amount = order.Amount;

            db.SaveChanges();

            return Ok();
        }

        public ActionResult Delete(int key)
        {
            var order = this.db.Orders.FirstOrDefault(d => d.Id == key);

            if (order != null)
            {
                db.Orders.Remove(order);
                db.SaveChanges();
            }

            return Ok();
        }

        public ActionResult CreateRefToCustomer(int key, int relatedKey)
        {
            var order = this.db.Orders.FirstOrDefault(d => d.Id == key);

            if (order == null)
            {
                return BadRequest();
            }

            var customer = this.db.Customers.FirstOrDefault(d => d.Id == relatedKey);

            if (customer == null)
            {
                return BadRequest();
            }

            order.Customer = customer;

            db.SaveChanges();

            return Ok();
        }
    }
}
