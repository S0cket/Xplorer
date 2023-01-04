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
using Xplorer.UserControls;
using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Threading;

namespace Xplorer {
	/// <summary>
	/// Логика взаимодействия для MainPage.xaml
	/// </summary>
	public partial class MainPage : UserControl {

		private DirectoryInfo path = null;
		private List<DirectoryInfo> history = new List<DirectoryInfo>();
		private List<FileItem> SelectedFiles = new List<FileItem>();
		private List<DirectoryItem> SelectedDirs = new List<DirectoryItem>();
		private object lastSelect;
		private int historyPosition;
		private object DownObject;
		private object ContextObject;
		private int Number;
		private MainWindow window;

		public MainPage(MainWindow window, int s) {
			this.window = window;
			Number = s;
			InitializeComponent();
			//TextBlock tb = new TextBlock();
			//tb.Text = s;
			//tb.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
			//Items.Children.Add(tb);
			PrintInfo();
			history.Add(null);
			historyPosition = 0;
			Space.MouseDown += MouseButtonDown;
		}

		public void OpenDirectoryInWindow(DirectoryInfo dir) {
			historyPosition++;
			history.Add(dir);
			ChangeEnableButton();
			path = dir;
			Refresh();
		}

		private void Clear() {
			ClearSelected();
			Items.Children.Clear();
		}

		private void PrintInfo() {
			Clear();
			GenContextSpace(Space);
			if (path == null) {
				DriveInfo[] drives = DriveInfo.GetDrives();
				foreach (DriveInfo drive in drives) {
					DriveItem item = new DriveItem(drive);
					item.Progress.Background = Resources["BackgroundProgress"] as SolidColorBrush;
					item.Progress.Fill = Resources["FillProgress"] as SolidColorBrush;
					item.MouseDoubleClick += OpenDrive;
					item.MouseDown += MouseButtonDown;
					GenContextDriveItem(item);
					Items.Children.Add(item);
				}
			}
			else {
				if (!Directory.Exists(path.FullName)) {
					path = null;
					PrintInfo();
					return;
				}
				DirectoryInfo[] dirs = path.GetDirectories();
				foreach (DirectoryInfo dir in dirs) {
					FileAttributes attr = dir.Attributes;
					if ((attr & FileAttributes.System) != 0) continue;
					DirectoryItem item = new DirectoryItem(dir);
					item.Height = 20;
					item.MouseDoubleClick += OpenDirectory;
					item.MouseDown += MouseButtonDown;
					item.MouseUp += MouseButtonUp;
					item.MouseMove += OnDrag;
					item.Drop += OnDrop;
					GenContextDirectoryItem(item);
					//item.Focusable = true;
					item.AllowDrop = true;
					Items.Children.Add(item);
				}
				FileInfo[] files = path.GetFiles();
				foreach (FileInfo file in files) {
					FileAttributes attr = file.Attributes;
					if ((attr & FileAttributes.System) != 0) continue;
					FileItem item = new FileItem(file);
					item.Height = 20;
					item.MouseDoubleClick += OpenFile;
					item.MouseDown += MouseButtonDown;
					item.MouseUp += MouseButtonUp;
					item.MouseMove += OnDrag;
					GenContextFileItem(item);
					item.AllowDrop = false;
					Items.Children.Add(item);
				}
			}
		}

		private void OnDrop(object sender, DragEventArgs e) {
			if (path == null) return;
			string dir;
			if (sender is DirectoryItem)
				dir = (sender as DirectoryItem).Directory.FullName;
			else if (sender is Label)
				dir = path.FullName;
			else return;
			DataObject obj = e.Data as DataObject;
			StringCollection paths = obj.GetFileDropList();
			if (paths == null || paths.IndexOf(dir) != -1) return;
			List<FileSystemInfo> infos = new List<FileSystemInfo>();
			for (int i = 0; i < paths.Count; ++ i) {
				FileAttributes attr = File.GetAttributes(paths[i]);
				if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
					infos.Add(new DirectoryInfo(paths[i]));
				else
					infos.Add(new FileInfo(paths[i]));
			}
			//DoPaste(dir, infos);
			//Refresh(null, null);
			Thread th = new Thread(() => DoPaste(dir, infos));
			th.SetApartmentState(ApartmentState.STA);
			th.Start();
		}

