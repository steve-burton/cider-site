using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CiderSite.Models
{
    [Table("Recipes")]
    public class Recipe
    {
        public Recipe()
        {
        }

        [Key]
        public int RecipeId { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Ingredients { get; set; }
        [Required]
        public string Directions { get; set; }
        public string Notes { get; set; }
        //public virtual ICollection<RecipeComment> RecipeComments { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Recipe(string author, string title, string ingredients, string directions, string notes)
        {
            Author = author;
            Title = title;
            Ingredients = ingredients;
            Directions = directions;
            Notes = notes;
        }
    }
}
