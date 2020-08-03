using System.Collections.Generic;

namespace LiteBulb.Common.DataModel
{
	public class PagedResult<T> : IPagedResult<T>
	{
		public IReadOnlyCollection<T> Data { get; set; }
		public long Total { get; set; }
	}
}
