using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AllegroCategories;

public class CategoriesResponse
{

    [JsonPropertyName("categories")]
    public List<Category> Categories { get; set; }
}
public class Options
{
    [JsonPropertyName("variantsByColorPatternAllowed")]
    public bool VariantsByColorPatternAllowed { get; set; }

    [JsonPropertyName("advertisement")]
    public bool Advertisement { get; set; }

    [JsonPropertyName("advertisementPriceOptional")]
    public bool AdvertisementPriceOptional { get; set; }

    [JsonPropertyName("offersWithProductPublicationEnabled")]
    public bool OffersWithProductPublicationEnabled { get; set; }

    [JsonPropertyName("productCreationEnabled")]
    public bool ProductCreationEnabled { get; set; }

    [JsonPropertyName("customParametersEnabled")]
    public bool CustomParametersEnabled { get; set; }
}

public class Category
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("parent")]
    public Parent Parent { get; set; }

    [JsonPropertyName("leaf")]
    public bool Leaf { get; set; }

    [JsonPropertyName("options")]
    public Options Options { get; set; }
}
public class Parent
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}