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
	/// Логика взаимодействия для DriveItem.xaml
	/// </summary>
	public partial class DriveItem : UserControl {
		private string name;
		private string label;
		private DriveInfo drive;

		public TextBlock DriveFullName;
		public MyProgress Progress;
		public TextBlock DriveSpace;
		public ContextMenu Context;

		public DriveInfo Drive { get { return drive; } }

		public DriveItem(DriveInfo drive) {
			this.drive = drive;
			InitializeComponent();
			Context = CtxMenu;
			DriveFullName = DriveLabel;
			Progress = DriveProgress;
			DriveSpace = Info;
			name = drive.Name;
			label = drive.VolumeLabel;
			if (label == null || label == "") {
				switch(drive.DriveType) {
					case DriveType.Fixed:
						label = "Локальный диск";
						break;
					case DriveType.CDRom:
						label = "Оптический диск";
						break;
					case DriveType.Network:
						label = "Сетевой диск";
						break;
					case DriveType.Removable:
						label = "Съёмный диск";
						break;
					default:
						label = "Неизвестное устройство";
						break;
				}

			}
				

			DriveFullName.Text = "(" + name + ") " + label;

			SpaceSizeView total = new SpaceSizeView(drive.TotalSize);
			SpaceSizeView free = new SpaceSizeView(drive.TotalFreeSpace);
			Progress.Max = total.Bytes();
			Progress.Value = total.Bytes() - free.Bytes();

			DriveSpace.Text = "Свободно " + free.ToString() + " из " + total.ToString();
		}
		~DriveItem() { }
	}
}
