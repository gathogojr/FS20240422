using FS20240422.Data;
using FS20240422.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FS20240422.Controllers
{
    public class CustomersController : ODataController
    {
        private readonly FsDbContext db;

        public CustomersController(FsDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public ActionResult<IQueryable<Customer>> Get()
        {
            return this.db.Customers;
        }

        [EnableQuery]
        public SingleResult<Customer> Get(int key)
        {
            return SingleResult.Create(this.db.Customers.Where(d => d.Id == key));
        }

        public ActionResult Post([FromBody] Customer customer)
        {
            if (customer == null || customer.Id == 0)
            {
                return BadRequest();
            }

            var dbCustomer = this.db.Customers.FirstOrDefault(d => d.Id == customer.Id);

            if (dbCustomer != null)
            {
                return Conflict();
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return Created(customer);
        }

        public ActionResult Patch(int key, [FromBody] Delta<Customer> delta)
        {
            if (delta == null)
            {
                return BadRequest();
            }

            var customer = this.db.Customers.FirstOrDefault(d => d.Id == key);

            if (customer == null)
            {
                return BadRequest();
            }

            delta.Patch(customer);

            db.SaveChanges();

            return Ok();
        }

        public ActionResult Put(int key, [FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            var dbOrder = this.db.Customers.FirstOrDefault(d => d.Id == key);

            if (dbOrder == null)
            {
                return BadRequest();
            }

            dbOrder.Name = customer.Name;
            dbOrder.City = customer.City;

            db.SaveChanges();

            return Ok();
        }

        public ActionResult Delete(int key)
        {
            var customer = this.db.Customers.FirstOrDefault(d => d.Id == key);

            if (customer != null)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }

            return Ok();
        }
    }
}
