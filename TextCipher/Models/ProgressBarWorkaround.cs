using System;
using Avalonia;
using Avalonia.Controls;

namespace TextCipher.Models;

// this class will serve as an attached property for DoubleTransition in TabInfoView. The reason for this property is
// that when using Transitions on ProgressBar, there is a memory leak. This attached property will save us from that
// problem: https://github.com/AvaloniaUI/Avalonia/issues/2881
public class ProgressBarWorkaround
{
    public static AvaloniaProperty<double> ValueProperty =
        AvaloniaProperty.RegisterAttached<ProgressBarWorkaround, ProgressBar, double>("Value");

    public static void SetValue(ProgressBar pb, double value) =>
        pb.SetValue(ValueProperty, value);

    static ProgressBarWorkaround()
    {
        ValueProperty.Changed.Subscribe(ev =>
        {
            ((ProgressBar) ev.Sender).Value = ev.NewValue.Value;
        });
    }
}