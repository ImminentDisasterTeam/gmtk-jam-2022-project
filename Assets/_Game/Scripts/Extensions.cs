using System.Collections.Generic;
using System.Linq;

namespace _Game.Scripts {
    public static class Extensions {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) {
            return enumerable == null || !enumerable.Any();
        }
    }
}