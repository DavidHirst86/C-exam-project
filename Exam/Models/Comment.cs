using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam.Models
{
	public class Comment
	{
		public int CommentId { get; set; }
		public string Content { get; set; }
		public string AuthorID { get; set; }
		public DateTime Published { get; set; }

		// the postid to link the comments to posts. related to the forign key.
		public int PostId { get; set; }
		public Post Post { get; set; }

		public Comment()
		{
			Published = DateTime.Now;
		}
	}
}