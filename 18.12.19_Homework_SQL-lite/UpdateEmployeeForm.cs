using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _18._12._19_Homework_SQL_lite_ClassLibrary;

namespace _18._12._19_Homework_SQL_lite
{    
    public partial class UpdateEmployeeForm : Form
    {
        private Employee _employee;

        public UpdateEmployeeForm(Employee emp)
        {

            _employee = emp;
            InitializeComponent();
            InitializeControls();
        }
        private void InitializeControls()
        {
            this.FormClosed += (object semder, FormClosedEventArgs e) => { this.Dispose(); };

            initializeEmployee();

        }
        private void initializeEmployee()
        {
            this.lblID.Text = "ID: " + _employee.ID; 
            this.txtName.Text = _employee.NAME;
            this.txtAddress.Text = _employee.ADDRESS;
            this.numAge.Value = _employee.AGE;
            this.numSalary.Value = (decimal)_employee.SALARY;
        }

        private void btnUpdateEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                
                CompanyDAO.SetData<Employee>(new Employee(_employee.ID, this.txtName.Text, this.txtAddress.Text, (double)this.numSalary.Value, (int)this.numAge.Value), DAOAcsessModifier.UpdateEmployee);
                this.BackgroundImage = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.MAX_WIDTH_FACTOR = Screen.PrimaryScreen.WorkingArea.Width;
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
            /*
            InitializeEmployeeDefaults();
            InitializeCombo();
            GetListOfEmployees();
            */
        }
    }
}
