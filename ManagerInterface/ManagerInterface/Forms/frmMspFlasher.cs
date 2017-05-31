using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;

namespace PannelloCharger
{
    public partial class frmMspFlasher : Form
    {
        public frmMspFlasher()
        {
            InitializeComponent();
        }

        private void frmMspFlasher_Load(object sender, EventArgs e)
        {

        }
        private void btnEseguiComando_Click(object sender, EventArgs e)
        {
            if (txtFileComandi.Text != "")
            {
                // ExecuteCommandSync(txtFileComandi.Text);

                if (cctlOsConsole.IsProcessRunning)
                {
                    cctlOsConsole.StopProcess();
                    cctlOsConsole.WriteOutput("STOP Esecuzione\n\r", Color.Red);
                }
                else
                {
                    cctlOsConsole.ClearOutput();
                    cctlOsConsole.WriteOutput("Inizio Esecuzione", Color.Blue);
                    cctlOsConsole.ShowDiagnostics= true;
                    cctlOsConsole.UseWaitCursor = true;

                    cctlOsConsole.StartProcess("C:\\TI\\MSP430Flasher_1.3.7\\MSP430Flasher.exe", "");
                }
            }
        }

        public void ExecuteCommandSync(object command)
        {
            int lineCount = 0;
            StringBuilder output = new StringBuilder();

            try
            {

                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                //txtOutputCoamndo.Text = "";
                txtCmdExitCode.Text = "";

                //this.

                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;

                // Now we create a process, assign its ProcessStartInfo and start it
                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.OutputDataReceived += new DataReceivedEventHandler(CommandOutputHandler);


                /*
                proc.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    // Prepend line numbers to each line of the output.
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        lineCount++;
                        output.Append("\r\n[" + lineCount + "]: " + e.Data);


                        //
                         Console.WriteLine(e.Data);
                        Application.DoEvents();
                    }
                });
*/
                proc.Start();

                proc.BeginOutputReadLine();
                proc.WaitForExit();

                // Write the redirected output to this application's window.
                //txtOutputCoamndo.Text = output.ToString();

                //proc.WaitForExit();
                proc.Close();


                // Get the output into a string
                //string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                //Console.WriteLine(result);
                //txtOutputCoamndo.Text = result;
                //txtCmdExitCode.Text = proc.ExitCode.ToString();

            }
            catch (Exception Ex)
            {
                // Log the exception
            }
        }


        void CommandOutputHandler(object sender, DataReceivedEventArgs e)
        {
            /*
            Trace.WriteLine(e.Data);
            this.BeginInvoke(new MethodInvoker(() =>
            {
                txtOutputCoamndo.AppendText(e.Data ?? string.Empty);
                txtOutputCoamndo.AppendText("\r\n");
            }));
            */
            Application.DoEvents();
        }

        private void cctlOsConsole_Load(object sender, EventArgs e)
        {

        }
    }
}
