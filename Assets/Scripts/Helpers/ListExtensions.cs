using System.Collections;

public static class ListExtensions
{
    public static bool HasDuplicates(this IList list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            var item = list[i];
            for (int j = i + 1; j < list.Count; j++)
            {
                if (item.Equals(list[j]))
                    return true;
            }
        }

        return false;
    }

    public static T RemoveDuplicates<T>(this T list) where T : IList
    {
        var finalList = list;
		for (int i = 0; i < list.Count - 1; i++)
		{
			var item = list[i];
			for (int j = i + 1; j < list.Count; j++)
			{
				if (item.Equals(list[j]))
					finalList.RemoveAt(j);
			}
		}

        return finalList;
	}

    public static bool Matches<T>(this T list1, T list2) where T : IList
    {
        if (list1 == null)
            return list2 == null;

        if (list1.Count != list2.Count)
            return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if (!list1[i].Equals(list2[i]))
                return false;
        }

        return true;
    }
}
