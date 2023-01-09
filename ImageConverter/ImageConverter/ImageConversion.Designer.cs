using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using System.Collections;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace ImageConverter
{

    public partial class ImageConversion : Form
    {
        //Variables below here
        private string type = "webp";
        private string filename = "";
        private string Checker;
        private string separator;
        string TempS;
        private string Converted;
        string sourceFile;
        string odbc;
        int length;
        private int Quality;
        private int count;
        private int Iteration = 0;
        private int counter = 0;
        private int Left1;
        OdbcCommand command;
        OdbcDataReader dataReader;
        OdbcConnection cnn;
        OdbcDataAdapter adapter;
        string connectionstring;
        string Odbc, output;
        //Variables abowe here

        public ImageConversion()
        {
            InitializeComponent();
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

        private void convert_Click(object sender, EventArgs e)
        {
            if (InputB.Items.Count > 0)
            {
                //Making sure rigth index is selected. Also getting amount of items.
                InputB.SelectedIndex = 0;
                count = InputB.Items.Count;
                InputB2.SelectedIndex = 0;


                //Looping trough to procces images
                for (int i = 0; i <= count; i++)
                {
                    //Changing path in a way to prevent fails. Double '
                    _ = InputB.Text.Replace("'", "''");

                    if (SmallImages.Checked)
                    {
                        //getting filename by running trough until hitting \
                        separator = InputB.Text;
                        length = InputB.Text.Length;
                        length--;
                        FNameG();
                        filename = TempS;
                        length = InputB.Text.Length - filename.Length;


                        TempS = InputB.Text.Substring(0, length) + "Pikkukuva" + filename;
                        length = TempS.Length - 4;
                        TempS = TempS.Substring(0, length) + "Jpeg";

                        //Creating copy of image to use for small image.
                        string sourceFile = InputB.Text;
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

                        TempS = "";
                        sourceFile = "";
                        destinationFile = "";
                        filename = "";
                    }
                    
                    //Checking if file already in webp
                    Checker = InputB.Text[^4..];
                    if (Checker != type)
                    {
                        //File convertion
                        separator = InputB.Text;
                        int index = separator.IndexOf(".");
                        if (index >= 0)
                        {
                            separator = separator[..index];
                        }

                        Converted = separator + "." + type;
                        string oldImagePath = InputB.Text;
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
                                    File.Delete(InputB.Text);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    else
                    {
                        Converted = InputB.Text;
                    }
                    //Creating folders and moving images
                    string folderName = PathB.Text + "Kuvat";
                    string pathString = System.IO.Path.Combine(folderName, "Tuote-numero-" + InputB2.Text);
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
                    string sourcePath = Converted;
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
                        string odbc = "INSERT INTO TUOTEKUV (TuoteNro, Tiedosto, Verkkokaupassa) VALUES('" + InputB2.Text + "', '" + pathString + @"\" + filename + "', 0); ";
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
                    odbc = "UPDATE TUOTEKUV SET Tiedosto = '" + pathString + @"\" + filename + "' Where Tiedosto = " + "'" + InputB.Text + "'";
                    command = new OdbcCommand(odbc, cnn);
                    adapter.UpdateCommand = new OdbcCommand(odbc, cnn);
                    try
                    {
                        _ = adapter.UpdateCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("<<< catch : " + ex.ToString());
                        using StreamWriter sw = File.AppendText(PathB.Text + @"Kuvat\Failed.Txt");
                        sw.WriteLine("Error in Sql syntax in " + filename + " this should be manually fixed. Path to it should be: " + sourcePath + " Error is: " + ex);

                    }
                    command.Dispose();

                    //Making log file to record everything done
                    using (StreamWriter sw = File.AppendText(PathB.Text +@"Kuvat\Log.Txt"))
                    {
                        if (Iteration == 0)
                        {
                            sw.WriteLine(DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'"));
                            sw.WriteLine("User who made the change: "+ System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                            sw.WriteLine("");
                            Iteration = 1;
                        }
                        sw.WriteLine("Moved " + filename + " from " + sourcePath + " to " + pathString);
                    }

                    //Making progress indicators
                    counter++;
                    Left1 = count - counter;
                    Done.Text = counter.ToString();
                    Left.Text = Left1.ToString();

                    //Progress bar
                    ProgressB.Minimum = 0;
                    ProgressB.Maximum = count + 1;
                    ProgressB.Value = counter;
                    if (ProgressB.Value == ProgressB.Maximum)
                    {
                        Left1++;
                        Left.Text = Left1.ToString();
                        ProgressB.Value = 0;
                        _ = MessageBox.Show("Done. Moved and converted " + counter + " images");
                    }

                    //Listbox colour change
                    //https://stackoverflow.com/questions/2554609/c-sharp-changing-listbox-row-color

                    filename = "";
                    if (count != i)
                    {
                        InputB.SelectedIndex = i;
                        InputB2.SelectedIndex = i;
                    }
                }
                Left1 = 0;
                counter = 0;
                Iteration = 0;
                cnn.Close();
            }
           
        }

        public void Connect_Click(object sender, EventArgs e)
        {
            this.ActiveControl = convert;
            if (DBName.Text != "")
            {
                try
                {
                    if (InputB.Items.Count > 0)
                    {
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
                    Odbc = "Select Tiedosto, TuoteNro from TUOTEKUV ORDER BY TuoteNro";
                    command = new OdbcCommand(Odbc, cnn);
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        output = dataReader.GetValue(0).ToString();
                        _ = InputB.Items.Add(output);
                        string output2 = dataReader.GetValue(1).ToString();
                        _ = InputB2.Items.Add(output2);
                    }
                    ConnectionBox.Text = "Connected to: " + DBName.Text;
                }
                catch
                {
                    MessageBox.Show("Invalid Database name!", "Invalid input");
                }
                
            }
            else
            {
                _ = MessageBox.Show("Invalid input");
            }
        }



        private void QualityB_ValueChanged(object sender, EventArgs e)
        {
            Quality = (int)QualityB.Value;
        }

        private ComboBox DBName;
        private Label label2;
        private Label label4;
        private TextBox ConnectionBox;
        private TextBox PathB;
        private Label label5;
        private CheckBox Deletion;
        private CheckBox SmallImages;
        private CheckBox checkBox3;
        private GroupBox Convertion;
    }
}