﻿namespace QLHSNS.DTOs.Response {
	public class ApiResponse<T> {
		public T? Data { get; set; }
		public bool IsSuccess { get; set; } = true;
		public string Message { get; set; } = string.Empty;
	}
}
