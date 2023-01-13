using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using System.Collections;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;
using System.Threading;
using System.ComponentModel;
using System.Linq;

namespace ImageConverter
{

    public partial class ImageConversion : Form
    {
        //Variables below here
        private string type = "webp";
        private string filename = "";
        string filenameS;
        private string Checker;
        private string separator;
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
        int length;
        int amount;
        int amount1;
        int Timer;
        private int Quality;
        private int count;
        private int Iteration = 0;
        private int counter = 0;
        private int Left1;
        OdbcCommand command;
        OdbcDataReader dataReader;
        OdbcConnection cnn;
        OdbcDataAdapter adapter;
        List<string> Files = new List<string>();
        List<string> FileN = new List<string>();
        List<string> FileK = new List<string>();
        double CDouble;
        bool FTime = true;
        //Variables abowe here

        public ImageConversion()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl= Connect;
            try
            {
                DBName.Text = Environment.GetEnvironmentVariable("kanta");
                Connect.PerformClick();
            }
            catch
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            type = TypeB.Text;
        }

        public void FNameG()
        {
            while (true)
            {
                try
                {
                    Checker = separator.Substring(length, 1);
                }
                catch (Exception)
                {
                    break;
                }
                if (Checker == @"\")
                {
                    break;
                }
                filename += Checker;
                Checker = "";
                length--;
            }
            //Settup for calculation.
            length = filename.Length;
            length--;
            //Inverting string. Because it comes out wrong way from procces above.
            for (int a = 0; a <= length;)
            {
                Checker = filename.Substring(length, 1);
                TempS += Checker;
                Checker = "";
                length--;
            }
        }

        public void DProblem(Exception ex)
        {
            Debug.WriteLine("<<< catch : " + ex.ToString());
            using StreamWriter sw = File.AppendText(PathB.Text + @"Kuvat\Failed.Txt");
            sw.WriteLine("Error in Sql syntax in " + filename + " this should be manually fixed. Path to it should be: " + sourcePath + " Error is: " + ex);
            sw.Close();
        }

        private void convert_Click(object sender, EventArgs e)
        {
                if (Files.Count > 0)
                {
                //Getting amount of items.
                    count = Files.Count - 1;
                try
                {
                    amount = count / 2;
                    amount1 = count / 2;
                }
                catch
                {
                    CDouble = count;
                    amount = (int)(CDouble / 2 + 0.5);
                    amount1 = (int)(CDouble / 2 - 0.5);
                }
                //Starting progress
                timer1.Start();
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.RunWorkerAsync();
                //Disabling some controls
                convert.Enabled= false;
                TypeB.Enabled= false;
                QualityB.Enabled= false;
                PathB.Enabled= false;
                Connect.Enabled= false;
                //Enabling cancel
                Cancel.Enabled = true;
                }
        }

        public void Connect_Click(object sender, EventArgs e)
        {
            this.ActiveControl = convert;
            if (DBName.Text != "")
            {
                try
                {
                    if (Files.Count > 0)
                    {
                        Files.Clear();
                        FileN.Clear();
                        InputB.Items.Clear();
                        InputB2.Items.Clear();
                    }
                    else
                    {
                       
                    }
                    //Opening ODBC connection
                    connectionstring = "Driver={Pervasive ODBC Unicode Interface}; ServerName=Localhost;DBQ=" + DBName.Text + "; TransportHint=TCP;";
                    cnn = new OdbcConnection(connectionstring);
                    cnn.Open();
                    //Reading database and inserting values into listboxes
                    Odbc = "Select Tiedosto, TuoteNro, kuvateksti from "+ TableN.Text +" ORDER BY TuoteNro";
                    command = new OdbcCommand(Odbc, cnn);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        output = dataReader.GetValue(0).ToString();
                        Files.Add(output);
                        InputB.Items.Add(output);
                        output = dataReader.GetValue(1).ToString();
                        FileN.Add(output);
                        InputB2.Items.Add(output);
                        output = dataReader.GetValue(2).ToString();
                        FileK.Add(output);
                        KuvaTeksti.Items.Add(output);
                    }
                    ConnectionBox.Text = "Connected to: " + DBName.Text;
                }
                catch
                {
                    MessageBox.Show("Invalid database or table name!", "Invalid input");
                }
                
            }
            else
            {
                _ = MessageBox.Show("Invalid input!", "Invalid input!");
            }
        }

        #region Backgroundworkers do stuff
        //Background worker to make code run smoother
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
                    //Looping trough to procces images
                    for (int i = 1; i <= amount; i++)
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

                        if (SmallImages.Checked)
                        {
                            //getting filename by running trough until hitting \
                            separator = Files[i];
                            length = Files[i].Length;
                            length--;
                            FNameG();
                            filename = TempS;
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
                                Image img = new Bitmap(stream);
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
                                    if (Deletion.Checked)
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
                        FNameG();
                        //Putting rigth filenames and reseting values
                        filename = TempS;

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

                        if (SmallImages.Checked)
                        {
                            //Inserting small images to database
                            OdbcDataAdapter adapter = new();
                            string odbc = "INSERT INTO " + TableN.Text + " (TuoteNro, Tiedosto, Kuvateksti, Verkkokaupassa) VALUES('" + FileN[i] + "', '" + pathString + @"\" + filenameS + "', PikkuKuva, 0); ";
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
                        odbc = "UPDATE " + TableN.Text + " SET Tiedosto = '" + pathString + @"\" + filename + "' Where Tiedosto = " + "'" + Files[i] + "'";
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

                        //Listbox colour change
                        //https://stackoverflow.com/questions/2554609/c-sharp-changing-listbox-row-color

                        filename = "";
                        counter++;
                        worker.ReportProgress(1 * 1);
                    }
                }
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //Looping trough to procces images
            for (int i = count; i >= amount; i--)
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

