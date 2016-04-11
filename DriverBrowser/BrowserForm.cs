using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace DriverBrowser
{
    public partial class BrowserForm : Form
    {
        DriveInfo[] drives;
        DirectoryInfo selectedDirectory;
        object[] items;
        int selectedDriveIndex;
        int selectedItemIndex;

        static void CopyArray(object[] src, out object[] dest)
        {
            dest = new object[src.Length];
            for (int i = 0; i < src.Length; ++i)
                dest[i] = src[i];
        }

        static void AppendArray(object[] src, object[] dest)
        {

            for (int i = 0; i < src.Length; ++i)
                dest[i] = src[i];
        }

        public BrowserForm()
        {
            InitializeComponent();
            drives = DriveInfo.GetDrives();
            driveBox.DataSource = drives;
            selectedDriveIndex = driveBox.SelectedIndex;
            driveBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void refreshListBox()
        {
            pathBox.Text = selectedDirectory.FullName;
            ArrayList items = new ArrayList();
            if (selectedDirectory.Parent != null)
                items.Add("..");
            items.AddRange(selectedDirectory.GetDirectories());
            items.AddRange(selectedDirectory.GetFiles());
            this.items = items.ToArray();
            listBox.DataSource = this.items;
        }

        private void driveBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DriveInfo selectedDrive = (DriveInfo)driveBox.SelectedItem;
            if (!selectedDrive.IsReady)
            {
                MessageBox.Show("Driver is not ready! Please choose another one!");
                driveBox.SelectedIndex = selectedDriveIndex;
                return;
            }
            selectedDirectory = selectedDrive.RootDirectory;
            refreshListBox();
            selectedDriveIndex = driveBox.SelectedIndex;
        }

        private void listBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (items.Length > 0)
            {
                if (listBox.SelectedItem.Equals(".."))
                    deleteButton.Enabled = false;
                else
                    deleteButton.Enabled = true;
                if (listBox.SelectedItem is FileInfo)
                    openButton.Enabled = false;
                else openButton.Enabled = true;
            }
            else
            {
                openButton.Enabled = false;
                deleteButton.Enabled = false;
            }
            selectedItemIndex = listBox.SelectedIndex;
        }

        private void openButton_Click(object sender, System.EventArgs e)
        {
            if (listBox.SelectedItem is DirectoryInfo)
            {
                selectedDirectory = (DirectoryInfo)listBox.SelectedItem;
            }
            else if (listBox.SelectedItem.Equals(".."))
            {
                selectedDirectory = selectedDirectory.Parent;
            }
            refreshListBox();
        }
    }
}
