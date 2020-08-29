using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace AdoNetSqlStudy
{
    class Program
    {
        private static string connSql = ConfigurationManager.ConnectionStrings["connSql"].ConnectionString;
        static void Main(string[] args)
        {
            //SelectSql();
            Program pm = new Program();
            SelectReader();
            pm.NoUsingSelect();
            Console.ReadKey();

        }


        private static void SelectReader() {
            SqlDataReader sr = null;
            using (SqlConnection conn = new SqlConnection(connSql))
            {
                string selSql = "select * from student";
                SqlCommand sc = new SqlCommand(selSql, conn);
                conn.Open();
                //sc.ExecuteReader()，返回的是一个对象:SqlDataReader
                //实时读取，只进不出，在读取的时候，conn对象必须全程打开，不能关闭
                sr = sc.ExecuteReader();
                //必须在conn.Close()之前读取数据
                while (sr.Read())
                {
                    string code = sr["code"].ToString().Trim();
                    string name = sr["name"].ToString().Trim();
                    int age = int.Parse(sr["age"].ToString().Trim());
                    Console.WriteLine($"学号:{code},姓名:{name },年龄:{age}");
                }
                sr.Close();

                //if (sr != null)
                //{
                //    Console.WriteLine(sr.ToString());
                //}
                
                conn.Close();
            }
            //sr如果在这里读取不到，因为conn.Close()已经关闭
            //if (sr != null)
            //{
            //    Console.WriteLine(sr.ToString());
            //}
            //Console.ReadKey();


        }
        /// <summary>
        /// 没有使用USING时connection读取数据
        /// </summary>
        private void NoUsingSelect() {
            SqlDataReader sr1 = null;
            SqlConnection conn1 = new SqlConnection(connSql);
            string selSql = "select * from student";
            SqlCommand sc = new SqlCommand(selSql, conn1);
            conn1.Open();
            //sc.ExecuteReader()，返回的是一个对象:SqlDataReader
            //实时读取，只进不出，在读取的时候，conn对象必须全程打开，不能关闭
            sr1 = sc.ExecuteReader();
            //必须在conn.Close()之前读取数据
            while (sr1.Read())
            {
                string code = sr1["code"].ToString().Trim();
                string name = sr1["name"].ToString().Trim();
                int age = int.Parse(sr1["age"].ToString().Trim());
                Console.WriteLine($"学号:{code},姓名:{name },年龄:{age}");
            }
            sr1.Close();
            //Console.ReadKey();
            conn1.Close();
        }
        private static void SelectSql() {
            Object o = null;
            using (SqlConnection conn = new SqlConnection(connSql)) {
                string selSql = "select * from student";
                SqlCommand sc = new SqlCommand(selSql, conn);
                conn.Open();
                //ExecuteScalar也可以用来做增删改，主要用于查询
                o = sc.ExecuteScalar();
                conn.Close();
            }
            if (o!=null)
            {
                Console.WriteLine(o.ToString());
            }
            Console.ReadKey();
        }
    }
}
