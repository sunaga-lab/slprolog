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

namespace SLProlog
{
    public class Token
    {
        public const int CT_TERMINAL = -1;
        public const int CT_NULL = 0;
        public const int CT_WHITESPACE = 1;
        public const int CT_ALPHA = 2;
        public const int CT_NUMBER = 3;
        public const int CT_PARENTHEIS = 4;
        public const int CT_COMMA = 5;
        public const int CT_PRIOD = 6;
        public const int CT_OPERATOR = 7;

        public string value;
        public int token_type;
    }
    
    public class Tokenizer
    {
        string source;
        int spos;
        int token_list_current;

        Stack<char> unread_char;
        List<Token> token_list;
        public int GetCharType(char ch)
        {
            switch(ch){
                case '(':
                case ')':
                    return Token.CT_PARENTHEIS;
                case ',':
                    return Token.CT_COMMA;
                case '.':
                    return Token.CT_PRIOD;
                case '=':
                case ':':
                case '-':
                case '>':
                    return Token.CT_OPERATOR;
            }
            if(char.IsLetter(ch))
            {
                return Token.CT_ALPHA;
            }
            if(char.IsWhiteSpace(ch))
            {
                return Token.CT_WHITESPACE;
            }
            if(char.IsNumber(ch))
            {
                return Token.CT_NUMBER;
            }
            return Token.CT_ALPHA;
        }

        public Tokenizer(string s)
        {
            source = s;
            spos = 0;
            unread_char = new Stack<char>();
            token_list = new List<Token>();
            token_list_current = 0;
            ParseAllToken();
        }

        
        public bool GetCh(ref char ch)
        {
            if (unread_char.Count != 0)
            {
                ch = unread_char.Pop();
                return true;
            }
            if (source.Length <= spos)
                return false;
            ch = source[spos++];
            if (ch == '\0')
            {
                spos--;
                return false;
            }
            return true;
        }
        public void UnreadChar(char ch)
        {
            if (ch == '\0')
                return;
            unread_char.Push(ch);
        }
        public void UnreadToken(Token tok)
        {
            token_list_current--;
        }
        public bool RequestToken(int token_type, ref Token token)
        {
            Token tok = null;
            if (!GetToken(ref tok))
                return false;
            if (tok.token_type == token_type)
            {
                token = tok;
                return true;
            }
            else
            {
                UnreadToken(tok);
                return false;
            }

        }
        public bool RequestTokenStr(string ch, ref Token token)
        {
            Token tok = null;
            if (!GetToken(ref tok))
                return false;
            if (tok.value == ch)
            {
                token = tok;
                return true;
            }
            else
            {
                UnreadToken(tok);
                return false;
            }

        }
        public int CurTokenPos()
        {
            return token_list_current;
        }
        public void RestoreTokenPos(int pos)
        {
            token_list_current = pos;
        }

        public bool GetToken(ref Token token)
        {
            if (token_list.Count <= token_list_current)
                return false;
            token = token_list[token_list_current++];
            return true;
        }

