using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Incentives;

public class FixedCashAmountIncentive : IIncentive
{
    public (bool Success, decimal Amount) Calculate(Rebate rebate, Product product, CalculateRebateRequest _)
    {

        if (
                rebate == null
            || rebate.Amount == 0
            || !product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount)
        )
        {
            return (false, 0m);
        }

        return (true, rebate.Amount);
    }
}
