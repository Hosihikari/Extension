using Hosihikari.Minecraft;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class CustomFormCallbackEventArgs : EventArgs
{
    private readonly Dictionary<string, CustomFormElement> elements;
    private readonly Player player;

    public Dictionary<string, CustomFormElement> Elements => elements;

    public Player Player => player;

    public CustomFormCallbackEventArgs(Dictionary<string, CustomFormElement> elements, Player player)
    {
        this.elements = elements;
        this.player = player;
    }
}

public class CustomForm : FormBase
{
    private FormElementCollection<(string elementName, CustomFormElement element)> elements;

    private string title = string.Empty;

    public CustomForm()
    {
        elements = new();
        elements.Changed += OnCollectionChanged;
    }

    [JsonPropertyName("title")]
    public string Title
    {
        get => title;
        set
        {
            title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    [JsonPropertyName("content")]
    public List<CustomFormElement> SerializedContents
    {
        get
        {
            var contents = new List<CustomFormElement>(elements.Count);
            foreach (var (_, element) in elements)
            {
                contents.Add(element);
            }
            return contents;
        }
        set => throw new NotSupportedException();
    }

    [JsonPropertyName("type")]
    public string Type { get; private set; } = "custom_form";


    private void OnCollectionChanged(object? sender, EventArgs args) => OnPropertyChanged(nameof(Elements));

    [JsonIgnore]
    public FormElementCollection<(string elementName, CustomFormElement element)> Elements
    {
        get => elements;
        set
        {
            elements.Changed -= OnCollectionChanged;
            elements = value;
            elements.Changed += OnCollectionChanged;
            OnPropertyChanged(nameof(Elements));
        }
    }

    public void Append(CustomFormElement element)
        => elements.Add((element.Name, element));

    public void Remove(CustomFormElement element)
        => elements.Remove((element.Name, element));

    public void Remove(string elementName)
    {
        foreach (var pair in elements)
        {
            if (pair.elementName == elementName)
            {
                elements.Remove(pair);
                return;
            }
        };
    }

    public void Remove(int index)
        => elements.RemoveAt(index);

    protected override string Serialize() => JsonSerializer.Serialize(this);

#nullable enable
    public event EventHandler<CustomFormCallbackEventArgs>? Callback;

    internal void InvokeCallback(Dictionary<string, CustomFormElement> elements, Player player)
        => Callback?.Invoke(this, new(elements, player));

    [JsonIgnore]
    internal bool IsNullCallback => Callback is null;
}
