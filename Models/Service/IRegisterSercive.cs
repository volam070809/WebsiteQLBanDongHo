using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.Models.ViewModel;

namespace WebsiteQLBanDongHo.Models.Service
{
    interface IRegisterSercive
    {
        bool isExistAccount(string account);
        bool isPasswordAccount(string password);
        void RegisterAccount(Register register);
    }
}