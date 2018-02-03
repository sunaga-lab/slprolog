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

namespace CSProlog
{

    public delegate void PrintDelegate(int mode, string val);

    /// <summary>
    /// 項
    /// </summary>
    public class Term
    {
        /// <summary>
        /// ForAllTermに渡すためのデリゲート
        /// </summary>
        /// <param name="term">対象の項</param>
        /// <returns></returns>
        public delegate bool ForAllTermDelegate(Term term);
	    /// <summary>
	    /// 変数かどうか
	    /// </summary>
        private bool variable;
        /// <summary>
        /// 値/変数名
        /// </summary>
        private string value;
        /// <summary>
        /// フレーム番号
        /// </summary>
        private int scopenum;
        /// <summary>
        /// 項の子
        /// </summary>
        private List<Term> children;
		private Term reference_term;
		
        public Term()
        {
            variable = false;
            value = "";
            children = null;
            scopenum = -1;
            reference_term = null;            
        }
        
        public void References(Term dest)
        {
        	variable = false;
        	value = "";
        	children = null;
        	reference_term = dest;
        }

        public void SetScopeNum(int num)
        {
            if (scopenum != -1)
                throw new Exception("ScopeNumber override.");
            scopenum = num;
            if(children != null)
                foreach(Term t in children)
                {
                    t.SetScopeNum(num);
                }
        }

        /// <summary>
        /// この項(this)と、子の項すべてにfuncデリゲートへ適用
        /// </summary>
        /// <param name="func">適用するデリゲート</param>
        /// <returns>すべての項に対してのfuncがtrueへ返した場合のみtrueへ返す</returns>
        public bool ForAllTerm(ForAllTermDelegate func)
        {
            if (!func(this))
                return false;
            if (children != null)
                foreach (Term t in children)
                {
                    if (!t.ForAllTerm(func))
                        return false;
                }
            return true;
        }

        /// <summary>
        /// スコープ番号を取得
        /// </summary>
        /// <returns></returns>
        public int GetScopeNum()
        {
        	return scopenum;
        }

        /// <summary>
        /// 項のディープコピー
        /// </summary>
        /// <returns>コピーされた新しい項</returns>
        public Term DeepCopy()
        {
        	return DeepCopy(-1);
        }

        /// <summary>
        /// ディープコピー
        /// </summary>
        /// <param name="newscopenum">新しいスコープ番号</param>
        /// <returns>コピーされた新しい項</returns>
        public Term DeepCopy(int newscopenum)
        {
        	Term term = new Term();
        	term.variable = variable;
        	term.value = value;
        	term.scopenum = newscopenum == -1 ? scopenum : newscopenum;
			term.reference_term = reference_term;			

            if(children != null)
            {
            	term.children = new List<Term>();
                foreach(Term t in children)
                {
		            term.children.Add(t.DeepCopy(newscopenum));
                }       		
			}
			else
				term.children = null;
			return term;
        }
        
        /// <summary>
        /// 変数項を生成
        /// </summary>
        /// <param name="name">変更名</param>
        /// <returns>生成された項</returns>
        public static Term var(string name)
        {
            Term t = new Term();
            t.SetAsVar(name);
            return t;
        }

        /// <summary>
        /// 非変数項を作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="terms">子になる項のリスト</param>
        /// <returns>生成された項</returns>
        public static Term imm(string name, params Term[] terms)
        {
            Term t = new Term();
            t.value = name;
            t.children = new List<Term>();
            foreach (Term c in terms)
            {
                t.children.Add(c);
            }
            return t;
        }

        /// <summary>
        /// 整数を整数項に変換
        /// </summary>
        /// <param name="n">整数</param>
        /// <returns>整数項</returns>
        public static Term Integer(int n)
        {
            return Term.imm(n.ToString());
        }
        /// <summary>
        /// 数値を数値項に変換
        /// </summary>
        /// <param name="n">数値</param>
        /// <returns>数値項</returns>
        public static Term Number(string n)
        {
            return Term.imm(n);
        }

        /// <summary>
        /// Nil項を生成
        /// </summary>
        /// <returns>Nil項</returns>
        public static Term Nil()
        {
            return Term.imm("nil");
        }

