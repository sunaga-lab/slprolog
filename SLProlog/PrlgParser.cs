using System;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSProlog
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
        public const int CT_BRACE = 8;
        public const int CT_PROGRAM = 100;
        public const int CT_TERM = 101;
        public const int CT_SINGLE_QUATED = 201;
        public const int CT_DOUBLE_QUATED = 202;

        public string value;
        public int token_type;
        public int row, col;
        public void SetPosition(int c, int r)
        {
            col = c;
            row = r;
        }

        public string PositionInfo()
        {
            return "(" + col.ToString() + ", line " + row.ToString() + ")";
        }
    }
   /*
    public class ParserLogger: CSPrologJay.yyDebug.yyDebug{
        public List<string> logs_;

        public ParserLogger(){
            logs_ = new List<string>();
        }

		void println (string s){
            logs_.Add(s);
		}
		 
		 public void push (int state, Object value) {
			 println ("push\tstate "+state+"\tvalue "+value);
		 }
		 
		 public void lex (int state, int token, string name, Object value) {
			 println("lex\tstate "+state+"\treading "+name+"\tvalue "+value);
		 }
		 
		 public void shift (int from, int to, int errorFlag) {
			 switch (errorFlag) {
			 default:				// normally
				 println("shift\tfrom state "+from+" to "+to);
				 break;
			 case 0: case 1: case 2:		// in error recovery
				 println("shift\tfrom state "+from+" to "+to
					     +"\t"+errorFlag+" left to recover");
				 break;
			 case 3:				// normally
				 println("shift\tfrom state "+from+" to "+to+"\ton error");
				 break;
			 }
		 }
		 
		 public void pop (int state) {
			 println("pop\tstate "+state+"\ton error");
		 }
		 
		 public void discard (int state, int token, string name, Object value) {
			 println("discard\tstate "+state+"\ttoken "+name+"\tvalue "+value);
		 }
		 
		 public void reduce (int from, int to, int rule, string text, int len) {
			 println("reduce\tstate "+from+"\tuncover "+to
				     +"\trule ("+rule+") "+text);
		 }
		 
		 public void shift (int from, int to) {
			 println("goto\tfrom state "+from+" to "+to);
		 }
		 
		 public void accept (Object value) {
			 println("accept\tvalue "+value);
		 }
		 
		 public void error (string message) {
			 println("error\t"+message);
		 }
		 
		 public void reject () {
			 println("reject");
		 }
     }
     */
    
    public class TokenizerJay: CSPrologJay.Parser.yyInput
    {
    	Tokenizer tknz_;
        bool first_read;
    	public TokenizerJay(Tokenizer t)
    	{
    		tknz_ = t;
            first_read = true;
    	}
    	
    	public bool Advance()
    	{
            if (!tknz_.HasNext())
                return false;
            if (first_read)
            {
                first_read = false;
                return true;
            }
    		tknz_.NextToken();
			return true;
    	}
    	
    	public int Token {get{return token();}}
		public int token()
		{
            if (first_read)
                throw new System.Exception("token before advance");
            Token token = null;
            tknz_.CurrentToken(ref token);
            switch (token.token_type)
            {
                case CSProlog.Token.CT_PROGRAM: return CSPrologJay.Parser.StProgram;
                case CSProlog.Token.CT_TERM: return CSPrologJay.Parser.StTerm;
                case CSProlog.Token.CT_SINGLE_QUATED: return CSPrologJay.Parser.SingleQuoted;
                case CSProlog.Token.CT_DOUBLE_QUATED: return CSPrologJay.Parser.DoubleQuoted;
            }
            switch(token.value)
			{
                case "!": return CSPrologJay.Parser.OpExclamation;
                case ",": return CSPrologJay.Parser.Comma;
                case ".": return CSPrologJay.Parser.Priod;
				case "(": return CSPrologJay.Parser.ParentheisOpen;
				case ")": return CSPrologJay.Parser.ParentheisClose;			
				case "|": return CSPrologJay.Parser.Guard;
				case "[": return CSPrologJay.Parser.BracketOpen;			
				case "]": return CSPrologJay.Parser.BracketClose;
				case "=": return CSPrologJay.Parser.OpEqual;
				case "+": return CSPrologJay.Parser.OpAdd;
				case "-": return CSPrologJay.Parser.OpSub;
				case "*": return CSPrologJay.Parser.OpMul;
				case "/": return CSPrologJay.Parser.OpDiv;
				case ":-": return CSPrologJay.Parser.BeginClauseBody;
				case "is": return CSPrologJay.Parser.OpIs;
				case "<": return CSPrologJay.Parser.OpLt;
				case ">": return CSPrologJay.Parser.OpGt;
				case "=<": return CSPrologJay.Parser.OpLe;
				case ">=": return CSPrologJay.Parser.OpGe;
            }
			switch(token.token_type)
			{
				case CSProlog.Token.CT_ALPHA: return CSPrologJay.Parser.Identifier;
                case CSProlog.Token.CT_NUMBER: return CSPrologJay.Parser.Number;
			}
			return CSPrologJay.Parser.Unknown;
		}

    	public object Value{get{return value();}}

		public object value()
		{
            if (first_read)
                throw new System.Exception("value before advance");
            Token token = null;
            tknz_.CurrentToken(ref token);
            return token.value;
		}
    }
    public class Tokenizer
    {
        string source;
        int spos;
        int token_list_current;
        int token_rownum, token_colnum;

        Stack<char> unread_char;
        List<Token> token_list;
        public int GetCharType(char ch)
        {
            if ("=:<>!@&|*/+-^".Contains(ch.ToString()))
                return Token.CT_OPERATOR;

            switch(ch){
                case '(':
                case ')':
                    return Token.CT_PARENTHEIS;
                case '[':
                case ']':
                    return Token.CT_BRACE;
                case ',':
                    return Token.CT_COMMA;
                case '.':
                    return Token.CT_PRIOD;
                case '_':
                    return Token.CT_ALPHA;
                case '\'':
                    return Token.CT_SINGLE_QUATED;
                case '"':
                    return Token.CT_DOUBLE_QUATED;
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
            throw new System.Exception("Unknown char: '" + ch.ToString() + "'");
        }

        public Tokenizer(int syntax_type, string s)
        {
            source = s;
            spos = 0;
            unread_char = new Stack<char>();
            token_list = new List<Token>();
            Token st_tok = new Token();
            st_tok.token_type = syntax_type;
            st_tok.value = "Type";
            token_list.Add(st_tok);
            token_list_current = 0;
            token_rownum = 0;
            token_colnum = -1;
            ParseAllToken();
        }


        public bool GetCh(ref char ch)
        {
			if(!GetChRaw(ref ch))
				return false;
			if(ch == '%')
			{
				while(true)
				{
					if(!GetChRaw(ref ch))
						return false;
					if(ch == '\r' || ch == '\n')
						return true;
				}
			}
			if(ch == '/')
			{
				char nch = '\0';
				if(!GetChRaw(ref nch))
				{
					return true;
				}
				if(nch != '*')
				{
					UnreadChar(nch);
					return true;
				}
				nch = '\0';
				while(true)
				{
					ch = nch;
					if(!GetChRaw(ref nch))
						return false;
					if(ch == '*' && nch == '/')
					{
						return GetChRaw(ref ch);
					}
				}
			}
			return true;
		}        
        public bool GetChRaw(ref char ch)
        {
			if (unread_char.Count != 0)
			{
				ch = unread_char.Pop();
				return true;
			}
			if (source.Length <= spos)
				return false;


			ch = source[spos++];
			token_colnum++;

			if (ch == '\n' || ch == '\r' && (source.Length > spos && source[spos] != '\n'))
			{
				token_rownum++;
				token_colnum = -1;
			}
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
            if (!HasNext())
                return false;
            CurrentToken(ref token);
            NextToken();
            return true;
        }

        public bool CurrentToken(ref Token token)
        {
           	token = token_list[token_list_current];
            return true;
        }
		public bool NextToken()
		{
			token_list_current++;
            return HasNext();
		}
		public bool HasNext()
		{
			return token_list.Count > (token_list_current+1);
		}

        public char PreGetNextCh()
        {
            char ret = '\0';
            GetCh(ref ret);
            UnreadChar(ret);
            return ret;
        }
        public bool ParseToken(ref Token token)
        {
            
            token = new Token();
            token.SetPosition(token_colnum, token_rownum);
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


                if (token.token_type == Token.CT_SINGLE_QUATED)
                {
                    if (ct_now == Token.CT_SINGLE_QUATED)
                        return true;
                    token.value += ch;
                    continue;
                }

                if (token.token_type == Token.CT_DOUBLE_QUATED)
                {
                    if (ct_now == Token.CT_DOUBLE_QUATED)
                        return true;
                    token.value += ch;
                    continue;
                }

                if (token.token_type == Token.CT_NULL)
                {
                    token.token_type = ct_now;
                    if (token.token_type == Token.CT_SINGLE_QUATED || token.token_type == Token.CT_DOUBLE_QUATED)
                        continue;
                }


                if(ct_now == token.token_type &&
                   (ct_now == Token.CT_WHITESPACE
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
                switch(ct_now)
                {
                    case Token.CT_PARENTHEIS:
                    case Token.CT_COMMA:
                    case Token.CT_PRIOD:
                    case Token.CT_BRACE:
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
            Token name = null;
            if (!tokenizer.RequestToken(Token.CT_OPERATOR, ref name))
			{
				return false;
			}
			oper = name.value;
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

    
}

