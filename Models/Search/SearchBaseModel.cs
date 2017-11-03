namespace VSBaseAngular.Models.Search
{
    public class SearchBaseModel<T> where T: ModelBase
    {
        public int Limit { get; set; } = int.MaxValue;
        public int Skip { get; set; } = 0;
    }
}