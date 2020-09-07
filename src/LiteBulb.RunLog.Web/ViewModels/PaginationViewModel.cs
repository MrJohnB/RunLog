
namespace LiteBulb.RunLog.Web.ViewModels
{
	public class PaginationViewModel
	{
		public int PageOffset { get; set; }
		public int PageSize { get; set; }
		public long TotalEntityCount { get; set; }
		public long CurrentEntityCount { get; set; }
		public long CurrentEntityIndex { get; set; }
		//public int TotalPageCount { get; set; }
		//public int CurrentPageIndex { get; set; }

		public PaginationViewModel()
		{
			PageOffset = 0;
			PageSize = 10;
			TotalEntityCount = 0;
			CurrentEntityCount = 0;
			CurrentEntityIndex = 0;
		}
	}
}
