using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PiskelBuild;

public class PiskelFile {
    public int ModelVersion { get; set; }

    public Piskel Piskel { get; set; }
}

public class Piskel {
    public string Name { get; set; }
    public string Description { get; set; }

    public int Fps { get; set; }

    public int Height { get; set; }
    public int Width { get; set; }

    public List<Layer> Layers { get; set; }
}

public class Layer {
    public string Name { get; set; }
    public int Opacity { get; set; }
    public int FrameCount { get; set; }
    public List<Chunk> Chunks { get; set; }
}

class LayerParser : JsonConverter<Layer> {
    public override Layer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        JsonSerializerOptions newOptions = new(options);
        newOptions.Converters.Remove(this);
        return JsonSerializer.Deserialize<Layer>(reader.GetString(), newOptions);
    }

    public override void Write(Utf8JsonWriter writer, Layer value, JsonSerializerOptions options) {
        throw new NotImplementedException();
    }
}

public class Chunk {
    public List<List<int>> Layout { get; set; }
    public string Base64PNG { get; set; }
}