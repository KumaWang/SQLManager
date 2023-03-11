namespace Manager.Page
{
    public interface IPageEditor : IPageItem
    {
        PageEditorType EditorType { get; }

        string Name { get; }

        string Descrption { get; }

        object Value { get; set; }
    }

    public interface IPageEditor<T> : IPageEditor
    {
    }
}
