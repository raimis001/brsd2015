using UnityEngine;
using System.Collections;

public class UpgradeData {
	
		public int price;
		public int value;
		public int energy;
		public int level = 1;
		
		public UpgradeData(int price, int value, int energy) {
			this.price = price;
			this.value = value;
			this.energy = energy;
	}
	
}
