using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Incentives;

public class AmountPerUomIncentive : IIncentive
{
    public (bool Success, decimal Amount) Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        if (
                rebate == null
            || product == null
            || !product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom)
            || rebate.Amount == 0
            || request.Volume == 0
        )
        {
            return (false, 0m);
        }

        return (true, rebate.Amount * request.Volume);
    }
}
