using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace APIMe.Utilities.JsonHelper
{
    public class JsonPatcher
    {
        public JsonElement ApplyPatch(object originalRecord, JsonElement patchDocument)
        {
            var originalJson = JObject.FromObject(originalRecord);
            var patchJson = JObject.Parse(patchDocument.GetRawText());

            originalJson.Merge(patchJson, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Concat,
                MergeNullValueHandling = MergeNullValueHandling.Merge
            });

            var jsonElement = JsonDocument.Parse(originalJson.ToString()).RootElement;
            return jsonElement;
        }
    }
}
