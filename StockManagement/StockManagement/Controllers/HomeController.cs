using MySql.Data.MySqlClient;
using StockManagement.Controllers.Database;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            JsonResult jr = LoadData();
            return View(jr);
        }

        public JsonResult LoadData()
        {
            StockedItemDBContext stockedItemDBContext = new StockedItemDBContext();
            JsonResult jr = stockedItemDBContext.getAll();

            return jr;
        }

        public JsonResult LoadDataInADay(string day)
        {
            StockedItemDBContext stockedItemDBContext = new StockedItemDBContext();
            string selectedDate = convertEngDateToThDate(day);
            JsonResult jr = stockedItemDBContext.getAllInADay(selectedDate);

            return jr;
        }

        //public string SubmitData(string itemName, string cost, string salePrice, string profit, string sendCost, string sendCostProfit, string packagingCost, string customerName)
        public string SubmitData(string itemName, string cost, string itemCount, string[] imagesData)
        {
            StockedItemDBContext stockedItemDBContext = new StockedItemDBContext();
            //string jr = stockedItemDBContext.AddNew(itemName, cost, salePrice, profit, sendCost, sendCostProfit, packagingCost, customerName);
            string imagesSrc = Common.FileSystem.WRITE_IMAGE(itemName, imagesData);
            string jr = stockedItemDBContext.AddNew(itemName, cost, itemCount, imagesSrc);
            
            return jr;
        }

        public JsonResult Summary(string day)
        {
            StockedItemDBContext stockedItemDBContext = new StockedItemDBContext();
            List<string> summary = new List<string>();
            stockedItemDBContext.downloadAllData();

            summary.Add(stockedItemDBContext.getDayCost(day));
            summary.Add(stockedItemDBContext.getDayIncome(day));
            summary.Add(stockedItemDBContext.getDayProfit(day));
            summary.Add(stockedItemDBContext.getMonthCost(day));
            summary.Add(stockedItemDBContext.getMonthIncome(day));
            summary.Add(stockedItemDBContext.getMonthProfit(day));
            summary.Add(stockedItemDBContext.getStockItemCount());

            stockedItemDBContext.releaseData();
            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private string convertEngDateToThDate(string date)
        {
            string[] spDate = date.Split('-');
            spDate[0] = (Int32.Parse(spDate[0]) + 543).ToString();

            return $"{spDate[0]}-{spDate[1]}-{spDate[2]}";
        }

        public JsonResult GetItemInformation(string itemName)
        {
            JsonResult jr = new JsonResult();
            StockedItemDBContext stockedItemDBContext = new StockedItemDBContext();
            ItemOnwerDBContext itemOnwerDBContext = new ItemOnwerDBContext();
            StockedItem stockedItem = new StockedItem();
            StockItemInfo stockItemInfo = new StockItemInfo();
            List<string> images = LoadImage(itemName);
            stockedItem = stockedItemDBContext.getItemInformation(itemName);

            stockItemInfo = new StockItemInfo(images, stockedItem, itemOnwerDBContext.getCustomerNames(stockedItem.ItemOwnerID));

            return Json(stockItemInfo, JsonRequestBehavior.AllowGet);
        }

        public string UpdateData(string name, int itemCount, int cost, int price, int profit, int sendCost, int sendCostProfit, int packagingCost, List<string> customers)
        {
            StockedItemDBContext stockedItemDBContext = new StockedItemDBContext();
            ItemOnwerDBContext itemOnwerDBContext = new ItemOnwerDBContext();
            int itemOwnerID = 0;

            string jr = stockedItemDBContext.updateItem(name, itemCount, cost, price, profit, sendCost, sendCostProfit, packagingCost);
            itemOwnerID = stockedItemDBContext.getOwnerItemID(name);
            jr = itemOnwerDBContext.updateCustomerNames(itemOwnerID, customers);

            return jr;
        }

        private List<string> LoadImage(string itemName)
        {
            List<string> imageContents = new List<string>();

            imageContents = Common.FileSystem.READ_IMAGE(itemName);

            return imageContents;
        }
    }

}