using Inventory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using log4net.Config;
using System.Runtime.Serialization.Json;


namespace Inventory.Controllers
{
    public class ItemsController : ApiController
    {
        public List<Item> items = new List<Item>();
        private List<string> txtData = new List<string>();
        private string dataPath;
        private int currentLabel = 0;
       // private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
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
                            System.Diagnostics.Debug.WriteLine(e);
                          // log.Error("there is bad data. stack trace:");
                          // log.Error(e);
                            continue;
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
        public List<Item> GetItems()
        {
            return items;
        }

        // GET: api/Items/5
        public Item GetItem(int label)
        {
            Item item = items.Where(x => x.Label == label).FirstOrDefault();
            return item;
        }

        // POST: api/Items
        public string PostItem(Item item)
        {
            string responseMessage = "";
            //vaidate input for minimum requirements
            if (item.Type != "")
            {
                //add to list
                //autoincrement ID
                item.Label = currentLabel + 1;

                items.Add(item);
                //fill string for txt file update
                string row = item.Label.ToString() + "," + item.Type + "," + item.IsExpired.ToString();
                //update current label to simulate auto increment from a DB
                currentLabel = currentLabel++;
                txtData.Add(row);

                File.WriteAllLines(dataPath, txtData);
                responseMessage="Item " + item.Label.ToString()
                + " of type " + item.Type + " has been added to the inventory";
            }
            else
            {
                responseMessage = "Please specify the item's type ";
            }
           // log.Info(responseMessage);
            System.Diagnostics.Debug.WriteLine(responseMessage);
            return (responseMessage);
        }

        // PUT: api/Items/5
        public String PutItem(int label, [FromBody]Item item)
        {
            string responseMessage = "";
            Item updateItem= items.Where(x => x.Label == label).FirstOrDefault();
            string oldRow = updateItem.Label.ToString() + "," + updateItem.Type + "," + updateItem.IsExpired.ToString();
            string newRow = updateItem.Label.ToString() + "," + updateItem.Type + "," + updateItem.IsExpired.ToString();
            bool changed = false;
            //only used to modify isExpired currently but could be used for different put requests with validation
            //you can't unexpire an item
            if (!updateItem.IsExpired && item.IsExpired)
            {
                updateItem.IsExpired = item.IsExpired;
                newRow = updateItem.Label.ToString() + "," + updateItem.Type + "," + updateItem.IsExpired.ToString();
                changed = true;
                //notify
                responseMessage = "Item " + updateItem.Label.ToString()
                + " of type " + updateItem.Type + " is now expired!";
            }
            else
            {
                responseMessage = "cannot unexpire Item " + updateItem.Label.ToString()
                + " of type " + updateItem.Type ;
            }
            if (changed)
            {
                items.Remove(items.Where(x => x.Label == label).FirstOrDefault());
                items.Add(updateItem);
                txtData.Remove(oldRow);
                txtData.Add(newRow);
                File.WriteAllLines(dataPath, txtData);
            }
            System.Diagnostics.Debug.WriteLine(responseMessage);
           // log.Info(responseMessage);
            return (responseMessage);
        }

        // DELETE: api/Items/5
        public String DeleteItem(int label)
        {
            Item item = items.Where(x => x.Label == label).FirstOrDefault();
            items.Remove(item);
            string row = item.Label.ToString() + "," + item.Type + "," + item.IsExpired.ToString();
            //update txt storage
            txtData.Remove(row);
            File.WriteAllLines(dataPath, txtData);
            string responseMessage = "Item " + item.Label.ToString() 
                + " of type " + item.Type + " was removed from the inventory!";
            System.Diagnostics.Debug.WriteLine(responseMessage);
           // log.Info(responseMessage);
            return (responseMessage);
        }
    }
}
