using System.Drawing;
using System.Windows.Forms;

namespace Manager.Page
{
    /// <summary>
    /// 设置
    /// </summary>
    public interface IPage
    {
        string Name { get; }

        string Descrption { get; }

        Image Icon { get; }

        IPageItem GetItem(Control host);
    }
}
