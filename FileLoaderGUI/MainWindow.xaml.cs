using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.Linq;
using FileLoaderGUI.Extentions;

namespace FileLoaderGUI
{
    public partial class MainWindow : Window
    {
        private static DispatcherTimer timer;
        private static string directorypath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Files");
        private static int frequency = 5;
       
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(frequency);

            timer.Start();

            Main();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Task.Run(() => Main());
        }

        private async Task Main()
        {
            try
            {
                string[] files = Directory.GetFiles(directorypath);
                List<string> FilesToRead = FileComparison.CompareFiles(files);

                List<Tuple<string, string>> fileDataList = FileContent.GetContent(FilesToRead);

                await DllLoading.LoadDll(fileDataList, dataGrid);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error occurred! Error Message:" + ex.Message);
            }
        }

        private void OpenFile(object sender,RoutedEventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    directorypath = folderDialog.SelectedPath;
                    FileComparison.processedFiles.Clear();
                    dataGrid.Items.Clear();
                    Task.Run(() => Main());
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myComboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)myComboBox.SelectedItem;

                string? selectedValue = selectedItem.Content.ToString();
                frequency = int.Parse(selectedValue);

                if (timer != null)
                {
                    timer.Stop();
                    timer.Interval = TimeSpan.FromSeconds(frequency);
                    timer.Start();
                }
            }
        }
    }
}
