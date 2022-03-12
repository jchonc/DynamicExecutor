using DynamicExecutor.Common;

namespace Executor1
{
    public class QueryTask1 : IWarehouseQuery
    {
        public string PerformQuery(string someParams)
        {
            return "*" + someParams + "*";
        }
    }
}