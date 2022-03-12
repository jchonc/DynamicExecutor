using DynamicExecutor;
using DynamicExecutor.Common;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Loader;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

const string executorZipFileName = @"C:\Work\DynamicExecutor\Buffer\Executor1.zip";
string extractedTempPath = Path.GetTempFileName();
File.Delete(extractedTempPath);
Directory.CreateDirectory(extractedTempPath);
ZipFile.ExtractToDirectory(executorZipFileName, extractedTempPath);

var loadContext = new ExecutorAssemblyLoadContext(extractedTempPath);
try
{
    var assembly = loadContext.LoadFromAssemblyPath(Path.Join(extractedTempPath, "Executor1.dll"));

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
}
finally
{
    loadContext.Unload();
}







