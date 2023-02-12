using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Foxtaur2D.Controls;

public partial class MapControl : UserControl
{
    public MapControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    /// <summary>
    /// Render the control
    /// </summary>
    public override void Render(DrawingContext context)
    {
        var testPen = new Pen(Brushes.Blue, 1, lineCap: PenLineCap.Round);
        context.DrawEllipse(Brushes.Green, testPen, new Point(100, 100), 20, 20);

        base.Render(context);
    }
}