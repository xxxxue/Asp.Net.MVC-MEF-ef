using System;
using System.ComponentModel.Composition;
using System.Linq;

using DAL.DBContext;
using DAL.IDal;

namespace DAL.Dal
{
    [Export(typeof(INoteDal))]
    public partial class NoteDal: Base<Entity.NoteContentSet>, INoteDal
    {
       


    }
}
