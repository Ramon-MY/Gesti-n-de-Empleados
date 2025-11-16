using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace EmployeeManagementSystem
{
    public class SalaryData
    {
        public string EmployeeID { get; set; } // 0 
        public string Name { get; set; } // 1
        public string Gender {get; set; } // 2
        public string Contact {get; set; } // 3
        public string Position { get; set; } // 4
        public int Salary { get; set; } // 5


        SqlConnection connect =
            new SqlConnection(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=dbSistema;Data Source=DESKTOP-G74O3JF");


        public List<SalaryData> salaryEmployeeListData()
        {
            List<SalaryData> listData = new List<SalaryData>();

            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string selectData = "SELECT * FROM employees WHERE delete_date IS NULL AND status = 'Activo'";

                    using(SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            SalaryData salaryData = new SalaryData();
                            salaryData.EmployeeID = reader["employee_id"].ToString();
                            salaryData.Name = reader["full_name"].ToString();
                            salaryData.Gender = reader["gender"].ToString();
                            salaryData.Contact = reader["contact_number"].ToString();
                            salaryData.Position = reader["position"].ToString();
                            salaryData.Salary = (int)reader["salary"];

                            listData.Add(salaryData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error:{ex}", "Mensaje Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
            return listData;
        }
    }
}
