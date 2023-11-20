using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Dropdown), "dropdown")]
[JsonDerivedType(typeof(Input), "input")]
[JsonDerivedType(typeof(Label), "label")]
[JsonDerivedType(typeof(Slider), "slider")]
[JsonDerivedType(typeof(StepSlider), "step_slider")]
[JsonDerivedType(typeof(Toggle), "toggle")]
public abstract class CustomFormElement : FormElementBase
{
    [JsonIgnore]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public string Value { get; set; } = string.Empty;

    [Flags]
    public enum ElementType
    {
        Label,
        Input,
        Toggle,
        Dropdown,
        Slider,
        StepSlider
    }

    [JsonIgnore]
    public abstract ElementType FormElementType { get; }

    public T GetValue<T>() where T : IParsable<T>, new()
    {
        if (T.TryParse(Value, null, out var result))
        {
            return result;
        }
        return new();
    }


    public CustomFormElement(string name)
    {
        Name = name;

        PropertyChanged += (obj, args) =>
        {
            switch (args.PropertyName)
            {
                case "Value": break;
                default: IsSerialized = false; break;
            }
        };
    }

#nullable enable
    public event EventHandler<EventArgs>? ValueChanged;

    internal void InvokeValueChanged()
        => ValueChanged?.Invoke(this, EventArgs.Empty);
}
