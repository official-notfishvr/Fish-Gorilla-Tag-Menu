/* 
OKAA Code
https://discord.gg/401
*/

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Fish_Menu.MainMenu
{
    public class ConsoleUtility : MonoBehaviour
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int AllocConsole();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleTitle(string lpConsoleTitle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteConsole(IntPtr hConsoleOutput, string lpBuffer, uint nNumberOfCharsToWrite, out uint lpNumberOfCharsWritten, IntPtr lpReserved);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, uint wAttributes);

        public static void OpenConsoleWindow()
        {
            ConsoleUtility.AllocConsole();
            ConsoleUtility.SetConsoleTitle("Console [Initializing]");
            ConsoleUtility.WriteToConsole("Debug log started!", ConsoleColor.Green);
            Debug.Log("Console was started!");
            ConsoleUtility.SetConsoleTitle("Console [Ready]");
        }
        public static void WriteToConsole(string message, ConsoleColor color)
        {
            IntPtr stdHandle = ConsoleUtility.GetStdHandle(-11);
            message += "\n";
            ConsoleUtility.SetConsoleTextAttribute(stdHandle, (uint)color);
            uint num;
            ConsoleUtility.WriteConsole(stdHandle, message, (uint)message.Length, out num, IntPtr.Zero);
            ConsoleUtility.SetConsoleTextAttribute(stdHandle, 15U);
        }
        public static void WriteLine(string text)
        {
            string text2 = "[INFO]: " + text;
            IntPtr stdHandle = ConsoleUtility.GetStdHandle(-11);
            text2 += "\n";
            ConsoleUtility.SetConsoleTextAttribute(stdHandle, 15U);
            uint num;
            ConsoleUtility.WriteConsole(stdHandle, text2, (uint)text2.Length, out num, IntPtr.Zero);
            ConsoleUtility.SetConsoleTextAttribute(stdHandle, 15U);
        }
        public static void LCE(string message)
        {
            IntPtr stdHandle = ConsoleUtility.GetStdHandle(-11);
            message += "\n";
            ConsoleUtility.SetConsoleTextAttribute(stdHandle, 12U);
            uint num;
            ConsoleUtility.WriteConsole(stdHandle, message, (uint)message.Length, out num, IntPtr.Zero);
            ConsoleUtility.SetConsoleTextAttribute(stdHandle, 15U);
        }
        public static IntPtr FindConsoleWindow(string windowName)
        {
            return ConsoleUtility.FindWindow(null, windowName);
        }
        public static IntPtr GetConsole()
        {
            return ConsoleUtility.FindWindow("cmd", null);
        }
        private static int co;
    }
}
