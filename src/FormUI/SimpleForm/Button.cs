using Hosihikari.Minecraft;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hosihikari.FormUI;

public class Button : SimpleFormElement
{
    public class ButtonClickedEventArgs : EventArgs
    {
        private readonly Player player;

        public Player Player => player;

        public ButtonClickedEventArgs(Player player)
        {
            this.player = player;
        }
    }

    internal struct ImageData
    {
        private string data;

        [JsonPropertyName("type")]
        public string Type { get; private set; }

        [JsonPropertyName("data")]
        public string Data
        {
            get => data;
            set
            {
                Type = value.Contains("textures/") ? "path" : "url";
                data = value;
            }
        }
    }

    private string text = string.Empty;

    private ImageData image;

    [JsonPropertyName("text")]
    public string Text
    {
        get => text;
        set
        {
            text = value;
            OnPropertyChanged(nameof(Text));
        }
    }

    [JsonPropertyName("image")]
    public string Image
    {
        get => image.Data;
        set
        {
            image.Data = value;
            OnPropertyChanged(nameof(Image));
        }
    }

#nullable enable
    public event EventHandler<ButtonClickedEventArgs>? Clicked;
#nullable disable

    internal void InvokeOnClicked(Player player)
        => Clicked?.Invoke(this, new(player));

    protected override string Serialize() => JsonSerializer.Serialize(this);
}
