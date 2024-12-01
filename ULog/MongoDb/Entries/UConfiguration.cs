namespace ULog.MongoDb.Entries
{
    public class UConfiguration
    {
        public string Keyword { get; set; } = string.Empty;
        internal bool UseCookies => !string.IsNullOrEmpty(Keyword);
        public UConfiguration() { }
        public UConfiguration(string keyword) { Keyword = keyword; }
    }
}
