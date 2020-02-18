using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProektDB
{
    public partial class Form1 : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\opilane\source\repos\ProektDB\AppData\Database1.mdf;Integrated Security=True");
        int ArduinoId = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                if (btnSave.Text == "Save")
                {
                    SqlCommand sqlCmd = new SqlCommand("ArduinoAddorEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@mode", "Add");
                    sqlCmd.Parameters.AddWithValue("@ArduinoId", 0);
                    sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Series", txtSeries.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Image", txtImage.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Saved Successfull");
                }
                else
                {
                    SqlCommand sqlCmd = new SqlCommand("ArduinoAddorEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@mode", "Edit");
                    sqlCmd.Parameters.AddWithValue("@ArduinoId", ArduinoId);
                    sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Series", txtSeries.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Image", txtImage.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Updated Successfull");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {
                sqlCon.Close();
            }
        }
        void FillDataGridView()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("ArduinoVieworSearch", sqlCon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.AddWithValue("@ArduinoName", txtSearch.Text.Trim());
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            dgvArduino.DataSource = dtbl;
            dgvArduino.Columns[0].Visible = false;
            sqlCon.Close();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();

                SqlCommand sqlCmd = new SqlCommand("ContactDeletion", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@ArduinoId", ArduinoId);
                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Successfull");
                Reset();
                FillDataGridView();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
        }
        void Reset()
        {
            txtName.Text = txtSeries.Text = txtDescription.Text= txtImage.Text = "";
            btnSave.Text = "Save";
            ArduinoId = 0;
            btnDelete.Enabled = false;
        }

        private void DgvArduino_DoubleClick(object sender, EventArgs e)
        {
            if (dgvArduino.CurrentRow.Index != -1)
            {
                ArduinoId = Convert.ToInt32(dgvArduino.CurrentRow.Cells[0].Value.ToString());
                txtName.Text = dgvArduino.CurrentRow.Cells[1].Value.ToString();
                txtSeries.Text = dgvArduino.CurrentRow.Cells[2].Value.ToString();
                txtDescription.Text = dgvArduino.CurrentRow.Cells[3].Value.ToString();
                txtImage.Text = dgvArduino.CurrentRow.Cells[4].Value.ToString();
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reset();
            FillDataGridView();
        }
    }
}
