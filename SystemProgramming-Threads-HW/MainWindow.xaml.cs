using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace SystemProgramming_Threads_HW
{
    

    public partial class MainWindow : Window
    {

        private Thread myThread;
        public MainWindow()
        {
            InitializeComponent();
            myThread = new Thread(MyThreadMethod);
        }

        private void MyThreadMethod()
        {

            try
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine($"{i}");
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("cerv");
            }

          
        }

        private void TextBoxFrom_TextChanged(object sender, TextChangedEventArgs e)
        {
          

        }

        public void CopyFile()
        {

            new Thread(() =>
            {
                string FromStr = TextBoxFrom.Text;
                string ToStr = TextBoxTo.Text;
                if (!File.Exists(FromStr))
                {
                   // MessageBox.Show("File Not Exists");
                    return;

                }

                using (FileStream fsRead = new FileStream(FromStr, FileMode.Open, FileAccess.Read))
                {
                   using(FileStream fsWrite = new FileStream(ToStr,FileMode.Open,FileAccess.Read))
                    {
                        var len = 50;
                        var fileSize = fsRead.Length;
                        byte[] buffer = new byte[fileSize];
                        do
                        {
                            len = fsRead.Read(buffer, 0, buffer.Length);
                            fsWrite.Write(buffer, 0, len);

                            Console.WriteLine(fileSize);
                            fileSize -= len;

                        } while (len != 0);
                    }
                }


            }).Start();
        }
        private Task PreocessData(List<string> list,IProgress<ProgressBarRepository> progress)
        {
            int index = 1;
            int totalProcess = list.Count;
            var progresRepo = new ProgressBarRepository();
            return Task.Run(() =>
            {
                for (int i = 0; i < totalProcess; i++)
                {
                    progresRepo.PercentComp = index++ * 100 / totalProcess;
                    progress.Report(progresRepo);
                    Thread.Sleep(10);

                }
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           List<string> list = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(i.ToString());
            }
            LableStatus.DataContext = "Working...";
            var progress = new Progress<ProgressBarRepository>();
            progress.ProgressChanged += (pb, report) =>

            {
                LableStatus.DataContext = ($"Processng...{0}",report.PercentComp);
                ProgressBar.Value = report.PercentComp;
                ProgressBar.UpdateLayout();

            };
            await PreocessData(list, progress);
            LableStatus.DataContext = "Done";

                CopyFile();


           
        }

        private async void ResumeBtn_Click(object sender, RoutedEventArgs e)
        {

            List<string> list = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(i.ToString());
            }
            LableStatus.DataContext = "Working...";
            var progress = new Progress<ProgressBarRepository>();
            progress.ProgressChanged += (pb, report) =>

            {
                LableStatus.DataContext = ($"Processng...{0}", report.PercentComp);
                ProgressBar.Value = report.PercentComp;
                ProgressBar.UpdateLayout();

            };
            await PreocessData(list, progress);
            LableStatus.DataContext = "Done";

            CopyFile();






        }

        private void AbortBtn_Click(object sender, RoutedEventArgs e)
        {


            Thread thread = new Thread(() =>
            {

                List<string> list = new List<string>();
                for (int i = 0; i < 1000; i++)
                {
                    Thread.Sleep(100);
                }


            });

            LableStatus.DataContext = "Working...";
            var progress = new Progress<ProgressBarRepository>();


            thread.Abort();




            LableStatus.DataContext = "Done";

            CopyFile();

        }

     

        private async void SuspendBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {

                List<string> list = new List<string>();
                for (int i = 0; i < 1000; i++)
                {
                    Thread.Sleep(100);
                }


            });

            LableStatus.DataContext = "Working...";
            var progress = new Progress<ProgressBarRepository>();


            thread.Suspend();




            LableStatus.DataContext = "Done";

            CopyFile();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            DialogResult result = folder.ShowDialog();
            TextBoxFrom.Text = folder.SelectedPath;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            FolderBrowserDialog folder = new FolderBrowserDialog();
            DialogResult result = folder.ShowDialog();
            TextBoxFrom.Text = folder.SelectedPath;
        }
    }
}
