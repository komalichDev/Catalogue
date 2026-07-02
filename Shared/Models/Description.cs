using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Models;

public struct Description
{
    public int Id { get; set; }

    [JsonPropertyName("shortText")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("longText")]
    public string LongDescription { get; set; }

    public int Weight { get; set; }
}
