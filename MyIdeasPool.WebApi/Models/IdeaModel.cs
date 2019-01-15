using MyIdeasPool.WebApi.Helpers;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyIdeasPool.WebApi.Models
{
	public class IdeaModel
	{
		public Guid Id { get; set; }
		[Required]
		public string Content { get; set; }
		[Range(0, 10)]
		public int Ease { get; set; }
		[Range(0, 10)]
		public int Confidence { get; set; }
		[Range(0, 10)]
		public int Impact { get; set; }

		[JsonProperty(PropertyName = "created_at")]
		[JsonConverter(typeof(CustomJsDateTimeConverter))]
		public DateTime CreatedAt { get; set; }


		[JsonProperty("average_score")]
		public double AverageScore { get; set; }
	}
}
