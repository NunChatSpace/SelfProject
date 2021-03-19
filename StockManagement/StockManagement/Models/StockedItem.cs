using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManagement.Models
{
    public class StockedItem
    {
        public string Date { get; set; }
        public string Item { get; set; }
        public int Cost { get; set; }
        public int SaleCost { get; set; }
        public int Profit { get; set; }
        public int SendCost { get; set; }
        public int SendCostProfit { get; set; }
        public int PakagingCost { get; set; }
        public int ItemCount { get; set; }
        public int ItemOwnerID { get; set; }
        public StockedItem()
        { }

        public StockedItem(string Date, string Item, int Cost, int SaleCost, int Profit, int SendCost, int SendCostProfit, int PakagingCost, int ItemCount, int ItemOwnerID) {
            this.Date = Date;
            this.Item = Item;
            this.Cost = Cost;
            this.SaleCost = SaleCost;
            this.Profit = Profit;
            this.SendCost = SendCost;
            this.SendCostProfit = SendCostProfit;
            this.PakagingCost = PakagingCost;
            this.ItemCount = ItemCount;
            this.ItemOwnerID = ItemOwnerID;
        }
    }
}