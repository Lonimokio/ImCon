using System.Windows.Forms.Integration;

namespace ImageConverter
{

    public partial class ImageConversion : Form
    {

        public ImageConversion()
        {
            InitializeComponent();
        }

        private void ImageConversion_Shown(object sender, EventArgs e)
        {
            Main Window1 = new Main();
            ElementHost.EnableModelessKeyboardInterop(Window1);
            Window1.Show();
            this.Hide();
        }
    }
}