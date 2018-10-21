using Inventory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Serialization.Json;


namespace Inventory.Controllers
{
    public class ItemsController : ApiController
    {
        public List<Item> items = new List<Item>();
        private List<string> txtData = new List<string>();
        private string dataPath;
        private int currentLabel = 0;

        public ItemsController()
        {          
            //establish some form of storage using file I/O to save data state
            string commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dataPath = commonAppData + "\\Invetory.txt";
            if (File.Exists(dataPath))
            {
                txtData = File.ReadAllLines(dataPath).ToList();
                //initialize list by converting CSV style data to objects 
                foreach (string row in txtData)
                {
                    string[] properties = row.Split(',');
                    Item newItem = new Item();
                    //parse row to Item
                    if(properties.Length == 3)
                    {
                        //Label
                        try
                        {
                            newItem.Label = System.Convert.ToInt32(properties[0]);
                            //update current label to simulate auto increment from a DB
                            currentLabel = newItem.Label;
                        }
                        catch (Exception e)
                        {
                            //TODO log bad data error
                            Console.Write(e);
                            break;
                        }

                        //Type
                        newItem.Type = properties[1];

                        //isExpired
                        switch (properties[2].ToLower())
                        {
                            case "true":
                                newItem.IsExpired = true;
                                break;
                            case "false":
                                newItem.IsExpired = false;
                                break;
                            default:
                                //default to unexpired
                                newItem.IsExpired = false;
                                break;

                        }
                        
                    }
                    //add Item to list  
                    items.Add(newItem);
                }
            }
            else
            {
                File.Create(dataPath);           
            }           
            
    
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
            //add to list
            //autoincrement ID
            item.Label = currentLabel + 1;

            items.Add(item);
            //fill string for txt file update
            string row= item.Label.ToString() +","+ item.Type+","+ item.IsExpired.ToString();
            //update current label to simulate auto increment from a DB
            currentLabel = currentLabel++;
            txtData.Add(row);

            File.WriteAllLines(dataPath, txtData);
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
            string row = item.Label.ToString() + "," + item.Type + "," + item.IsExpired.ToString();
            //update txt storage
            txtData.Remove(row);
            File.WriteAllLines(dataPath, txtData);
        }
    }
}