        public bool ParseToken(ref Token token)
        {
            token = new Token();
            token.token_type = Token.CT_NULL;
            token.value = "";
            int ct_now = Token.CT_NULL;
            while (ct_now != Token.CT_TERMINAL)
            {
                char ch = '\0';
                if (!GetCh(ref ch))
                {
                    ch = '\0';
                    ct_now = Token.CT_TERMINAL;
                    if (token.token_type == Token.CT_NULL)
                    {
                        return false;
                    }
                }
                else
                    ct_now = GetCharType(ch);
                if (token.token_type == Token.CT_NULL)
                {
                    token.token_type = ct_now;
                }
                if(ct_now == token.token_type &&
                   (   ct_now == Token.CT_WHITESPACE 
                    || ct_now == Token.CT_ALPHA
                    || ct_now == Token.CT_NUMBER
                    || ct_now == Token.CT_OPERATOR
                    )
                    )
                {
                    token.value += ch;
                    continue;
                }
                if(
                    (token.token_type == Token.CT_ALPHA && ct_now == Token.CT_NUMBER) 
                    || (token.token_type == Token.CT_NUMBER && ct_now == Token.CT_ALPHA && token.token_type == Token.CT_ALPHA)
                )
                {
                    token.value += ch;
                    continue;
                }

                // divide
                    
                if(token.token_type == Token.CT_WHITESPACE)
                {
                    token.token_type = Token.CT_NULL;
                    token.value = "";
                    UnreadChar(ch);
                    continue;
                }
                if (token.token_type == Token.CT_NUMBER || token.token_type == Token.CT_ALPHA || token.token_type == Token.CT_OPERATOR)
                {
                    UnreadChar(ch);
                    return true;
                }
                if (ct_now == Token.CT_PARENTHEIS || ct_now == Token.CT_COMMA || ct_now == Token.CT_PRIOD)
                {
                    token.value += ch;
                    return true;
                }
            }
            return false;
        }

        public void ParseAllToken()
        {
            Token token = null;
            while (ParseToken(ref token))
            {
                token_list.Add(token);
            }
            token = new Token();
            token.value = "";
            token.token_type = Token.CT_TERMINAL;
            token_list.Add(token);

            return;
        }
        
        public string CurrentTokenStr()
        {
        	return token_list[token_list_current == 0 ? 0 : token_list_current - 1].value;
        }

	}
    public class Parser
    {
    	private Tokenizer tokenizer;
        public Parser(Tokenizer t)
        {
			tokenizer = t;
        }
		
        private void ParseError(String msg)
        {
            throw new Exception("パースエラー: " + msg + " (Token:" + tokenizer.CurrentTokenStr() + ")");
        }


		public bool ReadOperator(ref string oper)
		{
			string name;
			if(!tokenizer.RequestToken(Token.CT_OPERATOR, ref name))
			{
				return false;
			}
			oper = name;
			return true;
		}

        public bool ReadTerm(ref Term term)
        {
            Token name = null, pydummy = null;
            if (tokenizer.RequestToken(Token.CT_ALPHA, ref name))
            {
                if (char.IsUpper(name.value[0]))
                    term = Term.var(name.value);
                else
                    term = Term.imm(name.value);
                if (!tokenizer.RequestTokenStr("(", ref pydummy))
                {
                    return true;
                }
                if (term.IsFunction())
                    ParseError("述語は変数にできません");
                while (true)
                {
                    Term prm = null;
                    ReadTerm(ref prm);

                    term.AddPrm(prm);
                    if (tokenizer.RequestTokenStr(")", ref pydummy))
                        break;
                    if (!tokenizer.RequestTokenStr(",", ref pydummy))
                        ParseError("パラメータの後には、コンマか丸括弧が必要です");
                }
                return true;
            }
            ParseError("項はアルファベットで始まる必要があります");
            return false;
        }

        public bool ReadClause(ref Clause clause)
        {
            Token dummy_token = null;
            Term head = null;
            clause = new Clause();

            ReadTerm(ref head);
            clause.head = head;

            if(tokenizer.RequestTokenStr(".", ref dummy_token))
                return true;
            if(!tokenizer.RequestTokenStr(":-", ref dummy_token))
                ParseError("節のヘッドの後にはピリオドか:-が必要です");
            while(true)
            {
                Term body = null;
                ReadTerm(ref body);
                clause.body.Add(body);

                if(tokenizer.RequestTokenStr(".", ref dummy_token))
                    break;
                if(!tokenizer.RequestTokenStr(",", ref dummy_token))
                    ParseError("ボディ部の項(term)の後にはコンマかピリオドが必要です");
            }
            return true;
        }

        public bool ReadProgram(ref PrologProgram prog)
        {
            Token dummy_token = null;
            prog = new PrologProgram();
            while (true)
            {
                if(tokenizer.RequestToken(Token.CT_TERMINAL, ref dummy_token))
                    break;
                Clause clause = null;
                ReadClause(ref clause);
                prog.Add(clause);
            }
            return true;
        }

    }

