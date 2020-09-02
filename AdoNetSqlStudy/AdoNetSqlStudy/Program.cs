using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

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
            //SqlParameter("1000000");
            //SqlParameter();
            SqlRead();
            //1、参数定义
            SqlParameter pa1 = new SqlParameter();
            pa1.ParameterName = "@Code";//定义参数名
            pa1.SqlDbType = SqlDbType.VarChar;//定义参数数据类型
            pa1.Size = 50;//定义参数大小
            pa1.Value = "code";//参数值
            //2、参数定义，参数名、值
            SqlParameter pa2 = new SqlParameter("@Sname", "陈年");
            //3、参数定义，参数名、SqlDbType
            SqlParameter pa3 = new SqlParameter("@Sex", SqlDbType.Int);
            pa3.Size = 1;
            pa3.Value = 0;

            //4、参数定义，参数名、类型、大小
            SqlParameter pa4 = new SqlParameter("@age", SqlDbType.Int, 10);

            //5、参数定义，参数名、类型、大小、源列名(数据库表的字段列名)
            SqlParameter pa5 = new SqlParameter("@teacher", SqlDbType.VarChar, 10, "classteacher");

            using (SqlConnection conn = new SqlConnection(connSql))
            {
                conn.Open();
                /*调用存储过程*/
                SqlCommand sc = new SqlCommand("TxxGetName", conn);
                //声明sc的sql命令为存储过程
                sc.CommandType = CommandType.StoredProcedure;
                //1.添加参数方式
                sc.Parameters.Add(new SqlParameter("@Scode", "1000000"));
                //输出参数
                SqlParameter spName = new SqlParameter("@Sname", SqlDbType.NVarChar, 50);
                //声明getName参数为输出参数
                spName.Direction = ParameterDirection.Output;
                sc.Parameters.Add(spName);

                sc.ExecuteScalar();

                conn.Close();
                Console.WriteLine(spName.Value.ToString());

            }
            Console.ReadKey();
        }

        private static void SqlRead() {
            using (SqlConnection conn = new SqlConnection(connSql))
            {
                string sSql = "select code,name,sex from student";
                SqlCommand cmd = new SqlCommand(sSql,conn);
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);//使用CommandBehavior会关闭所依赖的连接对象

                //DataTable dt = new DataTable();
                //dt.Load(sdr);//将数据加载到dt对象中
                List<StudentModel> lstu = new List<StudentModel>();
                if (sdr.HasRows)//判断读取对象是否存在
                {
                    int indexId = sdr.GetOrdinal("code");//获取制定列名的列序号
                    int indexName = sdr.GetOrdinal("name");//获取制定列名的列序号
                    int indexSex = sdr.GetOrdinal("sex");//获取制定列名的列序号
                    int indexAddress = sdr.GetOrdinal("address");//获取制定列名的列序号
                    int indexAge = sdr.GetOrdinal("age");//获取制定列名的列序号
                    //string idName = sdr.GetString(0);

                    while (sdr.Read())
                    {
                        //string codes = sdr[indexId].ToString();
                        //string names = sdr[indexName].ToString();
                        //string sexs = sdr[indexSex].ToString();
                        //string addresss = sdr[indexAddress].ToString();
                        //string ages = sdr[indexAge].ToString();
                        //string names = sdr[idName].ToString();
                        StudentModel sm = new StudentModel();
                        sm.code = sdr.GetString(indexId);
                        sm.name = sdr.GetString(indexName);
                        sm.sex = sdr.GetInt32(indexSex);
                        sm.address = sdr.GetString(indexAddress);
                        sm.age = sdr.GetInt32(indexAge);
                        lstu.Add(sm);
                    }

                }
                sdr.Close();//如果有返回值必须的关闭

            }
        }
        private static void SqlParameter() {
            using (SqlConnection conn = new SqlConnection(connSql))
            {
                //string selSql = "select * from student where code=@code";
                //SqlCommand sc = new SqlCommand(selSql, conn);
                
                /*调用存储过程*/
                SqlCommand sc = new SqlCommand("StuName", conn);
                //声明sc的sql命令为存储过程
                sc.CommandType = CommandType.StoredProcedure;
                //1.添加参数方式
                //sc.Parameters.Add(new SqlParameter("@code", "1000000"));
                //SqlParameter spa = new SqlParameter("@Scode", para1);
                //sc.Parameters.Add(spa);
                //输出参数
                SqlParameter spName = new SqlParameter("@Tname",SqlDbType.VarChar,50);
                spName.Value = '陈';
                //声明getName参数为输出参数
                spName.Direction = ParameterDirection.InputOutput;
                sc.Parameters.Add(spName);
                conn.Open();
                sc.ExecuteScalar();
                //2.添加参数方式AddWithValue();单个参数优先使用
                //sc.Parameters.AddWithValue("@scode", "1000000");
                //3。多个参数数组形式
                //SqlParameter[] sps = {
                //    new SqlParameter("@code", "1000000")
                //};
                //sc.Parameters.AddRange(sps);
                //sc.ExecuteReader()，返回的是一个对象:SqlDataReader
                //实时读取，只进不出，在读取的时候，conn对象必须全程打开，不能关闭
                //sr = sc.ExecuteReader();
                ////必须在conn.Close()之前读取数据
                //while (sr.Read())
                //{
                //    string code = sr["code"].ToString().Trim();
                //    string name = sr["name"].ToString().Trim();
                //    int age = int.Parse(sr["age"].ToString().Trim());
                //    Console.WriteLine($"学号:{code},姓名:{name },年龄:{age}");
                //}
                //sr.Close();

                //if (sr != null)
                //{
                //    Console.WriteLine(sr.ToString());
                //}

                conn.Close();
                Console.WriteLine(spName.Value.ToString());
            }
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
