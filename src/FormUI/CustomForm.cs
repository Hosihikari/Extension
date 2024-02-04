using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI;

public sealed class CustomFormCallbackEventArgs(Dictionary<string, CustomFormElement> elements, Player player)
    : EventArgs
{
    public Dictionary<string, CustomFormElement> Elements { get; } = elements;

    public Player Player { get; } = player;
}

public sealed class CustomForm : FormBase
{
    private FormElementCollection<(string elementName, CustomFormElement element)> _elements;

    private string _title = string.Empty;

    public CustomForm()
    {
        _elements = [];
        _elements.Changed += OnCollectionChanged;
    }

    [JsonPropertyName("title")]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    [JsonPropertyName("content")]
    public List<CustomFormElement> SerializedContents
    {
        get
        {
            List<CustomFormElement> contents = new(_elements.Count);
            foreach ((_, CustomFormElement element) in _elements)
            {
                contents.Add(element);
            }

            return contents;
        }
        set => throw new NotSupportedException();
    }

    [JsonPropertyName("type")] public string Type { get; private set; } = "custom_form";

    [JsonIgnore]
    public FormElementCollection<(string elementName, CustomFormElement element)> Elements
    {
        get => _elements;
        set
        {
            _elements.Changed -= OnCollectionChanged;
            _elements = value;
            _elements.Changed += OnCollectionChanged;
            OnPropertyChanged(nameof(Elements));
        }
    }

    [JsonIgnore] internal bool IsNullCallback => Callback is null;


    private void OnCollectionChanged(object? sender, EventArgs args)
    {
        OnPropertyChanged(nameof(Elements));
    }

    public void Append(CustomFormElement element)
    {
        _elements.Add((element.Name, element));
    }

    public void Remove(CustomFormElement element)
    {
        _elements.Remove((element.Name, element));
    }

    public void Remove(string elementName)
    {
        foreach ((string elementName, CustomFormElement element) pair in _elements)
        {
            if (pair.elementName != elementName)
            {
                continue;
            }

            _elements.Remove(pair);
            return;
        }
    }

    public void Remove(int index)
    {
        _elements.RemoveAt(index);
    }

    protected override string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

    public event EventHandler<CustomFormCallbackEventArgs>? Callback;

    internal void InvokeCallback(Dictionary<string, CustomFormElement> elements, Player player)
    {
        Callback?.Invoke(this, new(elements, player));
    }
}