using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TaxiService.Interfaces;
using TaxiService.Models;

namespace TaxiService.Repository
{
    public class CommentRepo : ICommentRepo
    {
        private string fileName = HttpContext.Current.Server.MapPath("~/App_Data/Comments.xml");
        public void AddNewComment(Comment comment)
        {
            if (!File.Exists(fileName))
            {
                XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),

                new XElement("Comments",
                new XElement("Comment", new XAttribute("Id", comment.CommentId),
                new XElement("CommentId", comment.CommentId),
                new XElement("CreatedById", comment.CreatedBy.Id),
                new XElement("CreatedOn", comment.CreatedOn),
                new XElement("Description", comment.Description),
                new XElement("Grade", comment.Grade))
                ));

                xmlDocument.Save(fileName);
            }
            else
            {
                try
                {
                    FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    XDocument doc = XDocument.Load(stream);
                    XElement customers = doc.Element("Comments");
                    customers.Add(new XElement("Comment", new XAttribute("Id", comment.CommentId),
                                                          new XElement("CommentId", comment.CommentId),
                                                          new XElement("CreatedById", comment.CreatedBy.Id),
                                                          new XElement("CreatedOn", comment.CreatedOn),
                                                          new XElement("Description", comment.Description),
                                                          new XElement("Grade", comment.Grade)));
                    doc.Save(fileName);
                }
                catch { }
            }
        }

        public Comment GetCommentById(Guid id)
        {
            if (File.Exists(fileName))
            {
                List<Comment> realComments = new List<Comment>();
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument doc = XDocument.Load(stream);
                IEnumerable<CommentDto> comments =
                    doc.Root
                    .Elements("Comment")
                    .Select(comment => new CommentDto
                    {
                        CreatedById = Guid.Parse(comment.Element("CreatedById").Value),
                        CreatedOn = DateTime.Parse(comment.Element("CreatedOn").Value),
                        DriveId = Guid.Parse(comment.Element("DriveId").Value),
                        CommentId = Guid.Parse(comment.Element("CommentId").Value),
                        Description = comment.Element("Description").Value,
                        Grade = Int32.Parse(comment.Element("Grade").Value)
                    }).ToList();

                foreach(var c in comments)
                {
                    realComments.Add(new Comment
                    {
                        CommentedOn = DataRepository._driveRepo.RetriveDriveById(c.DriveId),
                        CommentId = c.CommentId,
                        CreatedBy = DataRepository._customerRepo.RetriveCustomerById(c.CreatedById),
                        CreatedOn = c.CreatedOn,
                        Description = c.Description,
                        Grade = c.Grade
                    });
                }

                Comment commentx = realComments.FirstOrDefault(x => x.CommentId == id);

                return commentx;
            }
            else
            {
                return null;
            }
        }
    }
}