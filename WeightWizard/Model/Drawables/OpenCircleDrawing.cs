using RectF = Microsoft.Maui.Graphics.RectF;

namespace WeightWizard.Model.Drawables;

public class OpenCircleDrawing : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.White;
        canvas.StrokeSize = 3;
        canvas.DrawCircle(20, 20, 10);
    }
}