    public class Term
    {
        private bool variable;
        private string value;
        private int scopenum;
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
        public int GetScopeNum()
        {
        	return scopenum;
        }
        public Term DeepCopy()
        {
        	return DeepCopy(-1);
        }
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
        
        public static Term var(string name)
        {
            Term t = new Term();
            t.SetAsVar(name);
            return t;
        }

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

        public void SetValue(string val)
        {
            value = val;
            variable = false;
        }

        public string GetValue()
        {
        	if(reference_term != null)
        		return "&:" + reference_term.GetValue();

            if(variable)
                return value + "$" + scopenum.ToString();
            else
                return value;
        }

        public void SetAsVar(string name)
        {
            value = name;
            variable = true;
            children = null;
        }

        public bool IsVariable()
        {
            return variable;
        }

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

        public int GetPrmCount()
        {
            if (children != null)
                return children.Count;
            else
                return 0;            
        }

        public Term GetPrm(int i)
        {
            return children[i];
        }
        public void AddPrm(Term term)
        {
        	if(children == null)
        		children = new List<Term>();
            children.Add(term);
        }
        public string ToStr()
        {
            string ret = "";

            if (variable)
                ret += "?";
            if (value.Length == 0 && !IsReference())
                ret += "[NoName]";
            else
                ret += GetValue();

            if (GetPrmCount() == 0)
                return ret;
            ret += "(";
            for (int i = 0; i < GetPrmCount(); i++ )
            {
                if (i != 0)
                    ret += ", ";
                ret += children[i].ToStr();
            }
            ret += ")";
            return ret;
        }

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
    public class Clause
    {
        public Term head;
        public List<Term> body;
        public int ID;
        

        public Clause()
        {
            body = new List<Term>();
            ID = Prolog.GetUniqueNum();
        }

