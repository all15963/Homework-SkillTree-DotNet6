using System.ComponentModel.DataAnnotations;

namespace MVCHomework6.Areas.CreateBlog.Models
{
    public class CreateBlogViewModel
    {
        [Required(ErrorMessage = "請填寫{0}")]
        [Display(Name = "標題")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "請填寫{0}")]
        [Display(Name = "內容")]
        public string? Body { get; set; }

        [Required(ErrorMessage = "請上傳{0}")]
        [Display(Name = "封面照")]
        public IFormFile CoverPhoto { get; set; }

        [Required(ErrorMessage = "請選擇{0}")]
        [Display(Name = "創建日期")]
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "請選擇{0}")]
        [Display(Name = "標籤")]
        public string? Tags { get; set; }
    }
}