                    if (SmallImages.Checked)
                    {
                        //getting filename by running trough until hitting \
                        separator = Files[i];
                        length = Files[i].Length;
                        length--;
                        FNameG();
                        filename = TempS;
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
                            Image img = new Bitmap(stream);
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
                            separator = separator.Substring(0, index);
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
                                if (Deletion.Checked)
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
                    pathString1 = System.IO.Path.Combine(folderName, "Tuote-numero-" + FileN[i]);
                    _ = System.IO.Directory.CreateDirectory(pathString1);

                    separator = Converted;

                    length = separator.Length;
                    length--;

                    //getting filename by running trough until hitting \
                    FNameG();
                    //Putting rigth filenames and reseting values
                    filename1 = TempS;

                    TempS = "";

                    //Updating filepaths
                    string sourcePath1 = Converted;
                    sourceFile = System.IO.Path.Combine(sourcePath1);
                    string destFile = System.IO.Path.Combine(pathString1, filename1);
                    //Catching possible errors
                    try
                    {
                        System.IO.File.Move(sourceFile, destFile);
                    }
                    catch (Exception)
                    {

                    }

                    if (SmallImages.Checked)
                    {
                        //Inserting small images to database
                        OdbcDataAdapter adapter = new();
                        string odbc = "INSERT INTO " + TableN.Text + " (TuoteNro, Tiedosto, Kuvateksti, Verkkokaupassa) VALUES('" + FileN[i] + "', '" + pathString1 + @"\" + filenameS + "', PikkuKuva, 0); ";
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
                    odbc = "UPDATE " + TableN.Text + " SET Tiedosto = '" + pathString1 + @"\" + filename1 + "' Where Tiedosto = " + "'" + Files[i] + "'";
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

                    //Listbox colour change
                    //https://stackoverflow.com/questions/2554609/c-sharp-changing-listbox-row-color

                    filename1 = "";
                    counter++;
                    worker.ReportProgress(1 * 1);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (InputB.Items.Count > counter)
            {
                InputB.SelectedIndex = counter;
                InputB2.SelectedIndex = counter;
            }
            //Making progress indicators
            Left1 = count - counter;
            Done.Text = counter.ToString();
            Left.Text = Left1.ToString();

            //Progress bar
            ProgressB.Minimum = 0;
            ProgressB.Maximum = count + 1;
            if (ProgressB.Maximum > counter)
            {
                ProgressB.Value = counter;
            }
            //Making log file to record everything done
            using (StreamWriter sw = File.AppendText(PathB.Text + @"Kuvat\Log.Txt"))
            {
                if (Iteration == 0)
                {
                        sw.WriteLine(DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'"));
                        sw.WriteLine("User who made the change: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                        sw.WriteLine("");
                        Iteration = 1;
                }
                if (sender == backgroundWorker1)
                {
                    sw.WriteLine("Moved " + filename + " from " + sourcePath + " to " + pathString);
                    sw.Close();
                }
                else if (sender == backgroundWorker2) 
                {
                    sw.WriteLine("Moved " + filename1 + " from " + sourcePath1 + " to " + pathString1);
                    sw.Close();
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (FTime == true)
            {
                FTime = false;
            }
            else if (FTime == false)
            {
                timer1.Stop();
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                }
                else if (e.Cancelled)
                {
                    _ = MessageBox.Show("Cancelled. still moved and converted " + counter + " images");
                }
                else
                {
                    _ = MessageBox.Show("Done. Moved and converted " + counter + " images");
                }
                ProgressB.Value = 0;
                Left1 = 0;
                counter = 0;
                Iteration = 0;
                cnn.Close();
                timer1.Stop();

                //Enabling some controls
                convert.Enabled = true;
                TypeB.Enabled = true;
                QualityB.Enabled = true;
                PathB.Enabled = true;
                Connect.Enabled = true;
                //Disabling Cancel
                Cancel.Enabled = false;
            }
        }

        #endregion

        //Making quality be the thing its meant to be
        private void QualityB_ValueChanged(object sender, EventArgs e)
        {
            Quality = (int)QualityB.Value;
        }

        //Cancelation button to cancel convertion
        private void Cancel_Click(object sender, EventArgs e)
        {
            // Cancel the asynchronous operation.
            this.backgroundWorker1.CancelAsync();

            // Disable the Cancel button.
            Cancel.Enabled = false;
        }

        private ToolTip TipConvert;
        //Tooltips
        private void TipConvert_Popup(object sender, PopupEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timer++;
            TimeElapsed.Text = "Time elapsed: " + Timer;
        }

        private Label TimeElapsed;
    }
}