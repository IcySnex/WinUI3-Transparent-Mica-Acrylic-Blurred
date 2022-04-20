using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace TestWindow.Helpers;

public static class Win32
{
    public const int WM_ERASEBKGND = 0x0014;
    public const int WS_EX_LAYERED = 0x00080000;
    public const uint LWA_COLORKEY = 0x00000001;
    public const int GWL_EXSTYLE = -20;


    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(int Left, int Top, int Right, int Bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }

        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);


    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool FillRect(IntPtr hdc, [In] ref RECT rect, IntPtr hbrush);

    [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr CreateSolidBrush(int crColor);

    [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool DeleteObject([In] IntPtr hObject);


    public delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData);

    public static SUBCLASSPROC? SubClassDelegate;

    [DllImport("Comctl32.dll", SetLastError = true)]
    public static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, uint uIdSubclass, uint dwRefData);

    [DllImport("Comctl32.dll", SetLastError = true)]
    public static extern int DefSubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

    public static int WindowSubClass(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData)
    {
        switch (uMsg)
        {
            case WM_ERASEBKGND:
                {
                    RECT rect;
                    GetClientRect(hWnd, out rect);

                    rect.right = 0;

                    IntPtr hBrush = CreateSolidBrush(ColorTranslator.ToWin32(Color.Magenta));
                    FillRect(wParam, ref rect, hBrush);
                    DeleteObject(hBrush);
                    return 1;
                }
        }

        return DefSubclassProc(hWnd, uMsg, wParam, lParam);
    }


    public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
        if (IntPtr.Size == 4)
            return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
        return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
    }

    [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
    public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
    public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);


    public static long GetWindowLong(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size == 4)
            return GetWindowLong32(hWnd, nIndex);
        return GetWindowLongPtr64(hWnd, nIndex);
    }

    [DllImport("User32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
    public static extern long GetWindowLong32(IntPtr hWnd, int nIndex);

    [DllImport("User32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
    public static extern long GetWindowLongPtr64(IntPtr hWnd, int nIndex);


    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);


    [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool GetCursorPos(out Windows.Graphics.PointInt32 lpPoint);


    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


    public enum DWMWINDOWATTRIBUTE
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        DWMWA_MICA_EFFECT = 1029
    };

    [DllImport("dwmapi.dll", PreserveSig = true)]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);


    public enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AccentPolicy
    {
        public AccentState AccentState;
        public uint AccentFlags;
        public uint GradientColor;
        public uint AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    public enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19
    }

    [DllImport("user32.dll")]
    public static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
}
