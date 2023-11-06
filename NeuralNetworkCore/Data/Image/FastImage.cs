using System.Runtime.InteropServices;
using System.Drawing;

namespace NeuralNetwork.Core.Data.Image
{
    public sealed class FastImage
    {
        #region Fields, Properties

        private readonly byte[] _array;
        private readonly int Length;
        private readonly int Pixels;

        public readonly int Width;
        public readonly int Height;

        #endregion

        #region Constructors

        private FastImage(int width, int height, byte[] bytes)
        {
            Width = width;
            Height = height;
            _array = bytes;
            Pixels = width * height;
            Length = bytes.Length;
        }

        public FastImage(int width, int height) : this(width, height, new byte[width * height * 3])
        {

        }

        #endregion

        #region Private Methods

        private int GetIndex(int x, int y) => 3 * y * Width + 3 * x;

        private float[] FlatRGB()
        {
            float[] flatten = new float[Length];

            for (int i = 0; i < Length; i++)
            {
                flatten[i] = _array[i] / 255f;
            }

            return flatten;
        }

        private float[] FlatGrayscale()
        {
            int len = Length / 3;

            float[] flatten = new float[len];

            int index = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    GetPixel(x, y, out byte r, out byte g, out byte b);

                    flatten[index++] = (.299f * r + .587f * g + .114f * b) / 255f;
                }
            }

            //for (int i = 0; i < len; i++)
            //{
            //    flatten[i] = (.299f * _array[3 * i] + .587f * _array[3 * i + 1] + .114f * _array[3 * i + 2]) / 3 * 255f;
            //}

            return flatten;
        }



        #endregion

        #region Pixel Manipulation

        public void SetPixel(int x, int y, byte r, byte g, byte b)
        {
            int index = GetIndex(x, y);
            _array[index] = r;
            _array[index + 1] = g;
            _array[index + 2] = b;
        }

        public void GetPixel(int x, int y, out byte r, out byte g, out byte b)
        {
            int index = GetIndex(x, y);
            r = _array[index];
            g = _array[index + 1];
            b = _array[index + 2];
        }

        public void MovePixel(int x0, int y0, int x1, int y1)
        {
            int index0 = GetIndex(x0, y0);
            int index1 = GetIndex(x1, y1);

            byte temp = _array[index0];
            _array[index0] = _array[index1];
            _array[index1] = temp;

            temp = _array[index0 + 1];
            _array[index0 + 1] = _array[index1 + 1];
            _array[index1 + 1] = temp;

            temp = _array[index0 + 2];
            _array[index0 + 2] = _array[index1 + 2];
            _array[index1 + 2] = temp;
        }

        #endregion

        #region Image Manipulation

        public float[] Flat(FastImageColorMode colorMode)
            => colorMode switch
            {
                FastImageColorMode.RGB => FlatRGB(),
                FastImageColorMode.Grayscale or _ => FlatGrayscale(),
            };


        public FastImage FlipHorizontal()
        {
            byte[] bytes = new byte[Length];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = GetIndex(x, y);
                    int index2 = GetIndex(Width - x - 1, y);

                    bytes[index] = _array[index2];
                    bytes[index + 1] = _array[index2 + 1];
                    bytes[index + 2] = _array[index2 + 2];
                }
            }

            return new FastImage(Width, Height, bytes);
        }

        public FastImage FlipVertical()
        {
            byte[] bytes = new byte[Length];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = GetIndex(x, y);
                    int index2 = GetIndex(x, Height - y - 1);

                    bytes[index] = _array[index2];
                    bytes[index + 1] = _array[index2 + 1];
                    bytes[index + 2] = _array[index2 + 2];
                }
            }

            return new FastImage(Width, Height, bytes);
        }

        public FastImage Noise(float noise)
        {
            FastImage noised = Clone();

            byte[] bytes = noised._array;
            byte[] noiseBytes = new byte[Length];

            Random.Shared.NextBytes(noiseBytes);

            for (int i = 0; i < Length; i++)
            {
                float r = Random.Shared.NextSingle();

                if (r < noise)
                {
                    bytes[i] = noiseBytes[i];
                }
            }

            return noised;
        }

        public FastImage Scale(float scale)
        {
            int nheight = (int)(Height * scale);
            int nwidth = (int)(Width * scale);

            byte[] bytes = new byte[Length];

            for (int y = 0; y < nheight; y++)
            {
                for (int x = 0; x < nwidth; x++)
                {
                    int x2 = (int)(x / scale);
                    int y2 = (int)(y / scale);

                    if (x2 >= 0 && x2 < Width && y2 >= 0 && y2 < Height)
                    {
                        int index = GetIndex(x, y);
                        int index2 = GetIndex(x2, y2);

                        bytes[index2] = _array[index];
                        bytes[index2 + 1] = _array[index + 1];
                        bytes[index2 + 2] = _array[index + 2];
                    }
                }
            }

            int dx = (nwidth - Width) / 2;
            int dy = (nheight - Height) / 2;

            FastImage prescaled = new FastImage(nwidth, nheight, bytes);
            return prescaled.Offset(dx, dy);
        }

        public FastImage Offset(int dx, int dy)
        {
            byte[] bytes = new byte[Length];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int x2 = x + dx;
                    int y2 = y + dy;

                    if (x2 >= 0 && x2 < Width && y2 >= 0 && y2 < Height)
                    {
                        int index = GetIndex(x, y);
                        int index2 = GetIndex(x2, y2);

                        bytes[index2] = _array[index];
                        bytes[index2 + 1] = _array[index + 1];
                        bytes[index2 + 2] = _array[index + 2];
                    }
                }
            }

            return new FastImage(Width, Height, bytes);
        }

        public FastImage Rotate(float angle)
        {
            byte[] bytes = new byte[Length];

            float r00 = MathF.Cos(angle);
            float r01 = -MathF.Sin(angle);
            float r10 = MathF.Sin(angle);
            float r11 = MathF.Cos(angle);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int x2 = (int)(x * r00 - y * r01);
                    int y2 = (int)(x * r10 + y * r11);



                    if (x2 >= 0 && x2 < Width && y2 >= 0 && y2 < Height)
                    {
                        int index = GetIndex(x, y);
                        int index2 = GetIndex(x2, y2);

                        bytes[index2] = _array[index];
                        bytes[index2 + 1] = _array[index + 1];
                        bytes[index2 + 2] = _array[index + 2];
                    }
                }
            }

            return new FastImage(Width, Height, bytes);
        }

        #endregion

        #region Public Methods

        public static FastImage FromBmp(Bitmap bmp)
        {
            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            // This code is specific to a bitmap with 24 bits per pixels.
            int bytes = bmp.Width * bmp.Height * 3;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            return new FastImage(bmp.Width, bmp.Height, rgbValues);
        }

        public static FastImage FromFile(string path)
        {
            using Bitmap bmp = new Bitmap(path);
            return FromBmp(bmp);
        }

        public FastImage Clone()
        {
            byte[] array = new byte[Length];
            Buffer.BlockCopy(_array, 0, array, 0, Length);
            return new FastImage(Width, Height, array);
        }

        #endregion
    }
}