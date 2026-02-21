using System;
using System.Collections.Generic;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Incentives;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private readonly Mock<IRebateDataStore> _rebateDataStore = new();
    private readonly Mock<IProductDataStore> _productDataStore = new();
    private readonly Mock<IIncentiveFactory> _incentiveFactory = new();

    private RebateService CreateSut() =>
        new(_rebateDataStore.Object, _productDataStore.Object, _incentiveFactory.Object);

    [Theory]
    [MemberData(nameof(IncentiveTestCases))]
    public void Calculate_ReturnsSuccessAndStoresResult_ForEachIncentiveType(
    IncentiveType incentiveType,
    IIncentive incentive,
    Rebate rebate,
    Product product,
    CalculateRebateRequest request,
    decimal expectedAmount)
    {
        // Arrange
        _rebateDataStore
            .Setup(x => x.GetRebate(request.RebateIdentifier))
            .Returns(rebate);
        _productDataStore
            .Setup(x => x.GetProduct(request.ProductIdentifier))
            .Returns(product);
        _incentiveFactory
            .Setup(x => x.GetIncentive(incentiveType))
            .Returns(incentive);

        var sut = CreateSut();

        // Act
        var result = sut.Calculate(request);

        // Assert
        Assert.True(result.Success);
        _rebateDataStore.Verify(
            x => x.StoreCalculationResult(rebate, expectedAmount),
            Times.Once
        );
    }

    [Fact]
    public void Calculate_IncentiveNotSetUp_ThrowsException()
    {
        // Arrange
        _rebateDataStore
            .Setup(x => x.GetRebate(It.IsAny<string>()))
            .Returns(new Rebate { Identifier = "rebate-1", Incentive = IncentiveType.TestingOnly });
        _productDataStore
            .Setup(x => x.GetProduct(It.IsAny<string>()))
            .Returns(new Product { Identifier = "product-1", Price = 100m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate });
        _incentiveFactory
            .Setup(x => x.GetIncentive(IncentiveType.TestingOnly))
            .Returns((IIncentive)null);

        // Act
        var sut = CreateSut();
        Assert.Throws<NullReferenceException>(() =>
            sut.Calculate(
                new CalculateRebateRequest
                {
                    RebateIdentifier = "rebate-1",
                    ProductIdentifier = "product-1",
                    Volume = 10m
                })
        );

        // Assert
        _rebateDataStore.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
    }

    public static IEnumerable<object[]> IncentiveTestCases =>
    [
        [
        IncentiveType.FixedRateRebate,
        new FixedRateRebateIncentive(),
        new Rebate { Identifier = "rebate-1", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m },
        new Product { Identifier = "product-1", Price = 100m, SupportedIncentives = SupportedIncentiveType.FixedRateRebate },
        new CalculateRebateRequest { RebateIdentifier = "rebate-1", ProductIdentifier = "product-1", Volume = 10m },
        100m
    ],
    [
        IncentiveType.AmountPerUom,
        new AmountPerUomIncentive(),
        new Rebate { Identifier = "rebate-2", Incentive = IncentiveType.AmountPerUom, Amount = 5m },
        new Product { Identifier = "product-2", Price = 100m, SupportedIncentives = SupportedIncentiveType.AmountPerUom },
        new CalculateRebateRequest { RebateIdentifier = "rebate-2", ProductIdentifier = "product-2", Volume = 10m },
        50m
    ],
    [
        IncentiveType.FixedCashAmount,
        new FixedCashAmountIncentive(),
        new Rebate { Identifier = "rebate-3", Incentive = IncentiveType.FixedCashAmount, Amount = 75m },
        new Product { Identifier = "product-3", Price = 100m, SupportedIncentives = SupportedIncentiveType.FixedCashAmount },
        new CalculateRebateRequest { RebateIdentifier = "rebate-3", ProductIdentifier = "product-3", Volume = 10m },
        75m
    ],
];
}
