// created by jay 1.1.0 (c) 2002-2006 ats@cs.rit.edu
// skeleton c# 1.1.0 (c) 2002-2006 ats@cs.rit.edu

#line 5 "PrlgParserJay.jay"


using System;
using System.Collections.Generic;

using CSProlog;


namespace CSPrologJay
{
	using List_Term = List<Term>;
	
    public class Parser
    {
		private int yacc_verbose_flag = 0;
		private Env env;
		
		public void SetEnv(Env e){env = e;}

#line 25 "-"
  // %token constants
  public const int Number = 257;
  public const int BeginClauseBody = 258;
  public const int Guard = 259;
  public const int Priod = 260;
  public const int Comma = 261;
  public const int Identifier = 262;
  public const int Unknown = 263;
  public const int ParentheisOpen = 264;
  public const int ParentheisClose = 265;
  public const int BracketOpen = 266;
  public const int BracketClose = 267;
  public const int OpEqual = 268;
  public const int OpAdd = 269;
  public const int OpSub = 270;
  public const int OpMul = 271;
  public const int OpDiv = 272;
  public const int OpIs = 273;
  public const int OpNot = 274;
  public const int OpLe = 275;
  public const int OpGe = 276;
  public const int OpLt = 277;
  public const int OpGt = 278;
  public const int OpExclamation = 279;
  public const int SingleQuoted = 280;
  public const int DoubleQuoted = 281;
  public const int StProgram = 282;
  public const int StTerm = 283;
  public const int yyErrorCode = 256;

  /// <summary>
  ///   final state of parser.
  /// </summary>
  protected const int yyFinal = 3;

