using Avalonia.Threading;
using Foxtaur2D.ViewModels;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Foxtaur2D.Logging;

/// <summary>
/// NLog target to log into control
/// </summary>
[Target("ControlLogging")]
public class ControlLoggingTarget : TargetWithLayout
{
    public ControlLoggingTarget()
    {
        Host = "localhost";
    }

    [RequiredParameter] public string Host { get; set; }

    protected override void Write(LogEventInfo logEvent)
    {
        var logMessage = Layout.Render(logEvent);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var dataContext = Program.GetMainWindow()?.DataContext;
            if (dataContext != null)
            {
                (dataContext as MainWindowViewModel).AddLineToConsole(logMessage);
            }
        });
    }
}
