using LibraryManagementSystem.DataAccessLayer;
using LibraryManagementSystem.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static LibraryManagementSystem.Models.Enums;

namespace LibraryManagementSystem.UI.Student_Forms
{
    public partial class frmStudent : WeifenLuo.WinFormsUI.DockContent
    {
        int _staffID;
        public frmStudent(int staffID)
        {
            InitializeComponent();
            _staffID = staffID;
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            cmbSession.DataSource = SessionsHelper.GetSessionsNameList();
            cmbDepartment.DataSource = DepartmentsHelper.GetDepartmentsNameList();
            cmbProgram.DataSource = ProgramsHelper.GetProgramsNameList();
            cmbGender.DataSource = Enum.GetValues(typeof(Gender));
            FillGrid();
        }

        private void FillGrid()
        {
            dgvStudentList.Rows.Clear();
            foreach (var student in StudentsHelper.GetActiveStudentsList())
            {
                int row = dgvStudentList.Rows.Add();
                dgvStudentList.Rows[row].Cells[0].Value = student.StudentID;
                dgvStudentList.Rows[row].Cells[1].Value = student.Name;
                dgvStudentList.Rows[row].Cells[2].Value = student.TCNO;
                dgvStudentList.Rows[row].Cells[3].Value = student.EnrollNo;
                dgvStudentList.Rows[row].Cells[4].Value = SessionsHelper.GetByNameFromID(student.SessionID);
                dgvStudentList.Rows[row].Cells[5].Value = DepartmentsHelper.GetByNameFromID(student.DepartmentID);
                dgvStudentList.Rows[row].Cells[6].Value = ProgramsHelper.GetByNameFromID(student.ProgramID);
                dgvStudentList.Rows[row].Cells[7].Value = student.RegisterDate.ToString("dd MMMM yyyy");
                dgvStudentList.Rows[row].Cells[8].Value = student.Address;
                dgvStudentList.Rows[row].Cells[9].Value = student.ContactNo;
                dgvStudentList.Rows[row].Cells[10].Value = StaffsHelper.GetByNameFromID(student.StaffID);
                row++;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ep.Clear();

            if (txtStudentName.Text.Trim().Length == 0)
            {
                ep.SetError(txtStudentName, "Student name cannot be empty!");
                txtStudentName.Focus();
                return;
            }
            if (txtTCNO.Text.Trim().Length == 0)
            {
                ep.SetError(txtTCNO, "TC NO cannot be empty!");
                txtTCNO.Focus();
                return;
            }
            if (txtEnrollNo.Text.Trim().Length == 0)
            {
                ep.SetError(txtEnrollNo, "Enroll No cannot be empty!");
                txtEnrollNo.Focus();
                return;
            }
            if (txtAddress.Text.Trim().Length == 0)
            {
                ep.SetError(txtAddress, "Address cannot be empty!");
                txtAddress.Focus();
                return;
            }
            if (txtContactNo.Text.Trim().Length == 0)
            {
                ep.SetError(txtContactNo, "Contact No cannot be empty!");
                txtContactNo.Focus();
                return;
            }

            if (txtTCNO.Text.Trim().Length != 11)
            {
                MessageBox.Show("TC No must be 11 digits long.", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtContactNo.Text.Trim().Length < 9 || txtContactNo.Text.Trim().Length > 14)
            {
                MessageBox.Show("Please enter a valid contact number!", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!StudentsHelper.HaveTCNO(txtTCNO.Text) && !StudentsHelper.HaveEnrollNo(txtEnrollNo.Text) && !StudentsHelper.HaveContactNo(txtEnrollNo.Text))
            {
                Students s = new Students();
                s.SessionID = SessionsHelper.GetByName(cmbSession.SelectedItem.ToString());
                s.DepartmentID = DepartmentsHelper.GetByName(cmbDepartment.SelectedItem.ToString());
                s.ProgramID = ProgramsHelper.GetByName(cmbProgram.SelectedItem.ToString());
                s.StaffID = _staffID;
                s.Name = txtStudentName.Text;
                s.TCNO = txtTCNO.Text;
                s.Status = 1;
                s.Gender = cmbGender.SelectedIndex;
                s.EnrollNo = txtEnrollNo.Text;
                s.Address = txtAddress.Text;
                s.ContactNo = txtContactNo.Text;
                s.RegisterDate = DateTime.Now;
                StudentsHelper.Add(s);

                MessageBox.Show("Student added successfully", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cmbSession.SelectedIndex = 0;
                cmbDepartment.SelectedIndex = 0;
                cmbProgram.SelectedIndex = 0;
                txtStudentName.Text = string.Empty;
                txtTCNO.Text = string.Empty;
                cmbGender.SelectedIndex = 0;
                txtEnrollNo.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtContactNo.Text = string.Empty;

                FillGrid();
            }
            else
            {
                MessageBox.Show("The entered TC, Enroll, or Contact number is already registered in the system.", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtTCNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtEnrollNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ClearForm()
        {
            cmbSession.SelectedIndex = 0;
            cmbDepartment.SelectedIndex = 0;
            cmbProgram.SelectedIndex = 0;
            txtStudentName.Text = string.Empty;
            txtTCNO.Text = string.Empty;
            cmbGender.SelectedIndex = 0;
            txtEnrollNo.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtContactNo.Text = string.Empty;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStudentList.Rows.Count > 0)
                {
                    int selectIndex = dgvStudentList.CurrentRow.Index;
                    var studentID = dgvStudentList.Rows[selectIndex].Cells[0].Value;

                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this student?", "Library Management System", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        var s = StudentsHelper.GetById(Convert.ToInt32(studentID));
                        s.Status = 0;
                        StudentsHelper.Update(s);

                        MessageBox.Show("Student deleted successfully", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        FillGrid();
                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("You did not select any item!", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EnableComponent()
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            dgvStudentList.Enabled = false;
            btnUpdate.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void DisableComponent()
        {
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            dgvStudentList.Enabled = true;
            btnUpdate.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStudentList.Rows.Count > 0)
            {
                if (dgvStudentList.SelectedRows.Count == 1)
                {
                    int selectIndex = dgvStudentList.CurrentRow.Index;
                    var studentID = dgvStudentList.Rows[selectIndex].Cells[0].Value;

                    var student = StudentsHelper.GetById(Convert.ToInt32(studentID));

                    cmbSession.SelectedItem = SessionsHelper.GetByNameFromID(student.SessionID);
                    cmbDepartment.SelectedItem = DepartmentsHelper.GetByNameFromID(student.DepartmentID);
                    cmbProgram.SelectedItem = ProgramsHelper.GetByNameFromID(student.ProgramID);
                    txtStudentName.Text = student.Name;
                    txtTCNO.Text = student.TCNO;
                    cmbGender.SelectedIndex = student.Gender;
                    txtEnrollNo.Text = student.EnrollNo;
                    txtAddress.Text = student.Address;
                    txtContactNo.Text = student.ContactNo;

                    EnableComponent();
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            DisableComponent();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();

            if (txtStudentName.Text.Trim().Length == 0)
            {
                ep.SetError(txtStudentName, "Student name cannot be empty!");
                txtStudentName.Focus();
                return;
            }
            if (txtTCNO.Text.Trim().Length == 0)
            {
                ep.SetError(txtTCNO, "TC NO cannot be empty!");
                txtTCNO.Focus();
                return;
            }
            if (txtEnrollNo.Text.Trim().Length == 0)
            {
                ep.SetError(txtEnrollNo, "Enroll No cannot be empty!");
                txtEnrollNo.Focus();
                return;
            }
            if (txtAddress.Text.Trim().Length == 0)
            {
                ep.SetError(txtAddress, "Address cannot be empty!");
                txtAddress.Focus();
                return;
            }
            if (txtContactNo.Text.Trim().Length == 0)
            {
                ep.SetError(txtContactNo, "Contact No cannot be empty!");
                txtContactNo.Focus();
                return;
            }

            if (txtTCNO.Text.Trim().Length != 11)
            {
                MessageBox.Show("TC No must be 11 digits long.", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtContactNo.Text.Trim().Length < 9 || txtContactNo.Text.Trim().Length > 14)
            {
                MessageBox.Show("Please enter a valid contact number!", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var student = StudentsHelper.GetById(Convert.ToInt32(dgvStudentList.SelectedRows[0].Cells[0].Value));
            student.SessionID = SessionsHelper.GetByName(cmbSession.SelectedItem.ToString());
            student.DepartmentID = DepartmentsHelper.GetByName(cmbDepartment.SelectedItem.ToString());
            student.ProgramID = ProgramsHelper.GetByName(cmbProgram.SelectedItem.ToString());
            student.Name = txtStudentName.Text;
            student.TCNO = txtTCNO.Text;
            student.Gender = cmbGender.SelectedIndex;
            student.EnrollNo = txtEnrollNo.Text;
            student.Address = txtAddress.Text;
            student.ContactNo = txtContactNo.Text;

            StudentsHelper.Update(student);

            MessageBox.Show("Student updated successfully", "Library Management System", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearForm();
            DisableComponent();
            FillGrid();
        }
    }
}
