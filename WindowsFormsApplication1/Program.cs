using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    static class Program
    {
        static MainForm frm;
        private static IntPtr _hookID = IntPtr.Zero;

        private static KeyboardHook.LowLevelKeyboardProc proc = HookCallback;

        private unsafe static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)KeyboardHook.WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                switch (vkCode)
                {
                    case 123:
                        {
                            frm.ToggleFishing(); // F12
                            break;
                        }
                    case 122:
                        {
                            frm.IncreaseSens(); // F10
                            break;
                        }
                    case 121:
                        {
                            frm.DescreaseSens(); // F11
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
            }
            return KeyboardHook.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static unsafe void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _hookID = KeyboardHook.SetHook(proc);

            frm = new MainForm();

            Application.Run(frm);

            KeyboardHook.UnhookWindowsHookEx(_hookID);
        }
    }
}
