using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI.Element;

public sealed class Toggle(string name) : CustomFormElement(name)
{
    private bool? _default = false;

    private string _title = string.Empty;

    [JsonIgnore] public override ElementType FormElementType => ElementType.Toggle;

    [JsonPropertyName("type")] public string Type { get; private set; } = "toggle";

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

    [JsonPropertyName("default")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Default
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