        /// <summary>
        /// 空リストを生成
        /// </summary>
        /// <returns>空リスト項</returns>
        public static Term EmptyList()
        {
            return Term.imm("nil");
        }
        
        /// <summary>
        /// カンマリストから項のリストへ変換
        /// </summary>
        /// <param name="commalist">カンマリスト</param>
        /// <returns>項のリスト</returns>
        public static List<Term> CommaListToList(Term commalist)
        {
        	List<Term> lt = new List<Term>();
        	CommaListToList(commalist, lt);
        	return lt;
        }

        /// <summary>
        /// カンマリスト項から項のリストへ変換
        /// </summary>
        /// <param name="commalist">カンマリスト</param>
        /// <param name="lt">変換したものを追加する対象</param>
        public static void CommaListToList(Term commalist, List<Term> lt)
        {
        	if(commalist.GetValue() == "," && commalist.GetPrmCount() == 2){
        		CommaListToList(commalist.GetPrm(0), lt);
        		lt.Add(commalist.GetPrm(1));
        		return;
        	}
        	lt.Add(commalist);
        }

        /// <summary>
        /// 項リストからドットリスト項へ変換
        /// </summary>
        /// <param name="list">項リスト</param>
        /// <returns>ドットリスト項</returns>
        public static Term ListToDotList(List<Term> list)
        {
            return ListToDotList(list, Term.Nil());
        }

        /// <summary>
        /// 項リストからドットリスト項へ変換
        /// </summary>
        /// <param name="list">項リスト</param>
        /// <param name="list">項リスト</param>
        /// <returns>ドットリスト項</returns>
        public static Term ListToDotList(List<Term> list, Term term)
        {
            Term ret = term;
            for (int i = list.Count-1; i >= 0; i--)
            {
                ret = Term.imm(".", list[i], ret);
            }
            return ret;
        }

        /// <summary>
        /// 文字列からドットリスト項へ変換
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="term">項</param>
        /// <returns>ドットリスト項</returns>
        public static Term StringToList(string str)
        {
            Term ret = Term.Nil();
            for (int i = str.Length - 1; i >= 0; i--)
            {
                ret = Term.imm(".", Term.Integer(str[i]), ret);
            }
            return ret;
        }


        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="val">値</param>
        public void SetValue(string val)
        {
            value = val;
            variable = false;
        }

        /// <summary>
        /// 値を取得
        /// </summary>
        /// <returns>値</returns>
        public string GetValue()
        {
        	if(reference_term != null)
        		return "&:" + reference_term.GetValue();

            if(variable)
                return value + "$" + scopenum.ToString();
            else
                return value;
        }

        /// <summary>
        /// 整数値として項の値を取得
        /// </summary>
        /// <returns></returns>
		public int GetIntegerValue()
		{
			return int.Parse(GetValue());
		}

        /// <summary>
        /// 変数として項を設定
        /// </summary>
        /// <param name="name">変数名</param>
        public void SetAsVar(string name)
        {
            value = name;
            variable = true;
            children = null;
        }

        /// <summary>
        /// 変数
        /// </summary>
        /// <returns></returns>
        public bool IsVariable()
        {
            return variable;
        }

        /// <summary>
        /// アトム
        /// </summary>
        /// <returns></returns>
        public bool IsAtom()
        {
            return !variable && GetPrmCount() == 0;
        }


        /// <summary>
        /// カット
        /// </summary>
        /// <returns></returns>
        public bool IsCut()
        {
            return IsAtom() && (value == "cut" || value == "!");
        }

        /// <summary>
        /// 項が関数かどうか
        /// </summary>
        /// <returns>関数であれば</returns>
        public bool IsFunction()
        {
            return GetPrmCount() != 0;
        }
        
        public bool IsReference()
        {
        	return reference_term != null;
        }

		public Term Dereference()
		{
			return reference_term;
		}

        /// <summary>
        /// 項の子の数を取得
        /// </summary>
        /// <returns>子の数</returns>
        public int GetPrmCount()
        {
            if (children != null)
                return children.Count;
            else
                return 0;            
        }
        
