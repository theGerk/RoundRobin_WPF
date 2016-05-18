using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Personalized;

namespace WPFVersion
{
	/// <summary>
	/// Interaction logic for InsertableNameList.xaml
	/// </summary>
	public partial class InsertableNameList : UserControl
	{
		public InsertableNameList()
		{
			InitializeComponent();
		}

	///member functions

		//rewinds (ctrl-z)
		public bool Back()
		{
			//check if possible
			if (Cache.IsStart())
				return false;

			//might want to check to make sure list is the same as current Cache, but it should allways be the case

			Cache = Cache.Previous;

			SetList(Cache.Value);

			return true;
		}

		public void SetList<T>(IEnumerable<T> value)
		{
			list.Items.Clear();
			foreach (object item in value)
				list.Items.Add(item);
		}

		//goes forward (ctrl-y)
		public bool Forward()
		{
			if (Cache.IsEnd())
				return false;

			Cache = Cache.Next;

			SetList(Cache.Value);

			return true;
		}

		//checks if the cache has same contents as is currently displayed
		public bool CacheIsCurrent()
		{
			if (Cache.Value.Length != list.Items.Count)
				return false;

			for (int i = 0; i < Cache.Value.Length; i++)
				if (Cache.Value[i] != list.Items[i])
					return false;

			return true;
		}

		//preforms no check before updating cache
		public void UpdateCache()
		{
			Cache = Cache.Next = new IndependentDoublyLinkedListNode<object[]>(list.Items.Cast<object>().ToArray());
		}

		//Allows for programatic insertion
		//returns number of items inserted
		public int Insert(string items)
		{
			return Insert(items.Split('\n'));
		}
		public int Insert(string[] items)
		{
			int count = 0;
			foreach (string name in items)
				count += Convert.ToInt32(name);
			if (count != 0)
				UpdateCache();

			return count;
		}

		private bool insert(object item)
		{
			Algorithms.SearchReturn mySearch = Algorithms.BinarySearch(list.Items, item, list.Items.Count, delegate (ItemCollection lst, int pos) { return lst[pos]; }, delegate (object a, object b) { return a.ToString().CompareTo(b); });

			if (mySearch.exists)
				return false;

			if (mySearch.position >= list.Items.Count)
				list.Items.Add(item);
			else
				list.Items.Insert(mySearch.position, item);

			return true;
		}

		public bool ExistsInList(object item)
		{
			return Algorithms.BinarySearch(list.Items, item, list.Items.Count, delegate (ItemCollection lst, int pos) { return lst[pos]; }, delegate (object a, object b) { return a.ToString().CompareTo(b); }).exists;
		}

	///event handlers (implemented)

		private void text_KeyDown(object sender, KeyEventArgs e)
		{
			switch(e.Key)
			{
				case Key.Enter:
					if (insert(text.Text))
					{
						ItemInserted?.Invoke(this, text.Text);
						UpdateCache();
						text.Text = "";
					}
					break;
			}
		}

		private void list_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Delete:
				case Key.Back:
					while (list.SelectedItems.Count != 0)
						list.Items.Remove(list.SelectedItems[0]);
					UpdateCache();
					break;
			}
		}

		private void list_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Z:
					switch (Keyboard.Modifiers)
					{
						case ModifierKeys.Control:
							Back();
							break;
						case ModifierKeys.Shift | ModifierKeys.Control:
							Forward();
							break;
					}
					break;
				case Key.Y:
					switch(Keyboard.Modifiers)
					{
						case ModifierKeys.Control:
							Forward();
							break;
						case ModifierKeys.Shift | ModifierKeys.Control:
							Back();
							break;
					}
					break;
			}
		}


	///data

		//cache for ctrl z and ctrl y
		public IndependentDoublyLinkedListNode<object[]> Cache { get; set; } = new IndependentDoublyLinkedListNode<object[]>(new object[0]);

	///event handlers (for user control)

		//happens after item is inserted
		public event EventHandler<string> ItemInserted;
	}
}