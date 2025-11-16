using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public class EmployeeData
    {
        

        public int ID { get; set; } // 0
        public string EmployeeID { get; set; } // 1 
        public string Name { get; set; } // 2
        public string Gender { get; set; } // 3
        public string Contact { get; set; } // 4
        public string Position { get; set; } // 5
        public string Image { get; set; }  // 6
        public int Salary { get; set; } // 7
        public string Status { get; set; } // 8



        SqlConnection connect =
            new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\milen\OneDrive\Documents\employee.mdf;Integrated Security=True;Connect Timeout=30");


        public List<EmployeeData> employeeListData()
        {
            List<EmployeeData> listData = new List<EmployeeData>();

            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string selectData = "SELECT * FROM employees WHERE delete_date IS NULL";

                    using(SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            EmployeeData ed = new EmployeeData();
                            ed.ID = (int)reader["id"];
                            ed.EmployeeID = reader["employee_id"].ToString();
                            ed.Name = reader["full_name"].ToString();
                            ed.Gender = reader["gender"].ToString();
                            ed.Contact = reader["contact_number"].ToString();
                            ed.Position = reader["position"].ToString();
                            ed.Image = reader["image"].ToString();
                            ed.Salary = (int)reader["salary"];
                            ed.Status = reader["status"].ToString();

                            listData.Add(ed);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error:{ex}", "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
            return listData;
        }

        public List<EmployeeData> salaryEmployeeListData()
        {
            List<EmployeeData> listData = new List<EmployeeData>();

            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string selectData = "SELECT * FROM employees WHERE delete_date IS NULL";

                    using(SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            EmployeeData employeeData = new EmployeeData();
                            employeeData.EmployeeID = reader["employee_id"].ToString();
                            employeeData.Name = reader["full_name"].ToString();
                            employeeData.Position = reader["position"].ToString();
                            employeeData.Salary = (int)reader["salary"];

                            listData.Add(employeeData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error:{ex}", "Error Message"
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
