# NTAPIUnhooking

## What is API Hooking?
Various security solutions assign a hook (API Hooking) to various Win32 API calls that are used for malicious purposes to tell if these calls are legitimate or not. This changes the execution flow of the normal API instruction to a custom module controlled by the solution itself that will perform further analysis to the API call.

Here is an example with an NT API definition (NtOpenProcess):

![](https://raw.githubusercontent.com/Janrdrz/NTAPIUnhooking/refs/heads/main/assets/1.jpg)

This is how a security solution hooks the API:

![](https://raw.githubusercontent.com/Janrdrz/NTAPIUnhooking/refs/heads/main/assets/2.jpg)

To return it to the original definition, we can write the original bytes back by applying memory patching.

```csharp
/*
    * 0x4C, 0x8B, 0xD1 = mov r10, rcx
    * 0xB8, 0x26, 0x00, 0x00, 0x00 = mov eax, 26
*/
byte[] NtOpenProcessWrite = { 0x4C, 0x8B, 0xD1, 0xB8, 0x26, 0x00, 0x00, 0x00 };

if (!WriteProcessMemory(Win32.GetCurrentProcess(), nopAddress, NtOpenProcessWrite, 8, out bytesout))
{

    Console.WriteLine("[-] NtOpenProcess unhooking failed!");
    return;
}
Console.WriteLine("[+] NtOpenProcess unhooking succeded!");
```

And voilà!

![](https://raw.githubusercontent.com/Janrdrz/NTAPIUnhooking/refs/heads/main/assets/3.jpg)
