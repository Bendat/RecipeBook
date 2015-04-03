using System;
using System.Runtime.InteropServices;
using System.Windows;
using WinInterop = System.Windows.Interop;

namespace RecipeBook
{
    /// <summary>
    /// A collection of classes and methods that call unmanaged code.
    /// Used to implement behaviors that chromeless WPF does not support.
    /// <see href="!:http://www.codeproject.com/Tips/316111/Show-or-Activate-a-WPF-window-under-the-current-mo"/>
    /// <seealso href="!:http://blogs.msdn.com/b/llobo/archive/2006/08/01/maximizing-window-_2800_with-windowstyle_3d00_none_2900_-considering-taskbar.aspx"/>
    /// </summary>
    public static class WindowExtensions
    {
        #region Public Methods

        public static bool ActivateCenteredToMouse(this Window window)
        {
            ComputeTopLeft(ref window);
            return window.Activate();
        }

        public static void ShowCenteredToMouse(this Window window)
        {
            // in case the default start-up location isn't set to Manual
            WindowStartupLocation oldLocation = window.WindowStartupLocation;
            // set location to manual -> window will be placed by Top and Left property
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            ComputeTopLeft(ref window);
            window.Show();
            window.WindowStartupLocation = oldLocation;
        }

        #endregion

        #region Methods

        private static void ComputeTopLeft(ref Window window)
        {
            W32Point pt = new W32Point();
            if (!GetCursorPos(ref pt))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            // 0x00000002: return nearest monitor if pt is not contained in any monitor.
            IntPtr monHandle = MonitorFromPoint(pt, 0x00000002);
            W32MonitorInfo monInfo = new W32MonitorInfo();
            monInfo.Size = Marshal.SizeOf(typeof(W32MonitorInfo));

            if (!GetMonitorInfo(monHandle, ref monInfo))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            // use WorkArea struct to include the taskbar position.
            W32Rect monitor = monInfo.WorkArea;
            double offsetX = Math.Round(window.Width / 2);
            double offsetY = Math.Round(window.Height / 2);

            double top = pt.Y - offsetY;
            double left = pt.X - offsetX;

            Rect screen = new Rect(
                new Point(monitor.Left, monitor.Top),
                new Point(monitor.Right, monitor.Bottom));
            Rect wnd = new Rect(
                new Point(left, top),
                new Point(left + window.Width, top + window.Height));

            window.Top = wnd.Top;
            window.Left = wnd.Left;

            if (!screen.Contains(wnd))
            {
                if (wnd.Top < screen.Top)
                {
                    double diff = Math.Abs(screen.Top - wnd.Top);
                    window.Top = wnd.Top + diff;
                }

                if (wnd.Bottom > screen.Bottom)
                {
                    double diff = wnd.Bottom - screen.Bottom;
                    window.Top = wnd.Top - diff;
                }

                if (wnd.Left < screen.Left)
                {
                    double diff = Math.Abs(screen.Left - wnd.Left);
                    window.Left = wnd.Left + diff;
                }

                if (wnd.Right > screen.Right)
                {
                    double diff = wnd.Right - screen.Right;
                    window.Left = wnd.Left - diff;
                }
            }
        }

        #endregion

