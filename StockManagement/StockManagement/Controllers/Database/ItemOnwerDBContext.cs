using MySql.Data.MySqlClient;
using StockManagement.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers.Database
{
    public class ItemOnwerDBContext : Controller
    {
        string dbTable = "item_owner_test";
        string cs = @"server=localhost;userid=root;database=stockmanagement";

        public List<string> getCustomerNames(int id) 
        {
            List<string> customers = new List<string>();
            using (var con = new MySqlConnection(cs))
            {
                con.Open();
                var cmd = new MySqlCommand($"Select customer_name from {dbTable} WHERE item_id = {id}", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    customers.Add(rdr.GetString(0));
                }
            }

            return customers;
        }

        public string updateCustomerNames(int itemOwnerID, List<string> customers)
        {
            string result = "OK";
            string queryToDelete = $"DELETE FROM `stockeditem_test` WHERE `ItemOwnerID` ={itemOwnerID}";
            string queryToInsert = "INSERT INTO `item_owner_test`(`item_id`, `customer_name`) VALUES ({0},{1})";
            List<string> queriesToInsert = new List<string>();

            foreach (string customer in customers)
            {
                queriesToInsert.Add(string.Format(queryToInsert, itemOwnerID, customer));
            }

            using (var con = new MySqlConnection(cs))
            {
                try
                {
                    con.Open();
                    Logger.logWrite($"Query to DB : {queryToDelete}");
                    var cmd = new MySqlCommand(queryToDelete, con);
                    cmd.ExecuteNonQuery();
                    Logger.logWrite($"Query to DB : {queryToInsert}");
                    cmd = new MySqlCommand(queryToInsert, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    result = "ERROR";
                    Logger.logWrite($"Query to DB : {e.Message}");
                }
                
            }

            return result;
        }
    }
}