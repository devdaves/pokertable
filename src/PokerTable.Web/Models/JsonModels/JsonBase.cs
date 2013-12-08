namespace PokerTable.Web.Models.JsonModels
{
    public abstract class JsonBase
    {
        public int Status { get; set; }

        public string FailureMessage { get; set; }

        protected JsonBase()
        {
            this.Status = 0;
            this.FailureMessage = string.Empty;
        }
    }
}