  /// <summary>
  ///   parser tables.
  ///   Order is mandated by jay.
  /// </summary>
  protected static readonly short[] yyLhs = new short[] {
//yyLhs 33
    -1,     0,     0,     6,     6,     7,     2,     2,     1,     5,
     5,     5,     5,     5,     5,     5,     5,     3,     3,     3,
     4,     4,     4,     4,     4,     4,     4,     4,     4,     4,
     4,     4,     4,
    }, yyLen = new short[] {
//yyLen 33
     2,     2,     1,     2,     2,     1,     2,     1,     2,     1,
     1,     1,     1,     4,     1,     1,     1,     2,     3,     5,
     3,     3,     3,     3,     3,     3,     3,     3,     3,     3,
     3,     3,     3,
    }, yyDefRed = new short[] {
//yyDefRed 57
     0,     0,     0,     0,     0,    12,     0,     0,     0,     9,
    11,    16,     7,     0,    15,    14,     0,     3,     0,     1,
     0,     0,    17,     0,     6,     0,     8,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,    32,
     0,    18,     0,     0,     0,     0,     0,    23,    24,     0,
     0,     0,     0,     0,    13,     0,    19,
    }, yyDgoto = new short[] {
//yyDgoto 8
     3,    12,    13,    14,    15,    16,     4,    17,
    }, yySindex = new short[] {
//yySindex 57
  -263,  -212,  -212,     0,  -260,     0,  -251,  -212,  -220,     0,
     0,     0,     0,  -212,     0,     0,  -174,     0,   -84,     0,
  -212,  -153,     0,  -196,     0,  -212,     0,  -212,  -212,  -212,
  -212,  -212,  -212,  -212,  -212,  -212,  -212,  -212,  -132,     0,
  -212,     0,   -71,  -261,  -261,  -232,  -232,     0,     0,   -61,
  -214,  -214,  -214,  -214,     0,  -105,     0,
    }, yyRindex = new short[] {
//yyRindex 57
     0,     0,     0,     0,    21,     0,     1,     0,     0,     0,
     0,     0,     0,     2,     0,     0,     0,     0,     4,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,   179,   159,   169,    22,    43,     0,     0,   148,
    64,    85,   106,   127,     0,     0,     0,
    }, yyGindex = new short[] {
//yyGindex 8
     0,    11,     0,     0,     0,    -2,     0,     0,
    }, yyTable = new short[] {
//yyTable 447
    18,    10,     5,    19,     4,    21,    23,    28,    29,    30,
    31,    32,    33,    20,    34,    35,    36,    37,    38,     1,
     2,     2,    21,    42,    24,    43,    44,    45,    46,    47,
    48,    49,    50,    51,    52,    53,     0,     5,    55,    31,
    32,     0,     6,    22,     7,     5,     8,    22,     0,     0,
     6,     0,     7,     0,     8,    29,    30,    31,    32,     9,
    10,    11,    25,    40,    30,    27,     0,     9,    10,    11,
     0,    41,    28,    29,    30,    31,    32,    33,     0,    34,
    35,    36,    37,     0,    25,    28,    26,    27,     0,     0,
     0,     0,     0,     0,    28,    29,    30,    31,    32,    33,
     0,    34,    35,    36,    37,    25,    31,     0,    27,     0,
     0,     0,    39,     0,     0,    28,    29,    30,    31,    32,
    33,     0,    34,    35,    36,    37,    25,    29,     0,    27,
     0,     0,     0,    54,     0,     0,    28,    29,    30,    31,
    32,    33,     0,    34,    35,    36,    37,     0,    27,     0,
     0,     0,     0,    25,     0,     0,    27,     0,     0,    25,
     0,     0,    56,    28,    29,    30,    31,    32,    33,    20,
    34,    35,    36,    37,    25,     0,     0,    27,     0,    26,
     0,     0,     0,     0,    28,    29,    30,    31,    32,    33,
    27,    34,    35,    36,    37,     0,     0,    28,    29,    30,
    31,    32,    33,     0,    34,    35,    36,    37,    29,    30,
    31,    32,     0,     0,    34,    35,    36,    37,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,    10,
    10,    10,    10,     0,    10,     5,    10,     4,    10,    10,
    10,    10,    10,    10,    10,     0,    10,    10,    10,    10,
    21,    21,    21,    21,     0,    21,     0,    21,     0,    21,
    21,    21,    21,     0,     0,    21,     0,    21,    21,    21,
    21,    22,    22,    22,    22,     0,    22,     0,    22,     0,
    22,    22,    22,    22,     0,     0,    22,     0,    22,    22,
    22,    22,    30,    30,    30,    30,     0,    30,     0,    30,
     0,    30,    30,     0,     0,     0,     0,    30,     0,    30,
    30,    30,    30,    28,    28,    28,    28,     0,    28,     0,
    28,     0,    28,    28,     0,     0,     0,     0,    28,     0,
    28,    28,    28,    28,    31,    31,    31,    31,     0,    31,
     0,    31,     0,    31,    31,     0,     0,     0,     0,    31,
     0,    31,    31,    31,    31,    29,    29,    29,    29,     0,
    29,     0,    29,     0,    29,    29,     0,     0,     0,     0,
    29,     0,    29,    29,    29,    29,    27,    27,    27,    27,
     0,    27,     0,    27,     0,    27,    27,    25,    25,    25,
    25,    27,    25,     0,    25,     0,    25,    20,    20,    20,
    20,     0,    20,     0,    20,     0,    20,    26,    26,    26,
     0,     0,    26,     0,    26,     0,    26,
    }, yyCheck = new short[] {
//yyCheck 447
     2,     0,     0,   263,     0,     7,     8,   268,   269,   270,
   271,   272,   273,   264,   275,   276,   277,   278,    20,   282,
   283,     0,     0,    25,    13,    27,    28,    29,    30,    31,
    32,    33,    34,    35,    36,    37,    -1,   257,    40,   271,
   272,    -1,   262,     0,   264,   257,   266,   267,    -1,    -1,
   262,    -1,   264,    -1,   266,   269,   270,   271,   272,   279,
   280,   281,   258,   259,     0,   261,    -1,   279,   280,   281,
    -1,   267,   268,   269,   270,   271,   272,   273,    -1,   275,
   276,   277,   278,    -1,   258,     0,   260,   261,    -1,    -1,
    -1,    -1,    -1,    -1,   268,   269,   270,   271,   272,   273,
    -1,   275,   276,   277,   278,   258,     0,    -1,   261,    -1,
    -1,    -1,   265,    -1,    -1,   268,   269,   270,   271,   272,
   273,    -1,   275,   276,   277,   278,   258,     0,    -1,   261,
    -1,    -1,    -1,   265,    -1,    -1,   268,   269,   270,   271,
   272,   273,    -1,   275,   276,   277,   278,    -1,     0,    -1,
    -1,    -1,    -1,   258,    -1,    -1,   261,    -1,    -1,     0,
    -1,    -1,   267,   268,   269,   270,   271,   272,   273,     0,
   275,   276,   277,   278,   258,    -1,    -1,   261,    -1,     0,
    -1,    -1,    -1,    -1,   268,   269,   270,   271,   272,   273,
   261,   275,   276,   277,   278,    -1,    -1,   268,   269,   270,
   271,   272,   273,    -1,   275,   276,   277,   278,   269,   270,
   271,   272,    -1,    -1,   275,   276,   277,   278,    -1,    -1,
    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,
    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,
    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,
    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,    -1,   258,
   259,   260,   261,    -1,   263,   263,   265,   263,   267,   268,
   269,   270,   271,   272,   273,    -1,   275,   276,   277,   278,
   258,   259,   260,   261,    -1,   263,    -1,   265,    -1,   267,
   268,   269,   270,    -1,    -1,   273,    -1,   275,   276,   277,
   278,   258,   259,   260,   261,    -1,   263,    -1,   265,    -1,
   267,   268,   269,   270,    -1,    -1,   273,    -1,   275,   276,
   277,   278,   258,   259,   260,   261,    -1,   263,    -1,   265,
    -1,   267,   268,    -1,    -1,    -1,    -1,   273,    -1,   275,
   276,   277,   278,   258,   259,   260,   261,    -1,   263,    -1,
   265,    -1,   267,   268,    -1,    -1,    -1,    -1,   273,    -1,
   275,   276,   277,   278,   258,   259,   260,   261,    -1,   263,
    -1,   265,    -1,   267,   268,    -1,    -1,    -1,    -1,   273,
    -1,   275,   276,   277,   278,   258,   259,   260,   261,    -1,
   263,    -1,   265,    -1,   267,   268,    -1,    -1,    -1,    -1,
   273,    -1,   275,   276,   277,   278,   258,   259,   260,   261,
    -1,   263,    -1,   265,    -1,   267,   268,   258,   259,   260,
   261,   273,   263,    -1,   265,    -1,   267,   258,   259,   260,
   261,    -1,   263,    -1,   265,    -1,   267,   258,   259,   260,
    -1,    -1,   263,    -1,   265,    -1,   267,
    };

