using RectF = Microsoft.Maui.Graphics.RectF;

namespace WeightWizard.Model.Drawables;

public class ReportDrawable :IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.White;
        canvas.StrokeSize = 3;
        canvas.FillColor = Colors.Gray;

        PathF path = new();
        path.MoveTo(10, 25);
        path.LineTo(25, 25);
        path.LineTo(27, 27);
        path.LineTo(27, 52);
        path.LineTo(25, 54);
        path.LineTo(10, 54);
        path.LineTo(8, 52);
        path.LineTo(8, 27);
        path.Close();

        canvas.DrawPath(path);
    }

}