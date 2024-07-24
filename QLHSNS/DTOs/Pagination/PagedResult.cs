namespace QLHSNS.DTOs.Pagination {
	public class PagedResult<T> : PagedResultBase {
		public IList<T> Items { get; set; } = new List<T>();
		public PagedResult(IList<T> items, int count, int pageNumber, int pageSize) {
			TotalRecord = count;
			Items = items;
			PageSize = pageSize;
			PageNumber = pageNumber;
		}
	}
}
