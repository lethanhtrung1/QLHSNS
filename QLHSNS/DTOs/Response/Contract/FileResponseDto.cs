namespace QLHSNS.DTOs.Response.Contract {
	public class FileResponseDto {
		public string FileType { get; set; }
		public byte[] ArchiveData { get; set; }
		public string ArchiveName { get; set; }
	}
}
