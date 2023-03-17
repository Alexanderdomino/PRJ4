using RectF = Microsoft.Maui.Graphics.RectF;

namespace WeightWizard_test.Model.Drawables;

public class ReportDrawable :IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.White;
        canvas.StrokeSize = 3;
        canvas.FillColor = Colors.Gray;

        PathF path = new();
        path.MoveTo(10, 5);
        path.LineTo(25, 5);
        path.LineTo(27, 7);
        path.LineTo(27, 30);
        path.LineTo(25, 32);
        path.LineTo(10, 32);
        path.LineTo(8, 30);
        path.LineTo(8, 7);
        path.Close();

        canvas.DrawPath(path);
    }

}