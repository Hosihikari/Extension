using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class Dropdown : CustomFormElement
{
    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Dropdown;

    protected override string Serialize() => JsonSerializer.Serialize(this);

    private string title = string.Empty;

    private FormElementCollection<string> options;

    private int? @default = -1;

    public Dropdown(string name) : base(name)
    {
        options = new();
        options.Changed += (_, _) => OnPropertyChanged(nameof(Options));
    }

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "dropdown";

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

    [JsonPropertyName("options")]
    public FormElementCollection<string> Options
    {
        get => options;
        set
        {
            options = value;
            OnPropertyChanged(nameof(Options));
        }
    }

    [JsonPropertyName("default")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Default
    {
        get => @default;
        set
        {
            @default = value;
            OnPropertyChanged(nameof(Default));
        }
    }
}
