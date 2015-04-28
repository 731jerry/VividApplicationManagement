using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using System.Diagnostics;

namespace VividManagementApplication
{


    internal class PrintControllerFormat
    {
        private string _Name = String.Empty;
        private ImageFormat _Format = null;
        private ImageCodecInfo _Codec = null;

        public string Name { get { return _Name; } }
        public ImageFormat Format { get { return _Format; } }
        public ImageCodecInfo Codec { get { return _Codec; } }


        private PrintControllerFormat() { }


        public static PrintControllerFormat[] Formats
        {
            get
            {
                ArrayList a = new ArrayList();

                Type type = typeof(ImageFormat);
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);

                foreach (PropertyInfo property in properties)
                    if (property.PropertyType == type)
                    {
                        PrintControllerFormat format = new PrintControllerFormat();
                        format._Name = property.Name;
                        format._Format = (ImageFormat)property.GetValue(null, null);
                        format._Codec = GetImageCodecInfo(format._Format);
                        a.Add(format);
                    }

                PrintControllerFormat[] formats = new PrintControllerFormat[a.Count];
                a.CopyTo(formats);

                return formats;
            }
        }

        public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo codec in codecs)
                if (codec.FormatID == format.Guid)
                    return codec;

            return null;
        }
    }

    internal class PrintControllerFile : PreviewPrintController
    {
        private ImageFormat _Format = null;
        private float _Scale = 1f;
        private long _Quality = 75L;
        private string _Output = String.Empty;

        private ImageCodecInfo _Codec = null;
        private int _Page = 0;
        private Metafile _Metafile = null;

        private Boolean _isPrintToImageFile = true;

        public PrintControllerFile()
        {
            _Format = ImageFormat.Bmp;
            _Scale = 1f;
            _Quality = 1L;
            _isPrintToImageFile = false;

            _Codec = PrintControllerFormat.GetImageCodecInfo(_Format);
        }

        public PrintControllerFile(ImageFormat format, float scale, long quality, string output)
        {
            if (quality < 0) throw new ArgumentOutOfRangeException("quality", quality, "Quality must be between 0 and 100");
            if (quality > 100) throw new ArgumentOutOfRangeException("quality", quality, "Quality must be between 0 and 100");

            _Format = format;
            _Scale = scale;
            _Quality = quality;
            _Output = output;

            _Codec = PrintControllerFormat.GetImageCodecInfo(_Format);

            string dir = Path.GetDirectoryName(_Output);
            if (dir.Length > 0)
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
        }

        public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
        {
            _Page++;

            return base.OnStartPage(document, e);
        }

        public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
        {
            base.OnEndPage(document, e);

            // Get the current Metafile
            PreviewPageInfo[] ppia = GetPreviewPageInfo();
            PreviewPageInfo ppi = ppia[ppia.Length - 1];
            Image image = ppi.Image;
            _Metafile = (Metafile)image;

            //			using ( _Metafile )
            {
                if (_Format == ImageFormat.Emf) { _Metafile.Save(PagePath, _Format); return; }
                if (_Format == ImageFormat.Wmf) { _Metafile.Save(PagePath, _Format); return; }

                //				_Metafile.Save( PagePath, _Format );
                SaveViaBitmap(document, e);
            }
        }

        protected string PagePath
        {
            get
            {
                //return _Output + "." + _Page.ToString("000") + Extension;
                return _Output;
            }
        }

        protected string Extension
        {
            get
            {
                if (_Format == ImageFormat.Emf) return ".emf";
                if (_Format == ImageFormat.Wmf) return ".wmf";

                if (_Codec == null) return ".unknown";

                string[] extensions = _Codec.FilenameExtension.Split(new char[] { ';' });
                if (extensions.Length < 1) { Debug.Assert(false); return ".unknown"; }
                if (extensions[0].Length < 1) { Debug.Assert(false); return ".unknown"; }

                string extension = extensions[0].Substring(1);

                return extension.ToLower();
            }
        }

        protected void SaveViaBitmap(PrintDocument document, PrintPageEventArgs e)
        {
            int width = e.PageBounds.Width;
            int height = e.PageBounds.Height;

            using (Bitmap bitmap = new Bitmap((int)(width * _Scale), (int)(height * _Scale)))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);

                if (_Scale != 1) graphics.ScaleTransform(_Scale, _Scale);

                Point point = new Point(0, 0);
                Graphics.EnumerateMetafileProc callback = new Graphics.EnumerateMetafileProc(PlayRecord);
                graphics.EnumerateMetafile(_Metafile, point, callback);

                if (_Scale == 1 || true)
                {
                    Save(bitmap);
                }
                else
                {
                    using (Bitmap bitmap2 = new Bitmap(width, height))
                    using (Graphics graphics2 = Graphics.FromImage(bitmap2))
                    {
                        graphics2.DrawImage(bitmap, 0, 0, width, height);
                        Save(bitmap2);
                    }
                }
            }
        }

        protected bool PlayRecord(
            EmfPlusRecordType recordType,
            int flags,
            int dataSize,
            IntPtr data,
            PlayRecordCallback callbackData)
        {
            byte[] dataArray = null;
            if (data != IntPtr.Zero)
            {
                // Copy the unmanaged record to a managed byte buffer 
                // that can be used by PlayRecord.
                dataArray = new byte[dataSize];
                Marshal.Copy(data, dataArray, 0, dataSize);
            }

            _Metafile.PlayRecord(recordType, flags, dataSize, dataArray);

            return true;
        }

        protected void Save(Bitmap bitmap)
        {
            if (!_isPrintToImageFile)
            {
                MainWindow.SIGN_BITMAP = bitmap;
                return;
            }

            if (_Format == ImageFormat.Jpeg)
            {
                EncoderParameters parameters = new EncoderParameters(1);
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, _Quality);
                parameters.Param[0] = parameter;

                bitmap.Save(PagePath, _Codec, parameters);
                return;
            }

            bitmap.Save(PagePath, _Format);
        }
    }
}
