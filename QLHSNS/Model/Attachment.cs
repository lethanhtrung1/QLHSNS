﻿namespace QLHSNS.Model {
	public class Attachment {
		public Guid Id { get; set; }
		public Guid ContractId { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public DateTime UploadDate { get; set; }
	}
}
