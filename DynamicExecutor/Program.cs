using DynamicExecutor;
using DynamicExecutor.Common;
using System.IO.Compression;
using System.Reflection;

// Get where is the zipped plugin, in real life, could be somewhere downloaded from database or BlobStorage
var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var executorZipFileName = Path.GetFullPath(Path.Combine(currentPath, "..", "..", "..", "..", "Buffer", "Executor1.zip"));

// Create a temp folder and extract the zipped plugin into it
var fn = Path.GetTempFileName();
string extractedTempPath = Path.Join(Path.GetDirectoryName(fn), Path.GetFileNameWithoutExtension(fn));
if (File.Exists(extractedTempPath))
{
    File.Delete(extractedTempPath);
}
Directory.CreateDirectory(extractedTempPath);

// Extract it
ZipFile.ExtractToDirectory(executorZipFileName, extractedTempPath);


// I hardcoded the name of the DLL and the class name of the tasks, in real life probably 
// you'll put them into some sort of manifestal file

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
    // Unload it once done. 
    loadContext.Unload();
}
