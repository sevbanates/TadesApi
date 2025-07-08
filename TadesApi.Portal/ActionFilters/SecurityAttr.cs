namespace TadesApi.Portal.ActionFilters
{
    [AttributeUsage(AttributeTargets.All)]
    public class SecurityStateAttribute : Attribute
    {
        int actionNo;

        public SecurityStateAttribute(int actionNo)
        {
            this.actionNo = actionNo;
        }

        public int ActionNo
        {
            get { return actionNo; }
        }
    }
}
