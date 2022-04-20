using Microsoft.UI;
using Microsoft.UI.Windowing;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinRT.Interop;

namespace TestWindow.Helpers;

public static class Window
{
    public static IntPtr hWnd;
    public static AppWindow App => AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(hWnd));
    public static OverlappedPresenter Presenter => (OverlappedPresenter)App.Presenter;


    public static IntPtr GetHWnd(Microsoft.UI.Xaml.Window Window) =>
        WindowNative.GetWindowHandle(Window);


    public static void MakeTransparent()
    {
        Win32.SubClassDelegate = new(Win32.WindowSubClass);
        Win32.SetWindowSubclass(hWnd, Win32.SubClassDelegate, 0, 0);

        long nExStyle = Win32.GetWindowLong(hWnd, Win32.GWL_EXSTYLE);
        if ((nExStyle & Win32.WS_EX_LAYERED) == 0)
        {
            Win32.SetWindowLong(hWnd, Win32.GWL_EXSTYLE, (IntPtr)(nExStyle | Win32.WS_EX_LAYERED));
            Win32.SetLayeredWindowAttributes(hWnd, (uint)ColorTranslator.ToWin32(Color.Magenta), 255, Win32.LWA_COLORKEY);
        }
    }

    public static void SetMica(bool Enable, bool DarkMode)
    {
        int IsMicaEnabled = Enable ? 1 : 0;
        Win32.DwmSetWindowAttribute(hWnd, (int)Win32.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref IsMicaEnabled, sizeof(int));

        int IsDarkEnabled = DarkMode ? 1 : 0;
        Win32.DwmSetWindowAttribute(hWnd, (int)Win32.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref IsDarkEnabled, sizeof(int));
    }

    public static void SetAcrylic(bool Enable, bool DarkMode) =>
        SetComposition(Win32.AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND, Enable, DarkMode);

    public static void SetBlur(bool Enable, bool DarkMode) =>
        SetComposition(Win32.AccentState.ACCENT_ENABLE_BLURBEHIND, Enable, DarkMode);

    public static void SetComposition(Win32.AccentState AccentState, bool Enable, bool DarkMode)
    {
        var Accent = Enable ? new Win32.AccentPolicy()
        {
            AccentState = AccentState,
            GradientColor = Convert.ToUInt32(DarkMode ? 0x990000 : 0xFFFFFF)
        } : new Win32.AccentPolicy() { AccentState = 0 };

        var StructSize = Marshal.SizeOf(Accent);
        var Ptr = Marshal.AllocHGlobal(StructSize);
        Marshal.StructureToPtr(Accent, Ptr, false);

        var data = new Win32.WindowCompositionAttributeData()
        {
            Attribute = Win32.WindowCompositionAttribute.WCA_ACCENT_POLICY,
            SizeOfData = StructSize,
            Data = Ptr
        };

        Win32.SetWindowCompositionAttribute(hWnd, ref data);
        Marshal.FreeHGlobal(Ptr);
    }
}
