using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam.Models
{
	public class Post
	{
			
			public int PostId { get; set; }
			public string Title { get; set; }
			public string Content { get; set; }
			public string AuthorID { get; set; } //applicationuser to get user automatically. Creates bugs though.
			public DateTime Published { get; set; }
			public DateTime Expiry { get; set; }
			public string Imagefilename { get; set; }


		//creating the relationship between models (the database)
		public virtual ICollection<Comment> Comment { get; set; }
			public virtual ICollection<Category> Category { get; set; }

			public Post() //constructor that auto fills date.
			{
				Comment = new List<Comment>();
				Published = DateTime.Now;
				Expiry = Published.AddDays(5);
			}

	}
}