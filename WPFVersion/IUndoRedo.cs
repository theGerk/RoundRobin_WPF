using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndoRedo
{
	public enum OPCode { Add, Remove, Replace }

	public class UndoRedo<T>
	{
		UndoRedo(IList<T> obj)
		{
			reference = obj;
		}


		public struct SingleChange
		{
			SingleChange(OPCode OP, T Content, int Index)
			{
				this.OP = OP;
				this.content = Content;
				this.index = Index;
			}


			private OPCode OP;
			private T content;
			private int index;

			public void Apply(IList<T> reference)
			{
				//do stuff
				switch (OP)
				{
					case OPCode.Add:
						reference.Insert(index, content);
						OP = OPCode.Remove;
						break;
					case OPCode.Remove:
						content = reference[index];
						reference.RemoveAt(index);
						OP = OPCode.Add;
						break;
					case OPCode.Replace:
						T tmp = reference[index];
						reference[index] = content;
						content = tmp;
						break;
					default:
						throw new Exception(String.Format("OP Code {0} not implemented for UndoRedo.OPCode", OP));
				}
			}
		}

		private Stack<SingleChange[]> past;
		private Stack<SingleChange[]> future;
		private IList<T> reference;

		
		public bool Redo()
		{
			try
			{
				var change = future.Pop();
				past.Push(change);
				foreach (var item in change)
					item.Apply(reference);
				return true;
			}
#pragma warning disable CS0168 // Variable is declared but never used
			catch (InvalidOperationException e)
#pragma warning restore CS0168 // Variable is declared but never used
			{
				return false;
			}
		}

		public bool Undo()
		{
			try
			{
				var change = past.Pop();
				future.Push(change);
				foreach (var item in change)
					item.UnApply(reference);
				return true;
			}
#pragma warning disable CS0168 // Variable is declared but never used
			catch (InvalidOperationException e)
#pragma warning restore CS0168 // Variable is declared but never used
			{
				return false;
			}
		}

		public void Update(SingleChange[] changeSet)
		{
			future.Clear();
			foreach (var change in changeSet)
				change.Apply(reference);
		}

		public void Update(IEnumerable<SingleChange[]> changeSets)
		{
			future.Clear();
			foreach (var changeSet in changeSets)
				foreach (var change in changeSet)
					change.Apply(reference);
		}
	}
}