		private void OnDrag(object sender, MouseEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed && !isCtrlPressed() && !isShiftPressed() && isItemSelect(sender)) {
				Clipboard.Clear();
				StringCollection paths = new StringCollection();
				foreach (DirectoryItem dir in SelectedDirs) {
					paths.Add(dir.Directory.FullName);
				}
				foreach (FileItem file in SelectedFiles) {
					paths.Add(file.File.FullName);
				}
				DataObject data = new DataObject();
				data.SetFileDropList(paths);
				DragDrop.DoDragDrop(sender as DependencyObject, data, DragDropEffects.Copy);
			}
		}

		private void MouseButtonDown(object sender, MouseButtonEventArgs e) {
			if (e.ClickCount == 2) return;
			if (e.ChangedButton == MouseButton.Left) {
				DownObject = sender;
				if (sender is Label || sender is DriveItem) return; 
				if (isShiftPressed() && lastSelect != null) {
					int a = 0, b = 0;
					for (a = 0; a < Items.Children.Count; ++a) {
						if (lastSelect is DirectoryItem) {
							if (!(Items.Children[a] is DirectoryItem)) continue;
							if ((lastSelect as DirectoryItem) == (Items.Children[a] as DirectoryItem)) break;
						}
						else if (lastSelect is FileItem) {
							if (!(Items.Children[a] is FileItem)) continue;
							if ((lastSelect as FileItem) == (Items.Children[a] as FileItem)) break;
						}
					}
					for (b = 0; b < Items.Children.Count; ++b) {
						if (sender is DirectoryItem) {
							if (!(Items.Children[b] is DirectoryItem)) continue;
							if ((sender as DirectoryItem) == (Items.Children[b] as DirectoryItem)) break;
						}
						else if (sender is FileItem) {
							if (!(Items.Children[b] is FileItem)) continue;
							if ((sender as FileItem) == (Items.Children[b] as FileItem)) break;
						}
					}

					if (a > b) {
						int c = a;
						a = b;
						b = c;
					}
					if (!isCtrlPressed())
						ClearSelected();
					bool isUnselect = true;
					for (int i = a + 1; i <= b; ++i)
						if (!isItemSelect(Items.Children[i]))
							isUnselect = false;
					for (int i = a; i <= b; ++ i) {
						if (isUnselect)
							Unselect(Items.Children[i]);
						else
							Select(Items.Children[i]);
					}
				}
				else if (isCtrlPressed()) {
					if (isItemSelect(sender))
						Unselect(sender);
					else
						Select(sender);
				}
				else if (!isCtrlPressed() && !isShiftPressed() && !isItemSelect(sender)) {
					ClearSelected();
					Select(sender);
				}
			}
			else if (e.ChangedButton == MouseButton.Right) {
				ContextObject = sender;
				if (sender is Label || sender is DriveItem) return;
				if (!isItemSelect(sender)) {
					ClearSelected();
					Select(sender);
				}
				//MessageBox.Show(sender.ToString());
			}
		}

		private void MouseButtonUp(object sender, MouseButtonEventArgs e) {
			if (isCtrlPressed() || isShiftPressed() || e.ChangedButton != MouseButton.Left) return;
			if (isItemSelect(sender)) {
				ClearSelected();
				Select(sender);
			}
			//ContextObject = sender;
			//MessageBox.Show(sender.ToString());
		}


		private void ChangeEnableButton() {
			if (historyPosition == 0) {
				ButtonBack.IsEnabled = false;
				ButtonReturn.IsEnabled = false;
			}
			else {
				ButtonBack.IsEnabled = true;
				ButtonReturn.IsEnabled = true;
			}
			if (historyPosition == history.Count - 1)
				ButtonForward.IsEnabled = false;
			else
				ButtonForward.IsEnabled = true;
		}


		private void OpenDrive(object sender, MouseButtonEventArgs e) {
			DriveItem item = sender as DriveItem;
			DirectoryInfo oldpath = path;
			path = item.Drive.RootDirectory;
			historyPosition++;
			history.RemoveRange(historyPosition, history.Count - historyPosition);
			history.Add(path);
			try { 
			PrintInfo();
			} catch (Exception exc) { new MessageWindow("Невозможно открыть диск").ShowDialog(); historyPosition--; history.Remove(path); path = oldpath; PrintInfo(); }
			ChangeEnableButton();
			ClearSelected();
		}

