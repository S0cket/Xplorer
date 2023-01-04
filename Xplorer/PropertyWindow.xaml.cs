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
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Xplorer {
	/// <summary>
	/// Логика взаимодействия для PropertyWindow.xaml
	/// </summary>
	public partial class PropertyWindow : Window {
		MainWindow window;
		FileSystemInfo info;
		Thread countSize;

		public PropertyWindow(MainWindow window, FileSystemInfo info) {
			InitializeComponent();
			this.window = window;
			this.info = info;

			this.ItemName.Text = info.Name;
			this.ItemType.Text = GetItemType();
			this.FullPath.Text = info.FullName;
			this.CreationTime.Text = info.CreationTime.ToString("G");
			this.LastAccessTime.Text = info.LastAccessTime.ToString("G");
			this.LastWriteTime.Text = info.LastWriteTime.ToString("G");
			countSize = new Thread(() => {
				long bytes = CountItemSize(info);
				Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => {
					SpaceSizeView s = new SpaceSizeView(bytes);
					ItemSize.Text = s.ToString();
				}));
			});
			countSize.SetApartmentState(ApartmentState.MTA);
			countSize.Priority = ThreadPriority.Lowest;
			countSize.Start();
		}
		private void WindowClose(object sender, RoutedEventArgs e) {
			if (countSize.IsAlive) countSize.Abort();
			window.RefreshAll();
			this.DialogResult = false;
		}
		private void Confirm(object sender, RoutedEventArgs e) {
			if (info.Name != ItemName.Text) {
				if (info is DirectoryInfo) {
					string parent = (info as DirectoryInfo).Parent.FullName;
					string newname = ItemName.Text;
					if (ExistsDir(parent, newname)) {
						new MessageWindow("Каталог с таким именем уже существует!").ShowDialog();
						return;
					}
					(info as DirectoryInfo).MoveTo(System.IO.Path.Combine(parent, newname));
				}
				else {
					string parent = (info as FileInfo).Directory.FullName;
					string newname = ItemName.Text;
					if (ExistsFile(parent, newname)) {
						new MessageWindow("Файл с таким именем уже существует!").ShowDialog();
						return;
					}
					(info as FileInfo).MoveTo(System.IO.Path.Combine(parent, newname));
				}
			}
			if (countSize.IsAlive) countSize.Abort();
			window.RefreshAll();
			this.DialogResult = true;
		}
		private string GetItemType() {
			if (info is DirectoryInfo)
				return "Каталог";
			else if (info is FileInfo)
				return "Файл";
			return "Неизвестно";
		}
		private long CountItemSize(FileSystemInfo i) {
			long bytes = 0;
			if (i is DirectoryInfo) {
				DirectoryInfo dir = (i as DirectoryInfo);
				try {
					foreach (var a in dir.GetDirectories()) {
						bytes += CountItemSize(a);
					}
				}
				catch(Exception e) {}
				try {
					foreach (var a in dir.GetFiles()) {
						bytes += CountItemSize(a);
					}
				}
				catch(Exception e) { }
				return bytes;
			}
			else {
				try {
					bytes += (i as FileInfo).Length;
				}
				catch(Exception e) {}
				return bytes;
			}
		}
		private bool ExistsDir(string parent, string newname) {
			return Directory.Exists(System.IO.Path.Combine(parent, newname));
		}
		private bool ExistsFile(string parent, string newname) {
			return File.Exists(System.IO.Path.Combine(parent, newname));
		}
	}
}
