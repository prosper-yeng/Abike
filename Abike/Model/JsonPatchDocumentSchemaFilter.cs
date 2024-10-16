using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class JsonPatchDocumentSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(Microsoft.AspNetCore.JsonPatch.JsonPatchDocument))
        {
            schema.Properties.Clear();
            schema.Properties.Add("op", new OpenApiSchema { Type = "string", Description = "Operation type (replace, add, remove, etc.)" });
            schema.Properties.Add("path", new OpenApiSchema { Type = "string", Description = "Property path (e.g. /name, /phoneNumber)" });
            schema.Properties.Add("value", new OpenApiSchema { Type = "object", Description = "Value to apply in the operation" });
            schema.Required.Clear();
            schema.Required.Add("op");
            schema.Required.Add("path");
        }
    }
}
