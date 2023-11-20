namespace Hosihikari.FormUI;

public abstract class SimpleFormElement : FormElementBase
{
    public SimpleFormElement() => PropertyChanged += (obj, args) => IsSerialized = false;
}
