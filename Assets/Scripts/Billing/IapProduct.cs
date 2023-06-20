using UnityEngine.Purchasing;

public struct IapProduct
{
    private string id;
    private ProductType type;

    public IapProduct(string id, ProductType type)
    {
        this.id = id;
        this.type = type;
    }

    public string Id => id;
    public ProductType Type => type;
}

