namespace TadesApi.Portal.ActionFilters
{
    [AttributeUsage(AttributeTargets.All)]
    public class SecurityStateAttribute : Attribute
    {
        public int ActionNo { get; }
        public string Controller { get; }

        public SecurityStateAttribute(int actionNo, string controller = null)
        {
            ActionNo = actionNo;
            Controller = controller;
        }
    }
}
