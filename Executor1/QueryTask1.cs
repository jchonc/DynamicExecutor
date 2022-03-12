using DynamicExecutor.Common;


namespace Executor1
{
    public class QueryTask1 : IWarehouseQuery
    {
        public string PerformQuery(string someParams)
        {
            var result = new { 
                Name = "Jian",
                Age = 50
            };
            return someParams + Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }
}