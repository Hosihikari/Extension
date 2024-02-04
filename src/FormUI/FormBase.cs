using System.Collections;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI;

public sealed class PropertyChangedEventArgs : EventArgs
{
    internal PropertyChangedEventArgs(string propertyName)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; }
}

public sealed class FormElementCollection<T> : IList<T>
{
    private readonly List<T> _list = [];

    public int Count => _list.Count;

    public bool IsReadOnly => false;

    public T this[int index]
    {
        get => _list[index];
        set
        {
            _list[index] = value;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Add(T item)
    {
        _list.Add(item);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void Clear()
    {
        _list.Clear();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public bool Contains(T item) => _list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    public bool Remove(T item)
    {
        bool ret = _list.Remove(item);
        Changed?.Invoke(this, EventArgs.Empty);
        return ret;
    }

    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    public int IndexOf(T item) => _list.IndexOf(item);

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<EventArgs>? Changed;
}

public abstract class FormBase
{
    private string _serializedData = string.Empty;

    [JsonIgnore]
    public string SerializedData
    {
        get
        {
            if (IsSerialized)
            {
                return _serializedData;
            }

            _serializedData = Serialize();
            IsSerialized = true;
            return _serializedData;
        }
    }

    [JsonIgnore]
    public bool IsSerialized { get; internal set; }

    protected abstract string Serialize();

    public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;

    private bool _onHandlingPropertyChanged;
    public void OnPropertyChanged(string propertyName = "")
    {
        if (_onHandlingPropertyChanged)
        {
            return;
        }

        _onHandlingPropertyChanged = true;

        PropertyChanged?.Invoke(this, new(propertyName));

        _onHandlingPropertyChanged = false;

    }

    public FormBase()
    {
        PropertyChanged += (_, _) =>
        {
            IsSerialized = false;
        };
    }
}
