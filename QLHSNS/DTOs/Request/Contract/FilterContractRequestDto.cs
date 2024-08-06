using QLHSNS.DTOs.Pagination;

namespace QLHSNS.DTOs.Request.Contract {
	public class FilterContractRequestDto : PagingRequestBase {
		//public string? Keyword { get; set; }
		public Guid? ContractTypeId { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int IsDeleted { get; set; }

		public string? SortField { get; set; }
		public int SortOrder { get; set; }
	}
}
