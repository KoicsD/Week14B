using System;
using System.Collections;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace DriverBrowser
{
    public partial class BrowserForm : Form
    {
        // TODO if driver is not ready it jumps to root -- awful behaviour
        // TODO how about adding a button to jump to root
        // TODO how about enabling user to insert path into pathBox
        // TODO refactor casting -- both DirectoryInfo and FileInfo extends FileSystemInfo

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
            ArrayList items = new ArrayList();
            if (selectedDirectory.Parent != null)
                items.Add("..");
            items.AddRange(selectedDirectory.GetDirectories());
            items.AddRange(selectedDirectory.GetFiles());
            this.items = items.ToArray();
            listBox.DataSource = this.items;
            pathBox.Text = selectedDirectory.FullName;
        }

        private void openDirectory()
        {
            if (listBox.SelectedItem is DirectoryInfo)
            {
                selectedDirectory = (DirectoryInfo)listBox.SelectedItem;
            }
            else if (listBox.SelectedItem.Equals(".."))
            {
                selectedDirectory = selectedDirectory.Parent;
            }
            try
            {
                refreshListBox();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access Dennied!");
            }
        }

        private bool TryDeleteFile(FileInfo file)
        {
            try
            {
                file.Delete();
                return true;
            }
            catch (IOException)
            {
                MessageBox.Show("The file cannot be deleted because it is either open or referenced by an open handle.");
            }
            catch (SecurityException)
            {
                MessageBox.Show("Access dennied!");
            }
            return false;
        }

        private bool TryDeleteDirectoryRecursively(DirectoryInfo dir)
        {
            try
            {
                dir.Delete(true);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("The directory cannot be deleted because it contains one or more read-only files.");
            }
            catch (IOException)
            {
                string message = "The directory cannot be deleted.\nPossible reasons:\n";
                message += "- The directory is the current application's working directory.\n";
                message += "- The directory is open or referenced by an open handle.\n";
                MessageBox.Show(message, "capture");
            }
            catch (SecurityException)
            {
                MessageBox.Show("Access dennied!");
            }
            return false;
        }

        private bool TryDeleteDirectory(DirectoryInfo dir)
        {
            try
            {
                dir.Delete();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("The directory cannot be deleted because it contains one or more read-only files.");
            }
            catch (IOException)
            {
                string message = "The directory cannot be deleted.\nPossible reasons:\n";
                message += "- The directory is not empty. If this case, you can delete it in the recursive way.\n";
                message += "- The directory is the current application's working directory.\n";
                message += "- The directory is open or referenced by an open handle.\n";
                message += "\n";
                message += "Would you like to retry it recursively?";
                DialogResult result = MessageBox.Show(message, "capture", MessageBoxButtons.RetryCancel);
                if (result.Equals(DialogResult.Retry))
                    return TryDeleteDirectoryRecursively(dir);
            }
            catch (SecurityException)
            {
                MessageBox.Show("Access dennied!");
            }
            return false;
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

        private void openButton_Click(object sender, EventArgs e)
        {
            openDirectory();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem is DirectoryInfo)
            {
                DirectoryInfo dir = (DirectoryInfo)listBox.SelectedItem;
                DialogResult result = MessageBox.Show("Do you surely want to delete this directory?", "", MessageBoxButtons.YesNo);
                if (result.Equals(DialogResult.Yes))
                    TryDeleteDirectory(dir);
            }
            if (listBox.SelectedItem is FileInfo)
            {
                FileInfo file = (FileInfo)listBox.SelectedItem;
                DialogResult result = MessageBox.Show("Do you surely want to delete this directory?", "", MessageBoxButtons.YesNo);
                if (result.Equals(DialogResult.Yes))
                    TryDeleteFile(file);
            }
        }

        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            if (openButton.Enabled)
                openDirectory();
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && openButton.Enabled)
                openDirectory();
        }
    }
}
