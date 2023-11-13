// ����SharpZipLib�Ĵ�������޸�
// ��Ҫ�Ľ� 
//    1> �޸��ڲ����ʽ�ṹΪList<Regex> ���Ӷ���ø��õ�����
//    2> ���Ӳ��ֺ����Ĳ�����֤
//    3> �޸ĺ���˵��Ϊ����

namespace ExpertLib.IO
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using ExpertLib;

    /// <summary>
    /// StringFilter��һ��������ƥ��͸�ƥ����ַ���������.
    /// A filter is a sequence of independant <see cref="Regex">regular expressions</see> separated by semi-colons ';'
    /// Each expression can be prefixed by a plus '+' sign or a minus '-' sign to denote the expression
    /// is intended to include or exclude names.  If neither a plus or minus sign is found include is the default
    /// A given name is tested for inclusion before checking exclusions.  Only names matching an include spec 
    /// and not matching an exclude spec are deemed to match the filter.
    /// An empty filter matches any name.
    /// </summary>
    /// <example>The following expression includes all name ending in '.dat' with the exception of 'dummy.dat'
    /// "+\.dat$;-^dummy\.dat$"
    /// </example>
    public class StringFilter : IScanFilter
    {
        #region ������
        /// <summary>
        /// ����һ���ַ���������
        /// </summary>
        /// <param name="filter">���������ʽ</param>
        /// <remarks>����ǿ��ַ����������ȷ�Ĺ��������ʽ�������һ�������쳣
        /// </remarks>
        public StringFilter(string filter)
        {
            ArgumentValidation.CheckForEmptyString(filter,"filter");
            
            if (!IsValidFilterExpression(filter))
            {
                throw new ArgumentException(SR.ExceptionInvalidFilterExpress(filter));
            }

            filter_ = filter;
            inclusions_ = new List<Regex>();
            exclusions_ = new List<Regex>();
            Compile();
        }
        #endregion

        #region IsValidExpression
        /// <summary>
        /// ����һ���ַ����Ƿ�Ϸ���������ʽ.
        /// </summary>
        /// <param name="expression">��Ҫ���Եı��ʽ</param>
        /// <returns>True if expression is a valid <see cref="System.Text.RegularExpressions.Regex"/> false otherwise.</returns>
        public static bool IsValidExpression(string expression)
        {
            bool result = true;
            try
            {
                Regex exp = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region IsValidFilterExpression
        /// <summary>
        /// ����һ�����ʽ�Ƿ����Filter
        /// </summary>
        /// <param name="toTest">The filter expression to test.</param>
        /// <returns>True if the expression is valid, false otherwise.</returns>
        public static bool IsValidFilterExpression(string toTest)
        {
            ArgumentValidation.CheckForEmptyString(toTest, "toTest");

            bool result = true;

            try
            {
                string[] items = toTest.Split(';');
                for (int i = 0; i < items.Length; ++i)
                {
                    if (items[i] != null && items[i].Length > 0)
                    {
                        string toCompile;

                        if (items[i][0] == '+')
                        {
                            toCompile = items[i].Substring(1, items[i].Length - 1);
                        }
                        else if (items[i][0] == '-')
                        {
                            toCompile = items[i].Substring(1, items[i].Length - 1);
                        }
                        else
                        {
                            toCompile = items[i];
                        }

                        Regex testRegex = new Regex(toCompile, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region ToString
        /// <summary>
        /// ת���ַ���
        /// </summary>
        /// <returns>The string equivalent for this filter.</returns>
        public override string ToString()
        {
            return filter_;
        }
        #endregion

        #region IsIncluded
        /// <summary>
        /// �����ַ����Ƿ��ڰ���ƥ����
        /// </summary>
        /// <param name="name">The value to test.</param>
        /// <returns>True if the value is included, false otherwise.</returns>
        public bool IsIncluded(string name)
        {
            ArgumentValidation.CheckForEmptyString(name, "name");

            bool result = false;
            if (inclusions_.Count == 0)
            {
                result = true;
            }
            else
            {
                foreach (Regex r in inclusions_)
                {
                    if (r.IsMatch(name))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        #endregion

        #region IsExcluded
        /// <summary>
        /// �����ַ����Ƿ��ų�ƥ����
        /// </summary>
        /// <param name="name">The value to test.</param>
        /// <returns>True if the value is excluded, false otherwise.</returns>
        public bool IsExcluded(string name)
        {
            bool result = false;
            foreach (Regex r in exclusions_)
            {
                if (r.IsMatch(name))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion

        #region IsMatch
        /// <summary>
        /// �����Ƿ�ƥ��.
        /// </summary>
        /// <param name="name">The value to test.</param>
        /// <returns>True if the value matches, false otherwise.</returns>
        public bool IsMatch(string name)
        {
            return (IsIncluded(name) == true) && (IsExcluded(name) == false);
        }
        #endregion

        #region private 
        /// <summary>
        /// Compile this filter.
        /// </summary>
        void Compile()
        {
            string[] items = filter_.Split(';');
            for (int i = 0; i < items.Length; ++i)
            {
                if ((items[i] != null) && (items[i].Length > 0))
                {
                    bool include = (items[i][0] != '-');
                    string toCompile;

                    if (items[i][0] == '+')
                    {
                        toCompile = items[i].Substring(1, items[i].Length - 1);
                    }
                    else if (items[i][0] == '-')
                    {
                        toCompile = items[i].Substring(1, items[i].Length - 1);
                    }
                    else
                    {
                        toCompile = items[i];
                    }

                    if (include)
                    {
                        inclusions_.Add(new Regex(toCompile, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline));
                    }
                    else
                    {
                        exclusions_.Add(new Regex(toCompile, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline));
                    }
                }
            }
        }
        #endregion

        #region Instance Fields
        string filter_;
        List<Regex> inclusions_;
        List<Regex> exclusions_;
        #endregion
    }
}
