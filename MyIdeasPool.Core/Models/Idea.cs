using System;

namespace MyIdeasPool.Core.Models
{
	public class Idea
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public int Ease { get; set; }
		public int Confidence { get; set; }
		public int Impact { get; set; }
		public DateTime CreatedAt { get; set; }

		public decimal AverageScore
		{
			get
			{
				return (Confidence + Ease + Impact) / 3;
			}
		}
	}
}
