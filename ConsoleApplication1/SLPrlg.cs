using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;


using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SLProlog
{
	public class SLProlog
	{
		static public void Main(string[] args)
		{
           	string goalstr = "main(R)";

			// コマンドライン
			if(args.Length == 0)
			{
				Console.WriteLine("Usage: SLPrlg.exe source [goal]");
				return;
			}
            // ソースコードの読み込み
            string source = new StreamReader(new FileStream(args[0], FileMode.Open), 			
            								Encoding.GetEncoding("UTF-8")).ReadToEnd();
			if(args.Length == 2)
			{
				goalstr = args[1];
			}

            // CSProlog環境の作成
            CSProlog.Env env = new CSProlog.Env();
            // プログラムのロード
            env.LoadProgramFromString(source);
            // デフォルトのゴール
            
            
            
            CSProlog.Term goal = CSProlog.Prolog.StrToTerm(goalstr);
			goal.SetScopeNum(0);
			
			bool isfail = true;
            Console.WriteLine("Goal: " + goal.ToStr());
            foreach(Dictionary<string, CSProlog.Term> dict in env.Solve(goal))
            {
                foreach (KeyValuePair<string, CSProlog.Term> i in dict)
                {
                    // res += i.Key + " = " + i.Value.ToStr() + "\r\n";
                	if(i.Key.EndsWith("$0"))
                	{
                		CSProlog.Term t = env.ResolveTerm(i.Value, dict);
	                    Console.WriteLine("(*)" + i.Key + " = " + t.ToStr());
                	}
                }
			AskAction:
                Console.Write("?");
                char ch = (char)Console.Read();
                Console.WriteLine("");
                if(ch == ';')
                {
                	continue;
                }
                else if(ch == '.')
                {
                	isfail = false;
                	break;
                }
                goto AskAction;
            }
            if(isfail)
            {
            	Console.WriteLine("false.");
           	}
		}
	}

}
