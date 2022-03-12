using DynamicExecutor.Common;


namespace Executor1
{
    public class QueryTask1 : IWarehouseQuery
    {
        public string PerformQuery(string someParams)
        {
            var result = new { 
                Name = "Jian",
                Age = 51
            };
            var temp = someParams + Newtonsoft.Json.JsonConvert.SerializeObject(result);
            result = null;
            return temp;
        }
    }
}