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

        //Making some changes when the window is loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowLoaded = true;
            #region Display settings
            //Making windows to be displayed correctly
            DBConvertion.Visibility = System.Windows.Visibility.Hidden;
            FConversion.Visibility = System.Windows.Visibility.Hidden;
            Settings.Visibility = System.Windows.Visibility.Hidden;
            ConversionProces.Visibility = Visibility.Hidden;
            #endregion

            #region Getting settings
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"/Settings.txt"))
            {
                foreach (string line in System.IO.File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + @"/Settings.txt"))
                {
                    switch (SCounter)
                    {
                        case 1:
                            SettingMakeSmallImages.IsChecked = bool.Parse(line);
                            SmallImages.IsChecked = bool.Parse(line);
                            break;
                        case 2:
                            SettingKeepOldImages.IsChecked = bool.Parse(line);
                            Deletion.IsChecked = bool.Parse(line);
                            break;
                        case 3:
                            SettingMoveImages.IsChecked = bool.Parse(line);
                            MoveI.IsChecked = bool.Parse(line);
                            break;
                        case 4:
                            PathB2.Text = line;
                            PathB.Text = line;
                            break;
                        case 5:
                            FileNames1.Text = line;
                            FileNames.Text = line;
                            break;
                        case 6:
                            FolderName1.Text = line;
                            FolderName.Text = line;
                            break;
                        case 7:
                            SettingDataBaseName.Text = line;
                            DBName.Text = line;
                            break;
                        case 8:
                            SettingTableName.Text = line;
                            TableName.Text = line;
                            break;
                        case 9:
                            SettingDescription.Text = line;
                            break;
                        case 10:
                            SettingFile.Text = line;
                            break;
                        case 11:
                            SettingProductNumber.Text = line;
                            break;
                        case 12:
                            SettingBool.Text = line;
                            break;
                        case 13:
                            SettingsUpdateDataBase.IsChecked = bool.Parse(line);
                            UpdateDB.IsChecked = bool.Parse(line);
                            break;
                        case 14:
                            Quality = int.Parse(line);
                            scrollBar1.Value = int.Parse(line);
                            scrollBar2.Value = int.Parse(line);
                            break;
                        case 15:
                            type = line;
                            TypeB.Text = line;
                            TypeB1.Text = line;
                            break;
                    }
                    SCounter++;
                }
            }
            counter = 0;
            #endregion 

            #region DataBase Things
            #region Setting values in variables
            path = PathB.Text;
            TableNameS = TableName.Text;
            FolderNameS = FolderName.Text;
            FileNamesS = FileNames.Text;
            #endregion

            #region Default database
            //Getting default database
            try
            {
                DBName.Text = Environment.GetEnvironmentVariable("kanta");
                length = DBName.Items.Count;
                if (length >= 0)
                {
                    _ = DBName.Items.Add(Environment.GetEnvironmentVariable("kanta"));
                }
                for (int i = 0; i <= length; i++)
                {
                    DBName.SelectedIndex = i;
                    if (DBName.Text != Environment.GetEnvironmentVariable("kanta"))
                    {
                        _ = DBName.Items.Add(Environment.GetEnvironmentVariable("kanta"));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #endregion
        }

        #region Variables here
        #region Settings variables
        #region Strings
        private string SDataBaseName;
        private string STableName;
        private string SPathB;
        private string SFilenames;
        private string SFolderName;
        private string SDBProductNumber;
        private string SDBDescription;
        private string SDBImageFile;
        private string SDBStoreBool;
        private string SType;
        #endregion
        #region Ints
        int SCounter = 0;
        int SQuality= 0;
        #endregion
        #region Float
        #endregion
        #region Odbc 
        #endregion
        #region Lists
        #endregion
        #region Bool
        private bool SKeepOldImages;
        private bool SMoveImages;
        private bool SMakeSmallImages;
        private bool WindowLoaded;
        private bool SUpdateDB;
        #endregion
        #endregion
        #region DB Conversion variables
        #region Strings
        private string type = "webp";
        private string filename = "";
        private string LogFilename;
        private string filenameS;
        private string filenameT = "";
        private string Checker;
        private string separator;
        private string separatorT;
        private string TempS;
        private string Converted;
        private string SsourceFile;
        private string sourceFile;
        private string odbc;
        public string sourcePath;
        private string LogSourcePath;
        public string pathString;
        private string connectionstring;
        private string Odbc, output;
        private string path;
        private string TableNameS;
        private string FolderNameS;
        private string FileNamesS;
        private string destFile;
        private string LogDestFile;
        private string Updatable;
        private string NewFile;
        private string pathString1;
        private string SdestinationFile;
        #endregion
        #region Ints
        private int a = 0;
        private int index;
        private int FileNotFoundCounter;
        private int length;
        private int lengthT;
        private int Seconds;
        private int Minutes;
        private int Hours;
        private int Quality = 60;
        private int Iteration = 0;
        private int Left1;
        #endregion
        #region Float
        private float counter = -1;
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
        public List<string> Movables = new();
        public List<string> Locations = new();
        public List<string> AlreadyChecked = new();
        public List<string> DeletionList = new();
        #endregion
        #region Bool
        private bool SmallImagesBool = false;
        private bool DeletionBool = false;
        private bool MoveBool = true;
        private bool UpdateDBBool = true;
        private bool FileNotFound = false;
        private bool DeletionProcces = false;
        #endregion
        #endregion
        #endregion

        #region Data Base Convertion
        #region DB Components in here
        private readonly BackgroundWorker backgroundWorker1 = new();
        private readonly DispatcherTimer timer1 = new();
        #endregion

        #region DB Methods here
        private void FillDataGrid()

        {

            string ConString = "Driver={Pervasive ODBC Unicode Interface}; ServerName=Localhost;DBQ=" + DBName.Text + "; TransportHint=TCP;";
            using OdbcConnection con = new(ConString);

            string CmdString = "SELECT * FROM " + TableNameS;
            OdbcCommand cmd = new(CmdString, con);

            OdbcDataAdapter sda = new(cmd);

            DataTable dt = new(DBName.Text);

            _ = sda.Fill(dt);

            PreviewD.ItemsSource = dt.DefaultView;
        }
        public void GetFileN(int i, string Conversion)
        {
            //getting filename by running trough until hitting \
            separatorT = Conversion;
            length = Conversion.Length;
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
            LogFilename = filename;
            TempS = "";
        }
        public void DProblem(Exception ex, int i)
        {
            Debug.WriteLine("<<< catch : " + ex.ToString());
            using StreamWriter sw = File.AppendText(path + FolderNameS + @"\Failed.Txt");
            sw.WriteLine("Error in Sql syntax in " + filename + " this should be manually fixed. Path to it should be: " + Files[i] + " Error is: " + ex);
            sw.Close();
        }
        public void UpdateDBM(int i, string NewDatabaseName)
        {
            //Updating database
            adapter = new();
            odbc = "UPDATE " + TableNameS + " SET Tiedosto = '" + NewDatabaseName + "' Where Tiedosto = " + "'" + Files[i] + "' AND TuoteNro = '" + FileN[i] + "'";
            command = new OdbcCommand(odbc, cnn);
            adapter.UpdateCommand = new OdbcCommand(odbc, cnn);
            cnn.Open();
            try
            {
                _ = adapter.UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DProblem(ex, i);
            }
            command.Dispose();
            cnn.Close();
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
                        DeletionList.Clear();
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
                        DeletionList.Add(output);
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
                    _ = System.Windows.MessageBox.Show("Invalid database or table name!", "Invalid input");
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
            {
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
                    if (FileK.Count >= i)
                    {
                        if (FileK[i] == "PikkuKuva")
                        {
                            continue;
                        }
                    }
                    #endregion

                    //Changing path in a way to prevent fails and prevent sql injection. Double '
                    Files[i] = Files[i].Replace("'", "''");
                    LogSourcePath = Files[i];
                    //checking if small images are true
                    #region Small images making procces
                    if (SmallImagesBool == true)
                    {
                        //Settup for small images
                        TempS = Files[i];
                        separator = TempS;
                        index = separator.IndexOf(".");
                        if (index >= 0)
                        {
                            TempS = separator[..index];
                        }
                        TempS += "PikkuKuva.Jpeg";
                        //Creating copy of image to use for small image.
                        SsourceFile = Files[i];
                        SdestinationFile = TempS;
                        try
                        {
                            File.Copy(SsourceFile, SdestinationFile, true);

                            // Converting small images into jpeg
                            MemoryStream stream = new(File.ReadAllBytes(SdestinationFile));
                            System.Drawing.Image img = new Bitmap(stream);
                            img.Save(TempS, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        catch
                        {

                        }
                        pathString1 = TempS;
                        filenameS = filename;
                        TempS = "";
                        SsourceFile = "";
                        SdestinationFile = "";
                        filename = "";
                    }
                    #endregion

                    #region File convertion
                    //Checking file type
                    separator = Files[i];
                    index = separator.IndexOf(".");
                    if (index >= 0)
                    {
                        Checker = separator[index..];
                        separator = separator[..index];
                    }
                    Converted = separator + "." + type;
                    string oldImagePath = Files[i];
                    NewFile = Converted;
                    //Checking if file already in webp
                    if (Checker != "." + type)
                    {


                        //File convertion
                        if (File.Exists(oldImagePath))
                        {
                            try
                            {
                                //In case same picture on different products
                                AlreadyChecked.Add(oldImagePath);
                                //Converting image
                                using FileStream webPFileStream = new(NewFile, FileMode.Create);
                                using ImageFactory imageFactory = new(preserveExifData: false);
                                _ = imageFactory.Load(oldImagePath)
                                         .Format(new WebPFormat())
                                         .Quality(Quality)
                                         .Save(webPFileStream);
                            }
                            catch (Exception)
                            {

                            }

                        }
                        else
                        {
                            foreach (string file in AlreadyChecked)
                            {
                                if (file == oldImagePath)
                                {
                                    FileNotFound = false; break;
                                }
                                else
                                {
                                    FileNotFound = true;
                                }
                            }
                        }
                    }
                    else if (DeletionBool == false)
                    {
                        DeletionBool= true;
                        DeletionProcces = true;
                    }


                    else
                    {
                        Converted = Files[i];
                    }
                    #endregion

                    if (FileNotFound == true)
                    {
                        if (!File.Exists(Files[i]))
                        {
                            FileNotFoundCounter++;
                        }
                    }
                    else
                    {
                        #region File Manipulation
                        //File moving
                        if (MoveBool == true)
                        {
                            #region Creating folders
                            //Creating folders and moving images
                            string folderName = path + FolderNameS;
                            pathString = System.IO.Path.Combine(folderName, FileNamesS + FileN[i]);
                            _ = Directory.CreateDirectory(pathString);

                            separator = Converted;

                            length = separator.Length;
                            length--;
                            #endregion

                            //getting filename by running trough until hitting \
                            GetFileN(i, Converted);

                            #region File moving
                            //Updating filepaths
                            sourceFile = System.IO.Path.Combine(Converted);
                            destFile = System.IO.Path.Combine(pathString, filename);
                            LogDestFile = destFile;
                            Locations.Add(destFile);
                            if (SmallImagesBool == true)
                            {
                                GetFileN(i, pathString1);
                                Movables.Add(pathString1);
                            }
                            destFile = System.IO.Path.Combine(pathString, filename);
                            GetFileN(i, Converted);
                            //Adding things to list
                            Movables.Add(sourceFile);
                            Locations.Add(destFile);
                            //Catching possible errors
                            try
                            {
                                foreach (string file in Movables)
                                {
                                    System.IO.File.Move(file, Locations[a]);
                                    a++;
                                }

                            }
                            catch (Exception x)
                            {
                                Console.WriteLine("<<<Catch: " + x);
                            }
                            a = 0;
                            Locations.Clear();
                            Movables.Clear();
                            #endregion
                        }
                        #endregion
                        //If small images are enabled
                        #region Database updates for small images
                        if (SmallImagesBool == true)
                        {
                            //Inserting small images to database
                            OdbcDataAdapter adapter = new();
                            string odbc = "INSERT INTO " + TableNameS + " (TuoteNro, Tiedosto, Kuvateksti, Verkkokaupassa) VALUES('" + FileN[i] + "', '" + destFile + "', 'PikkuKuva', 0); ";
                            command = new OdbcCommand(odbc, cnn);
                            adapter.UpdateCommand = new OdbcCommand(odbc, cnn);
                            cnn.Open();
                            try
                            {
                                _ = adapter.UpdateCommand.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("<<< catch : " + ex.ToString());
                            }
                            cnn.Close();
                        }
                        #endregion
                    }
                    #region Updating database locations
                    if (UpdateDBBool == true)
                    {
                        //Making sure folder creation is enabled to update database
                        if (MoveBool == true)
                        {
                            Updatable = Path.Combine(pathString, filename);
                            UpdateDBM(i, Updatable);
                        }
                        else if (MoveBool == false)
                        {
                            Updatable = NewFile;
                            UpdateDBM(i, Updatable);
                        }
                    }
                    #endregion
                    //Incrementing counter if file was found
                    if (FileNotFound == false)
                    {
                        counter++;
                    }

                    if (DeletionProcces == true)
                    {
                        DeletionBool= false;
                        DeletionProcces = false;
                        DeletionList.RemoveAt(i);
                    }

                    worker.ReportProgress(1 * 1);
                }
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            #region Progress indicators
            //image
            ConversionPreview.Source = new BitmapImage(new Uri(Files[0], UriKind.Relative));

            //Making progress indicators
            Left1 = (int)(count - (counter + FileNotFoundCounter));
            //In case file was not found

            Done.Text = "Done: " + counter.ToString();
            Left.Text = "Left: " + Left1.ToString();


            //Percentage done
            float PercentTemp = counter / count * 100;
            DonePercent.Content = Math.Round((decimal)PercentTemp, 1, MidpointRounding.AwayFromZero).ToString() + "% Done";

            //Progress bar
            ProgressB.Minimum = 0;
            ProgressB.Maximum = count + 1;
            if (ProgressB.Maximum > counter)
            {
                ProgressB.Value = counter + FileNotFoundCounter;
            }
            #endregion

            #region Making log file to record everything done
            if (MoveBool == false || FileNotFound == true)
            {
                FolderNameS = "";
            }
            using StreamWriter sw = File.AppendText(path + FolderNameS + @"\Log.Txt");
            if (Iteration == 0)
            {
                sw.WriteLine(DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'"));
                sw.WriteLine("User who made the change: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                sw.WriteLine("");
                Iteration = 1;
            }
            sw.WriteLine("MOVED     " + LogFilename + "    FROM    " + LogSourcePath + "     TO      " + LogDestFile + " MADE SMALL IMAGES: " + SmallImagesBool);
            sw.Close();
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
                _ = System.Windows.MessageBox.Show(e.Error.Message);
            }
            //Handling cancelation
            else if (e.Cancelled)
            {
                _ = FileNotFoundCounter == 0
                    ? System.Windows.MessageBox.Show("Cancelled. still moved and converted " + counter + " images")
                    : System.Windows.MessageBox.Show("Cancelled. still moved and converted " + counter + " images " + FileNotFoundCounter + " Files were not found");
            }
            //Handling completion
            else
            {
                _ = FileNotFoundCounter == 0
                    ? System.Windows.MessageBox.Show("Done. Moved and converted " + counter + " images")
                    : System.Windows.MessageBox.Show("Done. Moved and converted " + counter + " images " + FileNotFoundCounter + " Files were not found");
            }
            #region Resetting some values and making rigth control visible/hidden
            ProgressB.Value = 0;
            Left1 = 0;
            counter = 0;
            Iteration = 0;
            FileNotFoundCounter = 0;
            cnn.Close();

            #region Deletion
            //If deletion is enabled delete
            try
            {
                if (DeletionBool == true)
                {
                    foreach (string file in DeletionList)
                    {
                        File.Delete(file);
                    }

                }
            }
            catch (Exception)
            {

            }
            #endregion

            //Making maingrid visible
            DBMainGrid.Visibility = Visibility.Visible;
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
        private void TypeB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            type = TypeB.Text;
        }

        private void scrollBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            QualityB.Text = scrollBar1.Value.ToString();
            scrollBar2.Value = scrollBar1.Value;
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
            backgroundWorker1.CancelAsync();

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

        #endregion

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

        #region Settings

        #region Setting rigth values to setting options
        private void SettingKeepOldImages_Checked(object sender, RoutedEventArgs e)
        {
            SKeepOldImages = true;
        }
        private void SettingKeepOldImages_Unchecked(object sender, RoutedEventArgs e)
        {
            SKeepOldImages = false;
        }
        private void SettingMakeSmallImages_Checked(object sender, RoutedEventArgs e)
        {
            SMakeSmallImages = true;
        }
        private void SettingMakeSmallImages_Unchecked(object sender, RoutedEventArgs e)
        {
            SMakeSmallImages = false;
        }
        private void SettingMoveImages_Checked(object sender, RoutedEventArgs e)
        {
            SMoveImages = true;
            if (WindowLoaded == true)
            {
                MoveImagesGrid1.Visibility = Visibility.Visible;
            }
        }
        private void SettingMoveImages_Unchecked(object sender, RoutedEventArgs e)
        {
            SMoveImages = false;
            MoveImagesGrid1.Visibility = Visibility.Hidden;
        }
        private void PathB2_TextChanged(object sender, TextChangedEventArgs e)
        {
            SPathB = PathB2.Text;
        }
        private void FolderName1_TextChanged(object sender, TextChangedEventArgs e)
        {
            SFolderName = FolderName1.Text;
        }
        private void FileNames1_TextChanged(object sender, TextChangedEventArgs e)
        {
            SFilenames = FileNames1.Text;
        }
        private void SettingDataBaseName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SDataBaseName = SettingDataBaseName.Text;
        }
        private void SettingTableName_TextChanged(object sender, TextChangedEventArgs e)
        {
            STableName = SettingTableName.Text;
        }
        private void SettingBool_TextChanged(object sender, TextChangedEventArgs e)
        {
            SDBStoreBool = SettingBool.Text;
        }
        private void SettingFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            SDBImageFile = SettingFile.Text;
        }
        private void SettingProductNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            SDBProductNumber = SettingProductNumber.Text;
        }
        private void SettingDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            SDBDescription = SettingDescription.Text;
        }
        private void SettingsUpdateDataBase_Checked(object sender, RoutedEventArgs e)
        {
            SUpdateDB = true;
        }
        private void SettingsUpdateDataBase_Unchecked(object sender, RoutedEventArgs e)
        {
            SUpdateDB = false;
        }
        private void QualityB1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (QualityB1.Text != "")
                {
                    if (int.Parse(QualityB1.Text) > 100)
                    {
                        QualityB1.Text = "100";
                    }
                    else if (int.Parse(QualityB1.Text) < 0)
                    {
                        QualityB1.Text = "0";
                    }
                }
                else
                {

                }
            }
            catch
            {
                QualityB1.Text = scrollBar2.Value.ToString();
            }
            try
            {
                SQuality = int.Parse(QualityB1.Text);
            }
            catch
            {

            }
        }
        private void scrollBar2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            QualityB1.Text = scrollBar2.Value.ToString();
        }
        private void TypeB1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SType = TypeB1.Text;
        }
        #endregion

        private void SettingSubmit_Click(object sender, RoutedEventArgs e)
        {
            //Writing settings file to save users setting
            using StreamWriter sw = new(AppDomain.CurrentDomain.BaseDirectory + @"/Settings.txt");
            #region Things to write to settings
            sw.WriteLine("Settings here");          //Row 1
            sw.WriteLine(SMakeSmallImages+ "");     //Row 2
            sw.WriteLine(SKeepOldImages+ "");       //Row 3
            sw.WriteLine(SMoveImages + "");         //Row 4
            sw.WriteLine(SPathB + "");              //Row 5
            sw.WriteLine(SFilenames + "");          //Row 6
            sw.WriteLine(SFolderName + "");         //Row 7
            sw.WriteLine(SDataBaseName + "");       //Row 8
            sw.WriteLine(STableName + "");          //Row 9
            sw.WriteLine(SDBDescription + "");      //Row 10
            sw.WriteLine(SDBImageFile + "");        //Row 11
            sw.WriteLine(SDBProductNumber + "");    //Row 12
            sw.WriteLine(SDBStoreBool + "");        //Row 13
            sw.WriteLine(SUpdateDB + "");           //Row 14
            sw.WriteLine(SQuality + "");            //Row 15
            sw.WriteLine(SType + "");               //Row 16

            #endregion 
            sw.Close();
            //Setting settings to other pages

        }
        #endregion
    }
}