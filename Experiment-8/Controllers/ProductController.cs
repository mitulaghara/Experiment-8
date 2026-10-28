using Experiment_8.Models; // Adjust this if your models folder name is different
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Experiment_8.Controllers
{
    public class ProductController : Controller
    {
        string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Inventorymanagement;Integrated Security=True";

        [HttpGet]
        public ActionResult Index()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Products ORDER BY ProductId";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    da.Fill(dt);
                }
            }

            return View(dt);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"INSERT INTO Products
                    (ProductName, Quantity, Price)
                    VALUES (@Name, @Quantity, @Price)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100)
                            .Value = product.ProductName;

                        cmd.Parameters.Add("@Quantity", SqlDbType.Int)
                            .Value = product.Quantity;

                        SqlParameter price =
                            cmd.Parameters.Add("@Price", SqlDbType.Decimal);

                        price.Precision = 10;
                        price.Scale = 2;
                        price.Value = product.Price;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Product product = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    "SELECT * FROM Products WHERE ProductId = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            product = new Product
                            {
                                ProductID = Convert.ToInt32(dr["ProductId"]),
                                ProductName = dr["ProductName"].ToString(),
                                Quantity = Convert.ToInt32(dr["Quantity"]),
                                Price = Convert.ToDecimal(dr["Price"])
                            };
                        }
                    }
                }
            }

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"UPDATE Products SET
                    ProductName = @Name,
                    Quantity = @Quantity,
                    Price = @Price
                    WHERE ProductId = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100)
                            .Value = product.ProductName;

                        cmd.Parameters.Add("@Quantity", SqlDbType.Int)
                            .Value = product.Quantity;

                        cmd.Parameters.Add("@Id", SqlDbType.Int)
                            .Value = product.ProductID;

                        SqlParameter price =
                            cmd.Parameters.Add("@Price", SqlDbType.Decimal);

                        price.Precision = 10;
                        price.Scale = 2;
                        price.Value = product.Price;

                        con.Open();

                        int rows = cmd.ExecuteNonQuery();

                        if (rows == 0)
                            return HttpNotFound();
                    }
                }

                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    "DELETE FROM Products WHERE ProductId = @Id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }
    }
}