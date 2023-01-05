using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using System.Data.Odbc;
using System.Diagnostics;

namespace ImageConverter
{

    public partial class ImageConversion : Form
    {
        //Variables below here
        private string type = "webp";
        private string filename = "";
        private string Checker;
        private string separator;
        private string TempS;
        private string Converted;
        private int Quality;
        private int count;
        private int Iteration = 0;
        private int counter = 0;
        private int Left1;
        //Variables abowe here

        public ImageConversion()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            type = TypeB.Text;
        }

        private void convert_Click(object sender, EventArgs e)
        {
            //Opening ODBC connection
            OdbcCommand command;
            OdbcDataReader dataReader;
            OdbcConnection cnn;
            string connectionstring;
            string Odbc, output;

            connectionstring = "Driver={Pervasive ODBC Unicode Interface}; ServerName=Localhost;DBQ=" + DBName.Text + "; TransportHint=TCP;";
            cnn = new OdbcConnection(connectionstring);
            cnn.Open();
            //Reading database and inserting values into listboxes
            Odbc = "Select Tiedosto, TuoteNro from TUOTEKUV";
            command = new OdbcCommand(Odbc, cnn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                output = dataReader.GetValue(0).ToString();
                _ = InputB.Items.Add(output);
                string output2 = dataReader.GetValue(1).ToString();
                _ = InputB2.Items.Add(output2);
            }

            //Making sure rigth index is selected. Also getting amount of items.
            InputB.SelectedIndex = 0;
            count = InputB.Items.Count;
            InputB2.SelectedIndex = 0;


            //Looping trough to procces images
            for (int i = 0; i <= count; i++)
            {
                //Changing path in a way to prevent fails. Double '
                _ = InputB.Text.Replace("'", "''");
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
                            File.Delete(InputB.Text);
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
                string folderName = @"C:\winskj\Kuvat";
                string pathString = System.IO.Path.Combine(folderName, "Tuote-numero-" + InputB2.Text);
                _ = System.IO.Directory.CreateDirectory(pathString);

                separator = Converted;

                int length = separator.Length;
                length--;

                //getting filename by running trough until hitting \
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
                //Putting rigth filenames and reseting values
                filename = TempS;

                TempS = "";

                //Updating filepaths
                string sourcePath = Converted;
                string sourceFile = System.IO.Path.Combine(sourcePath);
                string destFile = System.IO.Path.Combine(pathString, filename);
                //Catching possible errors
                try
                {
                    System.IO.File.Move(sourceFile, destFile);
                }
                catch (Exception)
                {

                }

                //Updating database
                OdbcDataAdapter adapter = new();
                string odbc = "UPDATE TUOTEKUV SET Tiedosto = '" + pathString + @"\" + filename + "' Where Tiedosto = " + "'" + InputB.Text + "'";
                command = new OdbcCommand(odbc, cnn);
                adapter.UpdateCommand = new OdbcCommand(odbc, cnn);
                try
                {
                    _ = adapter.UpdateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("<<< catch : " + ex.ToString());
                    using StreamWriter sw = File.AppendText(@"C:\winskj\Kuvat\Failed.Txt");
                    sw.WriteLine("Error in Sql syntax in " + filename + " this should be manually fixed. Path to it should be: " + sourcePath + " Error is: " + ex);

                }
                command.Dispose();

                //Making log file to record everything done
                using (StreamWriter sw = File.AppendText(@"C:\winskj\Kuvat\Log.Txt"))
                {
                    if (Iteration == 0)
                    {
                        sw.WriteLine(DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'"));
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
                InputB.SelectedIndex = i;
                InputB2.SelectedIndex = i;
            }

            Iteration = 0;
            cnn.Close();
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (DBName.Text != "")
            {
                Connect.Visible = false;
                DBName.Visible = false;
                FLabel.Visible = false;
                DBName.Enabled = false;
                TypeB.Visible = true;
                SLabel.Visible = true;
                convert.Visible = true;
                Back.Visible = true;
                DoneL.Visible = true;
                LeftL.Visible = true;
                ProgressB.Visible = true;
                Left.Visible = true;
                Done.Visible = true;
            }
            else
            {
                _ = MessageBox.Show("Invalid input");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect.Visible = true;
            DBName.Visible = true;
            FLabel.Visible = true;
            DBName.Enabled = true;
            TypeB.Visible = false;
            SLabel.Visible = false;
            convert.Visible = false;
            Back.Visible = false;
            DoneL.Visible = false;
            LeftL.Visible = false;
            ProgressB.Visible = false;
            Left.Visible = false;
            Done.Visible = false;
        }

        private void QualityB_ValueChanged(object sender, EventArgs e)
        {
            Quality = (int)QualityB.Value;
        }

        private ComboBox DBName;
    }
}