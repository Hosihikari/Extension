using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.Minecraft.Extension.FormUI.Element;

public sealed class Button : SimpleFormElement
{
    public class ButtonClickedEventArgs(Player player) : EventArgs
    {
        public Player Player { get; } = player;
    }

    private struct ImageData
    {
        private string _data;

        [JsonPropertyName("type")]
        public string Type { get; private set; }

        [JsonPropertyName("data")]
        public string Data
        {
            get => _data;
            set
            {
                Type = value.Contains("://") ? "url" : "path";
                _data = value;
            }
        }
    }

    private string _text = string.Empty;

    private ImageData _image;

    [JsonPropertyName("text")]
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged(nameof(Text));
        }
    }

    [JsonPropertyName("image")]
    public string Image
    {
        get => _image.Data;
        set
        {
            _image.Data = value;
            OnPropertyChanged(nameof(Image));
        }
    }

    public event EventHandler<ButtonClickedEventArgs>? Clicked;

    internal void InvokeOnClicked(Player player)
        => Clicked?.Invoke(this, new(player));

    protected override string Serialize() => JsonSerializer.Serialize(this);
}
