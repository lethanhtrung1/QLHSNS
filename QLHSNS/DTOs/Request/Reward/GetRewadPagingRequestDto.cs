using QLHSNS.DTOs.Pagination;

namespace QLHSNS.DTOs.Request.Reward {
	public class GetRewadPagingRequestDto : PagingRequestBase {
		public int Month { get; set; }
		public int Year {  get; set; }
		public int IsReceived { get; set; }
	}
}
