using EntertainMe.Infrastructure.Repositories;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace EntertainMeUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EntertainMeRepository EMRepository;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void mnuFileNew_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "EntertainMe file (*.db) | *.db";
            saveFileDialog.InitialDirectory = EntertainMe.Infrastructure.EMConstants.EntertainMePath;
            saveFileDialog.OverwritePrompt = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                string fullName = saveFileDialog.FileName;
                // Close current database
                if (EMRepository != null) EMRepository.Dispose();
                string name = Path.GetFileName(fullName);
                string path = Path.GetDirectoryName(fullName);
                // If there is a database already , delete it
                if (File.Exists(fullName))
                {
                    string oldFile = path + @"\" + name;
                    string oldLogFile = path +
                        @"\" +
                        name.Replace(Path.GetExtension(name), "") +
                        "-log" +
                        Path.GetExtension(name);
                    if (File.Exists(oldFile)) File.Delete(oldFile);
                    if (File.Exists(oldLogFile)) File.Delete(oldLogFile);
                }

                EMRepository = new EntertainMeRepository(path, name);

            }
        }

        private void mnuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Open existing EntertainMe dataabse");
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (EMRepository != null) EMRepository.Dispose();

            // If you want to cancel the closing event set e.Cancel = true
        }

        private void OnClosed(object sender, EventArgs e)
        {
        }
    }
}
