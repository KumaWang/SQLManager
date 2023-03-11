using System.Collections.Generic;

namespace Manager.Page
{
    public interface IPageListEditor<T> : IPageEditor<List<T>>
    {
        int FixedCount { get; }

        bool CanAdd { get; }

        bool CanRemove { get; }
    }
}
