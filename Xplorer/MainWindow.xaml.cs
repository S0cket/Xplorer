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

namespace Xplorer {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private bool isMaximized = false;
		private bool isLeft = true;
		private bool isRight = false;
		private string[] LeftFrameIcons = { "/Icons/ArrowRightLine.png", "/Icons/ArrowLeft.png" }; // 0-убрано, 1-показано
		private string[] RightFrameIcons = { "/Icons/ArrowLeftLine.png", "/Icons/ArrowRight.png" };
		private MainPage[] pages = new MainPage[2];

		public MainWindow() {
			InitializeComponent();
			pages[0] = new MainPage(this, 0);
			pages[1] = new MainPage(this, 1);
			LeftFrame.Content = pages[0];
			RightFrame.Content = pages[1];
			FramesStatusChange();
		}


		private void SwapFrameClick(object sender, RoutedEventArgs e) {
			object left = LeftFrame.Content;
			object right = RightFrame.Content;
			LeftFrame.Content = right;
			RightFrame.Content = left;
		}
		private void LeftFrameClick(object sender, RoutedEventArgs e) {
			isLeft = !isLeft;
			FramesStatusChange();
		}
		private void RightFrameClick(object sender, RoutedEventArgs e) {
			isRight = !isRight;
			FramesStatusChange();
		}
		private void FramesStatusChange() {
			Uri uriLeft, uriRight;
			if (isLeft)
				uriLeft = new Uri(LeftFrameIcons[1], UriKind.Relative);
			else
				uriLeft = new Uri(LeftFrameIcons[0], UriKind.Relative);
			if (isRight)
				uriRight = new Uri(RightFrameIcons[1], UriKind.Relative);
			else
				uriRight = new Uri(RightFrameIcons[0], UriKind.Relative);
			Image left = this.Resources["LeftFrameIcon"] as Image;
			Image right = this.Resources["RightFrameIcon"] as Image;
			left.Source = new BitmapImage(uriLeft);
			right.Source = new BitmapImage(uriRight);
			if (isLeft && isRight) {
				LeftFrameButton.IsEnabled = true;
				RightFrameButton.IsEnabled = true;
				SwapFrameButton.IsEnabled = true;
				LeftColumn.Width = new GridLength(1, GridUnitType.Star);
				CenterColumn.Width = new GridLength(1, GridUnitType.Auto);
				RightColumn.Width = new GridLength(1, GridUnitType.Star);
			}
			else if (isLeft) {
				LeftFrameButton.IsEnabled = false;
				RightFrameButton.IsEnabled = true;
				SwapFrameButton.IsEnabled = false;
				LeftColumn.Width = new GridLength(1, GridUnitType.Star);
				CenterColumn.Width = new GridLength(0, GridUnitType.Pixel);
				RightColumn.Width = new GridLength(0, GridUnitType.Pixel);
			}
			else if (isRight) {
				LeftFrameButton.IsEnabled = true;
				RightFrameButton.IsEnabled = false;
				SwapFrameButton.IsEnabled = false;
				LeftColumn.Width = new GridLength(0, GridUnitType.Pixel);
				CenterColumn.Width = new GridLength(0, GridUnitType.Pixel);
				RightColumn.Width = new GridLength(1, GridUnitType.Star);
			}
		}


		private void WindowMinimize(object sender, RoutedEventArgs e) {
			this.WindowState = WindowState.Minimized;
		}

		private void WindowMaximize(object sender, RoutedEventArgs e) {
			if (isMaximized)
				this.WindowState = WindowState.Normal;
			else
				this.WindowState = WindowState.Maximized;
		}

		private void WindowClose(object sender, RoutedEventArgs e) {
			this.Close();
		}

		private void WindowStateChanged(object sender, EventArgs e) {
			Image image = this.Resources["WindowMaximizeIcon"] as Image;
			Uri uri;
			if (isMaximized)
				uri = new Uri("/Icons/SizeUp.png", UriKind.Relative);
			else
				uri = new Uri("/Icons/SizeDown.png", UriKind.Relative);
			image.Source = new BitmapImage(uri);
			isMaximized = !isMaximized;
		}

		public void OpenInOtherPage(int number, DirectoryInfo dir) {
			number = (number == 0) ? 1 : 0;
			pages[number].OpenDirectoryInWindow(dir);
			if (number == 0 && !isLeft) LeftFrameClick(null, null);
			else if (number == 1 && !isRight) RightFrameClick(null, null);
		}

		public void RefreshAll() {
			pages[0].Refresh();
			pages[1].Refresh();
		}
	}
}
