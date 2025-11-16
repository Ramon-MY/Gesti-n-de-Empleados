using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace EmployeeManagementSystem
{
    public partial class AddEmployee : UserControl
    {
        SqlConnection connect =
           new SqlConnection(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=dbSistema;Data Source=DESKTOP-G74O3JF");
        public AddEmployee()
        {
            InitializeComponent();

            
            displayEmployeeData();
        }
        public void displayEmployeeData()
        {
            EmployeeData employeeData = new EmployeeData();
            List<EmployeeData> listData = employeeData.employeeListData();

            dataGridView1.DataSource = listData;
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }
            displayEmployeeData();
        }
        private void addEmployee_addBtn_Click(object sender, EventArgs e)
        {
            if (addEmployee_id.Text == ""
                || addEmployee_fullName.Text == ""
                || addEmployee_gender.Text == ""
                || addEmployee_phoneNumber.Text == ""
                || addEmployee_position.Text == ""
                || addEmployee_status.Text == ""
                || addEmployee_picture.Image == null)
            {
                MessageBox.Show("Please fill all blank fields.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State == ConnectionState.Closed)
                {
                    try
                    {
                        connect.Open();
                        string checkEmId = "SELECT COUNT(*) FROM employees WHERE employee_id = @emID AND delete_date IS NULL";

                        using (SqlCommand checkEm = new SqlCommand(checkEmId, connect))
                        {
                            checkEm.Parameters.AddWithValue("@emID", addEmployee_id.Text.Trim());

                            int count = (int)checkEm.ExecuteScalar();

                            if (count >= 1)
                            {
                                MessageBox.Show(addEmployee_id.Text.Trim() + " is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime today = DateTime.Today;
                                string insertData = "INSERT INTO employees " +
                                    "(employee_id, full_name, gender, contact_number, position, image, salary, insert_date, status)" +
                                    "VALUES(@employeeID, @fullName, @gender, @contactNum, @position, @image, @salary, @insert_date, @status)";


                                string path = Path.Combine(@"C:\Users\milen\source\repos\EmployeeManagementSystem\EmployeeManagementSystem\Directory"
                                + addEmployee_id.Text.Trim() + ".jpg");

                                string directoryPath = Path.GetDirectoryName(path);

                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                File.Copy(addEmployee_picture.ImageLocation, path, true);

                                using (SqlCommand cmd = new SqlCommand(insertData, connect))
                                {
                                    cmd.Parameters.AddWithValue("@employeeID", addEmployee_id.Text.Trim());
                                    cmd.Parameters.AddWithValue("@fullName", addEmployee_fullName.Text.Trim());
                                    cmd.Parameters.AddWithValue("@gender", addEmployee_gender.Text.Trim());
                                    cmd.Parameters.AddWithValue("@contactNum", addEmployee_phoneNumber.Text.Trim());
                                    cmd.Parameters.AddWithValue("@position", addEmployee_position.Text.Trim());
                                    cmd.Parameters.AddWithValue("@image", path);
                                    cmd.Parameters.AddWithValue("@salary", 0);
                                    cmd.Parameters.AddWithValue("@insert_date", today);
                                    cmd.Parameters.AddWithValue("@status", addEmployee_status.Text.Trim());

                                    cmd.ExecuteNonQuery();

                                    displayEmployeeData();

                                    MessageBox.Show("Added successfully!",
                                        "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    clearFields();
                                }
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
            }
        }

        private void addEmployee_importBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png";
                string imagePath = "";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dialog.FileName;
                    addEmployee_picture.ImageLocation = imagePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:{ex}", "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                addEmployee_id.Text = row.Cells[1].Value.ToString();
                addEmployee_fullName.Text = row.Cells[2].Value.ToString();
                addEmployee_gender.Text = row.Cells[3].Value.ToString();
                addEmployee_phoneNumber.Text = row.Cells[4].Value.ToString();
                addEmployee_position.Text = row.Cells[5].Value.ToString();

                string imagePath = row.Cells[6].Value.ToString();

                if (imagePath != null)
                {
                    addEmployee_picture.Image = Image.FromFile(imagePath);
                }
                else
                {
                    addEmployee_picture.Image = null;
                }

                addEmployee_status.Text = row.Cells[8].Value.ToString();
            }
        }


        public void clearFields()
        {
            addEmployee_id.Text = "";
            addEmployee_fullName.Text = "";
            addEmployee_gender.SelectedIndex = -1;
            addEmployee_phoneNumber.Text = "";
            addEmployee_position.SelectedIndex = -1;
            addEmployee_status.SelectedIndex = -1;
            addEmployee_picture.Image = null;
        }
        private void addEmployee_updateBtn_Click(object sender, EventArgs e)
        {
            if (addEmployee_id.Text == ""
                || addEmployee_fullName.Text == ""
                || addEmployee_gender.Text == ""
                || addEmployee_phoneNumber.Text == ""
                || addEmployee_position.Text == ""
                || addEmployee_status.Text == ""
                || addEmployee_picture.Image == null)
            {
                MessageBox.Show("Please fill all blank fields.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Are you sure you want to UPDATE " +
                    "Employee ID: " + addEmployee_id.Text.Trim() + "?", "Confirmation Message",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        connect.Open();
                        DateTime today = DateTime.Today;
                        string updateData = "UPDATE employees SET full_name = @fullName, gender = @gender, contact_number = @contactNum"
                            + ", position = @position, update_date = @updateDate, status = @status " +
                        "WHERE employee_id = @employeeID";

                        using (SqlCommand cmd = new SqlCommand(updateData, connect))
                        {
                            cmd.Parameters.AddWithValue("@fullName", addEmployee_fullName.Text.Trim());
                            cmd.Parameters.AddWithValue("@gender", addEmployee_gender.Text.Trim());
                            cmd.Parameters.AddWithValue("@contactNum", addEmployee_phoneNumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@position", addEmployee_position.Text.Trim());
                            cmd.Parameters.AddWithValue("@updateDate", today);
                            cmd.Parameters.AddWithValue("@status", addEmployee_status.Text.Trim());
                            cmd.Parameters.AddWithValue("@employeeId", addEmployee_id.Text.Trim());

                            cmd.ExecuteNonQuery();

                            displayEmployeeData();

                            MessageBox.Show("Updated successfully!",
                                "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clearFields();
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
                else
                {
                    MessageBox.Show("Cancelled.", "Information Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void addEmployee_clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void addEmployee_deleteBtn_Click(object sender, EventArgs e)
        {
            if (addEmployee_id.Text == ""
                || addEmployee_fullName.Text == ""
                || addEmployee_gender.Text == ""
                || addEmployee_phoneNumber.Text == ""
                || addEmployee_position.Text == ""
                || addEmployee_status.Text == ""
                || addEmployee_picture.Image == null)
            {
                MessageBox.Show("Please fill all blank fields.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Are you sure you want to DELETE " +
                    "Employee ID: " + addEmployee_id.Text.Trim() + "?", "Confirmation Message",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        connect.Open();
                        DateTime today = DateTime.Today;
                        string updateData = "UPDATE employees SET delete_date = @deleteDate " + 
                        "WHERE employee_id = @employeeID";

                        using (SqlCommand cmd = new SqlCommand(updateData, connect))
                        {
                            cmd.Parameters.AddWithValue("@deleteDate", today);
                            cmd.Parameters.AddWithValue("@employeeId", addEmployee_id.Text.Trim());

                            cmd.ExecuteNonQuery();

                            displayEmployeeData();

                            MessageBox.Show("Updated successfully!",
                                "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clearFields();
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
                else
                {
                    MessageBox.Show("Cancelled.", "Information Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
