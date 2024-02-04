namespace Hosihikari.Minecraft.Extension.FormUI;

public abstract class SimpleFormElement : FormElementBase
{
    public SimpleFormElement() => PropertyChanged += (obj, args) => IsSerialized = false;
}
