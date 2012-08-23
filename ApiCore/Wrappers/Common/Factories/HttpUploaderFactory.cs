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
        //public long CurrentBytesRead = 0;
        public long CurrentBytesWritten = 0;
        //public long TotalBytesRead = 0;
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
        //public long CurrentBytesRead = 0;
        public long CurrentBytesWritten = 0;
        //public long TotalBytesRead = 0;
        public long TotalBytesWritten = 0;
        public long FileSize = 0;
        public float TotalPercentComplete = 0f;
        public float FilePercentComplete = 0f;
        public string FieldName = "";
        public string FileName = "";
    }

    public delegate void HttpUploaderLogHandler(object sender, string message);

    public class HttpUploaderFactory
    {
        UploadFormProgressEventArgs formEventArgs;
        UploadFileProgressEventArgs filesEventArgs;

        private CookieCollection responseCookies = null;
        private CookieCollection requestCookies = null;
        
        public int Timeout = 150000;
        public int BufferSize = 4096;

        public bool UseCookies = false;

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

        #region Events

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

        public event HttpUploaderLogHandler OnLog;
        protected virtual void Log(string msg)
        {
            if (OnLog != null)
            {
                OnLog(this, msg);
            }
        }

        #endregion

        private long getFilesLength(NameValueCollection files)
        {
            long size = 0;
            if (files == null)
                return 0;
            foreach (string filename in files.Keys)
            {
                //System.Windows.Forms.MessageBox.Show(files[filename] + " => " +new FileInfo(files[filename]).Length.ToString());
                size += new FileInfo(files[filename]).Length;
            }
            return size;
        }



        private void updateFormUploadEvent(int currentBytes, int totalBytes, float percent)
        {
            this.formEventArgs.CurrentBytesWritten += currentBytes;
            this.formEventArgs.TotalBytesWritten += totalBytes;
            this.formEventArgs.PercentComplete = percent;
        }

        private void initFormUpload(string correntField, string currentValue)
        {
            this.formEventArgs.CurrentField = correntField;
            this.formEventArgs.CurrentFieldValue = currentValue;
            this.formEventArgs.CurrentBytesWritten = 0;
        }

        private void updateFileUploadEvent(int currentBytes, int totalBytes, int filesCompleted)
        {
            this.filesEventArgs.TotalFiles = filesCompleted;
            this.filesEventArgs.CurrentBytesWritten += currentBytes;
            this.filesEventArgs.TotalBytesWritten += totalBytes;
            this.filesEventArgs.FilePercentComplete = (float)this.filesEventArgs.CurrentBytesWritten / this.filesEventArgs.FileSize * 100;
        }

        private void initFileUpload(string correntFile, string currentFieldName)
        {
            this.filesEventArgs.FileName = correntFile;
            this.filesEventArgs.FieldName = currentFieldName;
            this.filesEventArgs.CurrentBytesWritten = 0;
        }

        private string formatFormUploadHeader(string boundary, string field, string value)
        {
            return string.Format(this.formHeaderTemplate, boundary, field, value);
        }

        private string formatFileUploadHeader(string field, string filename, string mime)
        {
            return string.Format(this.fileHeaderTemplate, field, filename, mime);
        }

        private string formHeaderTemplate = "\r\n--{0}\r\nContent-Disposition: form-data; name=\"{1}\";\r\n\r\n{2}";
        private string fileHeaderTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";


        private long calculateContentLength(string boundary, NameValueCollection formItems, NameValueCollection files)
        {
            long size = 0;
            //В HTTP Заголовок ContentLength нужно записывать не длину символов, а длину байт.
            byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            if (formItems != null && formItems.Count > 0)
            {
                foreach (string formFieldName in formItems.Keys)
                {
                    size += System.Text.Encoding.ASCII.GetBytes(this.formatFormUploadHeader(boundary, formFieldName, formItems[formFieldName])).Length;
                }
            }

            if (files != null && files.Count > 0)
            {
                size += this.getFilesLength(files);
                foreach (string fileFieldName in files.Keys)
                {
                    size += System.Text.Encoding.ASCII.GetBytes(this.formatFileUploadHeader(fileFieldName, Path.GetFileName(files[fileFieldName]), this.getMimeType(files[fileFieldName]))).Length + boundaryBytes.Length;
                }
            }
            size += boundaryBytes.Length;
            return size;
        }

        public string Upload(string url, NameValueCollection formItems, NameValueCollection files)
        {
            int totalItems = 0;
            int itemsCompleted = 0;

            int formFieldsItems = 0;
            int fileFieldsItems = 0;

            int formFieldsCompleted = 0;
            int fileFieldsCompleted = 0;

            long totalFilesLength = this.getFilesLength(files);
            this.Log(string.Format("make request to {0}, form values count: {1}, files: {2}", url, ((formItems == null) ? 0 : formItems.Count), ((files == null) ? 0 : files.Count)));

            #region init variables
            // разделитель полей данных в POST запросе
            string boundary = "--xtr---" + DateTime.Now.Ticks.ToString("x");
            // создаем запрос к переданному нам URL
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            if (this.UseCookies)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }
            httpRequest.ProtocolVersion = HttpVersion.Version11;
            httpRequest.Method = "POST"; // метод запроса - POST
            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.142 Safari/535.19";
            httpRequest.Timeout = this.Timeout;
            httpRequest.KeepAlive = false; // постоянное подключение
            //httpRequest.SendChunked = false; // Отправка данных кусками
            httpRequest.UserAgent = "xternalx uploader/0.1a"; // копирайт, хуле :)
            // указываем разделитель переданных параметров
            httpRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // получаем байты разделителя, для того, чтобы записать их в запрос к URL
            byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            httpRequest.ContentLength = this.calculateContentLength(boundary, formItems, files);

            //FileStream reqStream = new FileStream("D:\\stream.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            Stream reqStream = httpRequest.GetRequestStream();

            this.formEventArgs = new UploadFormProgressEventArgs();
            this.filesEventArgs = new UploadFileProgressEventArgs();

            #endregion

            #region form upload
            if (formItems != null && formItems.Count > 0)
            {
                formFieldsItems = formItems.Count;
                totalItems += formFieldsItems;
                // шаблон передаваемых строковых параметров "формы"

                // записываем переданные параметры "формы" в поток
                foreach (string key in formItems.Keys)
                {
                    this.initFormUpload(key, formItems[key]);

                    string formItem = this.formatFormUploadHeader(boundary, key, formItems[key]);// string.Format(formHeaderTemplate, key, formItems[key]);
                    byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(formItem);
                    reqStream.Write(formItemBytes, 0, formItemBytes.Length);

                    this.updateFormUploadEvent(formItems[key].Length, formItems[key].Length, (float)((float)this.formEventArgs.CurrentBytesWritten / formItemBytes.Length) * 100);

                    itemsCompleted++;
                    formFieldsCompleted++;

                    this.OnFormFieldUploadProgress(formEventArgs);
                }
                // дописали разделитель данных
                reqStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            }
            else
            {
                // если список полей формы пуст, нужно дописать boundary
                reqStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            }
            #endregion

            #region files upload
            if (files != null && files.Count > 0)
            {
                fileFieldsItems = files.Count;
                filesEventArgs.TotalFiles = fileFieldsItems;
                totalItems += fileFieldsItems;
                // шаблон для передаваемых файлов "формы"


                foreach (string formFieldName in files.Keys)
                {
                    this.initFileUpload(files[formFieldName], formFieldName);

                    string header = this.formatFileUploadHeader(formFieldName, Path.GetFileName(files[formFieldName]), this.getMimeType(files[formFieldName]));

                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                    reqStream.Write(headerbytes, 0, headerbytes.Length);

                    FileStream fileStream = new FileStream(files[formFieldName], FileMode.Open, FileAccess.Read, FileShare.Read);
                    this.filesEventArgs.FileSize = new FileInfo(files[formFieldName]).Length;
                    byte[] buffer = new byte[this.BufferSize];

                    int bytesRead = 0;

                    try
                    {
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            reqStream.Write(buffer, 0, bytesRead);
                            reqStream.Flush();
                            this.updateFileUploadEvent(bytesRead, bytesRead, fileFieldsCompleted);
                            this.OnFileUploadProgress(filesEventArgs);
                        }

                        itemsCompleted++;
                        fileFieldsCompleted++;
                        filesEventArgs.UploadedFiles = fileFieldsCompleted;

                        fileStream.Close();

                        reqStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    }
                    catch (WebException wex)
                    {
                        // При WebException возможно, что поток на запись будет разорван принудительно, 
                        // чтобы сделать возможным повторную отправку текущего файла, его нужно закрыть.
                        fileStream.Close();
                    }

                }
            }
            #endregion

            reqStream.Close();

            HttpWebResponse r = (HttpWebResponse)httpRequest.GetResponse();
            this.responseCookies = r.Cookies;
            StreamReader sr = new StreamReader(r.GetResponseStream());

            return sr.ReadToEnd();

        }

        public CookieCollection GetResponseCoockies()
        {
            return this.responseCookies;
        }

        public void SetRequestCookies(CookieCollection cookies)
        {
            this.requestCookies = cookies;
        }

        public void SetRequestCookies(string key, string val)
        {
            if (this.requestCookies == null)
            {
                this.requestCookies = new CookieCollection();
            }
            Cookie newCookie = new Cookie(key, val);
            this.requestCookies.Add(newCookie);
        }


    }
}