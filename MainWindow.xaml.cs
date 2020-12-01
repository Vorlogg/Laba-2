using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Excel = Microsoft.Office.Interop.Excel;
using Path = System.IO.Path;
using System.Diagnostics;
using System.Timers;

namespace Laba_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public static int start = 3;
        public static int stop = 17;

        public static int max;
        public static bool complete=false;
        public static string oldName = "Старые Угрозы.xlsx";
        public static string downloadName = "Загруженные Угрозы.xlsx";
        public static string actualName = "Актуальные угрозы.xlsx";
        public static string tempName=actualName;
        public static bool newFile = false;
        public MainWindow()
        {
            InitializeComponent();
            Collum3.Visibility = Visibility.Collapsed;
            Collum4.Visibility = Visibility.Collapsed;
            Collum5.Visibility = Visibility.Collapsed;
            Collum6.Visibility = Visibility.Collapsed;
            Collum7.Visibility = Visibility.Collapsed;
            Collum8.Visibility = Visibility.Collapsed;
            if (!File.Exists(actualName))
            {
                MessageBoxResult result = MessageBox.Show("Файл базы данных не был найден.\nВыполнить загрузку файла?", "My App", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                switch (result)
                {
                    case MessageBoxResult.Yes: 
                        DownloadDangers();  
                        dangerGrid.ItemsSource = GetStr(start, stop,tempName);

                        break;
                }
            }
            else
            {
                dangerGrid.ItemsSource = GetStr(start, stop, tempName);

            }

            Timer timerUpdate = new Timer(20000);
            timerUpdate.Elapsed += Timeout;
            timerUpdate.Start();

        }
        private void Timeout(Object source, ElapsedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Пришло время обновлятся,Вы согласны с этим?", "My App", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes:DownloadDangers(); dangerGrid.ItemsSource = GetStr(start, stop, tempName); break;
                case MessageBoxResult.No: break;
                default: break;


            }
        }
        public static List<Danger> GetStr(int start, int stop,string name)
        {
            
            List<Danger> dangers = new List<Danger>();
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Open(Path.GetFullPath(name));
            Excel.Worksheet worksheet = workbook.Worksheets["Sheet"];
            Excel.Range range = worksheet.UsedRange;
            max = range.Rows.Count;

            int i = 0;
            if (complete)
            {
                for (int row = start; row <= stop; row++)
                {
                    i++;
                    dangers.Add(new Danger(range.Cells[row, 1].Text, range.Cells[row, 2].Text, range.Cells[row, 3].Text,
                        range.Cells[row, 4].Text, range.Cells[row, 5].Text, range.Cells[row, 6].Text, range.Cells[row, 7].Text,
                       range.Cells[row, 8].Text, range.Cells[row, 9].Text, range.Cells[row, 10].Text));
                }
            }
            else
            {
               

                for (int row = start; row <= stop; row++)
                {
                    i++;
                    dangers.Add(new Danger(range.Cells[row, 1].Text, range.Cells[row, 2].Text));
                }
            }

            workbook.Close(null, null, null);
            app.Quit();

            CloseProcess();


            return dangers;
        }

        public static void CloseProcess()
        {
            Process[] List;
            List = Process.GetProcessesByName("EXCEL");
            foreach (Process proc in List)
            {
                proc.Kill();

            }
        }
        public static int CoutRows(string name)
        {
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Open(Path.GetFullPath(name));
            Excel.Worksheet worksheet = workbook.Worksheets["Sheet"];
            Excel.Range range = worksheet.UsedRange;
            max = range.Rows.Count;
            return max;
        }
        public static void TimeDownload()
        {
           
        }
        

        public static void DownloadDangers()
        {
                
            try
            {
                new WebClient().DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", downloadName);


                if (File.Exists(actualName))
                {
                    if (File.Exists(oldName))
                    {
                        File.Delete(oldName);
                    }
                    File.Move(actualName, oldName);
                    File.Move(downloadName, actualName);
                    MessageBox.Show("Успешно обновлено");
                }
                else
                {
                    File.Move(downloadName, actualName);
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Не удалось загрузить файл");

            }
           

            
        

        }
        
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            complete = true;
            Collum3.Visibility = Visibility.Visible;
            Collum4.Visibility = Visibility.Visible;
            Collum5.Visibility = Visibility.Visible;
            Collum6.Visibility = Visibility.Visible;
            Collum7.Visibility = Visibility.Visible;
            Collum8.Visibility = Visibility.Visible;
            dangerGrid.ItemsSource = GetStr(start, stop, tempName);


        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Collum3.Visibility = Visibility.Collapsed;
            Collum4.Visibility = Visibility.Collapsed;
            Collum5.Visibility = Visibility.Collapsed;
            Collum6.Visibility = Visibility.Collapsed;
            Collum7.Visibility = Visibility.Collapsed;
            Collum8.Visibility = Visibility.Collapsed;

            complete = false;
            dangerGrid.ItemsSource = GetStr(start, stop,tempName);

        }
        private void CheckBox_Checked_Vs(object sender, RoutedEventArgs e)
        {
            int old = CoutRows(oldName);
            int act = CoutRows(actualName);
            MessageBox.Show($"Было строчек:{old} Стало строчек:{act}");
            tempName = oldName;
            dangerGrid.ItemsSource = GetStr(start, stop, tempName);


        }
        private void CheckBox_Unchecked_Vs(object sender, RoutedEventArgs e)
        {
           

            tempName = actualName;
            dangerGrid.ItemsSource = GetStr(start, stop,tempName);

        }


        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            DownloadDangers();
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {

            if (Path.GetFullPath(actualName) == null)
            {
                MessageBox.Show("Файл не найден");
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Таблицы (*.xlsx)|*.xlsx";

                if (saveFileDialog.ShowDialog() == true)
                {

                    File.Copy(Path.GetFullPath(actualName), saveFileDialog.FileName);
                }

            }    
        }

        private void Button_Click_Next(object sender, RoutedEventArgs e)
        {
            if (stop < max)
            {

                start += 15;
                stop += 15;

                dangerGrid.ItemsSource = GetStr(start, stop,tempName);
            }

        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            if (start == 3 && stop == 17)
            {
                dangerGrid.ItemsSource = GetStr(start, stop,tempName);

            }
            else
            {
                start -= 15;
                stop -= 15;

                dangerGrid.ItemsSource = GetStr(start, stop,tempName);

            }

        }

      




    }
}
