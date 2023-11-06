using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;

namespace NeuralNetwork.Core.Data.Image
{
    public sealed class ImageLoader : IDataLoader
    {
        private DataPoint[] data;
        //private FastImage[] images;

        public DataPoint[] AllData => data;

        public readonly int Width;
        public readonly int Height;

        private readonly string Path;

        public ImageLoader(string path, int width, int height)
        {
            data = Array.Empty<DataPoint>();
            //images = Array.Empty<FastImage>();

            Width = width;
            Height = height;

            Path = path;
        }

        public void LoadData()
        {
            string[] labels = Directory.GetDirectories(Path);


            Debug.WriteLine("Labels:");
            foreach (var label in labels)
            {
                Debug.WriteLine($"=> {System.IO.Path.GetFileName(label)}");
            }

            ConcurrentBag<DataPoint> dataBag = new ConcurrentBag<DataPoint>();
            //ConcurrentBag<FastImage> imagesBag = new ConcurrentBag<FastImage>();

            for (int label = 0; label < labels.Length; label++)
            {
                string labelPath = labels[label];

                Parallel.ForEach(Directory.GetFiles(labelPath), fileName =>
                {

                    //foreach (var fileName in Directory.GetFiles(labelPath))
                    //{
                    try
                    {
                        using System.Drawing.Image image = System.Drawing.Image.FromFile(fileName);

                        using Bitmap bitmap = new Bitmap(image, Width, Height);

                        float[] data = new float[Width * Height];


                        int dataIndex = 0;
                        for (int x = 0; x < Width; x++)
                        {
                            for (int y = 0; y < Height; y++)
                            {
                                data[dataIndex++] = bitmap.GetPixel(x, y).GetBrightness();
                            }
                        }


                        dataBag.Add(new DataPoint(data, label, labels.Length));








                        //    using Bitmap image = new Bitmap(fileName);
                        //    using Bitmap resized = new Bitmap(image, new Size(Width, Height));

                        //    FastImage img = FastImage.FromBmp(resized);
                        //    float[] data = img.Flat(FastImageColorMode.RGB);

                        //    dataBag.Add(new DataPoint(data, label, labels.Length));
                        //    imagesBag.Add(img);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine($"{fileName} is not an image.");
                    }
            });
            //}
        }

            data = dataBag.ToArray();
            //images = imagesBag.ToArray();
            Debug.WriteLine($"Images loaded: {data.Length}");
        }

        public float[] GetImage(Bitmap image)
        {
            using Bitmap resized = new Bitmap(image, new Size(Width, Height));

            float[] data = new float[Width * Height];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    data[i * Width + j] = resized.GetPixel(j, i).GetBrightness();
                }
            }

            return data;
        }

        public float[] GetImage(string path)
        {
            using Bitmap opened = new Bitmap(path);
            return GetImage(opened);
        }

        public void UpdateData()
        {
            //TODO IMAGE MANIPULATION
            //Parallel.For(0, images.Length, i =>
            //{
            //    FastImage img = images[i];

            //    float angle = Random.Shared.NextSingle(-20, 20);
            //    int dx = Random.Shared.Next(-3, 3);
            //    int dy = Random.Shared.Next(-3, 3);
            //    float scale = Random.Shared.NextSingle(0.9f, 1.1f);
            //    float noise = Random.Shared.NextSingle(0, 0.075f);

            //    //FastImage moved = img.Offset(dx, dy);
            //    FastImage rotated = img.Rotate(angle);
            //    //FastImage noised = rotated.Noise(noise);

            //    data[i] = new DataPoint(rotated.Flat(FastImageColorMode.Grayscale), data[i].Label, data[i].NumLabels);
            //});
        }
    }
}
