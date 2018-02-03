using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace CSProlog
{
    /// <summary>
    /// 組み込み述語の例外
    /// </summary>
	public class EmbeddedPredicateError: System.Exception
	{
		public EmbeddedPredicateError(string msg)
			: base(msg)
		{}
	}

    /// <summary>
    /// 組み込み述語のストア
    /// </summary>
	public class EmbeddedPredicateStore
	{
		Dictionary<string, EmbeddedPredicate.EPBase> preds;

        /// <summary>
        /// コンストラクタ
        /// </summary>
		public EmbeddedPredicateStore()
		{
			preds = new Dictionary<string, EmbeddedPredicate.EPBase>();
		}
		
		/// <summary>
		/// 指定されたゴールにマッチ(パラメータ数、述語名のみ)する述語を検索する
		/// </summary>
		/// <param name="goal">ゴール</param>
        /// <param name="pred">発見した組み込み述語を格納する変数</param>
		/// <returns>発見された場合true</returns>
		public bool FindPredicate(Term goal, ref EmbeddedPredicate.EPBase pred)
		{
			string key = goal.GetValue() + "/" + goal.GetPrmCount().ToString();
			if(preds.ContainsKey(key))
			{
				pred = preds[key];
				return true;
			}
			return false;
		}
		
        /// <summary>
        /// 組み込み述語をストアにロード
        /// </summary>
        /// <param name="ep">ロードする述語</param>
        /// <returns></returns>
		public bool LoadPredicate(EmbeddedPredicate.EPBase ep)
		{			
			preds[ep.GetKey()] = ep;
			return true;
		}
		
        /// <summary>
        /// 標準の述語をロード
        /// </summary>
		public void LoadDefaultPredicates()
		{
			LoadPredicate(new EmbeddedPredicate.EP_Is());
			LoadPredicate(new EmbeddedPredicate.EP_Atom());

			LoadPredicate(new EmbeddedPredicate.EP_NewLine());
			LoadPredicate(new EmbeddedPredicate.EP_Print());

            LoadPredicate(new EmbeddedPredicate.EP_ArithmeticCompareOperater(
                    "</2", (l, r) => l.GetIntegerValue() < r.GetIntegerValue()
                ));
            LoadPredicate(new EmbeddedPredicate.EP_ArithmeticCompareOperater(
                    ">/2", (l, r) => l.GetIntegerValue() > r.GetIntegerValue()
                ));
            LoadPredicate(new EmbeddedPredicate.EP_ArithmeticCompareOperater(
                    "=</2", (l, r) => l.GetIntegerValue() <= r.GetIntegerValue()
                ));
            LoadPredicate(new EmbeddedPredicate.EP_ArithmeticCompareOperater(
                    ">=/2", (l, r) => l.GetIntegerValue() >= r.GetIntegerValue()
                ));


        }
	}

    /// <summary>
    /// 計算可能な項を扱うクラス
    /// </summary>
    public class ArithmeticTerm
    {
        /// <summary>
        /// 項を計算する
        /// </summary>
        /// <param name="expr">式</param>
        /// <returns>計算結果の項</returns>
		public static Term Calculate(Term expr)
		{
			if(expr.IsVariable())
			{
				throw new EmbeddedPredicateError("具体化されていない項があります: " + expr.GetValue());
			}
			if(expr.GetPrmCount() == 0)
			{
				return expr;
			}
			Term[] ct = new Term[expr.GetPrmCount()];
			for(int i = 0; i < expr.GetPrmCount(); i++)
			{
				ct[i] = ArithmeticTerm.Calculate(expr.GetPrm(i));
			}
			if(expr.GetValue() == "+" && expr.GetPrmCount() == 2){
				return Term.Integer(ct[0].GetIntegerValue() + ct[1].GetIntegerValue());
			}
			if(expr.GetValue() == "-" && expr.GetPrmCount() == 2){
				return Term.Integer(ct[0].GetIntegerValue() - ct[1].GetIntegerValue());
			}
			if(expr.GetValue() == "*" && expr.GetPrmCount() == 2){
				return Term.Integer(ct[0].GetIntegerValue() * ct[1].GetIntegerValue());
			}
			if(expr.GetValue() == "/" && expr.GetPrmCount() == 2){
				return Term.Integer(ct[0].GetIntegerValue() / ct[1].GetIntegerValue());
			}
            /*
			if(expr.GetValue() == "op_lt" && expr.GetPrmCount() == 2){
				return Term.Number(ct[0].GetIntegerValue() < ct[1].GetIntegerValue() ? "1" : "0");
			}
			if(expr.GetValue() == "op_gt" && expr.GetPrmCount() == 2){
				return Term.Number(ct[0].GetIntegerValue() > ct[1].GetIntegerValue() ? "1" : "0");
			}
			if(expr.GetValue() == "op_le" && expr.GetPrmCount() == 2){
				return Term.Number(ct[0].GetIntegerValue() <= ct[1].GetIntegerValue() ? "1" : "0");
			}
			if(expr.GetValue() == "op_ge" && expr.GetPrmCount() == 2){
				return Term.Number(ct[0].GetIntegerValue() >= ct[1].GetIntegerValue() ? "1" : "0");
			}
			if(expr.GetValue() == "==" && expr.GetPrmCount() == 2){
				return Term.Number(ct[0].GetIntegerValue() == ct[1].GetIntegerValue() ? "1" : "0");
			}
             */

			throw new EmbeddedPredicateError("未知のオペレータ: " + expr.GetValue());
		}

    }

	namespace EmbeddedPredicate
	{
        /// <summary>
        /// 組み込み述語の基底クラス
        /// </summary>
		public abstract class EPBase
		{
            /// <summary>
            /// 述語の名前を返す
            /// </summary>
            /// <returns>述語の名前</returns>
			public abstract string GetKey();
            /// <summary>
            /// 述語を適用する
            /// </summary>
            /// <param name="goal">ゴール</param>
            /// <returns>ユニフィケーション結果</returns>
			public abstract IEnumerable<Dictionary<string, Term>> Apply(Env env, Term goal);
        }

        /// <summary>
        /// 算術比較演算子
        /// </summary>
        public class EP_ArithmeticCompareOperater : EPBase
        {
            CompareOp Oper;
            string OpName;
            /// <summary>
            /// 項の比較デリゲート
            /// </summary>
            /// <param name="lhs">左辺値</param>
            /// <param name="rhs">右辺値</param>
            /// <returns>条件が満たされる場合のみtrue</returns>
            public delegate bool CompareOp(Term lhs, Term rhs);

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="name">オペレータの名前</param>
            /// <param name="op">比較デリゲート</param>
            public EP_ArithmeticCompareOperater(string name, CompareOp op)
            {
                OpName = name;
                Oper = op;
            }

            /// <summary>
            /// 述語名を返す
            /// </summary>
            /// <returns>述語名</returns>
            public override string GetKey() { return OpName; }

            /// <summary>
            /// 比較述語を適用
            /// </summary>
            /// <param name="goal">ゴール</param>
            /// <returns>ユニフィケーション結果</returns>
            public override IEnumerable<Dictionary<string, Term>> Apply(Env env, Term goal)
            {
                Term lhs = goal.GetPrm(0);
                Term rhs = goal.GetPrm(1);

                if(Oper(lhs, rhs))
                    yield return Prolog.EmptyDict();
            }

        }

        /// <summary>
        /// is/2演算子
        /// </summary>
        public class EP_Is: EPBase
		{
			public override string GetKey(){return "is/2";}

			public override IEnumerable<Dictionary<string, Term>> Apply(Env env, Term goal)
			{
				Term target = goal.GetPrm(0);
				Term expression = goal.GetPrm(1);
				
				Dictionary<string, Term> dict = new Dictionary<string, Term>();
				if(Prolog.Unify(target, ArithmeticTerm.Calculate(expression), dict, null))
					yield return dict;
			}

		}

        /// <summary>
        /// atom/1
        /// </summary>
        public class EP_Atom: EPBase
		{
			public override string GetKey(){return "atom/1";}

			public override IEnumerable<Dictionary<string, Term>> Apply(Env env, Term goal)
			{
				Term target = goal.GetPrm(0);
				
				
				if(target.IsAtom())
				{
					yield return Prolog.EmptyDict();
				}
			}

		}

        /// <summary>
        /// print/1
        /// </summary>
        public class EP_Print: EPBase
		{
			public override string GetKey(){return "print/1";}

			public override IEnumerable<Dictionary<string, Term>> Apply(Env env, Term goal)
			{
				env.PrintCallback(0, goal.GetPrm(0).ToStr());
				yield return Prolog.EmptyDict();
			}

		}
        /// <summary>
        /// nl/0
        /// </summary>
        public class EP_NewLine: EPBase
		{
			public override string GetKey(){return "nl/0";}

			public override IEnumerable<Dictionary<string, Term>> Apply(Env env, Term goal)
			{
				env.PrintCallback(0, "\n");
				yield return Prolog.EmptyDict();
			}

		}
	}

}


