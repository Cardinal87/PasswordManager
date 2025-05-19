using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Interfaces;

namespace Services;

internal class WritableOptions<T> : IWritableOptions<T> where T : class, new()
{
    private readonly IOptions<T> _options;
    private readonly string _file;
    private readonly string _section;

    public WritableOptions(IOptions<T> options, string file, string section)
    {
        _options = options;
        _file = file;
        _section = section;
        
    }

    public T Value => _options.Value;

    public void Update(Action<T> applyChanges)
    {
        string path = Path.Combine(AppContext.BaseDirectory, _file);
        var jObj = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path)) ?? throw new FileNotFoundException($"{_file} by path {path} was not found");
        var sectionObg = Value ?? new T();
        applyChanges(sectionObg);
        jObj[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObg));
        string json = JsonConvert.SerializeObject(jObj, Formatting.Indented);
        File.WriteAllText(path, json);
    }
}
