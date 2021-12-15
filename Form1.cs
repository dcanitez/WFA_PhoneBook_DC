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
using WFA_PhoneBook_DC.Classes;

namespace WFA_PhoneBook_DC
{
    public partial class Form1 : Form
    {
        private List<Contact> contacts;
        private Contact contactSelected;
        private string path = Application.StartupPath + @"\list.txt";        
        public Form1()
        {
            InitializeComponent();
            //When we use split container in our form, it changes the cursor type to VSplit
            //This method is written to reset the cursor type to default cursor for form controls.
            DefaultCursorSetting();
            contacts = new List<Contact>();
            
        }

        private void SaveRecordsToFile()
        {
            string path = Application.StartupPath + @"\list.txt";
            string[] recordLines = new string[contacts.Count];
            foreach (Contact item in contacts)
            {
                recordLines[contacts.IndexOf(item)] =$"{item.Name}|{item.Surname}|{item.PhoneNumber}|{item.Category}|{item.IsFemale}|{item.IsFavorite}";
            }

            System.IO.File.WriteAllLines(path, recordLines);

        }

        private void LoadRecordsFromFile()
        {

            if (File.Exists(path))
            {
                string[] recordLines= System.IO.File.ReadAllLines(path);
                foreach (string item in recordLines)
                {
                    Contact contact = new Contact
                    {
                        Name = item.Split('|')[0],
                        Surname = item.Split('|')[1],
                        PhoneNumber = item.Split('|')[2],
                        Category = (Category)Enum.Parse(typeof(Category), item.Split('|')[3]),
                        IsFemale = Convert.ToBoolean(item.Split('|')[4]),
                        IsFavorite = Convert.ToBoolean(item.Split('|')[5]),
                    };

                    contacts.Add(contact);
                }

                LoadToListBox();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadRecordsFromFile();
            gbContactInfo.Visible = false;

        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearTextBoxes(txtName, txtSurname, mtxtPhone);
            rdMale.Checked = true;
            chkFavorite.Checked = false;
            HideShowButtons(false, btnAdd);
            HideShowButtons(true, btnDelete, btnSave);
            FillCategoryComboBox();
            cmbCategory.SelectedIndex = -1;
            gbContactInfo.Visible = true;

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Contact contact = new Contact
            {
                Name = txtName.Text?.Trim(),
                Surname = txtSurname.Text?.Trim(),
                PhoneNumber = mtxtPhone.Text?.Trim(),
                Category = (Category)cmbCategory.SelectedValue,
                IsFemale = (rdFemale.Checked) ? true : false,
                IsFavorite = (chkFavorite.Checked) ? true : false
            };

            contacts.Add(contact);            
            LoadToListBox();
            SaveRecordsToFile();
            ClearTextBoxes(txtName, txtSurname, mtxtPhone);
            gbContactInfo.Visible = false;

        }
        private void lbContacts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            contactSelected = contacts[lbContacts.SelectedIndex];
            txtName.Text = contactSelected.Name;
            txtSurname.Text = contactSelected.Surname;
            mtxtPhone.Text = contactSelected.PhoneNumber;
            cmbCategory.SelectedItem = contactSelected.Category;
            if (contactSelected.IsFemale)
                rdFemale.Checked = true;
            else
                rdMale.Checked = true;
            chkFavorite.Checked = contactSelected.IsFavorite;

            HideShowButtons(false, btnDelete, btnSave);
            HideShowButtons(true, btnAdd);
            gbContactInfo.Visible = true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            contactSelected.Name = txtName.Text?.Trim();
            contactSelected.Surname = txtSurname.Text?.Trim();
            contactSelected.PhoneNumber = mtxtPhone.Text?.Trim();
            contactSelected.Category = (Category)cmbCategory.SelectedValue;
            contactSelected.IsFemale = (rdFemale.Checked) ? true : false;
            contactSelected.IsFavorite = (chkFavorite.Checked) ? true : false;

            LoadToListBox();
            SaveRecordsToFile();
            ClearTextBoxes(txtName, txtSurname, mtxtPhone);
            gbContactInfo.Visible = false;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result= MessageBox.Show($"{contactSelected}\n Are you sure to remove above contact from your PhoneBook?","Delete Contact",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result==DialogResult.Yes)
            {
                contacts.Remove(contactSelected);
            }

            LoadToListBox();
            SaveRecordsToFile();

            ClearTextBoxes(txtName, txtSurname, mtxtPhone);
            rdMale.Checked = true;
            chkFavorite.Checked = false;
            gbContactInfo.Visible = false;
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lbContacts.Items.Clear();

            foreach (Contact item in contacts)
            {
                string show = $"{(contacts.IndexOf(item) + 1).ToString()}: {item}";
                if (show.Contains(txtSearch.Text))
                {
                    lbContacts.Items.Add(show);
                }
            }

        }

        private void btnAtoZ_Click(object sender, EventArgs e)
        {
            contacts= contacts.OrderBy(c => c.Name).ToList();
            LoadToListBox();
        }

        private void btnZtoA_Click(object sender, EventArgs e)
        {
            contacts=contacts.OrderByDescending(c => c.Name).ToList();
            LoadToListBox();
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
        private void ClearTextBoxes(params TextBoxBase[] txts)
        {
            foreach (TextBoxBase item in txts)
            {
                item.Text = string.Empty;
            }
            
        }
    }
}
