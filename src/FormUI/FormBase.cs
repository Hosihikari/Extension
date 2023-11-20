using System.Collections;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class PropertyChangedEventArgs : EventArgs
{
    private readonly string propertyName;

    public string PropertyName => propertyName;

    public PropertyChangedEventArgs(string propertyName)
    {
        this.propertyName = propertyName;
    }
}

public class FormElementCollection<T> : ICollection<T>, IList<T>
{
    private readonly List<T> list = new();

    public int Count => list.Count;

    public bool IsReadOnly => false;

    public T this[int index]
    {
        get => list[index];
        set
        {
            list[index] = value;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Add(T item)
    {
        list.Add(item);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void Clear()
    {
        list.Clear();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public bool Contains(T item) => list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

    public bool Remove(T item)
    {
        var ret = list.Remove(item);
        Changed?.Invoke(this, EventArgs.Empty);
        return ret;
    }

    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

    public int IndexOf(T item) => list.IndexOf(item);

    public void Insert(int index, T item)
    {
        list.Insert(index, item);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveAt(int index)
    {
        list.RemoveAt(index);
        Changed?.Invoke(this, EventArgs.Empty);
    }

#nullable enable
    public event EventHandler<EventArgs>? Changed;
#nullable disable
}

public abstract class FormBase
{
    private string serializedData;

    [JsonIgnore]
    public string SerializedData
    {
        get
        {
            if (IsSerialized is false)
            {
                serializedData = Serialize();
                IsSerialized = true;
            }
            return serializedData;
        }
    }

    [JsonIgnore]
    public bool IsSerialized { get; internal set; }

    protected abstract string Serialize();

#nullable enable
    public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;

    private bool onHandlingPropertyChanged = false;
    public void OnPropertyChanged(string propertyName = "")
    {
        if (onHandlingPropertyChanged is false)
        {
            onHandlingPropertyChanged = true;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            onHandlingPropertyChanged = false;
        }

    }

    public FormBase()
    {
        PropertyChanged += (obj, args) =>
        {
            IsSerialized = false;
        };
    }
}
