namespace BloggingPlatformAPI.Helpers
{
    public class QueryParameters
    {
        private int _maxLimit = 100;
        private int _limit = 50;

        public int Page { get; set; } = 1;
        public int Limit 
        { 
            get 
            { 
                return _limit; 
            }
            set 
            {
                _limit = Math.Min(value, _maxLimit);
            }
        }
    }
}