		private void OpenDirectory(object sender, MouseButtonEventArgs e) {
			DirectoryItem item = sender as DirectoryItem;
			if (!Directory.Exists(item.Directory.FullName)) {
				window.RefreshAll();
				return;
			}
			DirectoryInfo oldpath = path;
			path = item.Directory;
			historyPosition++;
			history.RemoveRange(historyPosition, history.Count - historyPosition);
			history.Add(path);
			try { 
			PrintInfo();
			} catch (Exception exc) { new MessageWindow("Невозможно открыть каталог").ShowDialog(); historyPosition--; history.Remove(path); path = oldpath; PrintInfo(); }
			ChangeEnableButton();
			ClearSelected();
		}

		private void OpenFile(object sender, MouseButtonEventArgs e) {
			FileItem item = sender as FileItem;
			if (!File.Exists(item.File.FullName)) {
				window.RefreshAll();
				return;
			}
			try { 
			Process.Start(item.File.FullName);
			} catch (Exception exc) { new MessageWindow("Невозможно открыть файл").ShowDialog(); }
		}

		private void Back(object sender, RoutedEventArgs e) {
			ClearSelected();
			historyPosition--;
			path = history[historyPosition];
			PrintInfo();
			ChangeEnableButton();
		}
		private void Forward(object sender, RoutedEventArgs e) {
			ClearSelected();
			historyPosition++;
			path = history[historyPosition];
			PrintInfo();
			ChangeEnableButton();
		}

		private void Return(object sender, RoutedEventArgs e) {
			ClearSelected();
			historyPosition--;
			path = history[historyPosition];
			history.RemoveRange(historyPosition + 1, history.Count - (historyPosition + 1));
			PrintInfo();
			ChangeEnableButton();
		}

		private void Refresh(object sender, RoutedEventArgs e) {
			ClearSelected();
			PrintInfo();
			ChangeEnableButton();
		}
		public void Refresh() {
			Refresh(null, null);
		}

		private void Home(object sender, RoutedEventArgs e) {
			ClearSelected();
			historyPosition = 0;
			history.RemoveRange(1, history.Count - 1);
			path = history[historyPosition];
			PrintInfo();
			ChangeEnableButton();
		}

		private void OpenInOtherPage(object sender, RoutedEventArgs e) {
			//MessageBox.Show(sender.ToString());
			if (ContextObject is DriveItem) {
				DriveItem item = ContextObject as DriveItem;
				window.OpenInOtherPage(Number, item.Drive.RootDirectory);
			}
			else if (ContextObject is DirectoryItem) {
				DirectoryItem item = ContextObject as DirectoryItem;
				window.OpenInOtherPage(Number, item.Directory);
			}
			else if (ContextObject is Label) {
				window.OpenInOtherPage(Number, path);
			}
		}
		private void CreateDir(object sender, RoutedEventArgs e) {
			try { 
			string parentDir = null;
			if (ContextObject is Label) {
				if (path == null) return;
				parentDir = path.FullName;
			}
			else if (ContextObject is DriveItem) {
				parentDir = (ContextObject as DriveItem).Drive.RootDirectory.FullName;
			}
			else if (ContextObject is DirectoryItem) {
				parentDir = (ContextObject as DirectoryItem).Directory.FullName;
			}
			if (parentDir == null) return;
			string name = "dir";
			int n = 1;
			while (Directory.Exists(System.IO.Path.Combine(parentDir, name))) {
				name = $"dir-{n}";
				n++;
			}
			Directory.CreateDirectory(System.IO.Path.Combine(parentDir, name));
			window.RefreshAll();
			}
			catch(Exception exc) { }
		}
		private void CreateFile(object sender, RoutedEventArgs e) {
			try { 
			string parentDir = null;
			if (ContextObject is Label) {
				if (path == null) return;
				parentDir = path.FullName;
			}
			else if (ContextObject is DriveItem) {
				parentDir = (ContextObject as DriveItem).Drive.RootDirectory.FullName;
			}
			else if (ContextObject is DirectoryItem) {
				parentDir = (ContextObject as DirectoryItem).Directory.FullName;
			}
			if (parentDir == null) return;
			string name = "file";
			int n = 1;
			while (File.Exists(System.IO.Path.Combine(parentDir, name))) {
				name = $"file-{n}";
				n++;
			}
			File.Create(System.IO.Path.Combine(parentDir, name));
			window.RefreshAll();
			}
			catch(Exception exc) { }
		}

