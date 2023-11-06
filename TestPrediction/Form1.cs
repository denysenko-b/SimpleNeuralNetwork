using NeuralNetwork.Core;
using NeuralNetwork.Core.Data;
using NeuralNetwork.Core.Data.Image;
using System.Diagnostics;
using System.Windows.Forms;

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        Network? network;

        public Form1()
        {
            InitializeComponent();
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            DialogResult res = loadNetworkFIleDialog.ShowDialog();

            if (res.HasFlag(DialogResult.OK))
            {
                string path = loadNetworkFIleDialog.FileName;

                network = NetworkSaveHelper.Load(path);
            }

        }

        private void ImgBtn_Click(object sender, EventArgs e)
        {

            DialogResult res = openImageDialog.ShowDialog();

            if (res.HasFlag(DialogResult.OK))
            {
                string path = openImageDialog.FileName;

                Image img = Image.FromFile(path);

                pictureBox1.Image?.Dispose();
                pictureBox1.Image = img;
            }
        }

        private void predictBtn_Click(object sender, EventArgs e)
        {

            if (network == null)
            {
                MessageBox.Show("First you need to load network");
                return;
            }

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("First you need to load image");
                return;
            }

            if (int.TryParse(widthTextBox.Text, out int width) == false)
            {
                MessageBox.Show("Width is not a number");
                return;
            }

            if (int.TryParse(heightTextBox.Text, out int height) == false)
            {
                MessageBox.Show("Height is not a number");
                return;
            }

            using Bitmap bmp = new Bitmap(pictureBox1.Image, width, height);

            float[] inputs = FastImage.FromBmp(bmp).Flat(FastImageColorMode.Grayscale);

            var (predictedClass, _) = network.Predict(inputs);

            textBox1.Text = predictedClass.ToString();
        }
    }
}