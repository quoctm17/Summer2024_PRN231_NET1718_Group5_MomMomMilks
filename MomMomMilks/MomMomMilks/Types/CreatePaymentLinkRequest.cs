
using BusinessObject.Entities;

namespace MomMomMilks.Types
{
    public record CreatePaymentLinkRequest(
        int orderId,
        string description,
        string returnUrl,
        string cancelUrl
        );
}