		private void Copy(object sender, RoutedEventArgs e) {
			StringCollection paths = new StringCollection();
			foreach (DirectoryItem dir in SelectedDirs) {
				paths.Add(dir.Directory.FullName);
			}
			foreach (FileItem file in SelectedFiles) {
				paths.Add(file.File.FullName);
			}
			Clipboard.SetFileDropList(paths);
		}
		private void Paste(object sender, RoutedEventArgs e) {
			if (!Clipboard.ContainsFileDropList()) return;
			StringCollection paths = Clipboard.GetFileDropList();
			string dir;
			if (ContextObject is DriveItem)
				dir = (ContextObject as DriveItem).Drive.RootDirectory.FullName;
			else if (ContextObject is DirectoryItem)
				dir = (ContextObject as DirectoryItem).Directory.FullName;
			else if (ContextObject is Label) {
				if (path == null) return;
				dir = path.FullName;
			}
			else return;
			List<FileSystemInfo> infos = new List<FileSystemInfo>();
			for (int i = 0; i < paths.Count; ++i) {
				FileAttributes attr = File.GetAttributes(paths[i]);
				if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
					infos.Add(new DirectoryInfo(paths[i]));
				else
					infos.Add(new FileInfo(paths[i]));
			}
			//DoPaste(dir, infos);
			//Refresh(null, null);
			Thread th = new Thread(() => DoPaste(dir, infos));
			th.SetApartmentState(ApartmentState.STA);
			th.Start();
		}

