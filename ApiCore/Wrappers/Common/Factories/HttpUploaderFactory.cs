using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using System.Runtime.InteropServices;
using System.Net;
using System.IO;

namespace ApiCore
{
    public delegate void HttpUploaderFormProgressHandler(object sender, UploadFormProgressEventArgs e);
    public class UploadFormProgressEventArgs : EventArgs
    {
        public long CurrentBytesRead = 0;
        public long CurrentBytesWritten = 0;
        public long TotalBytesRead = 0;
        public long TotalBytesWritten = 0;
        public long Size = 0;
        public float PercentComplete = 0f;
        public string CurrentField = "";
        public string CurrentFieldValue = "";
    }

    public delegate void HttpUploaderFileProgressHandler(object sender, UploadFileProgressEventArgs e);
    public class UploadFileProgressEventArgs : EventArgs
    {
        public int UploadedFiles = 0;
        public int TotalFiles = 0;
        public long CurrentBytesRead = 0;
        public long CurrentBytesWritten = 0;
        public long TotalBytesRead = 0;
        public long TotalBytesWritten = 0;
        public long FileSize = 0;
        public float CurrentFilePercentComplete = 0f;
        public string CurrentFieldName = "";
        public string CurrentFile = "";
    }


    public class HttpUploaderFactory
    {
        public HttpUploaderFactory()
        {
        }

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
            System.UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            System.UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
            System.UInt32 dwMimeFlags,
            out System.UInt32 ppwzMimeOut,
            System.UInt32 dwReserverd
        );

        private string getMimeType(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename + " not found");

            byte[] buffer = new byte[256];
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }
            try
            {
                System.UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                System.IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch
            {
                return "application/unknown";
            }
        }

        public event HttpUploaderFormProgressHandler UploadFormProgress;
        protected virtual void OnFormFieldUploadProgress(UploadFormProgressEventArgs e)
        {
            if (UploadFormProgress != null)
            {
                UploadFormProgress(this, e);
            }
        }

        public event HttpUploaderFileProgressHandler UploadFileProgress;
        protected virtual void OnFileUploadProgress(UploadFileProgressEventArgs e)
        {
            if (UploadFileProgress != null)
            {
                UploadFileProgress(this, e);
            }
        }

        public string Upload(string url, NameValueCollection formItems, NameValueCollection files)
        {
            //long length = 0;
            int totalItems = 0;
            int itemsCompleted = 0;

            int formFieldsItems = 0;
            int fileFieldsItems = 0;

            int formFieldsCompleted = 0;
            int fileFieldsCompleted = 0;

            // разделитель полей данных в POST запросе
            string boundary = "--xtr---" + DateTime.Now.Ticks.ToString("x");

            // создаем запрос к переданному нам URL
            HttpWebRequest hr = (HttpWebRequest)HttpWebRequest.Create(url);
            hr.Method = "POST"; // метод запроса - POST
            hr.KeepAlive = true; // постоянное подключение
            hr.UserAgent = "xternalx uploader/0.1a"; // копирайт, хуле
            // указываем разделитель переданных параметров
            hr.ContentType = "multipart/form-data; boundary=" + boundary;

            Stream reqStream = hr.GetRequestStream(); // new System.IO.MemoryStream();

            UploadFormProgressEventArgs formEventArgs = new UploadFormProgressEventArgs();
            UploadFileProgressEventArgs filesEventArgs = new UploadFileProgressEventArgs();

            // получаем байты разделителя, для того, чтобы записать их в запрос к URL
            byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            if (formItems != null)
            {
                formFieldsItems = formItems.Count;
                totalItems += formFieldsItems;
                // шаблон передаваемых строковых параметров "формы"
                string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

                // записываем переданные параметры "формы" в поток
                foreach (string key in formItems.Keys)
                {
                    formEventArgs.CurrentField = key;
                    formEventArgs.CurrentFieldValue = formItems[key];
                    formEventArgs.CurrentBytesRead = 0;
                    formEventArgs.CurrentBytesWritten = 0;

                    string formItem = string.Format(formdataTemplate, key, formItems[key]);
                    byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(formItem);

                    formEventArgs.CurrentBytesRead += formItems[key].Length;
                    formEventArgs.TotalBytesRead += formItems[key].Length;

                    reqStream.Write(formItemBytes, 0, formItemBytes.Length);

                    formEventArgs.CurrentBytesWritten += formItems[key].Length;
                    formEventArgs.TotalBytesWritten += formItems[key].Length;
                    formEventArgs.PercentComplete = (float)((float)formEventArgs.CurrentBytesWritten / formItemBytes.Length) * 100;
                    itemsCompleted++;
                    formFieldsCompleted++;

                    this.OnFormFieldUploadProgress(formEventArgs);
                }
                // дописали разделитель данных
                reqStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                //reqStream.Position = 0;
            }

            if (files != null)
            {
                fileFieldsItems = files.Count;
                filesEventArgs.TotalFiles = fileFieldsItems;
                totalItems += fileFieldsItems;
                // шаблон для передаваемых файлов "формы"
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";

                foreach (string formFieldName in files.Keys)
                {
                    filesEventArgs.CurrentFile = files[formFieldName];
                    filesEventArgs.CurrentFieldName = formFieldName;
                    filesEventArgs.CurrentBytesRead = 0;
                    filesEventArgs.CurrentBytesWritten = 0;

                    string header = string.Format(headerTemplate, formFieldName, files[formFieldName], this.getMimeType(files[formFieldName]));
                    //System.Windows.Forms.MessageBox.Show(header);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                    reqStream.Write(headerbytes, 0, headerbytes.Length);

                    FileStream fileStream = new FileStream(files[formFieldName], FileMode.Open, FileAccess.Read);
                    filesEventArgs.FileSize = new FileInfo(files[formFieldName]).Length;
                    byte[] buffer = new byte[1024];

                    int bytesRead = 0;

                    filesEventArgs.CurrentBytesRead = 0;
                    filesEventArgs.CurrentBytesWritten = 0;

                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        filesEventArgs.CurrentBytesRead += bytesRead;
                        filesEventArgs.TotalBytesRead += bytesRead;

                        reqStream.Write(buffer, 0, bytesRead);

                        filesEventArgs.CurrentBytesWritten += bytesRead;
                        filesEventArgs.TotalBytesWritten += bytesRead;
                        filesEventArgs.CurrentFilePercentComplete = (float)((float)filesEventArgs.CurrentBytesWritten / filesEventArgs.FileSize) * 100;


                        this.OnFileUploadProgress(filesEventArgs);
                    }

                    itemsCompleted++;
                    fileFieldsCompleted++;
                    filesEventArgs.UploadedFiles = fileFieldsCompleted;
                    reqStream.Write(boundaryBytes, 0, boundaryBytes.Length);


                    fileStream.Close();
                }
            }

            //Stream reqStream = hr.GetRequestStream();

            //reqStream.Position = 0;
            //byte[] bytebuffer = new byte[1024];
            //while(reqStream.Read(bytebuffer, 0, bytebuffer.Length) != 0)
            //{
            //    reqStream.Write(bytebuffer, 0, bytebuffer.Length);
            //}
            //reqStream.Close();
            reqStream.Close();


            HttpWebResponse r = (HttpWebResponse)hr.GetResponse();
            StreamReader sr = new StreamReader(r.GetResponseStream());

            return sr.ReadToEnd();
        }
    }
}
