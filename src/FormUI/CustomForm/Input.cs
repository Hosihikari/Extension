using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class Input : CustomFormElement
{
    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Input;

    protected override string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

    private string title = string.Empty;

#nullable enable
    private string? placeHolder;

    private string? @default;
#nullable disable

    public Input(string name) : base(name)
    {
    }

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "input";

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


#nullable enable
    [JsonPropertyName("placeholder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PlaceHolder
    {
        get => placeHolder;
        set
        {
            placeHolder = value;
            OnPropertyChanged(nameof(PlaceHolder));
        }
    }

    [JsonPropertyName("default")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Default
    {
        get => @default;
        set
        {
            @default = value;
            OnPropertyChanged(nameof(Default));
        }
    }
#nullable disable
}
