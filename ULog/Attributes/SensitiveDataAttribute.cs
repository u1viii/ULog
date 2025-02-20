namespace ULog.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SensitiveDataAttribute : Attribute
    {
        public SensitiveDataAttribute(string mask = "******")
        {
            Mask = mask;
        }

        public string Mask { get; }
    }
}
