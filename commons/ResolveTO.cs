namespace splendor.net5.persistance.commons
{
    public class ResolveTO
    {
        public string Query { get; set; }
        public object[] Values {get; set;}
        public string Order { get; set; }
    }
}