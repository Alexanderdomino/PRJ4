using System.ComponentModel;
using System.Numerics;
using WeightWizard_test.ViewModel;

namespace WeightWizard_test.Model.Drawables
{
    public class TrendGraphDrawing : IDrawable
    {
		private ViewModel.TrendPageViewModel _viewModel;
		public TrendGraphDrawing(TrendPageViewModel viewModel) => _viewModel = viewModel;
		
		//public List<int> Vectors
		//{
		//	get => (List<int>)GetValue(VectorsProperty);
		//	set => SetValue(VectorsProperty, value);
		//}

		

		//public static BindableProperty VectorsProperty = BindableProperty.Create(nameof(Vectors), typeof(List<int>), typeof(TrendGraphDrawing));


		public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_viewModel.Values==null)
            {
                return;
            }
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 2;
            canvas.DrawRectangle(0, 0, 240, 200);
            PathF path = new PathF();
            Random rnd = new Random();
            path.MoveTo(0, 200);
            float x = 8;
            int y = 98;
            for (int i = 0; i < _viewModel.Values.Count; i++)
            {
                path.LineTo(x, _viewModel.Values[i]);
                x += 8;
               //=5
                
            }
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 2;
            canvas.DrawPath(path);
        }
    }
}
