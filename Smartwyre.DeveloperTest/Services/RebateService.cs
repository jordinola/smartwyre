using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IIncentiveFactory _incentiveFactory;

    public RebateService(
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore,
        IIncentiveFactory incentiveFactory
    )
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _incentiveFactory = incentiveFactory;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        try
        {
            Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
            Product product = _productDataStore.GetProduct(request.ProductIdentifier);

            var incentive = _incentiveFactory.GetIncentive(rebate.Incentive);
            var result = incentive.Calculate(rebate, product, request);

            if (result.Success)
            {
                _rebateDataStore.StoreCalculationResult(rebate, result.Amount);
            }

            return new CalculateRebateResult { Success = result.Success };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
