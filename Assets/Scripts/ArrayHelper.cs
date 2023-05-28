using UnityEngine;

public class ArrayHelper
{
    public static T[,] ResizeArray<T>(T[,] original, int rows, int cols)
    {
        T[,] newArray = new T[rows,cols];
        int minRows = Mathf.Min(rows, original.GetLength(0));
        int minCols = Mathf.Min(cols, original.GetLength(1));
        for (int i = 0; i < minRows; i++)
        {
            for (int j = 0; j < minCols; j++)
            {
                newArray[i, j] = original[i, j];
            }
        }

        return newArray;
    }

    public static T[,] InsertRowIntoArray<T>(T[,] original, int row)
    {
        T[,] newArray = new T[original.GetLength(0), original.GetLength(1) + 1];

        for (int i = 0; i < newArray.GetLength(0); i++)
        {
            for (int j = 0, k = 0; j < newArray.GetLength(1); j++)
            {
                if (j != row)
                {
                    newArray[i, j] = original[i, k];
                    k++;
                }
            }
        }

        return newArray;
    }

    public static T[,] RemoveRowFromArray<T>(T[,] original, int row)
    {
        T[,] newArray = new T[original.GetLength(0), original.GetLength(1) - 1];

        for (int i = 0; i < original.GetLength(0); i++)
        {
            for (int j = 0, k = 0; j < original.GetLength(1); j++)
            {
                if (j != row)
                {
                    newArray[i, k] = original[i, j];
                    k++;
                }
            }
        }

        return newArray;
    }
    
    public static T[,] RemoveColumnFromArray<T>(T[,] original, int column)
    {
        T[,] newArray = new T[original.GetLength(0) - 1, original.GetLength(1)];

        for (int i = 0, k = 0; i < original.GetLength(0); i++)
        {
            if (i == column)
            {
                continue;
            }
            
            for (int j = 0; j < original.GetLength(1); j++)
            {
                newArray[k, j] = original[i, j];
            }

            k++;
        }

        return newArray;
    }

    public static void PrintArray<T>(T[,] array)
    {
        string result = "";
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                result += array[i, j] + " ";
            }

            result += "\n";
        }
        Debug.Log(result);
    }
}