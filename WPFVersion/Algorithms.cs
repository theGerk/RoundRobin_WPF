using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalized
{
	public static class Algorithms
	{
		public struct SearchReturn
		{
			public SearchReturn(int position, bool exists)
			{ this.position = position; this.exists = exists; }
			public int position;
			public bool exists;
		}

		public static SearchReturn BinarySearch<T>(IList<T> orderedList, T value, Comparison<T> compare = default(Comparison<T>))
		{
			return BinarySearch(orderedList, value, 0, orderedList.Count, delegate (IList<T> lst, int pos) { return lst[pos]; }, compare);
		}

		public static SearchReturn BinarySearch<T>(IList<T> orderedList, T value, int min, int max, Comparison<T> compare = default(Comparison<T>))
		{
			return BinarySearch(orderedList, value, min, max, delegate (IList<T> lst, int pos) { return lst[pos]; }, compare);
		}

		public static SearchReturn BinarySearch<A,T>(A orderedList, T value, int length, Func<A,int,T> getAtPosFunc, Comparison<T> compare = default(Comparison<T>))
		{
			return BinarySearch(orderedList, value, 0, length, getAtPosFunc, compare);
		} 

		public static SearchReturn BinarySearch<A,T>(A orderedList, T value, int min, int max, Func<A,int,T> getAtPosFunc, Comparison<T> compare = default(Comparison<T>))
		{
			while (min < max)
			{
				int guess = (min + max) / 2;
				int compareOutput = compare(value, getAtPosFunc(orderedList, guess));

				if (compareOutput < 0)
					max = guess;

				else if (compareOutput == 0)
					return new SearchReturn(guess, true);

				else
					min = guess + 1;


			}


			return new SearchReturn(min, false);
		}
	}
}