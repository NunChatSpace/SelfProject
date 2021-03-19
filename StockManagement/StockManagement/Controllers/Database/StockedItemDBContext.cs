using MySql.Data.MySqlClient;
using StockManagement.Models;
using StockManagement.Services.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers.Database
{
    public class StockedItemDBContext: Controller
    {
        JsonResult data = new JsonResult();
        string dbTable = "stockeditem_test";
        public void downloadAllData()
        {
            this.data = getAll();
        }
        public void releaseData()
        {
            this.data = new JsonResult();
        }

        public JsonResult getAll()
        {
            string cs = @"server=localhost;userid=root;database=stockmanagement";
            List<StockedItem> stockItems = new List<StockedItem>();
            using (var con = new MySqlConnection(cs))
            {
                con.Open();
                var cmd = new MySqlCommand($"Select Date, Item, Cost, SaleCost, Profit, SendCost, SendCostProfit, PakagingCost, itemCount, ItemOwnerID from {dbTable}", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    StockedItem stockedItem = new StockedItem(rdr.GetString(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9));
                    stockItems.Add(stockedItem);
                }
            }

            return Json(stockItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllInADay(string date)
        {
            string cs = @"server=localhost;userid=root;database=stockmanagement";
            List<StockedItem> stockItems = new List<StockedItem>();
            string query = $"Select Date, Item, Cost, SaleCost, Profit, SendCost, SendCostProfit, PakagingCost, itemCount, ItemOwnerID from {dbTable} WHERE `Date` Like \'%{date}%\'";
            Logger.logWrite($"Query to DB : {query}");
            using (var con = new MySqlConnection(cs))
            {
                con.Open();
                var cmd = new MySqlCommand(query, con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    StockedItem stockedItem = new StockedItem(rdr.GetString(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9));
                    stockItems.Add(stockedItem);
                }
            }
            Logger.logWrite($"Receive Size : {stockItems.Count}");
            return Json(stockItems, JsonRequestBehavior.AllowGet);
        }

        public string getDayCost(string day)
        {
            List<StockedItem> stockItems = (List<StockedItem>)data.Data;
            string result = "";
            double totalDayCost = 0;
            day = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            totalDayCost = stockItems.Where(items => items.Date.Contains(day))
                .Select(items => items.Cost)
                .Sum(value => Convert.ToInt32(value));
            totalDayCost += stockItems.Where(items => items.Date.Contains(day))
                .Select(items => items.SendCost)
                .Sum(value => Convert.ToInt32(value));
            totalDayCost += stockItems.Where(items => items.Date.Contains(day))
                .Select(items => items.PakagingCost)
                .Sum(value => Convert.ToInt32(value));
            result = totalDayCost.ToString();
            return result;
        }

        public string getDayIncome(string day)
        {
            List<StockedItem> stockItems = (List<StockedItem>)data.Data;
            string result = "";

            day = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            result = stockItems.Where(items => items.Date.Contains(day))
                .Select(items => items.SaleCost)
                .Sum(value => Convert.ToInt32(value)).ToString();


            return result;
        }

        public string getDayProfit(string day)
        {
            List<StockedItem> stockItems = (List<StockedItem>)data.Data;
            string result = "";

            day = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            int profitSale = stockItems.Where(items => items.Date.Contains(day))
                .Select(items => items.Profit)
                .Sum(value => Convert.ToInt32(value));
            int profitSend = stockItems.Where(items => items.Date.Contains(day))
                .Select(items => items.SendCostProfit)
                .Sum(value => Convert.ToInt32(value));
            int total = profitSale + profitSend;
            result = $"{total}";
            return result;
        }

        public string getMonthIncome(string day)
        {
            List<StockedItem> stockItems = (List<StockedItem>)data.Data;
            string result = "";
            string month = "";

            month = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM");
            result = stockItems.Where(items => items.Date.Contains(month))
                .Select(items => items.SaleCost)
                .Sum(value => Convert.ToInt32(value)).ToString();

            return result;
        }

        public string getMonthCost(string day)
        {
            List<StockedItem> stockItems = (List<StockedItem>)data.Data;
            string result = "";
            string month = "";
            double totalDayCost = 0;
            month = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM");
            totalDayCost = stockItems.Where(items => items.Date.Contains(month))
                .Select(items => items.Cost)
                .Sum(value => Convert.ToInt32(value));
            totalDayCost += stockItems.Where(items => items.Date.Contains(month))
                .Select(items => items.SendCost)
                .Sum(value => Convert.ToInt32(value));
            totalDayCost += stockItems.Where(items => items.Date.Contains(month))
                .Select(items => items.PakagingCost)
                .Sum(value => Convert.ToInt32(value));

            result = totalDayCost.ToString();
            return result;
        }

        public string getMonthProfit(string day)
        {
            List<StockedItem> stockItems = (List<StockedItem>)data.Data;
            string result = "";
            string month = "";

            month = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM");
            int profitSale = stockItems.Where(items => items.Date.Contains(month))
                .Select(items => items.Profit)
                .Sum(value => Convert.ToInt32(value));
            int profitSend = stockItems.Where(items => items.Date.Contains(month))
                .Select(items => items.SendCostProfit)
                .Sum(value => Convert.ToInt32(value));
            int total = profitSale + profitSend;
            result = $"{total}";

            return result;
        }

        public string getStockItemCount() {
            string result = "";
            List<StockedItem> stockItems = (List<StockedItem>)data.Data;

            result = stockItems.Select(items => items.ItemCount)
                .Sum(value => Convert.ToInt32(value)).ToString();

            return result;
        }

        //public string AddNew(string item, string cost, string salePrice, string profit, string sendCost, string sendCostProfit, string packagingCost, string CustomerName)
        public string AddNew(string item, string cost, string itemCount, string imagesSrc)
        {
            string cs = @"server=localhost;userid=root;database=stockmanagement";
            string result = "OK";
            using (var con = new MySqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string queryString = string.Format($"INSERT INTO `{dbTable}`(`Date`, `Item`, `Cost`, `itemCount`, `ImagesSource`) " +
                        "VALUES ('{0}', '{1}', '{2}', {3}, '{4}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("th-TH")), item, cost, itemCount, imagesSrc.Replace("\\", "\\\\"));
                    var cmd = new MySqlCommand(queryString, con);
                    cmd.ExecuteNonQuery();

                    Logger.logWrite($"Query to DB : {queryString}");
                }
                catch (Exception e)
                {
                    result = "ERROR";
                    Logger.logWrite($"Query to DB : {e.Message}");
                }
                
            }
            return result;
        }

        public StockedItem getItemInformation(string itemName)
        {
            string cs = @"server=localhost;userid=root;database=stockmanagement";
            StockedItem stockItem = new StockedItem();
            using (var con = new MySqlConnection(cs))
            {
                try
                {
                    string queryString = $"SELECT `itemCount`, `Cost`, `SaleCost`, `Profit`, `SendCost`, `SendCostProfit`, `PakagingCost`, `ItemOwnerID` FROM `{dbTable}` WHERE `Item` = '{itemName}'";
                    Logger.logWrite($"Query to DB : {queryString}");
                    con.Open();
                    var cmd = new MySqlCommand(queryString, con);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        stockItem.Item = itemName;
                        stockItem.ItemCount = rdr.GetInt32(0);
                        stockItem.Cost = rdr.GetInt32(1);
                        stockItem.SaleCost = rdr.GetInt32(2);
                        stockItem.Profit = rdr.GetInt32(3);
                        stockItem.SendCost = rdr.GetInt32(4);
                        stockItem.SendCostProfit = rdr.GetInt32(5);
                        stockItem.PakagingCost = rdr.GetInt32(6);
                        stockItem.ItemOwnerID = rdr.GetInt32(7);
                    }
                }
                catch (Exception e)
                {
                    Logger.logWrite($"Query to DB : {e.Message}");
                }
            }
            return stockItem;
        }

        public string updateItem(string name, int itemCount, int cost, int price, int profit, int sendCost, int sendCostProfit, int packagingCost)
        {
            string cs = @"server=localhost;userid=root;database=stockmanagement";
            string result = "OK";
            using (var con = new MySqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string queryString = $"UPDATE `stockeditem_test` SET `Item`={name},`Cost`={cost},`SaleCost`={price}," +
                        $"`Profit`={profit},`SendCost`={sendCost},`SendCostProfit`={sendCostProfit},`PakagingCost`={packagingCost},`itemCount`={itemCount} " +
                        $"WHERE `Item` = '{name}'";
                    var cmd = new MySqlCommand(queryString, con);
                    cmd.ExecuteNonQuery();

                    Logger.logWrite($"Query to DB : {queryString}");
                }
                catch (Exception e)
                {
                    result = "ERROR";
                    Logger.logWrite($"Query to DB : {e.Message}");
                }

            }
            return result;
        }

        public int getOwnerItemID(string itemName)
        {
            string cs = @"server=localhost;userid=root;database=stockmanagement";
            int ownerItemID = 0;
            using (var con = new MySqlConnection(cs))
            {
                con.Open(); 
                var cmd = new MySqlCommand($"SELECT ItemOwnerID  from {dbTable} WHERE Item = '{itemName}'", con);

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ownerItemID = rdr.GetInt32(0);
                }
            }
            return ownerItemID;
        }
    }
}