using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using MefBase.IBaseEF;

namespace DAL.IDal
{
   public interface INoteDal : IRepository<NoteContentSet>
    {
        
    } 
   
}
