using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiderSite.Models
{
    [Table("Blogs")]
    public class Blog
    {
        public Blog()
        {
        }

        [Key]
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string BodyCopy { get; set; }
        public byte[] Data { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Blog(string author, string title, string intro, string bodyCopy, byte[] photoData)
        {
            Author = author;
            Title = title;
            Intro = intro;
            BodyCopy = bodyCopy;
            Data = photoData;
        }

        //public string getImage()
        //{
        //    var base64File = Convert.ToBase64String(Data);
        //    return String.Format("data:image/gif;base64,{0}", base64File);
        //}
    }
}
