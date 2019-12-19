using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _18._12._19_Homework_SQL_lite_ClassLibrary;

namespace _18._12._19_Homework_SQL_lite
{
    public partial class MainForm : Form
    {
        private Label lblPnlOutInfo = new Label() { Location = new Point(3, 3), AutoSize = true };
        private string[] namesToUsing = null;
        private Random _rnd = new Random();
        private Form _currentUpdateEmployeeForm;
        public MainForm()
        {
            InitializeComponent();
            Initialize();
            InitializeCombo();
            ReadFromFile();
            InitializeEmployeeDefaults();
            GetListOfEmployees();
        }
        private void Initialize()
        {
            ScrollablePanel pnlEmployees = new ScrollablePanel();
            pnlEmployees.Width = pnlEmployeesOutline.Width-2;
            pnlEmployees.Height = pnlEmployeesOutline.Height-2;
            pnlEmployees.Location = pnlEmployeesOutline.Location;
            pnlEmployees.Controls.Add(lblPnlOutInfo);
            this.Controls.Add(pnlEmployees);
            pnlEmployees.Location = new Point(1, 1);
            pnlEmployeesOutline.Controls.Add(pnlEmployees);

            pnlEmployeesOutline.drawBorder(1, Color.Black);                       

        }
        private void ReadFromFile()
        {
            string names = string.Empty;
            try
            {
                names = File.ReadAllText("_names.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} \n\nSo the program will use the defult names");
                names = " Alfred Benny Connnor Daniel Eran ";
            }
            namesToUsing = names.Split(new char[] { ' ', '\t', '\n' }).Where(x => !String.IsNullOrEmpty(x)).ToArray();
        }
        private void InitializeEmployeeDefaults()
        {
            txtName.Text = namesToUsing[_rnd.Next(0, namesToUsing.Length - 1)];
            txtAddress.Text = Statics.GetUniqueKeyOriginal_BIASED(10);
            numAge.Value = _rnd.Next((int)numAge.Minimum, (int)numAge.Maximum);
            numSalary.Value = _rnd.Next((int)numSalary.Minimum, (int)numSalary.Maximum);
        }
        private void InitializeCombo()
        {
            lblPnlOutInfo.Text = string.Empty;
            cmbEmployees.Items.Clear();
            try
            {
                CompanyDAO.GetData<List<Employee>>(DAOAcsessModifier.GetAllEmployees).ForEach(x => cmbEmployees.Items.Add(new ComboItem<Employee>(x)));
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.MAX_WIDTH_FACTOR = Screen.PrimaryScreen.WorkingArea.Width;
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
        }

        private void GetListOfEmployees()
        {
            InitializeCombo();
            lblPnlOutInfo.Text = string.Empty;
            try
            {
                CompanyDAO.GetData<List<Employee>>(DAOAcsessModifier.GetAllEmployees).ForEach(x => lblPnlOutInfo.Text += x);
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.MAX_WIDTH_FACTOR = Screen.PrimaryScreen.WorkingArea.Width;
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }            

        }
        private void btnGetListOfEmployees_Click(object sender, EventArgs e)
        {
            GetListOfEmployees();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            lblPnlOutInfo.Text = string.Empty;

                Employee emp = new Employee
                {
                    NAME = txtName.Text,
                    ADDRESS = txtAddress.Text,
                    AGE = (int)numAge.Value,
                    SALARY = (int)numSalary.Value
                };
            try
            {
                CompanyDAO.SetData<Employee>(emp, DAOAcsessModifier.InsertEmployee);
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.MAX_WIDTH_FACTOR = Screen.PrimaryScreen.WorkingArea.Width;
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
            InitializeEmployeeDefaults();
            InitializeCombo();
            GetListOfEmployees();            
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyDAO.SetData<Employee>((cmbEmployees.SelectedItem as ComboItem<Employee>).Value, DAOAcsessModifier.DeleteEmployee);
                if (CompanyDAO.IsInDataBase<Employee>((cmbEmployees.SelectedItem as ComboItem<Employee>).Value)) throw new Exception($"Something went wrong, {(cmbEmployees.SelectedItem as ComboItem<Employee>).Value} didn't deleted");
                else FlexibleMessageBox.Show($"{(cmbEmployees.SelectedItem as ComboItem<Employee>).Value} deleted sucsessfully");
            }
            catch(NullReferenceException nrex)
            {
                FlexibleMessageBox.Show($"Please choose from the ComboBox above\n\n{nrex.Message}");
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.MAX_WIDTH_FACTOR = Screen.PrimaryScreen.WorkingArea.Width;
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
            finally
            {
                InitializeCombo();
                GetListOfEmployees();
                
            }
        }

        private void cmbEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentUpdateEmployeeForm = new UpdateEmployeeForm(((sender as ComboBox).SelectedItem as ComboItem<Employee>).Value);
            _currentUpdateEmployeeForm.AutoSize = true;
            _currentUpdateEmployeeForm.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width - 20, Screen.PrimaryScreen.WorkingArea.Height - 20);
            _currentUpdateEmployeeForm.BackgroundImageChanged += (object sender2, EventArgs e2) => { GetListOfEmployees(); };
            _currentUpdateEmployeeForm.Show();
            
            
            
            //FlexibleMessageBox.MAX_WIDTH_FACTOR = Screen.PrimaryScreen.WorkingArea.Width;
            //FlexibleMessageBox.Show(((sender as ComboBox).SelectedItem as ComboItem<Employee>).Value.ToString());
            
        }
    }
}
