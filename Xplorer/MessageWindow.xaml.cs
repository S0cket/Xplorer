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

namespace Xplorer {
	/// <summary>
	/// Логика взаимодействия для MessageWindow.xaml
	/// </summary>
	public partial class MessageWindow : Window {
		static private double top = 0;
		static private double left = 0;
		public MessageWindow(string msg) {
			InitializeComponent();
			MainText.Text = msg;
			this.Top = top;
			this.Left = left;
		}

		private void WindowClose(object sender, RoutedEventArgs e) {
			top = this.Top;
			left = this.Left;
			this.DialogResult = true;
		}
	}
}
