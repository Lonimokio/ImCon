using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
            #region Database convertion stuff
            #region Menu buttons
            AboutB.Click += MenuClick;
            DBConversionB.Click += MenuClick;
            FConversionB.Click += MenuClick;
            SettingsB.Click += MenuClick;
            #endregion

            #region Background worker stuff
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            #endregion

            #region Timer
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = new TimeSpan(0, 0, 1);
            #endregion

            #region Numeric up and down code
            scrollBar1.ValueChanged += new RoutedPropertyChangedEventHandler<double>(scrollBar1_ValueChanged);
            scrollBar1.Minimum = 0;
            scrollBar1.Maximum = 100;
            scrollBar1.SmallChange = 1;
            #endregion
            #endregion
        }

        #region Variables here
        #region Strings
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
        string path;
        string TableNameS;
        string FolderNameS;
        string FileNamesS;
        string destFile;
        #endregion
        #region Ints
        int length;
        int lengthT;
        int Seconds;
        int Minutes;
        int Hours;
        private int Quality = 60;
        private int Iteration = 0;
        private int Left1;
        #endregion
        #region Float
        private float counter = 0;
        private float count;
        #endregion
        #region Odbc 
        public OdbcCommand command;
        public OdbcDataReader dataReader;
        public OdbcConnection cnn;
        public OdbcDataAdapter adapter;
        #endregion
        #region Lists
        public List<string> Files = new();
        public List<string> FileN = new();
        public List<string> FileK = new();
        #endregion
        #region Bool
        bool SmallImagesBool = false;
        bool DeletionBool = false;
        bool MoveBool = false;
        bool UpdateDBBool = true;
        #endregion
        #endregion

        #region Components in here
        private readonly BackgroundWorker backgroundWorker1 = new();
        DispatcherTimer timer1 = new();
        #endregion

        #region Methods here
        private void FillDataGrid()

        {

            string ConString = "Driver={Pervasive ODBC Unicode Interface}; ServerName=Localhost;DBQ=" + DBName.Text + "; TransportHint=TCP;";

            string CmdString = string.Empty;

            using (OdbcConnection con = new(ConString))

            {

                CmdString = "SELECT * FROM " + TableNameS;

                OdbcCommand cmd = new(CmdString, con);

                OdbcDataAdapter sda = new(cmd);

                DataTable dt = new(DBName.Text);

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
            TempS = "";
        }

        public void DProblem(Exception ex)
        {
            Debug.WriteLine("<<< catch : " + ex.ToString());
            using StreamWriter sw = File.AppendText(path + FolderNameS + @"\Failed.Txt");
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
                    Odbc = "Select Tiedosto, TuoteNro, kuvateksti from " + TableNameS + " ORDER BY TuoteNro";
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
                    System.Windows.MessageBox.Show("Invalid database or table name!", "Invalid input");
                }

            }
            else
            {
                //If input is empty
                _ = System.Windows.MessageBox.Show("Invalid input!", "Invalid input!");
            }
        }

        #region BackGroundworker do stuff
        //Background worker works
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            #region Initialization
            //Initializing backgroundworker
            BackgroundWorker worker = backgroundWorker1;
            #endregion

            //Looping trough to procces images
            for (int i = 0; i <= count; i++)

                #region Checking if cancel has been clicked
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                #endregion
                else
                {
                    #region Checking if image is small image
                    if (FileK.Count <= i)
                    {
                        if (FileK[i] == "PikkuKuva")
                        {
                            continue;
                        }
                    }
                    #endregion

                    //Changing path in a way to prevent fails and prevent sql injection. Double '
                    Files[i] = Files[i].Replace("'", "''");
                    //checking if small images are true
                    #region Small images making procces
                    if (SmallImagesBool == true)
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
                    #endregion

                    #region File convertion
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
                        string NewFile = Converted;

                        if (File.Exists(oldImagePath))
                        {
                            try
                            {
                                using FileStream webPFileStream = new(NewFile, FileMode.Create);
                                using ImageFactory imageFactory = new(preserveExifData: false);
                                _ = imageFactory.Load(oldImagePath)
                                         .Format(new WebPFormat())
                                         .Quality(Quality)
                                         .Save(webPFileStream);
                                //Bitmap bmp = new Bitmap(oldImagePath);
                                //using (WebP webp = new WebP())
                                //    webp.Save(bmp, NewFile, 80);
                            }
                            catch (Exception)
                            {

                            }
                            //If deletion is enabled delete
                            #region Deletion
                            try
                            {
                                if (DeletionBool == true)
                                {
                                    File.Delete(Files[i]);
                                }
                            }
                            catch (Exception)
                            {

                            }
                            #endregion
                        }
                    }


                    else
                    {
                        Converted = Files[i];
                    }
                    #endregion

                    #region File moving
                    //Creating folders and moving images
                    string folderName = path + FolderNameS;
                    pathString = System.IO.Path.Combine(folderName, FileNamesS + FileN[i]);
                    _ = Directory.CreateDirectory(pathString);

                    separator = Converted;

                    length = separator.Length;
                    length--;
                    #endregion

                    //getting filename by running trough until hitting \
                    GetFileN(i);

                    //Updating filepaths
                    sourcePath = Converted;
                    sourceFile = System.IO.Path.Combine(sourcePath);
                    destFile = System.IO.Path.Combine(pathString, filename);
                    //Catching possible errors
                    try
                    {
                        System.IO.File.Move(sourceFile, destFile);
                    }
                    catch (Exception)
                    {

                    }
                    //If small images are enabled
                    #region Database updates for small images
                    if (SmallImagesBool == true)
                    {
                        //Inserting small images to database
                        OdbcDataAdapter adapter = new();
                        string odbc = "INSERT INTO " + TableNameS + " (TuoteNro, Tiedosto, Kuvateksti, Verkkokaupassa) VALUES('" + FileN[i] + "', '" + pathString + @"\" + filenameS + "', PikkuKuva, 0); ";
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
                    #endregion

                    #region Updating database locations
                    if (UpdateDBBool == true)
                    {


                        //Updating database
                        adapter = new();
                        odbc = "UPDATE " + TableNameS + " SET Tiedosto = '" + pathString + @"\" + filename + "' Where Tiedosto = " + "'" + Files[i] + "'";
                        command = new OdbcCommand(odbc, cnn);
                        adapter.UpdateCommand = new OdbcCommand(odbc, cnn);
                        cnn.Open();
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
                        cnn.Close();
                    }
                    #endregion

                    counter++;
                    worker.ReportProgress(1 * 1);
                }
        }
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            #region Progress indicators
            //image
            ConversionPreview.Source = new BitmapImage(new Uri(Files[0], UriKind.Relative));

            //Making progress indicators
            Left1 = (int)(count - counter);
            Done.Text = "Done: " + counter.ToString();
            Left.Text = "Left: " + Left1.ToString();

            //Percentage done
            float PercentTemp = (counter / count * 100);
            DonePercent.Content = Math.Round((Decimal)PercentTemp, 1, MidpointRounding.AwayFromZero).ToString() + "% Done";

            //Progress bar
            ProgressB.Minimum = 0;
            ProgressB.Maximum = count + 1;
            if (ProgressB.Maximum > counter)
            {
                ProgressB.Value = counter;
            }
            #endregion

            #region Making log file to record everything done
            using (StreamWriter sw = File.AppendText(path + FolderNameS + @"\Log.Txt"))
            {
                if (Iteration == 0)
                {
                    sw.WriteLine(DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'"));
                    sw.WriteLine("User who made the change: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    sw.WriteLine("");
                    Iteration = 1;
                }
                sw.WriteLine("MOVED     " + filename + "    FROM    " + sourcePath + "     TO      " + destFile);
                sw.Close();
            }
            #endregion
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            counter++;
            Done.Text = counter.ToString();
            timer1.Stop();
            //handling error in progress
            if (e.Error != null)
            {
                System.Windows.MessageBox.Show(e.Error.Message);
            }
            //Handling cancelation
            else if (e.Cancelled)
            {
                _ = System.Windows.MessageBox.Show("Cancelled. still moved and converted " + counter + " images");
            }
            //Handling completion
            else
            {
                _ = System.Windows.MessageBox.Show("Done. Moved and converted " + counter + " images");
            }
            #region Resetting some values and making rigth control visible/hidden
            ProgressB.Value = 0;
            Left1 = 0;
            counter = 0;
            Iteration = 0;
            cnn.Close();

            //Making maingrid visible
            DBMainGrid.Visibility = System.Windows.Visibility.Visible;
            //Disabling Cancel and hiding conversion
            ConversionProces.Visibility = Visibility.Hidden;
            Cancel.IsEnabled = false;

            #region Enabling buttons
            AboutB.IsEnabled = true;
            DBConversionB.IsEnabled = true;
            FConversionB.IsEnabled = true;
            SettingsB.IsEnabled = true;
            #endregion

            #endregion
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
            MoveImagesGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void MoveI_Unchecked(object sender, RoutedEventArgs e)
        {
            MoveImagesGrid.Visibility = System.Windows.Visibility.Hidden;
        }
        #endregion

        //When Convert is clicked
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Checking if the list Files has something inside
            if (Files.Count > 0)
            {
                //Getting amount of items.
                count = Files.Count - 1;
                //Starting progress
                #region Disabling buttons
                AboutB.IsEnabled = false;
                DBConversionB.IsEnabled = false;
                FConversionB.IsEnabled = false;
                SettingsB.IsEnabled = false;
                #endregion
                #region Image processing
                ConversionPreview.Source = new BitmapImage(new Uri(Files[0], UriKind.Relative));
                #endregion
                DBMainGrid.Visibility = System.Windows.Visibility.Hidden;
                ConversionProces.Visibility = System.Windows.Visibility.Visible;
                timer1.Start();
                backgroundWorker1.RunWorkerAsync();
            }
        }

        #region Bunch of value setting

        #region Small things (Timer, option checkboxes, scrollbar, quality, cancel)
        private void timer1_Tick(object sender, EventArgs e)
        {
            #region dot animation
            //Making Dot animation
            if (ConversionProces.Header.ToString() == "Converting...")
            {
                ConversionProces.Header = "Converting.";
            }
            else if (ConversionProces.Header.ToString() == "Converting.")
            {
                ConversionProces.Header = "Converting..";
            }
            else if (ConversionProces.Header.ToString() == "Converting..")
            {
                ConversionProces.Header = "Converting...";
            }
            #endregion
            Seconds++;
            #region Time logic
            TimeElapsed.Text = "Time elapsed: " + Seconds + " S";
            if (Minutes > 0)
            {
                TimeElapsed.Text = "Time elapsed: " + Minutes + " M " + Seconds + " S";
            }
            if (Hours > 0)
            {
                TimeElapsed.Text = "Time elapsed: " + Hours + " H " + Minutes + " M " + Seconds + " S";
            }
            if (Seconds == 60)
            {
                Seconds = 0;
                Minutes++;
            }
            if (Minutes == 60)
            {
                Minutes = 0;
                Hours++;
            }
            #endregion
        }


        #region Option Checkboxes
        private void Deletion_Checked(object sender, RoutedEventArgs e)
        {
            DeletionBool = true;
        }

        private void Deletion_Unchecked(object sender, RoutedEventArgs e)
        {
            DeletionBool = false;
        }

        private void SmallImages_Checked(object sender, RoutedEventArgs e)
        {
            SmallImagesBool = true;
        }

        private void SmallImages_Unchecked(object sender, RoutedEventArgs e)
        {
            SmallImagesBool = false;
        }
        private void UpdateDB_Checked(object sender, RoutedEventArgs e)
        {
            UpdateDBBool = true;
        }

        private void UpdateDB_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateDBBool = false;
        }

        #endregion

        void scrollBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            QualityB.Text = scrollBar1.Value.ToString();
        }

        private void QualityB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (QualityB.Text != "")
                {
                    if (int.Parse(QualityB.Text) > 100)
                    {
                        QualityB.Text = "100";
                    }
                    else if (int.Parse(QualityB.Text) < 0)
                    {
                        QualityB.Text = "0";
                    }
                }
                else
                {

                }
            }
            catch
            {
                QualityB.Text = scrollBar1.Value.ToString();
            }
            try
            {
                Quality = int.Parse(QualityB.Text);
            }
            catch
            {

            }
        }

        //Database Cancel Button code
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the asynchronous operation.
            this.backgroundWorker1.CancelAsync();

            // Disable the Cancel button.
            Cancel.IsEnabled = false;
        }
        #endregion

        #region Setting values in variables
        //Updating rigth path
        private void PathB_TextChanged(object sender, TextChangedEventArgs e)
        {
            path = PathB.Text;
        }

        private void TableName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TableNameS = TableName.Text;
        }
        #endregion

        //Making some changes when the window is loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Setting values in variables
            path = PathB.Text;
            TableNameS = TableName.Text;
            FolderNameS = FolderName.Text;
            FileNamesS = FileNames.Text;
            #endregion

            #region Display settings
            //Making windows to be displayed correctly
            this.SizeToContent = SizeToContent.WidthAndHeight;
            DBConvertion.Visibility = System.Windows.Visibility.Hidden;
            FConversion.Visibility = System.Windows.Visibility.Hidden;
            Settings.Visibility = System.Windows.Visibility.Hidden;
            ConversionProces.Visibility = Visibility.Hidden;
            #endregion

            #region Default database
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
            #endregion
        }
        #endregion
    }
}