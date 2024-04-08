using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OpSis7_1
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowText(IntPtr hWnd, StringBuilder textBuf, int maxCount);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x положение верхнего левого угла
            public int Top;         // y положение верхнего левого угла
            public int Right;       // x положение правого нижнего угла
            public int Bottom;      // y положение правого нижнего угла
        }

        //При нажатии на эту кнопку происходит перебор всех процессов на компьютере и выводится заголовок главного окна каждого процесса в MessageBox.

        private void button1_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    StringBuilder textBuf = new StringBuilder(256);
                    GetWindowText(process.MainWindowHandle, textBuf, textBuf.Capacity);
                    //MessageBox.Show(textBuf.ToString());
                    textBox2.Text += textBuf.ToString() + Environment.NewLine;
                }
            }
        }

        //При вводе текста в textBox1 и нажатии на эту кнопку происходит поиск окна с указанным заголовком. Если окно найдено,
        //оно делается активным (SetForegroundWindow),а затем выводятся координаты его прямоугольника в MessageBox.
        private void button2_Click(object sender, EventArgs e)
        {
            IntPtr hWnd = FindWindow(null, textBox1.Text);
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
                RECT rect;
                GetWindowRect(hWnd, out rect);
                //MessageBox.Show($"Слева: {rect.Left}, Сверху: {rect.Top}, Справа: {rect.Right}, Снизу: {rect.Bottom}");
                textBox2.Text += $"Слева: {rect.Left}, Сверху: {rect.Top}, Справа: {rect.Right}, Снизу: {rect.Bottom}" + Environment.NewLine;
            }
            else
            {
                //MessageBox.Show("Окно не найдено");
                textBox2.Text += "Окно не найдено" + Environment.NewLine;

            }
        }

        //При нажатии на эту кнопку происходит перебор всех процессов на компьютере, но при этом также выводится дополнительная
        //информация о каждом процессе (идентификатор процесса (ID), заголовок главного окна, дескриптор главного окна и имя процесс)
        private void button3_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    //MessageBox.Show($"Process ID: {process.Id}, Main Window Title: {process.MainWindowTitle}, Main Window Handle: {process.MainWindowHandle}, Process Name: {process.ProcessName}");
                    textBox2.Text += $"Process ID: {process.Id}, Main Window Title: {process.MainWindowTitle}, Main Window Handle: {process.MainWindowHandle}, Process Name: {process.ProcessName}" + Environment.NewLine;
                }
            }
        }

        //При вводе текста в textBox1 и нажатии на эту кнопку происходит поиск окна с указанным заголовком. Если окно найдено, оно скрывается
        //с помощью функции ShowWindowAsync, используя параметр 0 (скрыть окно).Если окно не найдено, выводится сообщение "Window not found".
        private void button4_Click(object sender, EventArgs e)
        {
            IntPtr hWnd = FindWindow(null, textBox1.Text);
            if (hWnd != IntPtr.Zero)
            {
                ShowWindowAsync(hWnd, 0); // 0 - Hide, - Minimize, 9 - Restore, 6 - Maximize
            }
            else
            {
                //MessageBox.Show("Окно не найдено");
                textBox2.Text += "Окно не найдено" + Environment.NewLine;


            }
        }
        public Form1()
        {
            InitializeComponent();
        }
    }
}
