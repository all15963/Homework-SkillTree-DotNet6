using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCHomework6.Data.Database
{
    public partial class Articles
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        [StringLength(250)]
        public string CoverPhoto { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public string Tags { get; set; }
    }
}
