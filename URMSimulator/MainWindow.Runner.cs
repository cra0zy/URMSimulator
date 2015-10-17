using System;
using System.Collections.Generic;
using System.Threading;

using Xwt;

namespace URMSimulator
{
    public partial class MainWindow
    {
        Thread runThread;
        bool running = false;
        bool debugbreak = false;
        int regCount = 10;

        List<int> breakpoints;
        bool debugging;

        private void Run()
        {
            if (running)
            {
                EndRun("Aborted");
                return;
            }

            var reg = new int[10000];
            string[] reginit = entry1.Text.Split(' ');

            for (int i = 0; i < reginit.Length; i++)
            {
                try
                {
                    reg[i + 1] = int.Parse(reginit[i]);
                }
                catch { }
            }

            ShowMessage("Compiling...");
            string outputmessage = "";
            var result = URMCompiler.Compile(te1.Document.Text.Split(new [] { "\r\n", "\r", "\n" }, StringSplitOptions.None), out outputmessage);

            if (result)
            {
                ShowMessage("Running...");
                runMenuItem.Label = "Cancel Run";
                running = true;

                debugging = debugMenuItem.Checked;
                breakpoints = te1.GetBreakpoints();

                runThread = new Thread(new ThreadStart(() => RunThread(reg)));
                runThread.Start();
            }
            else
                ShowError(outputmessage);
        }

        private void EndRun(string message)
        {
            try
            {
                runThread.Abort();
            }
            catch { }

            ShowMessage(message);
            runMenuItem.Label = "Run";
            running = false;

            button1.Visible = false;
            te1.HighlightDebuggingLine(-1);
        }

        private void PauseDebugging(string message, int line)
        {
            debugbreak = true;

            Application.Invoke(delegate
                {
                    ShowMessage(message);
                    te1.HighlightDebuggingLine(line);
                    button1.Visible = true;
                });
        }

        private void ContinueDebugging()
        {
            debugging = debugMenuItem.Checked;
            breakpoints = te1.GetBreakpoints();

            button1.Visible = false;
            te1.HighlightDebuggingLine(-1);
            debugbreak = false;
        }

        private void ShowError(string message)
        {
            EndRun("<font color=\"red\">" + message + "</font>");
        }

        private void ShowMessage(string data)
        {
            var html = "<html><head><style>body {background-color: " + vpanned1.BackgroundColor.ToHexString().Substring(0, 7) + "; color: " + label2.TextColor.ToHexString().Substring(0, 7) + ";}</style></head><body><font size=\"2\">" + data + "</font></body></html>";
            web1.LoadHtml(html, "");
        }

        private string CreateMessage(int[] data, URMCommand com, bool bc)
        {
            string writeOut = "";

            for (int i = 1; i < regCount + 1; i++)
            {
                if (com != null && ((com.args.Length == 1 && com.args[0] == i) || (com.args.Length > 1 && ((bc && com.args[0] == i) || (!bc && com.args[1] == i)))))
                    writeOut += "<span style=\"background-color: #400000;color: #ffffff;\">" + data[i] + "</span> ";
                else
                    writeOut += data[i] + " ";
            }

            return writeOut;
        }

        private void RunThread(int[] reg)
        {
            var urmcommands = URMCompiler.urmcommands;
            string tmpData = "";

            for (int i = 0; i < urmcommands.Count; i++)
            {
                var com = urmcommands[i];
                var newi = i;
                if (debugging && breakpoints.Contains(i + 1))
                    tmpData = CreateMessage(reg, com, true);

                if (com.type == 'S')
                    reg[com.args[0]]++;
                else if (com.type == 'Z')
                    reg[com.args[0]] = 0;
                else if (com.type == 'T')
                    reg[com.args[1]] = reg[com.args[0]];
                else if (com.type == 'J' && reg[com.args[0]] == reg[com.args[1]])
                    newi = com.args[2] - 2;

                if (debugging && breakpoints.Contains(i + 1))
                {
                    PauseDebugging(tmpData + "<br>" + CreateMessage(reg, com, false), i + 1);

                    while (debugbreak) 
                    { 
                        Thread.Sleep(10);
                    }
                }

                i = newi;
            }

            Application.Invoke(delegate
                {
                    ShowMessage(CreateMessage(reg, null, false));
                    runMenuItem.Label = "Run";
                    running = false;
                });
        }
    }
}

