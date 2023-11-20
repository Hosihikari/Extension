using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class Label : CustomFormElement
{
    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Label;

    protected override string Serialize() => JsonSerializer.Serialize(this);

    private string text = string.Empty;

    public Label(string name) : base(name)
    {
    }

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "label";

    [JsonPropertyName("text")]
    public string Text
    {
        get => text;
        set
        {
            text = value;
            OnPropertyChanged(nameof(Text));
        }
    }
}
