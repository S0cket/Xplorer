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

namespace Xplorer.UserControls {
	/// <summary>
	/// Логика взаимодействия для MyProgress.xaml
	/// </summary>
	/// 

	public partial class MyProgress : UserControl {
		private Brush fill;
		private double max;
		private double curValue;
		private double multiplier;
		public Brush Fill {
			get {return fill;}
			set {
				fill = value;
				Progress.Fill = fill;
			}
		}
		public double Max {
			get {return max;}
			set {
				if (max <= 0) return;
				max = value;
				curValue = 0;
				multiplier = this.ActualWidth / max;
			}
		}
		public double Value {
			get { return curValue; }
			set {
				curValue = value;
				if (curValue > max) curValue = max;
				if (curValue < 0) curValue = 0;
				Progress.Width = multiplier * curValue;
			}
		}

		public MyProgress() {
			InitializeComponent();
			Progress.Width = 0;
			curValue = 0;
			max = 100;
			multiplier = this.ActualWidth / max;
			this.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
			this.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
		}

		~MyProgress() { }

		private void OnResize(object sender, EventArgs e) {
			multiplier = this.ActualWidth / max;
			Progress.Width = multiplier * curValue;
		}
	}
}