        /// <summary>
        /// 項の子を取得
        /// </summary>
        /// <param name="i">子のインデックス</param>
        /// <returns></returns>
        public Term GetPrm(int i)
        {
            return children[i];
        }
        /// <summary>
        /// 項の子を設定
        /// </summary>
        /// <param name="list">設定する子項のリスト</param>
        public void SetPrms(List<Term> list)
        {
        	children = list;
        }
        /// <summary>
        /// 子項の追加
        /// </summary>
        /// <param name="term">追加する項</param>
        public void AddPrm(Term term)
        {
        	if(children == null)
        		children = new List<Term>();
            children.Add(term);
        }
        /// <summary>
        /// 適切な文字列表現へ変換
        /// </summary>
        /// <returns>文字列表現</returns>
        public string ToStr()
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<string, Term> rcsv_detector = new Dictionary<string, Term>();
            ToStrImpl(sb, rcsv_detector);
            return sb.ToString();
        }

        /// <summary>
        /// 現在の項をリストとして、文字列表現へ変換
        /// </summary>
        /// <param name="sb">変換した文字列へ格納するバッファ</param>
        /// <param name="rcsv_dtctr">再帰検出のための辞書</param>
        private void ToStrList(StringBuilder sb, Dictionary<string, Term> rcsv_dtctr)
        {
            sb.Append("[");
            bool first = true;
            Term cur = this;
            while (true)
            {
                if (!cur.IsFunction() && cur.GetValue() == "nil")
                    break;
                if (!cur.IsFunction() || cur.GetValue() != "." || cur.GetPrmCount() != 2)
                {
                    sb.Append("|");
                    cur.ToStrImpl(sb, new Dictionary<string, Term>(rcsv_dtctr));
                    break;
                }
                if (first)
                    first = false;
                else
                    sb.Append(",");
                cur.GetPrm(0).ToStrImpl(sb, new Dictionary<string, Term>(rcsv_dtctr));
                cur = cur.GetPrm(1);
            }
            sb.Append("]");
        }
        /// <summary>
        /// 現在の項をリストとして、文字列表現へ変換
        /// </summary>
        /// <param name="sb">変換した文字列へ格納するバッファ</param>
        /// <param name="rcsv_dtctr">再帰検出のための辞書</param>
        private void ToStrImpl(StringBuilder sb, Dictionary<string, Term> rcsv_dtctr)
        {
        	if(variable){
		    	if(rcsv_dtctr.ContainsKey(GetValue()))
		    	{
		    		sb.Append(GetValue());
		    		sb.Append(":recursive");
		    		return;
		    	}
				rcsv_dtctr[GetValue()] = this;
			}
            if (GetValue() == "." && GetPrmCount() == 2)
            {
                ToStrList(sb, rcsv_dtctr);
                return;
            }

            if (variable)
                sb.Append("?");
            if (value.Length == 0 && !IsReference())
                sb.Append("[NoName]");
            else
                sb.Append(GetValue());

            if (GetPrmCount() == 0)
                return;
            sb.Append("(");
            for (int i = 0; i < GetPrmCount(); i++ )
            {
                if (i != 0)
                    sb.Append(", ");
                children[i].ToStrImpl(sb, new Dictionary<string, Term>(rcsv_dtctr));
            }
            sb.Append(")");
        }

