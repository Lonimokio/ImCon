using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Configuration;
using WebPWrapper;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using ImageProcessor.Processors;
using ImageProcessor;
using static System.Resources.ResXFileRef;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.ComponentModel;
using System.Windows.Threading;

namespace ImageConverter
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>

    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            AboutB.Click += MenuClick;
            DBConversionB.Click += MenuClick;
            FConversionB.Click += MenuClick;
            SettingsB.Click += MenuClick;

            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = new TimeSpan(0, 0, 1);
        }

        #region Variables here
        int length;

        private string type = "webp";
        private string filename = "";
        string filenameS;
        string filenameT = "";
        private string Checker;
        private string separator;
        string separatorT;
        string TempS;
        private string Converted;
        string sourceFile;
        string odbc;
        public string sourcePath;
        public string pathString;
        string connectionstring;
        string Odbc, output;
        string pathString1;
        string filename1 = "";
        string sourcePath1;
        int lengthT;
        int Timer;
        int minvalue = 0;
        int maxvalue = 100;
        int startvalue = 0;
        private int Quality;
        private int count;
        private int Iteration = 0;
        private int counter = 0;
        private int Left1;
        public OdbcCommand command;
        public OdbcDataReader dataReader;
        public OdbcConnection cnn;
        public OdbcDataAdapter adapter;
        public List<string> Files = new List<string>();
        public List<string> FileN = new List<string>();
        public List<string> FileK = new List<string>();
        #endregion

        #region Components in here
        private readonly BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        DispatcherTimer timer1 = new DispatcherTimer();
        #endregion

        #region Methods here
        private void FillDataGrid()

        {

            string ConString = "Driver={Pervasive ODBC Unicode Interface}; ServerName=Localhost;DBQ=" + DBName.Text + "; TransportHint=TCP;";

            string CmdString = string.Empty;

            using (OdbcConnection con = new OdbcConnection(ConString))

            {

                CmdString = "SELECT * FROM " + TableName.Text;

                OdbcCommand cmd = new OdbcCommand(CmdString, con);

                OdbcDataAdapter sda = new OdbcDataAdapter(cmd);

                DataTable dt = new DataTable(DBName.Text);

                sda.Fill(dt);

                PreviewD.ItemsSource = dt.DefaultView;

            }
        }
        public void GetFileN(int i)
        {
            //getting filename by running trough until hitting \
            separatorT = Files[i];
            length = Files[i].Length;
            length--;
            lengthT = separatorT.Length;
            lengthT--;
            while (true)
            {
                try
                {
                    Checker = separatorT.Substring(lengthT, 1);
                }
                catch (Exception)
                {
                    break;
                }
                if (Checker == @"\")
                {
                    break;
                }
                filenameT += Checker;
                Checker = "";
                lengthT--;
            }
            //Settup for calculation.
            lengthT = filenameT.Length;
            lengthT--;
            //Inverting string. Because it comes out wrong way from procces above.
            for (int a = 0; a <= lengthT;)
            {
                Checker = filenameT.Substring(lengthT, 1);
                TempS += Checker;
                Checker = "";
                lengthT--;
            }
            filenameT = "";

            filename = TempS;
        }

        public void DProblem(Exception ex)
        {
            Debug.WriteLine("<<< catch : " + ex.ToString());
            using StreamWriter sw = File.AppendText(PathB.Text + @"Kuvat\Failed.Txt");
            sw.WriteLine("Error in Sql syntax in " + filename + " this should be manually fixed. Path to it should be: " + sourcePath + " Error is: " + ex);
            sw.Close();
        }
        #endregion

        //Conecting to database
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DBName.Text != "")
            {
                try
                {
                    //Filling data grid
                    FillDataGrid();
                    #region Proccesing lists
                    if (Files.Count > 0)
                    {
                        //Clearing lists if already filled
                        Files.Clear();
                        FileN.Clear();
                        FileK.Clear();
                    }
                    else
                    {

                    }
                    //Opening ODBC connection
                    connectionstring = "Driver={Pervasive ODBC Unicode Interface}; ServerName=Localhost;DBQ=" + DBName.Text + "; TransportHint=TCP;";
                    cnn = new OdbcConnection(connectionstring);
                    cnn.Open();
                    //Reading database and inserting values into listboxes
                    Odbc = "Select Tiedosto, TuoteNro, kuvateksti from " + TableName.Text + " ORDER BY TuoteNro";
                    command = new OdbcCommand(Odbc, cnn);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        output = dataReader.GetValue(0).ToString();
                        Files.Add(output);
                        output = dataReader.GetValue(1).ToString();
                        FileN.Add(output);
                        output = dataReader.GetValue(2).ToString();
                        FileK.Add(output);
                    }
                    CStatus.Text = "Connected to: " + DBName.Text;
                    cnn.Close();
                    #endregion
                }
                catch
                {
                    //catching error with above procces
                    System.Windows.Forms.MessageBox.Show("Invalid database or table name!", "Invalid input");
                }

            }
            else
            {
                //If input is empty
                _ = System.Windows.Forms.MessageBox.Show("Invalid input!", "Invalid input!");
            }
        }
        #region BackGroundworker do stuff
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            BackgroundWorker worker = sender as BackgroundWorker;

            //Looping trough to procces images
            for (int i = 1; i <= count; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    if (FileK.Count == i)
                    {
                        if (FileK[i] == "PikkuKuva")
                        {
                            continue;
                        }
                    }

                    //Changing path in a way to prevent fails and prevent sql injection. Double '
                    Files[i].Replace("'", "''");

                    if (SmallImages.IsChecked == true)
                    {
                        GetFileN(i);
                        length = Files[i].Length - filename.Length;


                        TempS = Files[i].Substring(0, length) + "Pikkukuva" + filename;
                        length = TempS.Length - 4;
                        TempS = TempS.Substring(0, length) + "Jpeg";

                        //Creating copy of image to use for small image.
                        string sourceFile = Files[i];
                        string destinationFile = TempS;
                        try
                        {
                            File.Copy(sourceFile, destinationFile, true);

                            // Converting small images into jpeg
                            var stream = new MemoryStream(File.ReadAllBytes(destinationFile));
                            System.Drawing.Image img = new Bitmap(stream);
                            img.Save(TempS, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        catch
                        {

                        }

                        filenameS = filename;
                        TempS = "";
                        sourceFile = "";
                        destinationFile = "";
                        filename = "";
                    }

                    //Checking if file already in webp
                    Checker = Files[i][^4..];
                    if (Checker != type)
                    {
                        //File convertion
                        separator = Files[i];
                        int index = separator.IndexOf(".");
                        if (index >= 0)
                        {
                            separator = separator[..index];
                        }
                        Converted = separator + "." + type;
                        string oldImagePath = Files[i];
                        string webpFilePath = Converted;

                        if (File.Exists(oldImagePath))
                        {
                            try
                            {
                                using FileStream webPFileStream = new(webpFilePath, FileMode.Create);
                                using ImageFactory imageFactory = new(preserveExifData: false);
                                _ = imageFactory.Load(oldImagePath)
                                         .Format(new WebPFormat())
                                         .Quality(Quality)
                                         .Save(webPFileStream);
                            }
                            catch (Exception)
                            {

                            }
                            try
                            {
                                if (Deletion.IsEnabled)
                                {
                                    File.Delete(Files[i]);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    else
                    {
                        Converted = Files[i];
                    }
                    //Creating folders and moving images
                    string folderName = PathB.Text + "Kuvat";
                    string pathString = System.IO.Path.Combine(folderName, "Tuote-numero-" + FileN[i]);
                    _ = System.IO.Directory.CreateDirectory(pathString);

                    separator = Converted;

                    length = separator.Length;
                    length--;

                    //getting filename by running trough until hitting \
                    GetFileN(i);

                    TempS = "";

                    //Updating filepaths
                    sourcePath = Converted;
                    sourceFile = System.IO.Path.Combine(sourcePath);
                    string destFile = System.IO.Path.Combine(pathString, filename);
                    //Catching possible errors
                    try
                    {
                        System.IO.File.Move(sourceFile, destFile);
                    }
                    catch (Exception)
                    {

                    }

                    if (SmallImages.IsChecked == true)
                    {
                        //Inserting small images to database
                        OdbcDataAdapter adapter = new();
                        string odbc = "INSERT INTO " + TableName.Text + " (TuoteNro, Tiedosto, Kuvateksti, Verkkokaupassa) VALUES('" + FileN[i] + "', '" + pathString + @"\" + filenameS + "', PikkuKuva, 0); ";
                        command = new OdbcCommand(odbc, cnn);
                        adapter.UpdateCommand = new OdbcCommand(odbc, cnn);
                        try
                        {
                            _ = adapter.UpdateCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("<<< catch : " + ex.ToString());
                        }
                    }
                    //Updating database
                    adapter = new();
                    odbc = "UPDATE " + TableName.Text + " SET Tiedosto = '" + pathString + @"\" + filename + "' Where Tiedosto = " + "'" + Files[i] + "'";
                    command = new OdbcCommand(odbc, cnn);
                    adapter.UpdateCommand = new OdbcCommand(odbc, cnn);
                    try
                    {
                        _ = adapter.UpdateCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            DProblem(ex);
                        }
                        catch
                        {
                            Thread.Sleep(1);
                            DProblem(ex);
                        }
                    }
                    command.Dispose();

                    filename = "";
                    counter++;
                    worker.ReportProgress(1 * 1);
                }
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //if (InputB.Items.Count > counter)
            //{
            //    InputB.SelectedIndex = counter;
            //    InputB2.SelectedIndex = counter;
            //}
            ////Making progress indicators
            //Left1 = count - counter;
            //Done.Text = counter.ToString();
            //Left.Text = Left1.ToString();

            ////Progress bar
            //ProgressB.Minimum = 0;
            //ProgressB.Maximum = count + 1;
            //if (ProgressB.Maximum > counter)
            //{
            //    ProgressB.Value = counter;
            //}
            ////Making log file to record everything done
            //using (StreamWriter sw = File.AppendText(PathB.Text + @"Kuvat\Log.Txt"))
            //{
            //    if (Iteration == 0)
            //    {
            //        sw.WriteLine(DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'"));
            //        sw.WriteLine("User who made the change: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            //        sw.WriteLine("");
            //        Iteration = 1;
            //    }
            //    sw.WriteLine("Moved " + filename + " from " + sourcePath + " to " + pathString);
            //    sw.Close();
            //}
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //counter++;
            //Done.Text = counter.ToString();
            //timer1.Stop();
            //if (e.Error != null)
            //{
            //    System.Windows.Forms.MessageBox.Show(e.Error.Message);
            //}
            //else if (e.Cancelled)
            //{
            //    _ = System.Windows.Forms.MessageBox.Show("Cancelled. still moved and converted " + counter + " images");
            //}
            //else
            //{
            //    _ = System.Windows.Forms.MessageBox.Show("Done. Moved and converted " + counter + " images");
            //}
            //ProgressB.Value = 0;
            //Left1 = 0;
            //counter = 0;
            //Iteration = 0;
            //cnn.Close();
            //timer1.Stop();

            ////Enabling some controls
            //convert.Enabled = true;
            //TypeB.Enabled = true;
            //QualityB.Enabled = true;
            //PathB.Enabled = true;
            //Connect.Enabled = true;
            ////Disabling Cancel
            //Cancel.Enabled = false;
        }
        #endregion

        #region Menu button logic 
        private void MenuClick(object sender, RoutedEventArgs e)
        {
            AboutB.Background = System.Windows.Media.Brushes.Black;
            DBConversionB.Background = System.Windows.Media.Brushes.Black;
            FConversionB.Background = System.Windows.Media.Brushes.Black;
            SettingsB.Background = System.Windows.Media.Brushes.Black;
            if ((sender as System.Windows.Controls.Button) == AboutB)
            {
                AboutB.Background = System.Windows.Media.Brushes.Gray;
                About.Visibility = Visibility.Visible;
                DBConvertion.Visibility = System.Windows.Visibility.Hidden;
                FConversion.Visibility = System.Windows.Visibility.Hidden;
                Settings.Visibility = System.Windows.Visibility.Hidden;
            }
            if ((sender as System.Windows.Controls.Button) == DBConversionB)
            {
                DBConversionB.Background = System.Windows.Media.Brushes.Gray;
                About.Visibility = Visibility.Hidden;
                DBConvertion.Visibility = System.Windows.Visibility.Visible;
                FConversion.Visibility = System.Windows.Visibility.Hidden;
                Settings.Visibility = System.Windows.Visibility.Hidden;
            }
            if ((sender as System.Windows.Controls.Button) == FConversionB)
            {
                FConversionB.Background = System.Windows.Media.Brushes.Gray;
                About.Visibility = Visibility.Hidden;
                DBConvertion.Visibility = System.Windows.Visibility.Hidden;
                FConversion.Visibility = System.Windows.Visibility.Visible;
                Settings.Visibility = System.Windows.Visibility.Hidden;
            }
            if ((sender as System.Windows.Controls.Button) == SettingsB)
            {
                SettingsB.Background = System.Windows.Media.Brushes.Gray;
                About.Visibility = Visibility.Hidden;
                DBConvertion.Visibility = System.Windows.Visibility.Hidden;
                FConversion.Visibility = System.Windows.Visibility.Hidden;
                Settings.Visibility = System.Windows.Visibility.Visible;
            }
        }
        #endregion

        #region Expander logic for DBconversion
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Convert.Visibility = Visibility.Hidden;
            CStatus.Visibility = System.Windows.Visibility.Hidden;
            DBCBox.Visibility = System.Windows.Visibility.Hidden;
            FormatBox.Visibility = System.Windows.Visibility.Hidden;
            SettingsDBB.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            Convert.Visibility = Visibility.Visible;
            CStatus.Visibility = System.Windows.Visibility.Visible;
            DBCBox.Visibility = System.Windows.Visibility.Visible;
            FormatBox.Visibility = System.Windows.Visibility.Visible;
            SettingsDBB.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

        #region Move checked and unchecked
        private void MoveI_Checked(object sender, RoutedEventArgs e)
        {
            SaveP.Visibility = System.Windows.Visibility.Visible;
            PathB.Visibility = System.Windows.Visibility.Visible;
        }

        private void MoveI_Unchecked(object sender, RoutedEventArgs e)
        {
            SaveP.Visibility = System.Windows.Visibility.Hidden;
            PathB.Visibility = System.Windows.Visibility.Hidden;
        }
        #endregion

        //When Convert is clicked
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Checking if the list Files has something inside
            if (Files.Count > 0)
            {
                System.Windows.MessageBox.Show("Test");
                //Getting amount of items.
                count = Files.Count - 1;
                //Starting progress
                DBMainGrid.Visibility = System.Windows.Visibility.Hidden;
                ConversionProces.Visibility = System.Windows.Visibility.Visible;
                timer1.Start();
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timer++;
            TimeElapsed.Content = "Time elapsed:&#xA;           " + Timer;
        }

        private void Deletion_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Deletion_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void SmallImages_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SmallImages_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void NUD
        private void NUDTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.Up)
            {
                NUDButtonUP.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                typeof(System.Windows.Controls.Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { true });
            }


            if (e.Key == Key.Down)
            {
                NUDButtonDown.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                typeof(System.Windows.Controls.Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { true });
            }
        }

        private void NUDTextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                typeof(System.Windows.Controls.Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { false });

            if (e.Key == Key.Down)
                typeof(System.Windows.Controls.Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { false });
        }

        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (QualityB.Text != "")
                if (!int.TryParse(QualityB.Text, out number)) QualityB.Text = startvalue.ToString();
            if (number > maxvalue) QualityB.Text = maxvalue.ToString();
            if (number < minvalue) QualityB.Text = minvalue.ToString();
            QualityB.SelectionStart = QualityB.Text.Length;

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Making windows to be displayed correctly
            this.SizeToContent = SizeToContent.WidthAndHeight;
            DBConvertion.Visibility = System.Windows.Visibility.Hidden;
            FConversion.Visibility = System.Windows.Visibility.Hidden;
            Settings.Visibility = System.Windows.Visibility.Hidden;
            ConversionProces.Visibility = Visibility.Hidden;

            //Getting default database
            try
            {
                DBName.Text = Environment.GetEnvironmentVariable("kanta");
                length = DBName.Items.Count;
                if (length >= 0)
                {
                    DBName.Items.Add(Environment.GetEnvironmentVariable("kanta"));
                }
                for (int i = 0; i <= length; i++)
                {
                    DBName.SelectedIndex = i;
                    if (DBName.Text != Environment.GetEnvironmentVariable("kanta"))
                    {
                        DBName.Items.Add(Environment.GetEnvironmentVariable("kanta"));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}