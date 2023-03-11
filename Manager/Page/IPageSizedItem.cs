using System.Drawing;

namespace Manager.Page
{
    public interface IPageSizedItem 
    {
        Size ComputeSize(Size maxSize);
    }
}
