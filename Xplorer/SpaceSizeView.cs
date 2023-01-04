using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xplorer {
	class SpaceSizeView {
		private long cntBytes;
		private double Size;
		private int degree = 0;

		public readonly string[] names = { "Б", "КБ", "МБ", "ГБ", "ТБ" };

		public SpaceSizeView(long bytes) {
			cntBytes = bytes;
			Counting();
		}
		~SpaceSizeView() { }

		private void Counting() {
			Size = cntBytes;
			do {
				Size /= 1024.0;
				degree++;
			} while (degree != 4 && Size > 1024);
		}

		public long Bytes() {
			return cntBytes;
		}

		public override string ToString() {
			return Math.Round(Size, 2).ToString() + " " + names[degree];
		}
	}
}