  /// <summary>
  ///   maps symbol value to printable name.
  ///   see <c>yyExpecting</c>
  /// </summary>
  protected static readonly string[] yyNames = {
    "end-of-file",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,"Number","BeginClauseBody","Guard",
    "Priod","Comma","Identifier","Unknown","ParentheisOpen",
    "ParentheisClose","BracketOpen","BracketClose","OpEqual","OpAdd",
    "OpSub","OpMul","OpDiv","OpIs","OpNot","OpLe","OpGe","OpLt","OpGt",
    "OpExclamation","SingleQuoted","DoubleQuoted","StProgram","StTerm",
    };

//t  /// <summary>
//t  ///   printable rules for debugging.
//t  /// </summary>
//t  protected static readonly string [] yyRule = {
//t    "$accept : Entry",
//t    "Entry : SyntaxBody Unknown",
//t    "Entry : SyntaxBody",
//t    "SyntaxBody : StProgram Program",
//t    "SyntaxBody : StTerm Term",
//t    "Program : ClauseList",
//t    "ClauseList : ClauseList Clause",
//t    "ClauseList : Clause",
//t    "Clause : Term Priod",
//t    "Term : OpExclamation",
//t    "Term : Identifier",
//t    "Term : SingleQuoted",
//t    "Term : Number",
//t    "Term : Identifier ParentheisOpen Term ParentheisClose",
//t    "Term : Expr",
//t    "Term : List",
//t    "Term : DoubleQuoted",
//t    "List : BracketOpen BracketClose",
//t    "List : BracketOpen Term BracketClose",
//t    "List : BracketOpen Term Guard Term BracketClose",
//t    "Expr : Term OpEqual Term",
//t    "Expr : Term OpAdd Term",
//t    "Expr : Term OpSub Term",
//t    "Expr : Term OpMul Term",
//t    "Expr : Term OpDiv Term",
//t    "Expr : Term Comma Term",
//t    "Expr : Term BeginClauseBody Term",
//t    "Expr : Term OpIs Term",
//t    "Expr : Term OpGe Term",
//t    "Expr : Term OpGt Term",
//t    "Expr : Term OpLe Term",
//t    "Expr : Term OpLt Term",
//t    "Expr : ParentheisOpen Term ParentheisClose",
//t    };
//t
//t  /// <summary>
//t  ///   debugging support, requires <c>yyDebug</c>.
//t  ///   Set to <c>null</c> to suppress debugging messages.
//t  /// </summary>
//t  protected yyDebug.yyDebug yyDebug;
//t
//t  /// <summary>
//t  ///   index-checked interface to <c>yyNames[]</c>.
//t  /// </summary>
//t  /// <param name='token'>single character or <c>%token</c> value</param>
//t  /// <returns>token name or <c>[illegal]</c> or <c>[unknown]</c></returns>
//t  public static string yyName (int token) {
//t    if ((token < 0) || (token > yyNames.Length)) return "[illegal]";
//t    string name;
//t    if ((name = yyNames[token]) != null) return name;
//t    return "[unknown]";
//t  }
//t
  /// <summary>
  ///   thrown for irrecoverable syntax errors and stack overflow.
  /// </summary>
  /// <remarks>
  ///   Nested for convenience, does not depend on parser class.
  /// </remarks>
  public class yyException : System.Exception {
    public yyException (string message) : base (message) {
    }
  }

