using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI.Element;

public sealed class Dropdown : CustomFormElement
{
    private int? _default = -1;

    private FormElementCollection<string> _options;

    private string _title = string.Empty;

    public Dropdown(string name) : base(name)
    {
        _options = [];
        _options.Changed += (_, _) => OnPropertyChanged(nameof(Options));
    }

    [JsonIgnore] public override ElementType FormElementType => ElementType.Dropdown;

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "dropdown";

    [JsonPropertyName("text")]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    [JsonPropertyName("options")]
    public FormElementCollection<string> Options
    {
        get => _options;
        set
        {
            _options = value;
            OnPropertyChanged(nameof(Options));
        }
    }

    [JsonPropertyName("default")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Default
    {
        get => _default;
        set
        {
            _default = value;
            OnPropertyChanged(nameof(Default));
        }
    }

    protected override string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }
}