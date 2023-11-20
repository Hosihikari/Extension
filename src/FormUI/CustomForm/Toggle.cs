using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class Toggle : CustomFormElement
{

    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Toggle;

    protected override string Serialize() => JsonSerializer.Serialize(this);

    private string title = string.Empty;

    private bool? @default = false;

    public Toggle(string name) : base(name)
    {
    }

    [JsonPropertyName("type")]
    public string Type { get; private set; } = "toggle";

    [JsonPropertyName("text")]
    public string Title
    {
        get => title;
        set
        {
            title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    [JsonPropertyName("default")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Default
    {
        get => @default;
        set
        {
            @default = value;
            OnPropertyChanged(nameof(Default));
        }
    }
}
