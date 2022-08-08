using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierServer
{
	public static class SplitWithExcapeExtentions
	{

		public static string[] SplitWithEscape(this string inputStr, char delimiter, char escape, bool removeEmptyEntries=true)
		{
			StringBuilder sb = new StringBuilder();
			bool skip = false;
			
			List<string> split = new List<string>();

			foreach (char character in inputStr)
			{
				if (skip)
				{
					skip = false;
					continue;
				}

				if(character == escape)
				{
					skip = true;
					continue;
				}

				if(character == delimiter)
				{
					var str = sb.ToString();
					if (!string.IsNullOrWhiteSpace(str))
					{
						split.Add(str);
					}
					sb.Clear();

				}

			}

			return split.ToArray();

		}
	}
}
