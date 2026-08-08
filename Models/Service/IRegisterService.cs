using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteQLBanDongHo.Models.ViewModel;

namespace WebsiteQLBanDongHo.Models.Service
{
    interface IRegisterService
    {
        bool isExistAccount(string account);
        bool isValidPassword(string password);
        void RegisterAccount(RegisterViewModel register);
    }
}