using Smartwyre.DeveloperTest.Services.Incentives;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class IncentiveFactory : IIncentiveFactory
{
    public IIncentive GetIncentive(IncentiveType type)
    {
        return type switch
        {
            IncentiveType.FixedCashAmount => new FixedCashAmountIncentive(),
            IncentiveType.FixedRateRebate => new FixedRateRebateIncentive(),
            IncentiveType.AmountPerUom => new AmountPerUomIncentive(),
            _ => throw new NotImplementedException("Incentive type not implemented")
        };
    }
}
