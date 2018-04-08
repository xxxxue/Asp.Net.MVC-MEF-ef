using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Dal;
using Entity;
using MefBase.IBaseEF;

namespace DAL.IDal
{


    public interface IUserDal:IRepository<UserInfoSet>
    {
        UserInfoSet UserLogin(string userName, string userPwd);


    }
}
