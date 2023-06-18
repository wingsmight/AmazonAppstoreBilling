using UnityEngine;
using TMPro;

public class MoneyCountView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textView;

    private void Update()
    {
        textView.text = $"Монеты: {MoneyStore.Instance.MoneyCount}";
    }
}
