using UnityEngine.Purchasing;

public class ProductContainer
{
    public readonly IapProduct[] products;

    public readonly IapProduct goldCoins100 = new IapProduct("dotbake_test_purchase", ProductType.Consumable);
    public readonly IapProduct monthSubscription = new IapProduct("month_subscribe_2", ProductType.Subscription);

    private static ProductContainer instance;

    private ProductContainer()
    {
        products = new IapProduct[] {
            goldCoins100,
            monthSubscription,
        };
    }

    public static ProductContainer Instance
    {
        get
        {
            if (instance == null)
                instance = new ProductContainer();

            return instance;
        }
    }
}
