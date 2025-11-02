
using System.Text.Json;
using PiskelBuild;

if (args.Length < 2) {
    Console.WriteLine("Please provide two arguments");
    return;
}

string inputDirectory = args[0];
string outputDirectory = args[1];

string contentFilePath = outputDirectory + "/Content.mgcb";

File.Delete(contentFilePath);

using FileStream contentFile = File.OpenWrite(contentFilePath);

using StreamWriter contentFileWriter = new StreamWriter(contentFile);

contentFileWriter.WriteLine("""
/outputDir:bin/$(Platform)
/intermediateDir:obj/$(Platform)
/platform:DesktopGL
/config:
/profile:Reach
/compress:False

""");

string[] files = Directory.GetFiles(inputDirectory,"*.piskel",SearchOption.AllDirectories);

foreach (string filePath in files) {
    Console.WriteLine("Converting piskel " + filePath);

    string inputContent = File.ReadAllText(filePath);

    JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    options.Converters.Add(new LayerParser());
    PiskelFile piskel = JsonSerializer.Deserialize<PiskelFile>(inputContent, options);

    string dataUrl = piskel.Piskel.Layers[0].Chunks[0].Base64PNG;
    string base64 = dataUrl[(dataUrl.IndexOf(',') + 1)..];

    string outputFile = filePath[..filePath.LastIndexOf('.')].Replace(inputDirectory, outputDirectory) + ".png";

    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));

    File.WriteAllBytes(outputFile, Convert.FromBase64String(base64));

    contentFileWriter.WriteLine($"""
    /importer:TextureImporter
    /processor:TextureProcessor
    /processorParam:ColorKeyColor=255,0,255,255
    /processorParam:ColorKeyEnabled=True
    /processorParam:GenerateMipmaps=False
    /processorParam:PremultiplyAlpha=True
    /processorParam:ResizeToPowerOfTwo=False
    /processorParam:MakeSquare=False
    /processorParam:TextureFormat=Color
    /build:{outputFile[(outputFile.IndexOf(outputDirectory) + outputDirectory.Length + 1)..].Replace("\\","/")}

    """);
}

Console.WriteLine("All piskel files converted");