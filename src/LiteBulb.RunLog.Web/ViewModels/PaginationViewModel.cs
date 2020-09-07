
namespace LiteBulb.RunLog.Web.ViewModels
{
	public class PaginationViewModel
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public long TotalEntityCount { get; set; }
		public long CurrentEntityCount { get; set; }
		public long CurrentEntityIndex { get; set; }
		//public int TotalPageCount { get; set; }
		//public int TotalPageIndex { get; set; }

		public PaginationViewModel()
		{
			PageIndex = 0;
			PageSize = 10;
			TotalEntityCount = 0;
			CurrentEntityCount = 0;
			CurrentEntityIndex = 0;
		}
	}
}
