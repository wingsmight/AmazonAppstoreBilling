public class MoneyStore
{
    private int moneyCount;

    private static MoneyStore instance;

    private MoneyStore() { }

    public static MoneyStore Instance
    {
        get
        {
            if (instance == null)
                instance = new MoneyStore();

            return instance;
        }
    }

    public int MoneyCount => moneyCount;

    public void Increase(int value)
    {
        moneyCount += value;
    }
}
