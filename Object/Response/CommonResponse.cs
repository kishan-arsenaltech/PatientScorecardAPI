using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scorecard.API.Model
{
	public class CommonResponse
	{
		[NotMapped]
		public string Message { get; set; }

		[NotMapped]
		public bool Success { get; set; }

		public class KeyValuePair
		{
			public int Key { get; set; }

			public string value { get; set; }
		}
	}
}
