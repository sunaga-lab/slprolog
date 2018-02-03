using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CSProlog;

namespace SLProlog
{
    public partial class Page : UserControl
    {
        SolveEnumerator se;
        Env env;
        public Dictionary<string, string> args;  
           
        public Page()
        {
            InitializeComponent();
            se = null;
            env = null;
            
        }

        public void WriteInfoLog(string text)
        {
            LogBox.Text += text + "\n";
        }

        private void ProgRun_Click(object sender, RoutedEventArgs e)
        {
            env = new Env();
            env.ClearProgram();
            env.LoadProgramFromString(ProgText.Text);
            env.PrintCallback = this.PrintCallback;
            Term goal = env.LoadTermFromString(ProgGoal.Text);

            se = env.Start(goal);
            WriteInfoLog("Program loaded.");

            ProgNext_Click(sender, e);

        }

        string StdOut;
        public void PrintCallback(int mode, string val)
        {
            StdOut += val;
        }

        private void ProgNext_Click(object sender, RoutedEventArgs e)
        {
            WriteInfoLog("Start solving...");

            StdOut = "===== Output =====\r\n";
            if (se == null)
                return;
            bool ok = se.DoSolve();

            //res += "Goal: " + goal.ToStr() + "\r\n";
            if (ok)
            {
                StdOut += "\r\n===== Variables =====\r\n";
                foreach (KeyValuePair<string, Term> i in se.GetCurrent())
                {
                    if (i.Key.EndsWith("$0"))
                    {
                        StdOut += i.Key.Substring(0, i.Key.Length - 2) + " = " + env.ResolveTerm(i.Value, se.GetCurrent()).ToStr() + "\r\n";
                    }
                }
                StdOut += "\r\n===== Result =====\r\n";
                StdOut += "true.\r\n";
            }
            else
            {
                StdOut += "\r\n===== Result =====\r\n";
                StdOut += "false.\r\n";
            }
            ProgNextBtn.IsEnabled = ok;
            ProgResult.Text = StdOut;
            WriteInfoLog("Finish.");

        }
        
        public void SourceDownloadCompleted(Object sender, DownloadStringCompletedEventArgs e)
        {
            WriteInfoLog("Program download complete.");
            ProgText.Text = e.Result;
		}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (args.ContainsKey("src"))
            {
                WriteInfoLog("Downloading program from " + args["src"] + "...");
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += this.SourceDownloadCompleted;
                webClient.DownloadStringAsync(new Uri(args["src"]));
            }
        }

    }
}
