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
            pathBox.Text = selectedDirectory.FullName;
            ArrayList items = new ArrayList(selectedDirectory.GetDirectories());
            items.AddRange(selectedDirectory.GetFiles());
            listBox.DataSource = items.ToArray();
            selectedIndex = driveBox.SelectedIndex;
        }
    }
}
