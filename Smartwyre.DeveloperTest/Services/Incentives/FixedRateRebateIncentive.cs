using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Incentives;

public class FixedRateRebateIncentive : IIncentive
{
    public (bool Success, decimal Amount) Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (
            rebate == null
            || product == null
            || !product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate)
            || rebate.Percentage == 0
            || product.Price == 0
            || request.Volume == 0
        )
        {
            return (false, 0m);
        }

        return (true, product.Price * rebate.Percentage * request.Volume);

    }
}
