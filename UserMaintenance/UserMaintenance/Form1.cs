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
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();
        public Form1()
        {
            InitializeComponent();
            labelFullName.Text = Resource1.Name;
            buttonAdd.Text = Resource1.Add;
            buttonFile.Text = Resource1.File;
            buttonDelete.Text = Resource1.Delete;

            listBoxNames.DataSource = users;
            listBoxNames.ValueMember = "ID";
            listBoxNames.DisplayMember = "FullName";
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            User u = new User();
            u.FullName = textBox1.Text;
            users.Add(u);
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() != DialogResult.OK) return;
            using (StreamWriter sw = new StreamWriter(sfd.FileName, true, Encoding.UTF8))
            {
                foreach (var u in users)
                {
                    sw.Write(u.ID);
                    sw.Write(';');
                    sw.Write(u.FullName);
                    sw.Write(';');
                    sw.WriteLine();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var currentUser = ((User)listBoxNames.SelectedItem);
            users.Remove(currentUser);
            listBoxNames.Refresh();
        }
    }
}
