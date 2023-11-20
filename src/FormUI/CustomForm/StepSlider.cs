using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class StepSlider : CustomFormElement
{
    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Slider;

    protected override string Serialize() => JsonSerializer.Serialize(this);

    private string title = string.Empty;

    private FormElementCollection<string> options;

    private int @default = -1;

    public StepSlider(string name) : base(name)
    {
        options = new();
        options.Changed += (_, _) => OnPropertyChanged(nameof(Options));
    }

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "step_slider";

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

    [JsonPropertyName("steps")]
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
    public int Default
    {
        get => @default;
        set
        {
            @default = value;

            if (@default < 0 || @default >= Options.Count)
                @default = 0;

            OnPropertyChanged(nameof(Default));
        }
    }
}