        /// <summary>
        /// 辞書を参照して、変数を実体値に変換
        /// </summary>
        /// <param name="dict">変換に使用する辞書</param>
        /// <returns>変換結果</returns>
        public Term ReplaceVar(Dictionary<string, Term> dict)
        {
            if (IsVariable())
            {
                if (dict.ContainsKey(GetValue()))
                    return dict[GetValue()].ReplaceVar(dict);
                return this;
            }
            else
            {
                if (GetPrmCount() == 0)
                    return this;

                Term newc = new Term();
                newc.SetValue(GetValue());
                newc.children = new List<Term>();
                for (int i = 0; i < GetPrmCount(); i++)
                {
                    newc.children.Add(children[i].ReplaceVar(dict));
                }
                return newc;
            }
        }
    }
    /// <summary>
    /// Prolog節
    /// </summary>
    public class Clause
    {
        /// <summary>
        /// 頭部
        /// </summary>
        public Term head;
        /// <summary>
        /// body部
        /// </summary>
        public List<Term> body;
        
        /// <summary>
        /// スコープID
        /// </summary>
        public int ID;

        /// <summary>
        /// すべて初期化する
        /// </summary>
        public Clause()
        {
        	head = null;
            body = new List<Term>();
            ID = Prolog.GetUniqueNum();
        }

        /// <summary>
        /// termを節の項として節を初期化する
        /// </summary>
        /// <param name="term"></param>
        public Clause(Term term)
        {
        	if(term.GetPrmCount() == 2 && term.GetValue() == ":-")
        	{
        		head = term.GetPrm(0);
        		body = Term.CommaListToList(term.GetPrm(1));
        	}
        	else{
		    	head = term;
		        body = new List<Term>();
		    }
            ID = Prolog.GetUniqueNum();
        }

        /// <summary>
        /// 節の文字列表現を取得
        /// </summary>
        /// <returns>節の文字列表現</returns>
        public string GetSubject()
        {
            return head.GetValue();
        }

        /// <summary>
        /// スコープ番号を更新
        /// </summary>
        public void UpdateScopeNum()
        {
            head.SetScopeNum(ID);
            foreach(Term t in body)
            {
                t.SetScopeNum(ID);
            }
        }

        /// <summary>
        /// 節にあるすべての項に対して、デリゲートfuncを適用する
        /// </summary>
        /// <param name="func">適用するデリゲート</param>
        /// <returns>すべての項に対してfuncがtrueを返した場合のみtrue</returns>
        public bool ForAllTerm(Term.ForAllTermDelegate func)
        {
            if (!head.ForAllTerm(func))
                return false;
            foreach (Term t in body)
            {
                if (!t.ForAllTerm(func))
                    return false;
            }
            return true;
        }
        public Clause ReplaceVar(Dictionary<string, Term> dict)
        {
            Clause newc = new Clause();
            newc.head = head.ReplaceVar(dict);
            foreach (Term t in body)
            {
                newc.body.Add(t.ReplaceVar(dict));
            }
            return newc;
        }
        
        public Clause NewFrame(int fid)
        {
            Clause newc = new Clause();
            newc.head = head.DeepCopy(fid);
            foreach (Term t in body)
            {
                newc.body.Add(t.DeepCopy(fid));
            }
            return newc;
        }
    }
    /// <summary>
    /// Prologプログラム(節のmulti-set)
    /// </summary>
    public class PrologProgram
    {
        public Dictionary<string, List<Clause>> data;
        public PrologProgram()
        {
            data = new Dictionary<string, List<Clause>>();
        }
        /// <summary>
        /// 節の追加
        /// </summary>
        /// <param name="cl"></param>
        public void Add(Clause cl)
        {
            if(!data.ContainsKey(cl.GetSubject()))
                data[cl.GetSubject()] = new List<Clause>();
            data[cl.GetSubject()].Add(cl);
        }
        
    }

    /// <summary>
    /// Prologの実行に関する手続き
    /// </summary>
    public class Prolog
    {
        static private int serialNum = 1;
        /// <summary>
        /// プログラム実行全体でユニークな数値へ取得
        /// </summary>
        /// <returns></returns>
        public static int GetUniqueNum()
        {
            return serialNum++;
        }

        /// <summary>
        /// ユニフィケーション
        /// </summary>
        /// <param name="a">項A</param>
        /// <param name="b">項B</param>
        /// <param name="dict">ユニフィケーション結果を返す先</param>
        /// <param name="dout">デバッグ出力先(null: 無効)</param>
        /// <returns>ユニフィケーションが成功した場合のみtrueを返す</returns>
        public static bool Unify(Term a, Term b, Dictionary<string, Term> dict, StringBuilder dout)
        {
            if (a.IsVariable())
            {
                if (dict.ContainsKey(a.GetValue()))
                {
                    return Unify(dict[a.GetValue()], b, dict, dout);
                }
                else
                {
                    dict.Add(a.GetValue(), b);
                    if (dout != null)
                        dout.Append(a.GetValue() + "(:=" + b.ToStr() + ")");
                    return true;
                }
            }
            else if (b.IsVariable())
            {
                if (dict.ContainsKey(b.GetValue()))
                {
                    return Unify(a, dict[b.GetValue()], dict, dout);
                }
                else
                {
                    dict.Add(b.GetValue(), a);
                    if (dout != null) dout.Append(a.ToStr() + "(:=" + b.GetValue() + ")");
                    return true;
                }
            }

            if (a.GetValue() != b.GetValue())
            {
                return false;
            }
            if (a.GetPrmCount() != b.GetPrmCount())
            {
                return false;
            }

            if (dout != null) dout.Append(a.GetValue());
            if (!a.IsFunction())
            {
                return true;
            }
            else
            {
                if (dout != null) dout.Append("(");
                for (int i = 0; i < a.GetPrmCount(); i++)
                {
                    if (dout != null && i != 0) dout.Append(",");
                    if (!Unify(a.GetPrm(i), b.GetPrm(i), dict, dout))
                        return false;
                }
                if (dout != null) dout.Append(")");
                return true;
            }
        }
        /// <summary>
        /// 文字列を項に変換
        /// </summary>
        /// <param name="text">対象の文字列</param>
        /// <returns>項</returns>
        public static Term StrToTerm(string text)
        {
            CSPrologJay.Parser psr = new CSPrologJay.Parser();
            Tokenizer tok = new CSProlog.Tokenizer(Token.CT_TERM, text);
            TokenizerJay tokj = new TokenizerJay(tok);
            Term ret = null;
            // ParserLogger plog = new ParserLogger();
            try
            {
                ret = (Term)psr.yyParse(tokj, null);
            }
            catch (CSPrologJay.Parser.yyException ex)
            {
                Token ctok = null;
                tok.CurrentToken(ref ctok);
                throw new CompilationError(ctok, ex.Message);

            }
            return ret;
        }
        /// <summary>
        /// 空の辞書を生成
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Term> EmptyDict()
        {
            return new Dictionary<string, Term>();
        }
    }

    /// <summary>
    /// コンパイルエラー
    /// </summary>
    public class CompilationError : System.Exception
    {
        Token target_token;
        public CompilationError(Token tok, string msg)
            :base(tok.PositionInfo() + ": " + msg + "(token:" + tok.value + ")")
        {
            target_token = tok;
        }
		
		public Token GetTargetToken()
		{
			return target_token;
		}
    }

    /// <summary>
    /// 解決の列挙子
    /// </summary>
    public class SolveEnumerator
    {
        IEnumerator<Dictionary<string, Term>> enmrtr;
        Env env;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ev">Prolog環境</param>
        /// <param name="en">Solve関数から返された列挙子</param>
        public SolveEnumerator(Env ev, IEnumerator<Dictionary<string, Term>> en)
        {
            enmrtr = en;
            env = ev;
        }

        /// <summary>
        /// 現在の解決結果を返す
        /// </summary>
        /// <returns>現在の解決結果</returns>
        public Dictionary<string, Term> GetCurrent()
        {
            return enmrtr.Current;
        }

        /// <summary>
        /// 解決を試みる
        /// </summary>
        /// <returns>解決できた場合true</returns>
        public bool DoSolve()
        {
            return enmrtr.MoveNext();
        }

    }

    /// <summary>
    /// Prolog環境
    /// </summary>
    public class Env
    {
        /// <summary>
        /// フレームデータ
        /// </summary>
        public class FrameData
        {
            public int FrameID;
            public Clause OrignalClause;
            public String UnifyExpr;
        }
        PrologProgram program;
        int FrameID;
        int AnonyID;
        Stack<FrameData> frames_;
		EmbeddedPredicateStore epstore;
        bool enable_backtrace;

		public PrintDelegate PrintCallback;

        public Env()
        {
            program = new PrologProgram();
            FrameID = 1;
            AnonyID = 1;
            frames_ = new Stack<FrameData>();
			epstore = new EmbeddedPredicateStore();
			epstore.LoadDefaultPredicates();
            enable_backtrace = false;
        }

        public bool TermAnonyNamer(Term t)
        {
            if (!t.IsFunction() && t.GetValue().StartsWith("_"))
            {
                t.SetAsVar(t.GetValue() + AnonyID.ToString());
                AnonyID++;
            }
            return true;
        }

        public Object ParseObject(string str, int TextType)
        {
            CSPrologJay.Parser psr = new CSPrologJay.Parser();
            Tokenizer tok = new CSProlog.Tokenizer(TextType, str);
            TokenizerJay tokj = new TokenizerJay(tok);

            // ParserLogger plog = new ParserLogger();
            try
            {
                return psr.yyParse(tokj, null);
            }
            catch (CSPrologJay.Parser.yyException ex)
            {
                Token ctok = null;
                tok.CurrentToken(ref ctok);
                throw new CompilationError(ctok, ex.Message);
            }
        }

        public bool LoadProgram(PrologProgram newprog)
        {
            foreach (KeyValuePair<string, List<Clause>> i in newprog.data)
            {
                foreach (Clause c in i.Value)
                {
                    // c.UpdateScopeNum();
                    c.ForAllTerm(this.TermAnonyNamer);
                    program.Add(c);
                }
            }
            return true;
        }

        public bool LoadProgramFromString(string prog_str)
        {
            PrologProgram newprog = (PrologProgram)ParseObject(prog_str, Token.CT_PROGRAM);
            if(newprog == null)
                return false;
            return LoadProgram(newprog);
        }

        public Term LoadTermFromString(string term_str)
        {
            Term t = (Term)ParseObject(term_str, Token.CT_TERM);
            if (t == null)
                return null;
            t.SetScopeNum(0);
            t.ForAllTerm(this.TermAnonyNamer);
            return t;
        }

        public void ClearProgram()
        {
            program = new PrologProgram();
        }

        public bool Solve1(Term goal, ref Dictionary<string, Term> dict)
        {
            foreach (Dictionary<string, Term> d in Solve(goal))
            {
                dict = d;
                return true;
            }
            return false;
        }
	
        /// <summary>
        /// 辞書をマージする
        /// </summary>
        /// <param name="a">マージ対象A</param>
        /// <param name="b">マージ対象B</param>
        /// <returns></returns>
		public Dictionary<string, Term> MergeDict(Dictionary<string, Term> a, Dictionary<string, Term> b)
		{
			Dictionary<string, Term> ret = new Dictionary<string, Term>();
            foreach (KeyValuePair<string, Term> i in a)
            {
            	ret[i.Key] = i.Value;
            }
            foreach (KeyValuePair<string, Term> i in b)
            {
            	if(ret.ContainsKey(i.Key))
            	{
            		throw new Exception("DupVar: " + i.Key + " Values:["+a[i.Key].ToStr()+"],["+b[i.Key].ToStr()+"]");
            	}
            	ret[i.Key] = i.Value;
            }
            return ret;
		}

        /// <summary>
        /// 項の変数を展開する
        /// </summary>
        /// <param name="term">展開する項</param>
        /// <param name="dict">展開に使用する辞書</param>
        /// <returns></returns>
		public Term ResolveTerm(Term term, Dictionary<string, Term> dict)
		{
			if(term.IsVariable())
			{
				if(dict.ContainsKey(term.GetValue()))
				{
					return ResolveTerm(dict[term.GetValue()], dict);
				}
				return term;
			}
			Term ret = new Term();
			ret.SetValue(term.GetValue());
			if(term.IsFunction())
			{
				for(int i = 0; i < term.GetPrmCount(); i++)
				{
					Term t = ResolveTerm(term.GetPrm(i), dict);
					ret.AddPrm(t);
				}
			}
			return ret;
		}
		
		private class Frame
		{
		}
        /*
		/*
        public IEnumerable<Dictionary<string, Term>> RunBody(Clause c,
        											Dictionary<string, Term> dict)
        {
			Stack<Frame> runstack = new Stack<Frame>();
			Frame curfrm = new Frame();
			runstack.Push(curfrm);
        	int idx = 0;
			curfrm.answer = null;
       		while(true)
       		{
				if(curfrm.answer == null){
					Term replaced_term = c.body[idx].ReplaceVar(dict);
					curfrm.answers = Solve(replaced_term);
				}
				var next = curfrm.answers.Next();
				if(next == null)
				{
					runstack.Pop();
					if(runstack.Empty())
						break;
					curfrm = runstack.Top();
					continue;
				}
				
       		}
        }
		*/

        private IEnumerable<Dictionary<string, Term>> RunBody(Clause c, int idx, 
        											Dictionary<string, Term> dict)
        {
            if (c.body.Count == idx)
            {
                yield return dict;
            }
            else if(c.body[idx].IsCut()) // カットだったら
            {
				foreach (Dictionary<string, Term> d2 in RunBody(c, idx + 1, dict))
					yield return d2;
				yield return null;
			}
            else
            {
	            Term replaced_term = c.body[idx].ReplaceVar(dict);
	            //Console.WriteLine("Replaced: " + replaced_term.ToStr());
		        foreach (Dictionary<string, Term> d in Solve(replaced_term))
		        {
		        	Dictionary<string, Term> merged = MergeDict(dict, d);
		            foreach (Dictionary<string, Term> d2 in RunBody(c, idx + 1, merged))
		                yield return d2;
		        }
            }
        }

        private IEnumerable<Dictionary<string, Term>> GoClause(Clause c, Dictionary<string,Term> cldict)
        {
          	Clause clause_frame = c.NewFrame(FrameID);
			FrameID++;
            return RunBody(clause_frame, 0, cldict);
		}

        /// <summary>
        /// プログラム実行開始
        /// </summary>
        /// <param name="goal">ゴール</param>
        /// <returns>結果の列挙子</returns>
        public SolveEnumerator Start(Term goal)
        {
            SolveEnumerator se = new SolveEnumerator(this, Solve(goal).GetEnumerator());
            return se;
        }

        /// <summary>
        /// 解決を試みる
        /// </summary>
        /// <param name="goal">ゴール</param>
        /// <returns>解決結果の列挙子</returns>
        public IEnumerable<Dictionary<string, Term>> Solve(Term goal)
        {
			// 組み込み述語の実行
			EmbeddedPredicate.EPBase ep = null;
			if(epstore.FindPredicate(goal, ref ep))
			{
				foreach(var i in ep.Apply(this, goal))
				{
					yield return i;
				}
				yield break;
			}
			
			//述語が登録されていない場合終了
        	if(!program.data.ContainsKey(goal.GetValue()))
        	{
				throw new Exception("No such predicate: " + goal.GetValue()+"]");
        		yield break;
        	}
			
			ep = null;
            List<Clause> clist = program.data[goal.GetValue()];
            
            foreach (Clause c in clist)
            {
                Dictionary<string, Term> cldict = new Dictionary<string, Term>();
                Term cheadframe = c.head.DeepCopy(FrameID);
                StringBuilder sb = enable_backtrace ? new StringBuilder() : null;
                if (Prolog.Unify(goal, cheadframe, cldict, sb))
                {
                    if (enable_backtrace)
                    {
                        FrameData fd = new FrameData();
                        fd.OrignalClause = c;
                        fd.FrameID = FrameID;
                        fd.UnifyExpr = sb != null ? sb.ToString() : null;
                        frames_.Push(fd);
                    }
                    foreach (Dictionary<string, Term> d in GoClause(c, cldict))
                    {
						if(d == null) // カットがあった
						{
							if (enable_backtrace)
								frames_.Pop();
							yield break;
						}
                        yield return d;
                    }
                    if (enable_backtrace)
                        frames_.Pop();
                }
            }
        }
        /// <summary>
        /// スタックトレースを取得
        /// </summary>
        /// <param name="sb">スタックトレースの文字列表現</param>
        public void GetStackTrace(StringBuilder sb)
        {
            foreach(FrameData fd in frames_)
            {
                sb.Append(fd.FrameID);
                sb.Append(": ");
                sb.Append(fd.UnifyExpr);
                sb.AppendLine("");
            }

        }
    }
}
