using Newtonsoft.Json;
using System;

namespace MyIdeasPool.WebApi.Models
{
	public class IdeaModel
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public int Ease { get; set; }
		public int Confidence { get; set; }

		public int Impact { get; set; }

		[JsonProperty("created_at")]
		public DateTime CreatedAt { get; set; }


		[JsonProperty("average_score")]
		public decimal AverageScore { get; set; }
	}
}
