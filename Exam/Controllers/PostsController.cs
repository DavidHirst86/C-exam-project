using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using SignalRChat;

namespace Exam.Models
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

		public object GetImg { get; private set; }

		// GET: Posts
		public ActionResult Index()
		{
			return View(db.Posts.ToList());




			//if (!User.Identity.IsAuthenticated)
			//	return RedirectToAction("Login");

			//ApplicationUser UserID = repository.GetUser(User.Identity.Name);
			//ViewData["Fullname"] = UserID.UserName;



		}

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

		// GET: Posts/Create
		[Microsoft.AspNet.SignalR.Authorize(Roles = "Admin")]
		public ActionResult Create()
        {
            return View(new Post()); //calls the constructor when creating the post. So that the dates set.
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,Title,Content,AuthorID,Published,Expiry,Imagefilename")] Post post, HttpPostedFileBase file)
        {


			/* Uploading images */
			if (file != null)
			{
				string pic = Path.GetFileName(file.FileName);
				string path = Path.Combine(Server.MapPath("/img"), pic);
				// file is uploaded 
				file.SaveAs(path);

				//set attirbute to the path of the image + to save in the database.
				post.Imagefilename = "../img/"+pic;

				/*How to fix this problem https://stackoverflow.com/questions/1206662/the-saveas-method-is-configured-to-require-a-rooted-path-and-the-path-fp-is-n
				we need to cut off the stuff at the start using this method then it'll be happy */
			}
			else
			{
				//display error message here!
			}

			if (ModelState.IsValid) //needs to wrap the whole code otherwise it'll upload images but not save them if its not valid.
			{
				db.Posts.Add(post);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(post);
        }

		/*create comments controller */
		[HttpPost]
		public ActionResult CreateComment([Bind(Include = "PostId,Content,AuthorID")] Comment comment) //model + local variable name
		{
			//now save the data to the database via the comments controller!
			if (ModelState.IsValid) //needs to wrap the whole code otherwise it'll upload images but not save them if its not valid.
			{
				db.Comments.Add(comment);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(comment);
		}


			// GET: Posts/Edit/5
			public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,Title,Content,AuthorID,Published,Expiry,Image")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

		

		/* related to chat: https://github.com/SignalR/SignalR/issues/1214 
		public override System.Threading.Tasks.Task OnConnected()
		{
			// Get UserID. Assumed the user is logged before connecting to chat and userid is saved in session.

			string UserID = HttpContext.Current.User.Identity.Name;

			// Get ChatHistory and call the client function. See below
			this.GetHistory(UserID);

			// Get ConnID
			//string ConnID = Context.ConnectionId;

			// Save them in DB. You got to create a DB class to handle this. (Optional)
			db.SaveChanges(UserID);
		}

		private void GetHistory(UserID)
		{
			// Get Chat History from DB. You got to create a DB class to handle this.
			string History = db.GetChatHistory(UserID);

			// Send Chat History to Client. You got to create chatHistory handler in Client side.
			Clients.Caller.chatHistory(History);
		}

		// This method is to be called by Client 
		public void Chat(string Message)
		{
			// Get UserID. Assumed the user is logged before connecting to chat and userid is saved in session.
			string UserID = (string)HttpContext.Current.Session["userid"];

			// Save Chat in DB. You got to create a DB class to handle this
			db.SaveChat(UserID, Message);

			// Send Chat Message to All connected Clients. You got to create chatMessage handler in Client side.
			Clients.All.chatMessage(Message);
		}


		/*The end*/
	}
}
