using UnityEngine;
using UnityEngine.UI;

public class BuyCoinsButton : MonoBehaviour
{
    private readonly IapProduct BoughtProduct = ProductContainer.Instance.goldCoins100;

    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        AmazonAppstoreBilling.Instance.BuyProduct(BoughtProduct.Id, OnPurchased, OnPurchaseFailed);
    }

    private void OnPurchased(string productID)
    {
        MoneyStore.Instance.Increase(100);
    }

    private void OnPurchaseFailed(string info)
    {
        Debug.Log($"Buying product has been failed. {info}");
    }
}
