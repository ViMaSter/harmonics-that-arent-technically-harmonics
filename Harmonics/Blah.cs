public class Blah
{
    private static void Swap(ref decimal a, ref decimal b)
    {
        if (a == b) return;

        var temp = a;
        a = b;
        b = temp;
    }

    public static void GetPer(decimal[] list)
    {
        int x = list.Length - 1;
        GetPer(list, 0, x);
    }

    public static List<List<Decimal>> a = new List<List<decimal>>(); 
    private static void GetPer(decimal[] list, int k, int m)
    {
        if (k == m)
        {
            a.Add(list.ToList());
        }
        else
            for (int i = k; i <= m; i++)
            {
                Swap(ref list[k], ref list[i]);
                GetPer(list, k + 1, m);
                Swap(ref list[k], ref list[i]);
            }
    }
}