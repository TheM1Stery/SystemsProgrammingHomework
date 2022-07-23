// See https://aka.ms/new-console-template for more information

using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace OpenFileDialogWrapper;

    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Console.WriteLine("Hello world :)");
            using var dialog = new OpenFileDialogWrapper();
            var result = dialog.GetTextFileContent();
            if (result is null)
            {
                Console.WriteLine("couldn't read file");
                return;
            }
            Console.WriteLine(result);
        }
    }
    
    internal class OpenFileDialogWrapper : IDisposable
    {
        private readonly object? _openFileDialog;

        private readonly Type? _openFileDialogType;

        private readonly AssemblyLoadContext _loadContext;

    
    
        public OpenFileDialogWrapper()
        {
            _loadContext = new AssemblyLoadContext("MyContext", true);
            var assembly = _loadContext.LoadFromAssemblyName(new AssemblyName("PresentationFramework"));
            _openFileDialogType = assembly.GetType("Microsoft.Win32.OpenFileDialog");
            if (_openFileDialogType is not null)
                _openFileDialog = Activator.CreateInstance(_openFileDialogType);
        }


        public string? GetTextFileContent()
        {
            _openFileDialogType?.GetProperty("Filter")?
                .SetValue(_openFileDialog, "Text documents (.txt)|*.txt");
            var methodInfo = _openFileDialogType?.GetMethod("ShowDialog", Array.Empty<Type>());
            var result = methodInfo?.Invoke(_openFileDialog, null);
            if (result is not true) 
                return null;
            if (_openFileDialogType?.GetProperty("FileName")?.GetValue(_openFileDialog) is not string salam)
                return null;
            return File.ReadAllText(salam);
        }

        public void Dispose()
        {
            _loadContext.Unload();
        }
    }