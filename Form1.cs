using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFA_PhoneBook_DC.Classes;

namespace WFA_PhoneBook_DC
{
    public partial class Form1 : Form
    {
        private List<Contact> contacts;
        public Form1()
        {
            InitializeComponent();
            //When we use split container in our form it changes the cursor type to VSplit
            //This method is written to reset the cursor type to default cursor for form controls.
            DefaultCursorSetting();
            contacts = new List<Contact>();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            gbContactInfo.Visible = false;

        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            gbContactInfo.Visible = true;
            HideShowButtons(true, btnDelete, btnSave);
            FillCategoryComboBox();

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Contact contact = new Contact
            {
                Name = txtName.Text?.Trim(),
                Surname = txtSurname.Text?.Trim(),
                PhoneNumber = mtxtPhone.Text?.Trim(),
                IsFemale = (rdFemale.Checked) ? true : false,
                IsFavorite = (chkFavorite.Checked) ? true : false
            };

            contacts.Add(contact);
            LoadToListBox();
            gbContactInfo.Visible = false;
        }
        private void lbContacts_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            gbContactInfo.Visible = true;
        }
        private void LoadToListBox()
        {
            lbContacts.Items.Clear();
            foreach (Contact item in contacts)
            {
                lbContacts.Items.Add($"{(contacts.IndexOf(item)+1).ToString()}: {item}");
            }
        }

        private void FillCategoryComboBox()
        {
            Category[] list = Enum.GetValues<Category>();
            cmbCategory.DataSource = null;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Value";
            cmbCategory.DataSource = list;
        }

        /// <summary>
        /// Resets the Form's cursor type.
        /// </summary>
        private void DefaultCursorSetting()
        {
            foreach (Control item in this.Controls)
            {
                item.Cursor = Cursors.Default;
            }
        }

        private void HideShowButtons(bool hide, params Button[] buttons)
        {
            foreach (Button item in buttons)
            {
                if (hide)
                    item.Visible = false;
                else
                    item.Visible = true;
            }
        }

    }
}
