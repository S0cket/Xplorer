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
	/// Логика взаимодействия для FileItem.xaml
	/// </summary>
	public partial class FileItem : UserControl {

		private string name;
		private FileInfo file;
		private bool state = false;

		public bool State { get { return state; } }
		public TextBlock FileName;

		public FileInfo File { get { return file; } }
		public ContextMenu Context;

		public FileItem(FileInfo file) {
			InitializeComponent();
			this.file = file;
			name = file.Name;
			FileName = FName;
			FileName.Text = file.Name;
			Context = CtxMenu;
		}
		~FileItem() { }

		public void StateUp() {
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
