using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalized
{
	public class IndependentLinkedListNode<T>
	{
		public IndependentLinkedListNode<T> Next { get; set; }
		public T Value { get; set; }

		public IndependentLinkedListNode() { }
		public IndependentLinkedListNode(T value) { Value = value; }
		public IndependentLinkedListNode(T value, IndependentLinkedListNode<T> next) { Value = value; Next = next; }
		public IndependentLinkedListNode(IndependentLinkedListNode<T> next, T value = default(T)) { Next = next; Value = value; }
		public IndependentLinkedListNode(IEnumerable<T> values)
		{
			var tmp = this;
			foreach (T item in values)
			{
				tmp.Value = item;
				tmp.Next = new IndependentLinkedListNode<T>();
				tmp = tmp.Next;
			}
		}

		public bool IsEnd() { return Next == null; }

		public IndependentLinkedListNode<T> getEnd()
		{
			var fast = this;
			var slow = this;
			while (!fast.IsEnd())
			{
				slow = slow.Next;
				fast = fast.Next;

				if (fast.IsEnd())
					break;

				fast = fast.Next;
				if (fast == slow)
					throw new Exception("Infinite Loop");
			}

			return fast;
		}

		//returns last node inserted
		public IndependentLinkedListNode<T> Insert(T insert)
		{
			var tmp = Next;
			Next = new IndependentLinkedListNode<T>(insert);
			Next.Next = tmp;
			return Next;
		}
		public IndependentLinkedListNode<T> Insert(IndependentLinkedListNode<T> insert)
		{
			var ret = insert.getEnd();
			ret.Next = Next;
			Next = insert;
			return ret;
		}
		public IndependentLinkedListNode<T> Insert(IndependentLinkedListNode<T> insertStart, IndependentLinkedListNode<T> insertEnd)
		{ insertEnd.Next = Next; Next = insertStart; return insertEnd; }
		public IndependentLinkedListNode<T> Insert(IEnumerable<T> insert)
		{
			var hold = Next;
			var tmp = this;
			foreach (var item in insert)
			{
				tmp.Next = new IndependentLinkedListNode<T>(item);
				tmp = tmp.Next;
			}
			tmp.Next = hold;
			return tmp;
		}

		public void Append(T value) { getEnd().Next = new IndependentLinkedListNode<T>(value); }
		public void Append(IndependentLinkedListNode<T> append) { getEnd().Next = append; }
		public void Append(IEnumerable<T> append) { getEnd().Next = new IndependentLinkedListNode<T>(append); }

		public IndependentLinkedListNode<T> Prepend(T value) { return new IndependentLinkedListNode<T>(value, this); }
		public IndependentLinkedListNode<T> Prepend(IndependentLinkedListNode<T> prepend)
		{
			prepend.Next = this;
			return prepend;
		}

		public IEnumerator<T> getEnumerator()
		{
			var pos = this;
			do
			{
				yield return pos.Value;
				pos = pos.Next;
			} while (!pos.IsEnd());
		}
		public IEnumerator<IndependentLinkedListNode<T>> getNodeEnumerator()
		{
			var pos = this;
			do
			{
				yield return pos;
				pos = pos.Next;
			} while (!pos.IsEnd());
		}
	}

	public class IndependentDoublyLinkedListNode<T>
	{
		private IndependentDoublyLinkedListNode<T> _next;
		private IndependentDoublyLinkedListNode<T> _previous;
		public IndependentDoublyLinkedListNode<T> Next
		{
			get { return _next; }
			set
			{
				value._previous = this;
				_next = value;
			}
		}
		public IndependentDoublyLinkedListNode<T> Previous
		{
			get { return _previous; }
			set
			{
				value._next = this;
				_previous = value;
			}
		}
		public T Value { get; set; }

		public IndependentDoublyLinkedListNode() { }
		public IndependentDoublyLinkedListNode(T value) { Value = value; }
		public IndependentDoublyLinkedListNode(IndependentDoublyLinkedListNode<T> previous, T value, IndependentDoublyLinkedListNode<T> next)
		{ Previous = previous; Value = value; Next = next; }
		public IndependentDoublyLinkedListNode(T value, IndependentDoublyLinkedListNode<T> next, IndependentDoublyLinkedListNode<T> previous = null)
		{ Previous = previous; Value = value; Next = next; }
		public IndependentDoublyLinkedListNode(IEnumerable<T> values, IEnumerable<T> valuesPrevious = null)
		{
			var tmp = this;
			foreach (T item in values)
			{
				tmp.Value = item;
				tmp.Next = new IndependentDoublyLinkedListNode<T>();
				tmp = tmp.Next;
			}

			tmp = this;
			foreach (T item in valuesPrevious)
			{
				tmp.Previous = new IndependentDoublyLinkedListNode<T>(item);
				tmp = tmp.Previous;
			}
		}
		public IndependentDoublyLinkedListNode(T value, IEnumerable<T> valuesAfter, IEnumerable<T> valuesPrevious = null)
		{
			Value = value;

			var tmp = this;
			foreach (var item in valuesAfter)
				tmp = tmp.Next = new IndependentDoublyLinkedListNode<T>(item);

			tmp = this;
			foreach (var item in valuesPrevious)
				tmp = tmp.Previous = new IndependentDoublyLinkedListNode<T>(item);
		}

		public bool IsEnd() { return _next == null; }
		public bool IsStart() { return _previous == null; }

		public IndependentDoublyLinkedListNode<T> getEnd()
		{
			var fast = this;
			var slow = this;
			while (!fast.IsEnd())
			{
				slow = slow.Next;
				fast = fast.Next;

				if (fast.IsEnd())
					break;

				fast = fast.Next;
				if (fast == slow)
					throw new Exception("Infinite Loop");
			}

			return fast;
		}
		public IndependentDoublyLinkedListNode<T> getStart()
		{
			var fast = this;
			var slow = this;
			while (!fast.IsStart())
			{
				slow = slow.Previous;
				fast = fast.Previous;

				if (fast.IsStart())
					break;

				fast = fast.Previous;
				if (fast == slow)
					throw new Exception("Infinite Loop");
			}

			return fast;
		}

		public void InsertAfter(T insert)
		{
			new IndependentDoublyLinkedListNode<T>(this, insert, Next);
		}
		public void InsertAfter(IndependentDoublyLinkedListNode<T> insert)
		{
			Next.Previous = insert.getEnd();
			Next = insert.getStart();
		}
		public void InsertAfter(IndependentDoublyLinkedListNode<T> insertStart, IndependentDoublyLinkedListNode<T> insertEnd)
		{
			Next.Previous = insertEnd;
			Next = insertStart;
		}
		public void InsertAfter(IEnumerable<T> insert)
		{
			var hold = Next;
			var tmp = this;
			foreach (var item in insert)
			{
				tmp.Next = new IndependentDoublyLinkedListNode<T>(item);
				tmp = tmp.Next;
			}
			tmp.Next = hold;
		}

		public void InsertBefore(T insert)
		{
			new IndependentDoublyLinkedListNode<T>(Previous, insert, this);
		}
		public void InsertBefore(IndependentDoublyLinkedListNode<T> insert)
		{
			Previous.Next = insert.getStart();
			Previous = insert.getEnd();
		}
		public void InsertBefore(IndependentDoublyLinkedListNode<T> insertStart, IndependentDoublyLinkedListNode<T> insertEnd)
		{
			Previous.Next = insertStart;
			Previous = insertEnd;
		}
		public void InsertBefore(IEnumerable<T> insert)
		{
			var hold = Previous;
			var tmp = this;
			foreach (var item in insert)
			{
				tmp.Previous = new IndependentDoublyLinkedListNode<T>(item);
				tmp = tmp.Previous;
			}
			tmp.Previous = hold;
		}

		public void Append(T value) { getEnd().Next = new IndependentDoublyLinkedListNode<T>(value); }
		public void Append(IndependentDoublyLinkedListNode<T> append) { getEnd().Next = append; }
		public void Append(IEnumerable<T> append) { getEnd().Next = new IndependentDoublyLinkedListNode<T>(append); }

		public void Prepend(T value) { getStart().Previous = new IndependentDoublyLinkedListNode<T>(value); }
		public void Prepend(IndependentDoublyLinkedListNode<T> prepend) { getStart().Previous = prepend; }
		public void Prepend(IEnumerable<T> prepend) { getStart().Previous = new IndependentDoublyLinkedListNode<T>(null, prepend).Previous; }

		public IEnumerator<T> getForwardEnumerator()
		{
			var pos = this;
			do
			{
				yield return pos.Value;
				pos = pos.Next;
			} while (!pos.IsEnd());
		}
		public IEnumerator<IndependentDoublyLinkedListNode<T>> getForwardNodeEnumerator()
		{
			var pos = this;
			do
			{
				yield return pos;
				pos = pos.Next;
			} while (!pos.IsEnd());
		}

		public IEnumerator<T> getBackwardsEnumerator()
		{
			var pos = this;
			do
			{
				yield return pos.Value;
				pos = pos.Previous;
			} while (!pos.IsEnd());
		}
		public IEnumerator<IndependentDoublyLinkedListNode<T>> getBackwardsNodeEnumerator()
		{
			var pos = this;
			do
			{
				yield return pos;
				pos = pos.Previous;
			} while (!pos.IsEnd());
		}
	}
}