using System;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class AmazonAppstoreBilling : IDetailedStoreListener
{
    public event Action Initialized;
    public event Action<InitializationFailureReason, string> InitializationFailed;
    public event Action<string> Purchased;
    public event Action<Product, PurchaseFailureDescription> PurchaseFailed;

    private static AmazonAppstoreBilling instance;

    private IStoreController controller;
    private IExtensionProvider extensions;
    private Action<string> oneTimePurchaseAction;

    private AmazonAppstoreBilling()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (var product in ProductContainer.Instance.products)
        {
            builder.AddProduct(product.Id, product.Type);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public static AmazonAppstoreBilling Instance
    {
        get
        {
            if (instance == null)
                instance = new AmazonAppstoreBilling();

            return instance;
        }
    }

    private bool IsInitialized => controller != null;

    public void BuyProduct(string productId, Action<string> onPurchased, Action<string> onFailure = null)
    {
        try
        {
            if (!IsInitialized)
            {
                onFailure?.Invoke("BuyProduct FAIL. Not initialized.");
                return;
            }

            Product product = controller.products.WithID(productId);
            if (product == null || !product.availableToPurchase)
            {
                onFailure?.Invoke("BuyProduct: FAIL. Not purchasing product, either is not found or is not available for purchase");
                return;
            }

            oneTimePurchaseAction = onPurchased;
            controller.InitiatePurchase(product);
        }
        catch (Exception exception)
        {
            onFailure?.Invoke($"BuyProduct: FAIL. Exception during purchase. {exception}");
        }
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;

        Initialized?.Invoke();
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, "");
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        InitializationFailed?.Invoke(error, message);
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs eventArgs)
    {
        string productId = eventArgs.purchasedProduct.definition.id;
        Purchased?.Invoke(productId);

        oneTimePurchaseAction?.Invoke(productId);
        oneTimePurchaseAction = null;

        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        PurchaseFailureDescription failureDescription = new PurchaseFailureDescription(product.definition.id, failureReason, "");
        OnPurchaseFailed(product, failureDescription);
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        PurchaseFailed?.Invoke(product, failureDescription);
    }
}
