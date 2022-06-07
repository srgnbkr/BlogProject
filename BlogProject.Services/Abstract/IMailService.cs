using BlogProject.Entities.DTOs.EmailDto;
using BlogProject.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Abstract
{
    public  interface IMailService
    {
        IResult SendMail(EmailSendDto emailSendDto);
        IResult SendContactMail(EmailSendDto emailSendDto);
    }
}
