using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace DriverBrowser
{
    public partial class BrowserForm : Form
    {
        DriveInfo[] drives;
        int selectedIndex;
        DirectoryInfo selectedDirectory;

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
            selectedIndex = driveBox.SelectedIndex;
            driveBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void refreshListBox()
        {
            pathBox.Text = selectedDirectory.FullName;
            ArrayList items = new ArrayList(selectedDirectory.GetDirectories());
            items.AddRange(selectedDirectory.GetFiles());
            listBox.DataSource = items.ToArray();
            if (items.Count > 0)
            {
                if (listBox.SelectedItem is DirectoryInfo)
                    openButton.Enabled = true;
                else
                    openButton.Enabled = false;
                deleteButton.Enabled = true;
            }
            else
            {
                openButton.Enabled = false;
                deleteButton.Enabled = false;
            }
        }

        private void driveBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DriveInfo selectedDrive = (DriveInfo)driveBox.SelectedItem;
            if (!selectedDrive.IsReady)
            {
                MessageBox.Show("Driver is not ready! Please choose another one!");
                driveBox.SelectedIndex = selectedIndex;
                return;
            }
            selectedDirectory = selectedDrive.RootDirectory;
            refreshListBox();
            selectedIndex = driveBox.SelectedIndex;
        }

        private void openButton_Click(object sender, System.EventArgs e)
        {
            selectedDirectory = (DirectoryInfo)listBox.SelectedItem;
            refreshListBox();
        }
    }
}
