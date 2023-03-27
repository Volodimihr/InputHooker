using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Exam_InputHooker_Karvatyuk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // varibles
        private static HookerData hookerData = null;
        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;

        static IEnumerable<int> funcs = Enumerable.Range(112, 24);
        static IEnumerable<int> nums = Enumerable.Range(48, 10);
        static IEnumerable<int> letters = Enumerable.Range(65, 26);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, ref KeyHookStruct lParam);

        bool txtchanged = false;
        bool keywordschanged = false;
        List<int[]> indexes = null;
        int iC = 0;

        // Key hook Struction
        [StructLayout(LayoutKind.Sequential)]
        private struct KeyHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public uint dwExtraInfo;
        }

        // Import C++ System files
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callBack, IntPtr hinstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hinstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyHookStruct lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll")]
        static extern short GetKeyState(int vkCode);

        // Again varibles
        private LowLevelKeyboardProc _proc = hookProc;
        private static IntPtr hhook = IntPtr.Zero;

        static bool control = false;
        static bool shift = false;
        static bool alt = false;
        static System.Windows.Forms.KeyEventArgs ke = null;

        // Hooking process
        private static IntPtr hookProc(int nCode, IntPtr wParam, ref KeyHookStruct lParam)
        {
            if (nCode >= 0)
            {
                control = ((GetKeyState(0xA2) & 0x80) != 0) ||
                               ((GetKeyState(0xA3) & 0x80) != 0);

                shift = ((GetKeyState(0xA0) & 0x80) != 0) ||
                             ((GetKeyState(0xA1) & 0x80) != 0);

                alt = ((GetKeyState(0xA4) & 0x80) != 0) ||
                           ((GetKeyState(0xA5) & 0x80) != 0);

                bool capslock = (GetKeyState(0x14) != 0);

                // Save Key state to Key Event Argument
                ke = new System.Windows.Forms.KeyEventArgs(
                    (Keys)(lParam.vkCode |
                    (control ? (int)Keys.Control : 0) |
                    (shift ? (int)Keys.Shift : 0) |
                    (alt ? (int)Keys.Alt : 0)
                    ));

                // If Key down
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    // Show main window
                    if (ke.Control && ke.Shift && ke.KeyCode == Keys.D9)
                    {
                        System.Windows.Application.Current.MainWindow.Show();
                    }

                    // F1 ... F24
                    if (funcs.Contains(lParam.vkCode))
                    {
                        hookerData.InputsStr += (Keys)lParam.vkCode;
                    }
                    // Digits
                    else if (nums.Contains(lParam.vkCode))
                    {
                        hookerData.InputsStr += lParam.vkCode - 48;
                    }
                    // If Shift presed
                    else if (Keyboard.Modifiers == ModifierKeys.Shift)
                    {
                        if (!(lParam.vkCode == 160 || lParam.vkCode == 161))
                        {
                            if (letters.Contains(lParam.vkCode))
                                hookerData.InputsStr += (Keys)lParam.vkCode;
                            else
                                hookerData.InputsStr += hookerData.KeyShift(lParam.vkCode);
                        }
                    }
                    // If Control pressed
                    else if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        if (!(lParam.vkCode == 162 || lParam.vkCode == 163))
                            hookerData.InputsStr += $" Ctrl+{(Keys)lParam.vkCode} ";
                    }
                    /* BackSpace
                    else if(lParam.vkCode == 8)
                    {
                        if(hookerData.InputsStr.Length > 0)
                            hookerData.InputsStr = hookerData.InputsStr.Remove(hookerData.InputsStr.Length-1);
                    }
                    */
                    else
                    {
                        if (!(lParam.vkCode == 160 || lParam.vkCode == 161 ||
                              lParam.vkCode == 162 || lParam.vkCode == 163))
                        {
                            // If letter or simbol
                            if (letters.Contains(lParam.vkCode))
                                hookerData.InputsStr += ((Keys)lParam.vkCode).ToString().ToLower();
                            else
                                hookerData.InputsStr += hookerData.Simbol(lParam.vkCode);
                        }
                    }
                }

                // If System Key down (Alt and combinations)
                if (wParam == (IntPtr)WM_SYSKEYDOWN)
                {
                    if ((lParam.vkCode == 0x09) && (lParam.flags == 0x20)) hookerData.InputsStr += " (Alt+Tab) ";
                    else if ((lParam.vkCode == 0x1B) && (lParam.flags == 0x20)) hookerData.InputsStr += " (Alt+Esc) ";
                    else if ((lParam.vkCode == 0x73) && (lParam.flags == 0x20)) hookerData.InputsStr += " (Alt+F4) ";
                    else if ((lParam.vkCode == 0x20) && (lParam.flags == 0x20)) hookerData.InputsStr += " (Alt+Space) ";
                    else if ((lParam.vkCode == 0xA0) && (lParam.flags == 0x20)) hookerData.InputsStr += " (Alt+Shift) ";
                }

                return (IntPtr)0;
            }
            else
            {
                return CallNextHookEx(hhook, nCode, (int)wParam, ref lParam);
            }
        }

        public void SetHook()
        {
            IntPtr hinstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hinstance, 0);
        }
        public static void UnHook()
        {
            UnhookWindowsHookEx(hhook);
        }

        public MainWindow()
        {
            InitializeComponent();

            hookerData = new HookerData();
            indexes = new List<int[]>();

            // Binding
            DataContext = hookerData;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetHook();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            UnHook();
        }

        // Search specific words
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtchanged || keywordschanged)
            {
                // Search button control
                indexes.Clear();
                txtchanged = false;
                keywordschanged = false;
                btnSearch.Content = "Next";

                foreach (string word in hookerData.KeyWordsArray)
                {

                    MatchCollection matches = Regex.Matches(hookerData.InputsStr, word, RegexOptions.IgnoreCase);
                    if (matches.Count > 0)
                    {
                        int wl = word.Length;
                        for (int i = 0; i < matches.Count; i++)
                        {
                            indexes.Add(new int[] { matches[i].Index, wl });
                        }
                    }
                }

                // Show first specific word in TextBox
                if (indexes.Count > 0)
                {
                    iC = 0;
                    tbxInputs.Focus();
                    tbxInputs.Select(indexes[iC][0], indexes[iC][1]);
                }
            }
            // Show Next specific word in TextBox
            else
            {
                tbxInputs.Focus();
                if (iC < indexes.Count - 1)
                    iC++;
                else
                    iC = 0;

                tbxInputs.Select(indexes[iC][0], indexes[iC][1]);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Hide main window and notifycation
        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            System.Windows.MessageBox.Show("Press Crtl+Shift+9 to show window again");
        }

        // Save inputs to log file
        private void SaveToFileButton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText($"Log_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.log", hookerData.InputsStr);
        }

        // Focus on search TextBox and unhook
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UnHook();
        }

        // Lost focus on search TextBox and hook
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetHook();
        }

        // Reset search if inputs chaged
        private void InputsStr_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtchanged)
            {
                txtchanged = true;
                btnSearch.Content = "Search";
            }
        }

        // Reset search if keywords changed
        private void KeyWords_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!keywordschanged)
            {
                keywordschanged = true;
                btnSearch.Content = "Search";
            }
        }
    }
}
