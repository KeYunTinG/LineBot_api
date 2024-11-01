using LineBot_api.Services.Interfaces;
using System.Text;

namespace LineBot_api.Services
{
    public class IDCreate: IIDCreate
    {
        private static readonly object _lock = new object();
        private static readonly Random _random = new Random();
        private readonly string _traceId;
        public string traceId { get { return _traceId; } }

        public IDCreate()
        {
            lock (_lock)
            {
                string guid = Guid.NewGuid().ToString();
                int seed = guid.GetHashCode();

                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
                sb.Append(seed.ToString().Substring(2, 3));
                sb.Append(_random.Next(10000, 99999).ToString());

                _traceId = sb.ToString();
            }
        }
    }
}
