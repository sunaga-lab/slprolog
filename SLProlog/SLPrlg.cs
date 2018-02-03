using System;



using System.Collections.Generic;
using System.IO;
using System.Text;
using CSProlog;


namespace System
{
    //public class MarshalByRefObject{}

}

namespace SLProlog
{
	public class SLProlog
	{
        static public void TestTokenizer()
        {
            Tokenizer tok = new Tokenizer(Token.CT_PROGRAM, "unify(R, af, er+wr).");
            TokenizerJay tokj = new TokenizerJay(tok);
            while (tokj.Advance())
            {
                int type1 = tokj.token();
                string token1 = (string)tokj.value();
                int type2 = tokj.token();
                string token2 = (string)tokj.value();
                if (type1 != type2)
                    throw new System.Exception("E1");
                if (!token1.Equals(token2))
                    throw new System.Exception("E1");
                Console.WriteLine("Type: " + type2.ToString() + ": " + token1);
            }
        }
        
        static public void PrintCallback(int mode, string val)
        {
            Console.Write(val);
		}
        
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
            env.PrintCallback = PrintCallback;
            // プログラムのロード
            env.LoadProgramFromString(source);
            // ゴール
            CSProlog.Term goal = env.LoadTermFromString(goalstr);
			
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
                char ch = (char)System.Console.In.Read();
                Console.WriteLine("");
                if(ch == ';')
                {
                	continue;
                }
                else if (ch == '.')
                {
                    isfail = false;
                    break;
                }
                else if (ch == 's')
                {
                    StringBuilder sb = new StringBuilder();
                    env.GetStackTrace(sb);
                    Console.Write(sb.ToString());
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
