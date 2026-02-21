using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Incentives;

public interface IIncentive
{
    (bool Success, decimal Amount) Calculate(Rebate rebate, Product product, CalculateRebateRequest request = null);
}
