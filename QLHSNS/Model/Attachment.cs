namespace QLHSNS.Model {
	public class Attachment {
		public Guid Id { get; set; }
		public Guid ContractId { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		//public byte[] Content { get; set; }
		//public string FileType { get; set; }
		//public string FileSize { get; set; }
		public DateTime UploadDate { get; set; }
		public Contract Contract { get; set; }
	}
}
