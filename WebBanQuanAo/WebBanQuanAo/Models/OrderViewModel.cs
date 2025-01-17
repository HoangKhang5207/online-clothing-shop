﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanQuanAo.Models
{
	public class OrderViewModel
	{
		[Required(ErrorMessage = "Tên khách hàng không được để trống")]
		public string CustomerName { get; set; }
		[Required(ErrorMessage = "Số điện thoại không được để trống")]
		public string Phone { get; set; }
		[Required(ErrorMessage = "Địa chỉ không được để trống")]
		public string Address { get; set; }
		[Required(ErrorMessage = "Email không được để trống")]
		[EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
		public string Email { get; set; }
		public int TypePayment { get; set; }
	}
}