  /// <summary>
  ///   must be implemented by a scanner object to supply input to the parser.
  /// </summary>
  /// <remarks>
  ///   Nested for convenience, does not depend on parser class.
  /// </remarks>
  public interface yyInput {

    /// <summary>
    ///   move on to next token.
    /// </summary>
    /// <returns><c>false</c> if positioned beyond tokens</returns>
    /// <exception><c>IOException</c> on input error</exception>
    bool Advance ();

    /// <summary>
    ///   classifies current token by <c>%token</c> value or single character.
    /// </summary>
    /// <remarks>
    ///   Should not be called if <c>Advance()</c> returned false.
    /// </remarks>
    int Token { get; }

    /// <summary>
    ///   value associated with current token.
    /// </summary>
    /// <remarks>
    ///   Should not be called if <c>Advance()</c> returned false.
    /// </remarks>
    object Value { get; }
  }

  /// <summary>
  ///   simplified error message.
  /// </summary>
  public void yyError (string message) {
    yyError(message, null);
  }

  /// <summary>
  ///   (syntax) error message.
  ///   Can be overwritten to control message format.
  /// </summary>
  /// <param name='message'>text to be displayed</param>
  /// <param name='expected'>list of acceptable tokens, if available</param>
  public void yyError (string message, string[] expected) {
    if ((expected != null) && (expected.Length > 0)) {
      System.Console.Write (message+", expecting");
      for (int n = 0; n < expected.Length; ++ n)
        System.Console.Write(" "+expected[n]);
        System.Console.WriteLine();
    } else
      System.Console.WriteLine(message);
  }

