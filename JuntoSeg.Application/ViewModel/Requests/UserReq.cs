using System;
using System.Collections.Generic;
using System.Text;

namespace JuntoSeg.Application.ViewModel.Requests
{
    public class UserReq : UserVm
    {
        public int Id { get; set; }
        public string LoginPassword { get; set; }
        public string NotHashedPassword { get; set; }
    }
}
