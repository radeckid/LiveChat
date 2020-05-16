using System.Collections.Generic;

namespace LiveChatRegisterLogin.Helpers
{
    public static class IListExtension
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> newRange)
        {
            foreach(T element in newRange)
            {
                list.Add(element);
            }
        }
    }
}
