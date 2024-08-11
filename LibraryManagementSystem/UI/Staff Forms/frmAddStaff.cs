using LibraryManagementSystem.DataAccessLayer;
using LibraryManagementSystem.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LibraryManagementSystem.Models.Enums;

namespace LibraryManagementSystem.UI.Staff_Forms
{
    public partial class frmAddStaff : WeifenLuo.WinFormsUI.DockContent
    {
        public frmAddStaff()
        {
            InitializeComponent();
        }

        private void frmAddStaff_Load(object sender, EventArgs e)
        {
            cmbGender.DataSource = Enum.GetValues(typeof(Gender));
            cmbDesignation.DataSource = DesignationsHelper.GetDesignationsNameList();
            FillGrid();
            SetColumnsWidth();
        }

        private void SetColumnsWidth()
        {
            dgvStaffList.Columns[0].Width = 220;
            dgvStaffList.Columns[1].Width = 220;
            dgvStaffList.Columns[2].Width = 220;
            dgvStaffList.Columns[3].Width = 220;
            dgvStaffList.Columns[4].Width = 220;
            dgvStaffList.Columns[5].Width = 220;
            dgvStaffList.Columns[6].Width = 220;
        }

        private void FillGrid()
        {
            dgvStaffList.Rows.Clear();
            foreach (var staff in StaffsHelper.GetStaffsModelList())
            {
                int row = dgvStaffList.Rows.Add();
                dgvStaffList.Rows[row].Cells[0].Value = staff.StaffID;
                dgvStaffList.Rows[row].Cells[1].Value = staff.Designations.Name;
                dgvStaffList.Rows[row].Cells[2].Value = staff.Name;
                dgvStaffList.Rows[row].Cells[3].Value = staff.TCNO;
                dgvStaffList.Rows[row].Cells[4].Value = staff._Gender;
                dgvStaffList.Rows[row].Cells[5].Value = staff.Address;
                dgvStaffList.Rows[row].Cells[6].Value = staff.ContactNo;
                row++;
            }
        }

        private void txtTcNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            epName.Clear();
            bool check1 = StaffsHelper.ControlValidate(txtName, "Please enter the staff name.", epName);

            epTCNO.Clear();
            bool check2 = StaffsHelper.ControlValidate(txtTcNo, "Please enter the TC No.", epTCNO);

            epAddress.Clear();
            bool check3 = StaffsHelper.ControlValidate(txtAddress, "Please enter the address.", epAddress);

            epContactNo.Clear();
            bool check4 = StaffsHelper.ControlValidate(txtContactNo, "Please enter the contact number.", epContactNo);

            if (txtTcNo.Text.Trim().Length != 11)
            {
                MessageBox.Show("TC No must be 11 digits long.", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtContactNo.Text.Trim().Length < 9 || txtContactNo.Text.Trim().Length > 14)
            {
                MessageBox.Show("Please enter a valid phone number!", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (check1 && check2 && check3 && check4)
            {
                if (!StaffsHelper.HaveTCNO(txtTcNo.Text.Trim()) && !StaffsHelper.HaveContactNo(txtContactNo.Text))
                {
                    Staffs s = new Staffs
                    {
                        DesignationID = DesignationsHelper.GetByName(cmbDesignation.SelectedItem.ToString()),
                        Name = txtName.Text,
                        TCNO = txtTcNo.Text,
                        Gender = cmbGender.SelectedIndex,
                        Address = txtAddress.Text,
                        ContactNo = txtContactNo.Text,
                        Status = 1
                    };

                    StaffsHelper.Add(s);

                    MessageBox.Show("Staff addition successful.", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillGrid();
                }
                else
                {
                    MessageBox.Show("The entered TC No or phone number is already registered in the system.", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearForm()
        {
            cmbGender.SelectedIndex = 0;
            txtName.Text = string.Empty;
            txtTcNo.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtContactNo.Text = string.Empty;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStaffList.Rows.Count > 0)
                {
                    int selectIndex = dgvStaffList.CurrentRow.Index;
                    var staffID = dgvStaffList.Rows[selectIndex].Cells[0].Value;

                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this staff?", "Library Management System", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        var s = StaffsHelper.GetById(Convert.ToInt32(staffID));

                        s.Status = 0;

                        StaffsHelper.Update(s);

                        MessageBox.Show("Staff deletion successful.", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        FillGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No staff member selected!", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EnableComponent()
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            dgvStaffList.Enabled = false;
            btnUpdate.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void DisableComponent()
        {
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            dgvStaffList.Enabled = true;
            btnUpdate.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStaffList.Rows.Count > 0)
            {
                if (dgvStaffList.SelectedRows.Count == 1)
                {
                    int selectIndex = dgvStaffList.CurrentRow.Index;
                    var staffID = dgvStaffList.Rows[selectIndex].Cells[0].Value;

                    var s = StaffsHelper.GetById(Convert.ToInt32(staffID));
                    var d = DesignationsHelper.GetById(s.DesignationID);

                    cmbDesignation.SelectedItem = d.Name;
                    txtName.Text = s.Name;
                    txtTcNo.Text = s.TCNO;
                    cmbGender.SelectedIndex = s.Gender;
                    txtAddress.Text = s.Address;
                    txtContactNo.Text = s.ContactNo;

                    EnableComponent();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            DisableComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            epName.Clear();
            bool check1 = StaffsHelper.ControlValidate(txtName, "Please enter the staff name.", epName);

            epTCNO.Clear();
            bool check2 = StaffsHelper.ControlValidate(txtTcNo, "Please enter the TC No.", epTCNO);

            epAddress.Clear();
            bool check3 = StaffsHelper.ControlValidate(txtAddress, "Please enter the address.", epAddress);

            epContactNo.Clear();
            bool check4 = StaffsHelper.ControlValidate(txtContactNo, "Please enter the contact number.", epContactNo);

            if (check1 && check2 && check3 && check4)
            {
                int selectIndex = dgvStaffList.CurrentRow.Index;
                var staffID = dgvStaffList.Rows[selectIndex].Cells[0].Value;

                var s = StaffsHelper.GetById(Convert.ToInt32(staffID));

                s.DesignationID = DesignationsHelper.GetByName(cmbDesignation.SelectedItem.ToString());
                s.Name = txtName.Text;
                s.TCNO = txtTcNo.Text;
                s.Gender = cmbGender.SelectedIndex;
                s.Address = txtAddress.Text;
                s.ContactNo = txtContactNo.Text;

                StaffsHelper.Update(s);

                MessageBox.Show("Staff update successful.", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                FillGrid();
                DisableComponent();
            }
        }
    }
}
