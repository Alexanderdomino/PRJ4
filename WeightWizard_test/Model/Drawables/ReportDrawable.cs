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
        path.MoveTo(15, 10);
        path.LineTo(15, 20);
        path.LineTo(20, 25);
        path.LineTo(25, 20);
        path.LineTo(25, 10);
        path.Close();

        canvas.DrawPath(path);
    }

}