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
using System.Windows.Shapes;

namespace WPFVersion
{
	/// <summary>
	/// Interaction logic for Tournament.xaml
	/// </summary>
	public partial class Tournament : Window
	{
		

		public Tournament()
		{
			InitializeComponent();
		}

		public Tournament(IEnumerable<string> enumerable)
		{
			InitializeComponent();

		}
	}
}
