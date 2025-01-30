
namespace Scorecard.API.Model
{
	public class CommonSelectRequest : CheckUserRequest
	{
		public string ModuleName { get; set; }
		public int LoggedInUserId { get; set; }
		public int IsSuperAdmin { get; set; }
		public string Search { get; set; }
		public string OrderByFieldParam { get; set; }
		public string SortOrderParam { get; set; }
		public int PageSize { get; set; }
		public int RowsToSkip { get; set; }
	}
}
