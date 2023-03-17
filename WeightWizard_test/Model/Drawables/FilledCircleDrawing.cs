using RectF = Microsoft.Maui.Graphics.RectF;

namespace WeightWizard_test.Model.Drawables;

public class FilledCircleDrawing : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.WhiteSmoke;
        canvas.FillCircle(20, 20, 10);
    }
}