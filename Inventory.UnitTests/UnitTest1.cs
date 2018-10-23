using System;
using Inventory.Models;
using Inventory.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inventory.UnitTests
{
    [TestClass]
    public class inventoryTests

    {
        [TestMethod]
        public void GetAllItems_ReturnallItems()
        {
           var controller = new ItemsController();
           var items= controller.GetItems();
            Assert.IsNotNull(items);
        }
        [TestMethod]
        public void GetItem_ShouldReturnCorrectItem()
        {
            var controller = new ItemsController();
            

            var result = controller.GetItem(1);
            Assert.IsNotNull(result);
            


        }

        [TestMethod]
        public void PostGoodItem_ShouldReturnConfirmation()
        {
            var controller = new ItemsController();
            Item item = new Item();
            item.Type = "Bannana";

            string result = controller.PostItem(item);
            Assert.IsNotNull(result);
          
            Assert.IsTrue(result.Contains(" of type " + item.Type + " has been added to the inventory"));

        }

        [TestMethod]
        public void PostBadItem_ShouldReturnError()
        {
            var controller = new ItemsController();
            Item item = new Item();

            string result = controller.PostItem(item);
            Assert.IsNotNull(result);

            Assert.IsTrue(result.Contains("Please specify the item's type "));

        }
      


    }
}
