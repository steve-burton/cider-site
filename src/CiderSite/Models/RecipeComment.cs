using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiderSite.Models
{
    [Table("RecipeComments")]
    public class RecipeComment
    {
        [Key]
        public int RecipeCommentId { get; set; }
        [Required]
        public string Body { get; set; }
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual ApplicationUser AppUser { get; set; }
    }
}
