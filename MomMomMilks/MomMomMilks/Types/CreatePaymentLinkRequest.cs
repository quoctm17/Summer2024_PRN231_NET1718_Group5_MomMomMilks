
using BusinessObject.Entities;

namespace MomMomMilks.Types
{
    public record CreatePaymentLinkRequest(
        int mealPlanId,
        int cartId,
        string description,
        string returnUrl,
        string cancelUrl
        );
}
