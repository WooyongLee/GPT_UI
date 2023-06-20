using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace GPT_UI
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        static HttpClient _httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();

            this.Activated += MainWindow_Activated;
        }

        private async void MainWindow_Activated(object sender, EventArgs e)
        {
            await CallOpenAI();
        }

        public static async Task CallOpenAI()
        {
              string modelUrl = "https://api.openai.com/v1/models";

        ModelList? models =  await _httpClient.GetFromJsonAsync<ModelList>(modelUrl, SourceGenerationContext.Default.ModelList);
        if (models == null)
        {
            return;
        }

        foreach (Model model in models.Data)
        {
            Console.WriteLine($"{model.Id}");
        }
          
        }
    }

#if DEBUG
    [JsonSourceGenerationOptions(WriteIndented = true)]
#endif
    [JsonSerializable(typeof(ModelList))]
    [JsonSerializable(typeof(Model))]
    [JsonSerializable(typeof(Permission))]
    internal partial class SourceGenerationContext : JsonSerializerContext // .NET 6부터 SourceGenerator와 통합된 System.Text.Json
    {
        public SourceGenerationContext(JsonSerializerOptions options) : base(options)
        {
        }

        protected override JsonSerializerOptions GeneratedSerializerOptions => throw new NotImplementedException();

        public override JsonTypeInfo GetTypeInfo(Type type)
        {
            throw new NotImplementedException();
        }
    }

    public class ModelList
    {
        [JsonPropertyName("object")]
        public string Object { get; set; } = "";

        [JsonPropertyName("data")]
        public List<Model> Data { get; set; } = new List<Model>(); // System.Text.Json의 역직렬화 시 필드/속성 주의

        public override string ToString()
        {
            return $"{Object}: # of data: {Data.Count}";
        }
    }

    public class Model
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("object")]
        public string Object { get; set; } = "";

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("owned_by")]
        public string OwnedBy { get; set; } = "";

        [JsonPropertyName("permission")]
        public List<Permission> Permissions { get; set; } = new List<Permission>();

        [JsonPropertyName("root")]
        public string Root { get; set; } = "";

        [JsonPropertyName("parent")]
        public string Parent { get; set; } = "";

        public override string ToString()
        {
            return $"{Id}, # of permissions: {Permissions.Count}";
        }
    }

    public class Permission
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("object")]
        public string Object { get; set; } = "";

        [JsonPropertyName("created")]
        public int Created { get; set; }

        [JsonPropertyName("allow_create_engine")]
        public bool AllowCreateEngine { get; set; }

        [JsonPropertyName("allow_sampling")]
        public bool AllowSampling { get; set; }

        [JsonPropertyName("allow_logprobs")]
        public bool AllowLogprobs { get; set; }

        [JsonPropertyName("allow_search_indices")]
        public bool AllowSearchIndices { get; set; }

        [JsonPropertyName("allow_view")]
        public bool AllowView { get; set; }

        [JsonPropertyName("allow_fine_tuning")]
        public bool AllowFineTuning { get; set; }

        [JsonPropertyName("organization")]
        public string Organization { get; set; } = "";

        [JsonPropertyName("group")]
        public string Group { get; set; } = "";

        [JsonPropertyName("is_blocking")]
        public bool IsBlocking { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Object}";
        }
    }
}
