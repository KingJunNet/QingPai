using System;
using System.Collections;
using ExpertLib;
namespace ExpertLib.Formula
{
    /// <summary>
    /// ��ʽ������
    /// </summary>
	public static class Evaluator 
	{
        /// <summary>
        /// ����ʽֵ
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
		public static Variant Evaluate(string s)
		{
			ExecutionQueue eq = ParseIt(s);
			return CalcIt(eq);
		}

        /// <summary>
        /// �������ʽ
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
		public static ExecutionQueue ParseIt(string s)
		{
			Parser p = new Parser();
			p.ParseIt(s);
			return p.eqResult;
		}

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="eq"></param>
        /// <returns></returns>
		public static Variant CalcIt(ExecutionQueue eq)
		{
			Calculator c = new Calculator();
			return c.CalcIt(eq);
		}

        /// <summary>
        /// �������������ϣ��
        /// </summary>
		public static Hashtable Variables
		{
			get
			{
				return Calculator.Variables;
			}
			set
			{
				Calculator.Variables = value;
			}
		}

	
	}
}
