using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightWizard_test.Model
{
	public class weightModel
	{
		public double Weight { get; set; }
        public DateTime Date { get; set; }
		public int Calories { get; set; }
		public int Steps { get; set; }


        public weightModel(DateTime date, double weight, int steps, int calories)
		{
			
			this.Date = date;
			Weight= weight;
			Steps= steps;
			Calories= calories;
		}


	}
}
