using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace JukeBox.Utilities
{
    public static class HoldToLoad
    {
        public static void Start(double timeToWait, Action callback)
        {
            DispatcherTimer holdToLoadTimer = new DispatcherTimer();
            holdToLoadTimer.Interval = TimeSpan.FromSeconds(timeToWait);
            holdToLoadTimer.Tick += delegate
            {
                callback();
                holdToLoadTimer.Stop();
            };
            holdToLoadTimer.Start();
        }
    }
}
