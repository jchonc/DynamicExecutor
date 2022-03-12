using DynamicExecutor.Common;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WinForm
{
    public partial class Form1 : Form
    {
        private string _executorZipFileName;
        public Form1()
        {
            InitializeComponent();
            var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _executorZipFileName = Path.GetFullPath(Path.Combine(currentPath, "..", "..", "..", "..", "Buffer", "Executor1.zip"));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                textBox1.Clear();
                var fn = Path.GetTempFileName();
                string extractedTempPath = Path.Join(Path.GetDirectoryName(fn), Path.GetFileNameWithoutExtension(fn));
                if (File.Exists(extractedTempPath))
                {
                    File.Delete(extractedTempPath);
                }
                Directory.CreateDirectory(extractedTempPath);
                try
                {
                    ZipFile.ExtractToDirectory(_executorZipFileName, extractedTempPath);

                    var executorFile = Path.Join(extractedTempPath, "Executor1.dll");
                    var loadContext = new ExecutorAssemblyLoadContext(executorFile);
                    var alcWeakRef = new WeakReference(loadContext);
                    try
                    {
                        var assembly = loadContext.LoadFromAssemblyPath(executorFile);
                        var type = assembly.GetType("Executor1.QueryTask1");
                        if (type != null && typeof(IWarehouseQuery).IsAssignableFrom(type))
                        {
                            var query = Activator.CreateInstance(type) as IWarehouseQuery;
                            if (query != null)
                            {
                                var result = query.PerformQuery(textBox2.Text);
                                textBox1.Text = new string(result);
                                result = null;
                            }
                            query = null;
                        }
                        type = null;
                        assembly = null;
                        loadContext.FreeAll();
                        loadContext.Unload();
                    }
                    finally
                    {
                        int counter = 0;
                        for (counter = 0; alcWeakRef.IsAlive && (counter < 10); counter++)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        if (alcWeakRef.IsAlive)
                        {
                            MessageBox.Show("Bad");
                        }
                    }
                }
                finally
                {
                    //Directory.Delete(extractedTempPath, true);
                }
            }
            finally
            {
                button1.Enabled = true;
            }
        }
    }
}