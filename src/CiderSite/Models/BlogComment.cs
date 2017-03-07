using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiderSite.Models
{
    [Table("BlogComments")]
    public class BlogComment
    {
        [Key]
        public int BlogCommentId { get; set; }
        public string Body { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual ApplicationUser AppUser { get; set; }
    }
}
