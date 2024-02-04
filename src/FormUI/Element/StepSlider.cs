using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI.Element;

public sealed class StepSlider : CustomFormElement
{
    [JsonIgnore]
    public override ElementType FormElementType => ElementType.Slider;

    protected override string Serialize() => JsonSerializer.Serialize(this);

    private string _title = string.Empty;

    private FormElementCollection<string> _options;

    private int _default = -1;

    public StepSlider(string name) : base(name)
    {
        _options = [];
        _options.Changed += (_, _) => OnPropertyChanged(nameof(Options));
    }

    //[JsonPropertyName("type")]
    //public string Type { get; private set; } = "step_slider";

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

    [JsonPropertyName("steps")]
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
    public int Default
    {
        get => _default;
        set
        {
            _default = value;

            if (_default < 0 || _default >= Options.Count)
                _default = 0;

            OnPropertyChanged(nameof(Default));
        }
    }
}
