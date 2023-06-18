using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class PurchaseInfoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textView;

    private void Awake()
    {
        AmazonAppstoreBilling.Instance.Initialized += OnBillingInitialized;
        AmazonAppstoreBilling.Instance.InitializationFailed += OnBillingInitializationFailed;
        AmazonAppstoreBilling.Instance.Purchased += OnBillingPurchased;
        AmazonAppstoreBilling.Instance.PurchaseFailed += OnBillingPurchaseFailed;
    }
    private void OnDestroy()
    {
        AmazonAppstoreBilling.Instance.Initialized -= OnBillingInitialized;
        AmazonAppstoreBilling.Instance.InitializationFailed -= OnBillingInitializationFailed;
        AmazonAppstoreBilling.Instance.Purchased -= OnBillingPurchased;
        AmazonAppstoreBilling.Instance.PurchaseFailed -= OnBillingPurchaseFailed;
    }

    public void Show(string newText)
    {
        textView.text += "\n";
        textView.text += newText;
    }

    private void OnBillingInitialized()
    {
        Show("AmazonAppstoreBilling.Initialized");
    }

    private void OnBillingInitializationFailed(InitializationFailureReason reason, string message)
    {
        Show($"AmazonAppstoreBilling.InitializationFailed({reason}, {message})");
    }

    private void OnBillingPurchased(string productId)
    {
        Show($"AmazonAppstoreBilling.Purchased({productId})");
    }

    private void OnBillingPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Show($"AmazonAppstoreBilling.PurchaseFailed({product}, {failureDescription})");
    }
}
