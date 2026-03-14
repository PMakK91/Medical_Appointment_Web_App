using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Rendering;

namespace MedicalAppointmentWebApp.Services
{
    public class QRcodeService : IQRcodeService
    {

        public byte[] GenerateQRCode(string text, int width = 250, int height = 250)
        {
            var barcodeWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height,
                    Margin = 1
                }
            };

            var pixelData = barcodeWriter.Write(text);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                                                 ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }

        public string ScanQRCode(byte[] rgbRawPixels, int width, int height)
        {
            var luminanceSource = new RGBLuminanceSource(rgbRawPixels, width, height);
            var binarizer = new HybridBinarizer(luminanceSource);
            var binaryBitmap = new BinaryBitmap(binarizer);

            var reader = new MultiFormatReader();
            var result = reader.decode(binaryBitmap);

            return result?.Text ?? "Not found";
        }
    }
}
