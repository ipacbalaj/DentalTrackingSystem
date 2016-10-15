using System;
using System.Windows.Forms;
using DSA.Common.Controls.Message;
using WinSCP;

namespace DTS.FtpConnection
{
    public static class FTPConnectionHandler
    {

        public static void UploadFile(string ftpFolder, string ftpIP, string ftpUser, string ftpPass)
        {
            try
            {               
                {
                    // Setup session options
                    SessionOptions sessionOptions = new SessionOptions
                    {
                        Protocol = Protocol.Ftp,
                        HostName = ftpIP,
                        UserName = ftpUser,
                        Password = ftpPass,
                    };

                    using (Session session = new Session())
                    {
                        // Connect
                        session.Open(sessionOptions);

                        // Upload files
                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;

                        TransferOperationResult transferResult;
                        var localFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +"\\DTS\\dsa.sdf";
                        var remotePath = ftpFolder + "/dsa.sdf";
                        transferResult = session.PutFiles(localFile, @remotePath, false, transferOptions);

                        // Throw on any error
                        transferResult.Check();
                    }
                }

                MessageBox.Show("Baza de date a fost salvată.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Baza de date nu a putut fi salvată. Verificați conexiunea la FTP");
            }
        }

        public static void  DownloadFile(string ftpFolder, string ftpIP, string ftpUser, string ftpPass)
        {
            try
            {
                {
                    // Setup session options
                    SessionOptions sessionOptions = new SessionOptions
                    {
                        Protocol = Protocol.Ftp,
                        HostName = ftpIP,
                        UserName = ftpUser,
                        Password = ftpPass,
                    };

                    using (Session session = new Session())
                    {
                        // Connect
                        session.Open(sessionOptions);

                        // Upload files
                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;

                        TransferOperationResult transferResult;
                        var localFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DTS\\dsa.sdf";
                        var remotePath = ftpFolder + "/dsa.sdf";
                        transferResult = session.GetFiles(@remotePath,localFile, false, transferOptions);

                        // Throw on any error
                        transferResult.Check();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Baza de date nu a putut fi incărcată. Verificați conexiunea la FTP");
            }
        }

    }
}
