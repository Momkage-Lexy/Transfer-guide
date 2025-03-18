using System;
using System.Runtime.Loader; 

namespace transferguide.Utilities
{
    // Custom class to handle loading of unmanaged libraries
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        // Public method to load an unmanaged library from a specified absolute path
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            // Calls the protected method LoadUnmanagedDll to load the library
            return LoadUnmanagedDll(absolutePath);
        }

        // Override the base method to load an unmanaged DLL
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllPath)
        {
            // Calls the base method LoadUnmanagedDllFromPath to load the unmanaged DLL from the specified path
            return LoadUnmanagedDllFromPath(unmanagedDllPath);
        }
    }
}