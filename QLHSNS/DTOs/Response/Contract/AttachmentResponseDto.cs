namespace QLHSNS.DTOs.Response.Contract {
	public class AttachmentResponseDto {
		public string FileType { get; set; }
		public byte[] ArchiveData { get; set; }
		public string ArchiveName { get; set; }
	}
}
