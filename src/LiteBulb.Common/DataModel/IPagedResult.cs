using System.Collections.Generic;

namespace LiteBulb.Common.DataModel
{
	public interface IPagedResult<T>
	{
		IReadOnlyCollection<T> Data { get; }
		long Total { get; }
	}
}
