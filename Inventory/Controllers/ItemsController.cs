using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Inventory.Controllers
{
    public class ItemsController : ApiController
    {
        public List<Item> items = new List<Item>();

        public ItemsController()
        {
            items.Add(new Item {  IsExpired = false, Type = "Eggs",Label = 1});
            items.Add(new Item {  IsExpired = false, Type = "Milk", Label = 2 });
            items.Add(new Item {  IsExpired = false, Type = "Bannana", Label = 3});
        }

        // GET: api/Items
        public List<Item> Get()
        {
            return items;
        }

        // GET: api/Items/5
        public Item Get(int label)
        {
            Item item = items.Where(x => x.Label == label).FirstOrDefault();
            return item;
        }

        // POST: api/Items
        public void Post(Item item)
        {
            items.Add(item);
        }

        // PUT: api/Items/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Items/5
        public void Delete(int label)
        {
            Item item = items.Where(x => x.Label == label).FirstOrDefault();
            items.Remove(item);
        }
    }
}
