namespace WeightWizard.Model.Drawables
{
    public class GraphicsDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 2;
            canvas.DrawRectangle(0, 0, 240, 200);
            PathF path = new PathF();
            Random rnd = new Random();
            path.MoveTo(0, 200);
            float x = 8;
            float y = 198;
            for (int i = 0; i < 30; i++)
            {
                path.LineTo(x, y);
                x += 8;
                y -= rnd.Next(0, 16);
                if (y < 0)
                {
                    y = 30;
                }
            }
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 2;
            canvas.DrawPath(path);
        }
    }
}
