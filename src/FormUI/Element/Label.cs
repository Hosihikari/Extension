using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI.Element;

public sealed class Label(string name) : CustomFormElement(name)
{
    private string _text = string.Empty;

    [JsonIgnore] public override ElementType FormElementType => ElementType.Label;

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "label";

    [JsonPropertyName("text")]
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged(nameof(Text));
        }
    }

    protected override string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }
}