        #region W32 API

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref W32Point pt);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref W32MonitorInfo lpmi);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(W32Point pt, uint dwFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct W32Point
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct W32MonitorInfo
        {
            public int Size;
            public W32Rect Monitor;
            public W32Rect WorkArea;
            public uint Flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct W32Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion


    }

    public class Maximiser
    {
        private readonly Window _primaryWindow;

        public Maximiser(Window primaryWindow)
        {
            this._primaryWindow = primaryWindow;
        }
        #region Maximise-Screen fix

        /* Borderless windows cause issues with full screen mode
         * where they cover the task bar. The following fixes it.
         * from
         * http://blogs.msdn.com/b/llobo/archive/2006/08/01/maximizing-window-_2800_with-windowstyle_3d00_none_2900_-considering-taskbar.aspx
         */

        public void Run()
        {
            _primaryWindow.SourceInitialized += (win_SourceInitialized);
            _primaryWindow.Loaded += (win_Loaded);
        }
        private void win_SourceInitialized(object sender, EventArgs e)
        {
            var handle = (new WinInterop.WindowInteropHelper(_primaryWindow)).Handle;
            var hwndSource = WinInterop.HwndSource.FromHwnd(handle);
            if (hwndSource != null)
                hwndSource.AddHook(WindowProc);
        }


        private static IntPtr WindowProc(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (Minmaxinfo)Marshal.PtrToStructure(lParam, typeof(Minmaxinfo));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            const int monitorDefaultToNearest = 0x00000002;
            var monitor = MonitorFromWindow(hwnd, monitorDefaultToNearest);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new Monitorinfo();
                GetMonitorInfo(monitor, monitorInfo);
                var rcWorkArea = monitorInfo.rcWork;
                var rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
                mmi.ptMinTrackSize.x = 800;
                mmi.ptMinTrackSize.y = 600;

            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }


        /// <summary>
        ///     Point aka POINTAPI
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            /// <summary>
            ///     x coordinate of point.
            /// </summary>
            public int x;

            /// <summary>
            ///     y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            ///     Construct a point of coordinates (x,y).
            /// </summary>
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Minmaxinfo
        {
            public Point ptReserved;
            public Point ptMaxSize;
            public Point ptMaxPosition;
            public Point ptMinTrackSize;
            public Point ptMaxTrackSize;
        };

        private void win_Loaded(object sender, RoutedEventArgs e)
        {
            _primaryWindow.WindowState = WindowState.Maximized;
        }


        /// <summary>
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class Monitorinfo
        {
            /// <summary>
            /// </summary>
            public int cbSize = Marshal.SizeOf(typeof(Monitorinfo));

            /// <summary>
            /// </summary>
            public Rect rcMonitor = new Rect();

            /// <summary>
            /// </summary>
            public Rect rcWork = new Rect();

            /// <summary>
            /// </summary>
            public int dwFlags = 0;
        }


        /// <summary> Win32 </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct Rect
        {
            /// <summary> Win32 </summary>
            public readonly int left;

            /// <summary> Win32 </summary>
            public readonly int top;

            /// <summary> Win32 </summary>
            public readonly int right;

            /// <summary> Win32 </summary>
            public readonly int bottom;

            /// <summary> Win32 </summary>
            public static readonly Rect Empty;

            /// <summary> Win32 </summary>
            public int Width
            {
                get { return Math.Abs(right - left); } // Abs needed for BIDI OS
            }

            /// <summary> Win32 </summary>
            public int Height
            {
                get { return bottom - top; }
            }

            /// <summary> Win32 </summary>
            public Rect(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }


            /// <summary> Win32 </summary>
            public Rect(Rect rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }

            /// <summary> Win32 </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return left >= right || top >= bottom;
                }
            }

            /// <summary> Return a user friendly representation of this struct </summary>
            public override string ToString()
            {
                if (this == Empty)
                {
                    return "Rect {Empty}";
                }
                return "Rect { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom +
                       " }";
            }

            /// <summary> Determine if 2 Rect are equal (deep compare) </summary>
            public override bool Equals(object obj)
            {
                if (!(obj is System.Windows.Rect))
                {
                    return false;
                }
                return (this == (Rect)obj);
            }

            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }


            /// <summary> Determine if 2 Rect are equal (deep compare)</summary>
            public static bool operator ==(Rect rect1, Rect rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right &&
                        rect1.bottom == rect2.bottom);
            }

            /// <summary> Determine if 2 Rect are different(deep compare)</summary>
            public static bool operator !=(Rect rect1, Rect rect2)
            {
                return !(rect1 == rect2);
            }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, Monitorinfo lpmi);

        /// <summary>
        /// </summary>
        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);


        #endregion  
    }
}
