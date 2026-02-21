using Smartwyre.DeveloperTest.Services.Incentives;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public interface IIncentiveFactory
{
    IIncentive GetIncentive(IncentiveType type);
}
