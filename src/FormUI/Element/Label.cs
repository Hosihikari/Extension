using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI.Element;

public sealed class Label(string name) : CustomFormElement(name)
{
    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Label;

    protected override string Serialize() => JsonSerializer.Serialize(this);

    private string _text = string.Empty;

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
}
