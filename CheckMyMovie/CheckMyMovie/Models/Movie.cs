using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CheckMyMovie.Models
{
    public class Movie
    {
        [JsonProperty(PropertyName = "imdbID")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Plot")]
        public string Plot { get; set; }
        [JsonProperty(PropertyName = "Poster")]
        public string PosterUrl { get; set; }
        [JsonProperty(PropertyName = "imdbRating")]
        public double Rating { get; set; }
        [JsonProperty(PropertyName = "RunTime")]
        public string RunTime { get; set; }
        [JsonProperty(PropertyName = "Checked")]
        public bool Checked { get; set; } = false;
    }
}
