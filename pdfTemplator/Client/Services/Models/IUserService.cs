﻿using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IUserService : IService
    {
        Task<IResult<List<UserInfo>>> GetAllAsync();
    }
}
