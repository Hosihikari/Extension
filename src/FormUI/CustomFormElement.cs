using Hosihikari.Minecraft.Extension.FormUI.Element;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Dropdown), "dropdown")]
[JsonDerivedType(typeof(Input), "input")]
[JsonDerivedType(typeof(Label), "label")]
[JsonDerivedType(typeof(Slider), "slider")]
[JsonDerivedType(typeof(StepSlider), "step_slider")]
[JsonDerivedType(typeof(Toggle), "toggle")]
public abstract class CustomFormElement : FormElementBase
{
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


    public CustomFormElement(string name)
    {
        Name = name;

        PropertyChanged += (_, args) =>
        {
            switch (args.PropertyName)
            {
                case "Value": break;
                default:
                    IsSerialized = false;
                    break;
            }
        };
    }

    [JsonIgnore] public string Name { get; set; }

    [JsonIgnore] public string Value { get; set; } = string.Empty;

    [JsonIgnore] public abstract ElementType FormElementType { get; }

    public T GetValue<T>() where T : IParsable<T>, new()
    {
        return T.TryParse(Value, null, out T? result) ? result : new();
    }

    public event EventHandler<EventArgs>? ValueChanged;

    internal void InvokeValueChanged()
    {
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }
}