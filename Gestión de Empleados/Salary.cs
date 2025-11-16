using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EmployeeManagementSystem
{
    public partial class Salary : UserControl
    {
        SqlConnection connect =
            new SqlConnection(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=dbSistema;Data Source=DESKTOP-G74O3JF");
        public Salary()
        {
            InitializeComponent();
            displayEmployee();
            disableFields();
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }

            displayEmployee();
            disableFields();
        }
        public void disableFields()
        {
            salary_employeeID.Enabled = false;
            salary_name.Enabled = false;
            salary_position.Enabled = false;
        }
        public void displayEmployee()
        {
            SalaryData salaryData = new SalaryData();
            List<SalaryData> listData = salaryData.salaryEmployeeListData();

            dataGridView1.DataSource = listData;
        }
        private void salary_updateBtn_Click(object sender, EventArgs e)
        {
            if (salary_employeeID.Text == ""
                || salary_name.Text == ""
                || salary_position.Text == ""
                || salary_salary.Text == "")
            {
                MessageBox.Show("Por favor complete todos los campos en blanco.", "Mensaje Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show($"¿Está seguro de que desea ACTUALIZAR la ID del empleado?:{salary_employeeID.Text.Trim()}?",
                    "Mensaje de confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        connect.Open();

                        DateTime today = DateTime.Today;

                        string updateData = "UPDATE employees SET salary = @salary," +
                            " update_date = @updateDate WHERE employee_id = @employeeID";

                        using (SqlCommand cmd = new SqlCommand(updateData, connect))
                        {
                            cmd.Parameters.AddWithValue("@salary", salary_salary.Text.Trim());
                            cmd.Parameters.AddWithValue("@updateDate", today);
                            cmd.Parameters.AddWithValue("@employeeID", salary_employeeID.Text.Trim());

                            cmd.ExecuteNonQuery();

                            displayEmployee();

                            MessageBox.Show("Actualizar exitosamente!", "Mensaje informativo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clearFields();
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
                else
                {
                    MessageBox.Show("Cancelar", "Mensaje informativo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public void clearFields()
        {
            salary_employeeID.Text = "";
            salary_name.Text = "";
            salary_position.Text = "";
            salary_salary.Text = "";
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                salary_employeeID.Text = row.Cells[0].Value.ToString();
                salary_name.Text = row.Cells[1].Value.ToString();
                salary_position.Text = row.Cells[4].Value.ToString();
                salary_salary.Text = row.Cells[5].Value.ToString();
            }
        }

        private void salary_clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }
    }
}
