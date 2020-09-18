namespace BaseIT.SCSM.Client.Operations
{
    public abstract class BaseOperation
    {
        protected SCSMClient _client { get; }

        internal BaseOperation(SCSMClient client)
        {
            _client = client;
        }
    }
}