		private void DoPaste(string dir, List<FileSystemInfo> infos) {
			TaskResult? checkAll = null;
			bool stopWork = false;
			PasteRecursive(dir, infos, ref checkAll, ref stopWork);
			Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => {Refresh(); new MessageWindow("Копирование завершено").ShowDialog(); }));
		}
		private void PasteRecursive(string target_path, List<FileSystemInfo> infos, ref TaskResult? checkAll, ref bool stopWork) {
			for (int i = 0; i < infos.Count; ++ i) {
				if (infos[i] is DirectoryInfo) {
					DirectoryInfo info = infos[i] as DirectoryInfo;
					string curdir = System.IO.Path.Combine(target_path, info.Name);
					List<FileSystemInfo> dirinfos = new List<FileSystemInfo>();
					dirinfos.AddRange(info.GetDirectories());
					dirinfos.AddRange(info.GetFiles());
					if (!Directory.Exists(curdir))
						Directory.CreateDirectory(curdir);
					PasteRecursive(curdir, dirinfos, ref checkAll, ref stopWork);
					if (stopWork) return;
				}
				else if (infos[i] is FileInfo) {
					string curfile = System.IO.Path.Combine(target_path, infos[i].Name);
					if (File.Exists(curfile)) {
						TaskResult res;
						if (checkAll == null) {
							TaskWindow tskw = new TaskWindow(infos[i].Name);
							if (tskw.ShowDialog() == false)
								stopWork = true;
							if (stopWork) return;
							if (tskw.Target == TaskTarget.All)
								checkAll = tskw.Result;
							res = tskw.Result;
						}
						else
							res = (TaskResult)checkAll;
						if (res == TaskResult.Replace) {
							File.Delete(curfile);
							File.Copy(infos[i].FullName, curfile);
						}
					}
					else
						File.Copy(infos[i].FullName, curfile);
				}
			}
		}

		private void Delete(object sender, RoutedEventArgs e) {
			//new MessageWindow("Ошибка при удалении!").ShowDialog();
			List<FileSystemInfo> infos = new List<FileSystemInfo>();
			foreach (DirectoryItem item in SelectedDirs)
				infos.Add(item.Directory);
			foreach (FileItem item in SelectedFiles)
				infos.Add(item.File);
			bool isError = false;
			try {
			DeleteRecursive(infos, ref isError);
			} catch(Exception exc) { new MessageWindow("Ошибка при удалении!").ShowDialog(); }
			if (isError)
				new MessageWindow("Ошибка при удалении!").ShowDialog();
			window.RefreshAll();
		}
		private void DeleteRecursive(List<FileSystemInfo> infos, ref bool isError) {
			for (int i = 0; i < infos.Count; ++ i) {
				if (infos[i] is DirectoryInfo) {
					DirectoryInfo dir = infos[i] as DirectoryInfo;
					List<FileSystemInfo> newinfos = new List<FileSystemInfo>();
					newinfos.AddRange(dir.GetDirectories());
					newinfos.AddRange(dir.GetFiles());
					DeleteRecursive(newinfos, ref isError);
					try {
						dir.Delete();
					}
					catch (Exception e) {
						isError = true;
					}
				}
				else if (infos[i] is FileInfo) {
					FileInfo file = infos[i] as FileInfo;
					try {
						file.Delete();
					}
					catch (Exception e) {
						isError = true;
					}
				}
			}
		}
		private void Property(object sender, RoutedEventArgs e) {
			if (ContextObject is DirectoryItem) {
				new PropertyWindow(window, (ContextObject as DirectoryItem).Directory).ShowDialog();
			}
			else if (ContextObject is FileItem) {
				new PropertyWindow(window, (ContextObject as FileItem).File).ShowDialog();
			}
			else if (ContextObject is Label) {
				if (path == null) return;
				new PropertyWindow(window, path).ShowDialog();
			}
		}

		private bool isCtrlPressed() {
			return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
		}
		private bool isShiftPressed() {
			return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
		}

		private bool isItemSelect(object sender) {
			if (sender is DirectoryItem && (sender as DirectoryItem).State)
				return true;
			if (sender is FileItem && (sender as FileItem).State)
				return true;
			return false;
		} 

		private void ClearSelected() {
			foreach (DirectoryItem item in SelectedDirs)
				item.StateDown();
			foreach (FileItem item in SelectedFiles)
				item.StateDown();
			SelectedDirs.Clear();
			SelectedFiles.Clear();
			lastSelect = null;
		}

		private void Select(object sender) {
			if (sender is DirectoryItem) {
				DirectoryItem item = sender as DirectoryItem;
				item.StateUp();
				SelectedDirs.Add(item);
			}
			else if (sender is FileItem) {
				FileItem item = sender as FileItem;
				item.StateUp();
				SelectedFiles.Add(item);
			}
			lastSelect = sender;
		}

		private void Unselect(object sender) {
			if (sender is DirectoryItem) {
				DirectoryItem item = sender as DirectoryItem;
				item.StateDown();
				SelectedDirs.Remove(item);
			}
			else if (sender is FileItem) {
				FileItem item = sender as FileItem;
				item.StateDown();
				SelectedFiles.Remove(item);
			}
			lastSelect = sender;
		}

		private void GenContextDriveItem(DriveItem item) {
			MenuItem mi;
			mi = new MenuItem();
			mi.Header = "Открыть в другой вкладке";
			mi.Click += OpenInOtherPage;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Создать папку";
			mi.Click += CreateDir;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Создать файл";
			mi.Click += CreateFile;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Вставить";
			mi.Click += Paste;
			item.Context.Items.Add(mi);
		}
		private void GenContextDirectoryItem(DirectoryItem item) {
			MenuItem mi;
			mi = new MenuItem();
			mi.Header = "Открыть в другой вкладке";
			mi.Click += OpenInOtherPage;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Создать папку";
			mi.Click += CreateDir;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Создать файл";
			mi.Click += CreateFile;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Копировать";
			mi.Click += Copy;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Вставить";
			mi.Click += Paste;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Удалить";
			mi.Click += Delete;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Свойства";
			mi.Click += Property;
			item.Context.Items.Add(mi);
		}
		private void GenContextFileItem(FileItem item) {
			MenuItem mi;
			mi = new MenuItem();
			mi.Header = "Копировать";
			mi.Click += Copy;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Удалить";
			mi.Click += Delete;
			item.Context.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Свойства";
			mi.Click += Property;
			item.Context.Items.Add(mi);
		}
		private void GenContextSpace(Label label) {
			label.ContextMenu.Items.Clear();
			MenuItem mi;
			mi = new MenuItem();
			mi.Header = "Открыть в другой вкладке";
			mi.Click += OpenInOtherPage;
			label.ContextMenu.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Создать папку";
			mi.Click += CreateDir;
			label.ContextMenu.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Создать файл";
			mi.Click += CreateFile;
			label.ContextMenu.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Вставить";
			mi.Click += Paste;
			label.ContextMenu.Items.Add(mi);
			mi = new MenuItem();
			mi.Header = "Свойства";
			mi.Click += Property;
			label.ContextMenu.Items.Add(mi);
		}
	}
}
