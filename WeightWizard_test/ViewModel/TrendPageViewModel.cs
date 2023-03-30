using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using WeightWizard_test.Model.Drawables;

namespace WeightWizard_test.ViewModel
{
	public class GraphData
	{
		public double XValue { get; set; }
		public double YValue { get; set; }
	}
	 public class TrendPageViewModel : GraphicsView
	{

		//Expose properties for the X and Y values
		private List<int> _values;

		public List<int> Values { get { return _values; } set { _values = value; } } 

		public TrendPageViewModel()
		{
			_values = new List<int>() { 1, 2, 3, 4, 5, 7 };

			Drawable = new TrendGraphDrawing(this);
		}


	}
}
