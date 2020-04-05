using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using Ionic.Zlib;

namespace lwoPatcher
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            cbServer.SelectedIndex = 0; //Choose the first server as the default
        }

        protected string GetMD5HashFromFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static byte[] Decompress(byte[] gzip)
        {
            var ogStream = new MemoryStream(gzip);
            using (var stream = new Ionic.Zlib.ZlibStream(ogStream, Ionic.Zlib.CompressionMode.Decompress))
            {
                var outStream = new MemoryStream();
                const int size = 262145;
                byte[] buffer = new byte[size];

                int read;

                while ((read = stream.Read(buffer, 0, size)) > 0)
                {
                    outStream.Write(buffer, 0, read);
                }

                ogStream.Close();
                stream.Close();
                return outStream.ToArray();
            }
        }

        private void DownloadVersions(WebClient webClient)
        {
            Directory.CreateDirectory(@"..\versions\");
            fileNameLabel.Text = "version.txt";
            fileNameLabel.Refresh();
            webClient.DownloadFile(new Uri("http://cache.lbbstudios.net/lwoclient/version.txt"), @"..\versions\version.txt");

            fileNameLabel.Text = "hotfix.txt";
            fileNameLabel.Refresh();
            webClient.DownloadFile(new Uri("http://cache.lbbstudios.net/lwoclient/hotfix.txt"), @"..\versions\hotfix.txt");

            fileNameLabel.Text = "quickcheck.txt";
            fileNameLabel.Refresh();
            webClient.DownloadFile(new Uri("http://cache.lbbstudios.net/lwoclient/quickcheck.txt"), @"..\versions\quickcheck.txt");
        }

        private void DownloadFile(String line, WebClient webClient)
        {
            //Set up some of the vars:
            string[] tokens = line.Split(',');

            //if (tokens.Length < 5) return;
            String fileName = tokens[0];
            int uncompressedSize = Int32.Parse(tokens[1]);
            String uncompressedChecksum = tokens[2];
            int compressedSize = Int32.Parse(tokens[3]);
            String compressedChecksum = tokens[4];

            //Download the file into a temp file:
            //fileNameLabel.Text = fileName;
            //fileNameLabel.Refresh();
            String sd0Name = uncompressedChecksum[0] + "/" + uncompressedChecksum[1] + "/" + uncompressedChecksum + ".sd0";

            string[] fileNameArray = fileName.Split('/');
            String tempFileName = fileNameArray[fileNameArray.Length - 1] + ".sd0";

            //Create the dirs for this file:
            String foldersToMake = @"..\";
            foreach (string folder in fileNameArray)
            {
                if (!folder.Contains(".")) //!= a file, so it's a folder
                {
                    foldersToMake = foldersToMake + @"\" + folder;
                } else if (folder == ".mayaSwatches") //exception
                {
                    foldersToMake = foldersToMake + @"\" + folder;
                }
            }

            Directory.CreateDirectory(foldersToMake);

            //Make sure we don't already have this file:
            if (File.Exists(@"..\" + fileName))
            {
                if (GetMD5HashFromFile(@"..\" + fileName) == uncompressedChecksum)
                {
                    return;
                }
            }


            try
            {
                // try to download file here
                webClient.DownloadFile(new Uri("http://cache.lbbstudios.net/lwoclient/" + sd0Name), @"..\versions\" + tempFileName);

                //Decompress the sd0 if the file isn't an uncompressed .txt:
                if (fileName != "version.txt" & fileName != "hotfix.txt")
                {
                    if (fileNameArray.Length == 1) fileName = @"versions\" + fileName; //If no subdir, just download to /versions/

                    byte[] data = File.ReadAllBytes(@"..\versions\" + tempFileName).Skip(0x9).ToArray();
                    MemoryStream bos = new MemoryStream(data.Length);

                    byte[] buffer = new byte[262144];
                    int iLocation = 5;
                    bool bIsDecompressing = true;
                   // ICSharpCode.SharpZipLib.Zip.Compression.Inflater decompressor = new ICSharpCode.SharpZipLib.Zip.Compression.Inflater();

                    while (bIsDecompressing)
                    {
                        using (var fileStream = File.Open(@"..\versions\" + tempFileName, FileMode.Open))
                        using (var binaryStream = new BinaryReader(fileStream))
                        {
                            binaryStream.ReadInt32();
                            binaryStream.ReadChar(); //Skipped the SD0 header
                            var totalLength = (int)binaryStream.BaseStream.Length;
                            while (iLocation < totalLength)
                            {
                                int compressedLength = binaryStream.ReadInt32();
                                iLocation += sizeof(int);

                                if (compressedLength == 0)
                                {
                                    bIsDecompressing = false;
                                    break;
                                }

                                int iBytesToSkip = iLocation;

                                byte[] compData = binaryStream.ReadBytes(compressedLength);

                                byte[] decompBuff = Decompress(compData);
                                bos.Write(decompBuff, 0, decompBuff.Length);

                                iLocation += compressedLength;
                            }

                            bIsDecompressing = false;
                        }
                    }

                    // Get the decompressed data  
                    byte[] decompressedData = bos.ToArray();

                    File.WriteAllBytes(@"..\" + fileName, decompressedData);
                    File.Delete(@"..\versions\" + tempFileName);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        //MessageBox.Show(fileName, "404 error! - " + fileName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //Application.Exit();
                        return;
                    }

                    MessageBox.Show(fileName, "Unknown error! - " + fileName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Application.Exit();
                }
            }
           
        }

        private void DownloadFilesFromTxt(WebClient webClient, String versionTxt, bool shouldThread)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = File.ReadLines(versionTxt).Count();
            progressBar1.Value = 0;

            foreach (string line in File.ReadLines(versionTxt))
            {
                if (line.Contains(".")) //To skip any files that don't have a file extension. (and other useless lines)
                {
                    if (shouldThread) {
                        /*new Thread(() =>
                        {
                            Thread.CurrentThread.IsBackground = true;
                            DownloadFile(line, webClient);
                        }).Start();*/

                        string[] tokens = line.Split(',');
                        fileNameLabel.Text = tokens[0];
                        fileNameLabel.Refresh();

                        new Task(() => { DownloadFile(line, webClient); }).RunSynchronously();
                    } else {
                        DownloadFile(line, webClient);
                    }
                    
                    progressBar1.Value++;
                }
            } //foreach loop
        }

        private void bStartPlay_Click(object sender, EventArgs e)
        {
            if (bStartPlay.Text == "Start")
            {
                //Start patching:
                WebClient webClient = new WebClient();
                DownloadVersions(webClient); //These are always re-downloaded.
                DownloadFilesFromTxt(webClient, @"..\versions\version.txt", false);
                DownloadFilesFromTxt(webClient, @"..\versions\index.txt", false);

                DownloadFilesFromTxt(webClient, @"..\versions\frontend.txt", true);
                
                DownloadFilesFromTxt(webClient, @"..\versions\hotfix.txt", false);

                //Only download trunk's files if we selected a trunk server. (We don't care about quickcheck.)
                //if (cbServer.SelectedIndex == 1) {
                //    new Thread(() => {
                //        Thread.CurrentThread.IsBackground = true;
                //        DownloadFilesFromTxt(webClient, @"..\versions\trunk.txt", false);
                //    }).Start();
                //}

                if (cbServer.SelectedIndex == 1) DownloadFilesFromTxt(webClient, @"..\versions\trunk.txt", true);

                //Download done, so change this to a "play" button:
                webClient.Dispose();
                fileNameLabel.Text = "Patching complete!";
                fileNameLabel.Refresh();
                progressBar1.Value = progressBar1.Maximum;

                bStartPlay.Text = "Play!";
                bStartPlay.Refresh();
            }
            else
            {
                //Generate boot.cfg:

                //Start lego_mmog.exe & exit patcher:
                var p = new Process();
                p.StartInfo.FileName = @"LEGO_MMOG.exe";
                p.StartInfo.WorkingDirectory = @"..\client\";
                p.Start();
                Application.Exit();
            }
        }
    }
}
