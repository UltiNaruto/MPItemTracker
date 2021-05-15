using System;
using System.Drawing;

namespace Utils
{
    class ImageUtils
    {
        // Make an outline image.
        public static Image MakeOutline(Image original, Color outline_color, int outline_size)
        {
            byte r, g, b, a;
            int i, j;
            Bitmap32 new_image = new Bitmap32((Bitmap)original);
            Bitmap32 mask = new Bitmap32(new Bitmap(original.Width, original.Height));
            new_image.LockBitmap();
            mask.LockBitmap();

            for (i = 0; i < original.Width; i++)
                for (j = 0; j < original.Height; j++)
                {
                    new_image.GetPixel(i, j, out r, out g, out b, out a);
                    if (a == Color.Transparent.A)
                    {
                        mask.SetAlpha(i, j, Color.Transparent.A);
                        mask.SetRed(i, j, Color.Transparent.R);
                        mask.SetGreen(i, j, Color.Transparent.G);
                        mask.SetBlue(i, j, Color.Transparent.B);
                    }
                    else
                    {
                        mask.SetAlpha(i, j, outline_color.A);
                        mask.SetRed(i, j, outline_color.R);
                        mask.SetGreen(i, j, outline_color.G);
                        mask.SetBlue(i, j, outline_color.B);
                    }
                }

            Bitmap img = new Bitmap(original.Width, original.Height);
            using (Graphics gfx = Graphics.FromImage(img))
            {
                for (i = 0; i < original.Width; i++)
                    for (j = 0; j < original.Height; j++)
                    {
                        mask.GetPixel(i, j, out r, out g, out b, out a);
                        gfx.DrawRectangle(new Pen(Color.FromArgb(a, r, g, b)), new Rectangle(i, j, outline_size, outline_size));
                        new_image.GetPixel(i, j, out r, out g, out b, out a);
                        gfx.DrawRectangle(new Pen(Color.FromArgb(a, r, g, b)), new Rectangle(i, j, 1, 1));
                    }
            }

            mask.UnlockBitmap();
            new_image.UnlockBitmap();
            return (Image)img;
        }
    }
}
