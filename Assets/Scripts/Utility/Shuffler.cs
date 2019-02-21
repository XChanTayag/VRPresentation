using System.Collections;

public static class Shuffler
{
    public static void Shuffle<T> (T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}