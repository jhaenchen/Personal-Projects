using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

namespace MicAndNotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = false;
            bw.DoWork +=
    new DoWorkEventHandler(bw_DoWork);
        }
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private List<TimeNote> timesForNote = new List<TimeNote>();

        private class TimeNote
        {
            public TimeSpan occurance = new TimeSpan();
            public string note;

            public TimeNote(TimeSpan o, string n)
            {
                occurance = o;
                note = n;
            }

            public string serialize()
            {
                return occurance.ToString() + "," + note+",";
            }
        }

        private int currentLine = 0;
        private string lastTextFilled = "";
        private bool wasLastKeyEnter = false;
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && wasLastKeyEnter)
            {
                
                string toStore = Textbox.Text;
                if (lastTextFilled != "")
                {
                    toStore = Textbox.Text.Replace(lastTextFilled, String.Empty);
                }
                lastTextFilled = Textbox.Text;
                
                
                timesForNote.Add(new TimeNote(recordingTimer.Elapsed, toStore));
                wasLastKeyEnter = false;
                currentLine++;
            }
            else if (e.Key == Key.Enter)
            {
                wasLastKeyEnter = true;
            }
            else
            {
                wasLastKeyEnter = false;
            }
        }
        Stopwatch recordingTimer = new Stopwatch();
        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            recordingTimer.Start();
            mciSendString("record recsound", "", 0, 0);
            

            
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            MediaPlayer play = new MediaPlayer();

            play.Open(new Uri("c:\\result.wav"));
            play.Position = (TimeSpan) e.Argument;
            play.Play();
            
        }

        BackgroundWorker bw = new BackgroundWorker();
        private void PlayBackButton_Click(object sender, RoutedEventArgs e)
        {
            string textArchive = Textbox.Text;
            TimeSpan toStart = new TimeSpan(0);
            
            
            
            bw.RunWorkerAsync(toStart);
            


            
            //Textbox.Text = "";
            
            //for (int i = 0; i < timesForNote.Count; i++)
            //{
               
            //    TheLabel.Content = i.ToString();
            //    while (t.Elapsed < timesForNote[i])
            //    {
            //        Thread.Sleep(1);
            //    }
            //}

            ThreadPool.QueueUserWorkItem(o =>
            {
                bool shouldContinue = true;
                int result = 0;
                int counter = 0;
                Stopwatch t = new Stopwatch();
                
                t.Start();

                while(shouldContinue)
                {
                    result++;
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            if (t.Elapsed+toStart < timesForNote[counter].occurance) Textbox.Text = timesForNote[counter].note;
                            else
                            {

                                counter++;
                            }
                        }
                        catch (Exception)
                        {
                            
                            shouldContinue = false;
                            Textbox.Text = textArchive;
                            Textbox.ScrollToEnd();
                        }
                    
                        
                    }));
                    Thread.Sleep(1);
                }
            });
            

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            recordingTimer.Stop();
            mciSendString("save recsound c:\\result.wav", "", 0, 0);
            mciSendString("close recsound ", "", 0, 0);
            theSlider.Maximum = recordingTimer.Elapsed.Ticks;
        }

        

        private void FromCursor_Click(object sender, RoutedEventArgs e)
        {
            string textArchive = Textbox.Text;
            TimeSpan toStart = new TimeSpan();
            int toPlayIndex = Textbox.CaretIndex;
            int lineIndex = Textbox.GetLineIndexFromCharacterIndex(toPlayIndex);
            String text = Textbox.GetLineText(lineIndex);
            for (int i = 0; i < timesForNote.Count; i++)
            {
                if (timesForNote[i].note.Contains(text))
                {
                    if (i > 0)
                    {
                        toStart = timesForNote[i - 1].occurance;
                    }
                    else
                    {
                        toStart = new TimeSpan(0);
                    }

                }
            }

            

            bw.RunWorkerAsync(toStart);




            //Textbox.Text = "";

            //for (int i = 0; i < timesForNote.Count; i++)
            //{

            //    TheLabel.Content = i.ToString();
            //    while (t.Elapsed < timesForNote[i])
            //    {
            //        Thread.Sleep(1);
            //    }
            //}

            ThreadPool.QueueUserWorkItem(o =>
            {
                bool shouldContinue = true;
                int result = 0;
                int counter = 0;
                Stopwatch t = new Stopwatch();

                t.Start();

                while (shouldContinue)
                {
                    result++;
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            if (t.Elapsed + toStart < timesForNote[counter].occurance) Textbox.Text = timesForNote[counter].note;
                            else
                            {
                                counter++;
                            }
                        }
                        catch (Exception)
                        {

                            shouldContinue = false;
                            Textbox.Text = textArchive;
                            Textbox.ScrollToEnd();
                        }


                    }));
                    Thread.Sleep(1);
                }
            });
        }

        

        private void theSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var sliderValue = new TimeSpan((long) theSlider.Value);
            for (int i = 0; i < timesForNote.Count-1; i++)
            {
                int theFirst = timesForNote[i].occurance.CompareTo(sliderValue);
                int theSecond = timesForNote[i + 1].occurance.CompareTo(sliderValue);
                if (theFirst == -1 && theSecond == 1)
                {
                    Textbox.Text = timesForNote[i + 1].note;
                }
                
                else if (i == 0 && timesForNote[i].occurance.CompareTo(sliderValue) == 1)
                {
                    Textbox.Text = timesForNote[i].note;
                }
                else if (i == timesForNote.Count - 2 && timesForNote[i + 1].occurance.CompareTo(sliderValue) == -1)
                {
                    Textbox.Text = timesForNote[i + 1].note;
                }
            }
        }

        private void SaveToFile()
        {
           
            //File.WriteAllLines("c:\\toSave.txt",timesForNote);
        }
        
    }
}
