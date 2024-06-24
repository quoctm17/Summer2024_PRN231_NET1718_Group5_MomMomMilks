namespace MomMomMilks.Types
{
    public class CancelPaymentLinkRequest
    {
        public long orderCode { get; set; }
        public string? cancellationReason { get; set; }
    }
}
