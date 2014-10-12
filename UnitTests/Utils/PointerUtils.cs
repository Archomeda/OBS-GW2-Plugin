using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.UnitTests.Utils
{
    [ExcludeFromCodeCoverage]
    public static class PointerUtils
    {
        unsafe public static void CopyArrayToPointer(float[] source, float* target)
        {
            for (int i = 0; i < source.Length; i++)
                target[i] = source[i];
        }

        unsafe public static void CopyArrayToPointer(char[] source, char* target)
        {
            for (int i = 0; i < source.Length; i++)
                target[i] = source[i];
        }

        unsafe public static void CopyArrayToPointer(byte[] source, byte* target)
        {
            for (int i = 0; i < source.Length; i++)
                target[i] = source[i];
        }

        public static byte[] GetBytes(object structure)
        {
            int size = Marshal.SizeOf(structure);
            byte[] result = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(structure, ptr, true);
            Marshal.Copy(ptr, result, 0, size);
            Marshal.FreeHGlobal(ptr);

            return result;
        }
    }
}