  /// <summary>
  ///   computes list of expected tokens on error by tracing the tables.
  /// </summary>
  /// <param name='state'>for which to compute the list</param>
  /// <returns>list of token names</returns>
  protected string[] yyExpecting (int state) {
    int token, n, len = 0;
    bool[] ok = new bool[yyNames.Length];

    if ((n = yySindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    if ((n = yyRindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }

    string [] result = new string[len];
    for (n = token = 0; n < len;  ++ token)
      if (ok[token]) result[n++] = yyNames[token];
    return result;
  }

  /// <summary>
  ///   the generated parser, with debugging messages.
  ///   Maintains a dynamic state and value stack.
  /// </summary>
  /// <param name='yyLex'>scanner</param>
  /// <param name='yyDebug'>debug message writer implementing <c>yyDebug</c>,
  ///   or <c>null</c></param>
  /// <returns>result of the last reduction, if any</returns>
  /// <exceptions><c>yyException</c> on irrecoverable parse error</exceptions>
  public object yyParse (yyInput yyLex, object yyDebug) {
//t    this.yyDebug = (yyDebug.yyDebug)yyDebug;
    return yyParse(yyLex);
  }

  /// <summary>
  ///   initial size and increment of the state/value stack [default 256].
  ///    This is not final so that it can be overwritten outside of invocations
  ///    of <c>yyParse()</c>.
  /// </summary>
  protected int yyMax;

  /// <summary>
  ///   executed at the beginning of a reduce action.
  ///   Used as <c>$$ = yyDefault($1)</c>, prior to the user-specified action, if any.
  ///   Can be overwritten to provide deep copy, etc.
  /// </summary>
  /// <param first value for $1, or null.
  /// <return first.
  protected object yyDefault (object first) {
    return first;
  }

  /// <summary>
  ///   the generated parser, with debugging messages.
  ///   Maintains a dynamic state and value stack.
  /// </summary>
  /// <param name='yyLex'>scanner</param>
  /// <returns>result of the last reduction, if any</returns>
  /// <exceptions><c>yyException</c> on irrecoverable parse error</exceptions>
  public object yyParse (yyInput yyLex) {
    if (yyMax <= 0) yyMax = 256;			// initial size
    int yyState = 0;                                   // state stack ptr
    int [] yyStates = new int[yyMax];	                // state stack 
    object yyVal = null;                               // value stack ptr
    object [] yyVals = new object[yyMax];	        // value stack
    int yyToken = -1;					// current input
    int yyErrorFlag = 0;				// #tokens to shift

    int yyTop = 0;
    goto skip;
    yyLoop:
    yyTop++;
    skip:
    for (;; ++ yyTop) {
      if (yyTop >= yyStates.Length) {			// dynamically increase
        int[] i = new int[yyStates.Length+yyMax];
        yyStates.CopyTo (i, 0);
        yyStates = i;
        object[] o = new object[yyVals.Length+yyMax];
        yyVals.CopyTo (o, 0);
        yyVals = o;
      }
      yyStates[yyTop] = yyState;
      yyVals[yyTop] = yyVal;
//t      if (yyDebug != null) yyDebug.push(yyState, yyVal);

      yyDiscarded: for (;;) {	// discarding a token does not change stack
        int yyN;
        if ((yyN = yyDefRed[yyState]) == 0) {	// else [default] reduce (yyN)
          if (yyToken < 0) {
            yyToken = yyLex.Advance() ? yyLex.Token : 0;
//t            if (yyDebug != null)
//t              yyDebug.lex(yyState, yyToken, yyName(yyToken), yyLex.Value);
          }
          if ((yyN = yySindex[yyState]) != 0 && ((yyN += yyToken) >= 0)
              && (yyN < yyTable.Length) && (yyCheck[yyN] == yyToken)) {
//t            if (yyDebug != null)
//t              yyDebug.shift(yyState, yyTable[yyN], yyErrorFlag > 0 ? yyErrorFlag-1 : 0);
            yyState = yyTable[yyN];		// shift to yyN
            yyVal = yyLex.Value;
            yyToken = -1;
            if (yyErrorFlag > 0) -- yyErrorFlag;
            goto yyLoop;
          }
          if ((yyN = yyRindex[yyState]) != 0 && (yyN += yyToken) >= 0
              && yyN < yyTable.Length && yyCheck[yyN] == yyToken)
            yyN = yyTable[yyN];			// reduce (yyN)
          else
            switch (yyErrorFlag) {
  
            case 0:
              yyError("syntax error", yyExpecting(yyState));
//t              if (yyDebug != null) yyDebug.error("syntax error");
              goto case 1;
            case 1: case 2:
              yyErrorFlag = 3;
              do {
                if ((yyN = yySindex[yyStates[yyTop]]) != 0
                    && (yyN += yyErrorCode) >= 0 && yyN < yyTable.Length
                    && yyCheck[yyN] == yyErrorCode) {
//t                  if (yyDebug != null)
//t                    yyDebug.shift(yyStates[yyTop], yyTable[yyN], 3);
                  yyState = yyTable[yyN];
                  yyVal = yyLex.Value;
                  goto yyLoop;
                }
//t                if (yyDebug != null) yyDebug.pop(yyStates[yyTop]);
              } while (-- yyTop >= 0);
//t              if (yyDebug != null) yyDebug.reject();
              throw new yyException("irrecoverable syntax error");
  
            case 3:
              if (yyToken == 0) {
//t                if (yyDebug != null) yyDebug.reject();
                throw new yyException("irrecoverable syntax error at end-of-file");
              }
//t              if (yyDebug != null)
//t                yyDebug.discard(yyState, yyToken, yyName(yyToken),
//t  							yyLex.Value);
              yyToken = -1;
              goto yyDiscarded;		// leave stack alone
            }
        }
        int yyV = yyTop + 1-yyLen[yyN];
//t        if (yyDebug != null)
//t          yyDebug.reduce(yyState, yyStates[yyV-1], yyN, yyRule[yyN], yyLen[yyN]);
        yyVal = yyDefault(yyV > yyTop ? null : yyVals[yyV]);
        switch (yyN) {
case 1:
#line 77 "PrlgParserJay.jay"
  {
		yyVal = yyVals[-1+yyTop];
      }
  break;
case 2:
#line 81 "PrlgParserJay.jay"
  {
		yyVal = yyVals[0+yyTop];
      }
  break;
case 3:
#line 88 "PrlgParserJay.jay"
  {
		yyVal = ((PrologProgram)yyVals[0+yyTop]);
	  }
  break;
case 4:
#line 92 "PrlgParserJay.jay"
  {
		yyVal = ((Term)yyVals[0+yyTop]);
	  }
  break;
case 5:
#line 98 "PrlgParserJay.jay"
  {
		yyVal = ((PrologProgram)yyVals[0+yyTop]);
      }
  break;
case 6:
#line 105 "PrlgParserJay.jay"
  {
	  	((PrologProgram)yyVals[-1+yyTop]).Add(((Clause)yyVals[0+yyTop]));
	  	yyVal = ((PrologProgram)yyVals[-1+yyTop]);
	  }
  break;
case 7:
#line 110 "PrlgParserJay.jay"
  {
	  	PrologProgram ret = new PrologProgram();
	  	ret.Add(((Clause)yyVals[0+yyTop]));
	  	yyVal = ret;
	  }
  break;
case 8:
#line 119 "PrlgParserJay.jay"
  {
	  	yyVal = new Clause(((Term)yyVals[-1+yyTop]));
	  }
  break;
case 9:
#line 127 "PrlgParserJay.jay"
  {
		Term ret;
		yyVal = Term.imm(((String)yyVals[0+yyTop]));
	  }
  break;
case 10:
#line 132 "PrlgParserJay.jay"
  {
		Term ret;
		if(char.IsUpper(((String)yyVals[0+yyTop])[0]))
		{
			ret = Term.var(((String)yyVals[0+yyTop]));
		}
		else
		{
			ret = Term.imm(((String)yyVals[0+yyTop]));
		}
		yyVal = ret;
	  }
  break;
case 11:
#line 145 "PrlgParserJay.jay"
  {
		yyVal = Term.imm(((String)yyVals[0+yyTop]));
	  }
  break;
case 12:
#line 149 "PrlgParserJay.jay"
  {
		yyVal = Term.Number(((String)yyVals[0+yyTop]));
	  }
  break;
case 13:
#line 153 "PrlgParserJay.jay"
  {
		Term ret = new Term();
		if(char.IsUpper(((String)yyVals[-3+yyTop])[0]))
		{
			ret = Term.imm(((String)yyVals[-3+yyTop]));
		}
		else
		{
			ret = Term.imm(((String)yyVals[-3+yyTop]));
		}
		ret.SetPrms(Term.CommaListToList(((Term)yyVals[-1+yyTop])));
		yyVal = ret;
	  }
  break;
case 14:
#line 167 "PrlgParserJay.jay"
  {
	  	yyVal = ((Term)yyVals[0+yyTop]);
	  }
  break;
case 15:
#line 171 "PrlgParserJay.jay"
  {
	    yyVal = ((Term)yyVals[0+yyTop]);
	  }
  break;
case 16:
#line 175 "PrlgParserJay.jay"
  {
	    yyVal = Term.StringToList(((String)yyVals[0+yyTop]));
	  }
  break;
case 17:
#line 183 "PrlgParserJay.jay"
  {
		yyVal = Term.EmptyList();
	  }
  break;
case 18:
#line 187 "PrlgParserJay.jay"
  {
		yyVal = Term.ListToDotList(Term.CommaListToList(((Term)yyVals[-1+yyTop])));
	  }
  break;
case 19:
#line 191 "PrlgParserJay.jay"
  {
		yyVal = Term.ListToDotList(Term.CommaListToList(((Term)yyVals[-3+yyTop])), ((Term)yyVals[-1+yyTop]));
	  }
  break;
case 20:
#line 198 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 21:
#line 202 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 22:
#line 206 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 23:
#line 210 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 24:
#line 214 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 25:
#line 218 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 26:
#line 222 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 27:
#line 226 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 28:
#line 230 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 29:
#line 234 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 30:
#line 238 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 31:
#line 242 "PrlgParserJay.jay"
  {
      	yyVal = Term.imm(((String)yyVals[-1+yyTop]), ((Term)yyVals[-2+yyTop]), ((Term)yyVals[0+yyTop]));
      }
  break;
case 32:
#line 246 "PrlgParserJay.jay"
  {
        yyVal = ((Term)yyVals[-1+yyTop]);
      }
  break;
#line 730 "-"
        }
        yyTop -= yyLen[yyN];
        yyState = yyStates[yyTop];
        int yyM = yyLhs[yyN];
        if (yyState == 0 && yyM == 0) {
//t          if (yyDebug != null) yyDebug.shift(0, yyFinal);
          yyState = yyFinal;
          if (yyToken < 0) {
            yyToken = yyLex.Advance() ? yyLex.Token : 0;
//t            if (yyDebug != null)
//t               yyDebug.lex(yyState, yyToken,yyName(yyToken), yyLex.Value);
          }
          if (yyToken == 0) {
//t            if (yyDebug != null) yyDebug.accept(yyVal);
            return yyVal;
          }
          goto yyLoop;
        }
        if (((yyN = yyGindex[yyM]) != 0) && ((yyN += yyState) >= 0)
            && (yyN < yyTable.Length) && (yyCheck[yyN] == yyState))
          yyState = yyTable[yyN];
        else
          yyState = yyDgoto[yyM];
//t        if (yyDebug != null) yyDebug.shift(yyStates[yyTop], yyState);
	 goto yyLoop;
      }
    }
  }

#line 251 "PrlgParserJay.jay"

      

  }


}

#line 769 "-"
