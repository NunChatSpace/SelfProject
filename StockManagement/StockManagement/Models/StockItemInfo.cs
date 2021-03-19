using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Models
{
    public class StockItemInfo
    {
      
        public string Item { get; set; }
        public int Cost { get; set; }
        public int SaleCost { get; set; }
        public int Profit { get; set; }
        public int SendCost { get; set; }
        public int SendCostProfit { get; set; }
        public int PakagingCost { get; set; }
        public int ItemCount { get; set; }
        public List<string> CustomerNames { get; set; }
        public List<string> images { get; set; }
        public StockItemInfo() {}
        public StockItemInfo(int ItemCount, string Item, int Cost, int SaleCost, int Profit, int SendCost, int SendCostProfit, int PakagingCost)
        {
            this.Item = Item;
            this.Cost = Cost;
            this.SaleCost = SaleCost;
            this.Profit = Profit;
            this.SendCost = SendCost;
            this.SendCostProfit = SendCostProfit;
            this.PakagingCost = PakagingCost;
            this.ItemCount = ItemCount;
        }

        public StockItemInfo(List<string> images, StockedItem stockedItem, List<string> CustomerNames)
        {
            this.CustomerNames = CustomerNames;
            this.images = images;
            Item = stockedItem.Item;
            ItemCount = stockedItem.ItemCount;
            Cost = stockedItem.Cost;
            SaleCost = stockedItem.SaleCost;
            Profit = stockedItem.Profit;
            SendCost = stockedItem.SendCost;
            SendCostProfit = stockedItem.SendCostProfit;
            PakagingCost = stockedItem.PakagingCost;
        }
    }
}