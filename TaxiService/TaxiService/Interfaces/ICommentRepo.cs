using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiService.Models;

namespace TaxiService.Interfaces
{
    public interface ICommentRepo
    {
        Comment GetCommentById(Guid id);
        void AddNewComment(Comment comment);
    }
}
