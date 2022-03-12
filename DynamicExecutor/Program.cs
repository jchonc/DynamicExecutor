using DynamicExecutor;
using DynamicExecutor.Common;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Loader;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

const string executorZipFileName = @"C:\Work\DynamicExecutor\Buffer\Executor1.zip";
var fn = Path.GetTempFileName();
string extractedTempPath = Path.Join(Path.GetDirectoryName(fn), Path.GetFileNameWithoutExtension(fn));
File.Delete(extractedTempPath);
Directory.CreateDirectory(extractedTempPath);
extractedTempPath += Path.DirectorySeparatorChar;
ZipFile.ExtractToDirectory(executorZipFileName, extractedTempPath);
var executorFile = Path.Join(extractedTempPath, "Executor1.dll");

var loadContext = new ExecutorAssemblyLoadContext(executorFile);
try
{
    var assembly = loadContext.LoadFromAssemblyPath(executorFile);

    var type = assembly.GetType("Executor1.QueryTask1");

    if (type != null && typeof(IWarehouseQuery).IsAssignableFrom(type))
    {
        var query = Activator.CreateInstance(type) as IWarehouseQuery;
        if (query != null)
        {
            var result = query.PerformQuery("abcd");
            Console.WriteLine(result);
        }
    }
    Console.ReadKey();
}
finally
{
    loadContext.Unload();
}
