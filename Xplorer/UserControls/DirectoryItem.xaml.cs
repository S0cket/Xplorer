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
using System.IO;

namespace Xplorer.UserControls {
	/// <summary>
	/// Логика взаимодействия для DirectoryItem.xaml
	/// </summary>
	public partial class DirectoryItem : UserControl {
		private string name;
		private DirectoryInfo dir;
		private bool state = false;

		public bool State { get { return state; } }
		public TextBlock DirectoryName;
		public ContextMenu Context;

		public DirectoryInfo Directory { get { return dir; } }

		public DirectoryItem(DirectoryInfo dir) {
			InitializeComponent();
			this.dir = dir;
			name = dir.Name;
			DirectoryName = DirName;
			DirName.Text = dir.Name;
			Context = CtxMenu;
		}
		~DirectoryItem() { }
		public void StateUp() {
			//this.Background = this.Resources["WindowChromeBackground"] as SolidColorBrush;
			//Root.Background = new SolidColorBrush(Colors.Red);
			Root.Background = this.Resources["WindowItemSelect"] as SolidColorBrush;
			state = true;
		}
		public void StateDown() {
			Root.Background = this.Resources["WindowBackground"] as SolidColorBrush;
			state = false;
		}
	}
}
