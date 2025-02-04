using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static NTAPIUnhooking.Win32;

namespace NTAPIUnhooking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IntPtr bytesout;
            IntPtr ntdllHandle = Win32.GetModuleHandle("ntdll.dll");

            /*
                * 0x4C, 0x8B, 0xD1 = mov r10, rcx
                * 0xB8, 0x26, 0x00, 0x00, 0x00 = mov eax, 26
            */
            byte[] NtOpenProcessWrite = { 0x4C, 0x8B, 0xD1, 0xB8, 0x26, 0x00, 0x00, 0x00 };

            IntPtr nopAddress = Win32.GetProcAddress(ntdllHandle, "NtOpenProcess");

            if (ntdllHandle == IntPtr.Zero)
            {
                Console.WriteLine("[-] Failed to get module handle for ntdll.dll. Error code: " + Marshal.GetLastWin32Error());
                return;
            }
            Console.WriteLine("[+] Successfully got module handle for ntdll.dll!");

            if (nopAddress == IntPtr.Zero)
            {
                Console.WriteLine("[-] Failed to get address of NtOpenProcess. Error code: " + Marshal.GetLastWin32Error());
            }
            Console.WriteLine($"[+] NtOpenProcess address is: 0x{nopAddress.ToString("X")}");

            if (!WriteProcessMemory(Win32.GetCurrentProcess(), nopAddress, NtOpenProcessWrite, 8, out bytesout))
            {

                Console.WriteLine("[-] NtOpenProcess unhooking failed!");
                return;
            }
            Console.WriteLine("[+] NtOpenProcess unhooking succeded!");
            Console.WriteLine("[*] - Press [Enter] to terminate.");
            Console.ReadLine();
        }
    }
}
