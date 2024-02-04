using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI.Element;

public sealed class Input(string name) : CustomFormElement(name)
{
    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Input;

    protected override string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

    private string _title = string.Empty;

    private string? _placeHolder;

    private string? _default;

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "input";

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


    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PlaceHolder
    {
        get => _placeHolder;
        set
        {
            _placeHolder = value;
            OnPropertyChanged(nameof(PlaceHolder));
        }
    }

    [JsonPropertyName("default")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Default
    {
        get => _default;
        set
        {
            _default = value;
            OnPropertyChanged(nameof(Default));
        }
    }
}
