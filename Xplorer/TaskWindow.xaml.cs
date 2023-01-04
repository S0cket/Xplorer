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
	/// Логика взаимодействия для TaskWindow.xaml
	/// </summary>
	/// 

	public enum TaskResult {
		Replace,
		Skip,
		Cancel
	}
	public enum TaskTarget {
		One,
		All
	}
	public partial class TaskWindow : Window {
		static private double top = 0;
		static private double left = 0;

		private TaskResult result = TaskResult.Cancel;
		private TaskTarget target = TaskTarget.One;
		public TaskResult Result { get { return result; } }
		public TaskTarget Target { get { return target; } }

		public TaskWindow(string fileName) {
			InitializeComponent();
			FileName.Text = fileName;
			this.Top = top;
			this.Left = left;
		}

		private void WindowClose(object sender, RoutedEventArgs e) {
			result = TaskResult.Cancel;
			top = this.Top;
			left = this.Left;
			this.DialogResult = false;
		}

		private void ReplaceClick(object sender, RoutedEventArgs e) {
			result = TaskResult.Replace;
			if ((bool)CheckAll.IsChecked) target = TaskTarget.All;
			else target = TaskTarget.One;
			top = this.Top;
			left = this.Left;
			this.DialogResult = true;
		}
		private void SkipClick(object sender, RoutedEventArgs e) {
			result = TaskResult.Skip;
			if ((bool)CheckAll.IsChecked) target = TaskTarget.All;
			else target = TaskTarget.One;
			top = this.Top;
			left = this.Left;
			this.DialogResult = true;
		}

	}
}
