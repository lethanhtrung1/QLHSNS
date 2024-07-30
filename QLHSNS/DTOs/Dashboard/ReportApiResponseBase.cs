namespace QLHSNS.DTOs.Dashboard {
	public class ReportApiResponseBase {
		public string? NameChart {  get; set; }
		public string? Total {  get; set; }
		public List<ReportData> Data { get; set; }
	}

	public class ReportData {
		public int? ValueOrder { get; set; }
		public string? FieldName { get; set; }
		public string? Value { get; set; }
		public string? Rate { get; set; }
	}
}