        public string GetSubject()
        {
            return head.GetValue();
        }
        public void UpdateScopeNum()
        {
            head.SetScopeNum(ID);
            foreach(Term t in body)
            {
                t.SetScopeNum(ID);
            }
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
    public class PrologProgram
    {
        public Dictionary<string, List<Clause>> data;
        public PrologProgram()
        {
            data = new Dictionary<string, List<Clause>>();
        }
        public void Add(Clause cl)
        {
            if(!data.ContainsKey(cl.GetSubject()))
                data[cl.GetSubject()] = new List<Clause>();
            data[cl.GetSubject()].Add(cl);
        }
        
    }

    public class Prolog
    {
        static private int serialNum = 1;
        public static int GetUniqueNum()
        {
            return serialNum++;
        }
        public static bool Unify(Term a, Term b, Dictionary<string, Term> dict)
        {
            if (a.IsVariable())
            {
                if (dict.ContainsKey(a.GetValue()))
                {
                    return Unify(dict[a.GetValue()], b, dict);
                }
                else
                {
                    dict.Add(a.GetValue(), b);
                    return true;
                }
            }
            else if (b.IsVariable())
            {
                if (dict.ContainsKey(b.GetValue()))
                {
                    return Unify(a, dict[b.GetValue()], dict);
                }
                else
                {
                    dict.Add(b.GetValue(), a);
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

            if (!a.IsFunction())
            {
                return true;
            }
            else
            {
                for (int i = 0; i < a.GetPrmCount(); i++)
                {
                    if (!Unify(a.GetPrm(i), b.GetPrm(i), dict))
                        return false;
                }
                return true;
            }
        }
        public static Term StrToTerm(string text)
        {
            Parser parser = new Parser(new Tokenizer(text));
            Term ret = null;
            parser.ReadTerm(ref ret);
            return ret;
        }

    }
    public class Env
    {
        PrologProgram program;
        public Env()
        {
            program = new PrologProgram();
        }

        public bool LoadProgram(PrologProgram newprog)
        {
            foreach (KeyValuePair<string, List<Clause>> i in newprog.data)
            {
                foreach (Clause c in i.Value)
                {
                    // c.UpdateScopeNum();
                    program.Add(c);
                }
            }
            return true;
        }

        public bool LoadProgramFromString(string prog_str)
        {
            Parser parser = new Parser(new Tokenizer(prog_str));
            PrologProgram newprog = null;
            parser.ReadProgram(ref newprog);
            return LoadProgram(newprog);
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
		
        public IEnumerable<Dictionary<string, Term>> RunBody(Clause c, int idx, Dictionary<string, Term> dict)
        {
            if (c.body.Count == idx)
            {
                yield return dict;
            }
            else{
	            Term replaced_term = c.body[idx].ReplaceVar(dict);
	            Console.WriteLine("Replaced: " + replaced_term.ToStr());
		        foreach (Dictionary<string, Term> d in Solve(replaced_term))
		        {
		        	Dictionary<string, Term> merged = MergeDict(dict, d);
		            foreach (Dictionary<string, Term> d2 in RunBody(c, idx + 1, merged))
		                yield return d2;
		        }
            }
        }
		static int FrameID = 1;
        public IEnumerable<Dictionary<string, Term>> Solve(Term goal)
        {
            List<Clause> clist = program.data[goal.GetValue()];
            foreach (Clause c in clist)
            {
                Dictionary<string, Term> cldict = new Dictionary<string,Term>();
                Term cheadframe = c.head.DeepCopy(FrameID);
                if (Prolog.Unify(goal, cheadframe, cldict))
                {
	              	Clause clause_frame = c.NewFrame(FrameID++);
                    foreach (Dictionary<string, Term> d in RunBody(clause_frame, 0, cldict))
                    {
                        // yield return Resolve(d);
                        yield return d;
                    }
                }
            }
        }
    }

	public class SLProlog
	{
		static public void Main()
		{
            string res = "";
            Env env = new Env();
            env.LoadProgramFromString(
            	"u(W,W)." +
            	"plus(X, X, z). " +
            	"plus(X, m(X), p(z))." +
            	"plus(p(p(X)), p(X), p(z))." +
            	"plus(m(m(X)), m(X), m(z))." +
            	"plus(X, p(X), m(z))." +
            	"plus(Ans, X, p(Y)) :- plus(Ans1, X, Y), plus(Ans, Ans1, p(z)). " +
            	"plus(Ans, X, m(Y)) :- plus(Ans1, X, Y), plus(Ans, Ans1, m(z)). " +
            	"inv(z, z). " +
            	"inv(m(X), p(Y)) :- inv(X, Y). " +
            	"inv(p(X), m(Y)) :- inv(X, Y). " +
            	"mul(z, X, z). " +
            	"mul(z, z, Y). " +
            	"mul(Ans, X, p(Y)) :- mul(Ans1, X, Y), plus(Ans, Ans1, X). " +
            	"main(R) :- plus(A, p(p(z)), p(p(z))), mul(B, A, A), inv(C,B), plus(R, C, m(m(m(m(m(m(z))))))). "
            	);
           	string goalstr = "main(R)";
            Dictionary<string, Term> dict = null;
            Term goal = Prolog.StrToTerm(goalstr);
			goal.SetScopeNum(0);
            res += "Goal1: " + goal.ToStr() + "\r\n";
            bool ok = env.Solve1(goal, ref dict);

            res += "Goal: " + goal.ToStr() + "\r\n";
            if (ok)
            {
                res += "Run ok.\r\n";
                foreach (KeyValuePair<string, Term> i in dict)
                {
                    res += i.Key + " = " + i.Value.ToStr() + "\r\n";
                	if(i.Key[i.Key.Length-1] == '0' && i.Key[i.Key.Length-2] == '$')
                	{
                		Term t = env.ResolveTerm(i.Value, dict);
	                    res += "(*)" + i.Key + " = " + t.ToStr() + "\r\n";
                	}
                }
            }
            Console.Write(res);
		}
	}
}
