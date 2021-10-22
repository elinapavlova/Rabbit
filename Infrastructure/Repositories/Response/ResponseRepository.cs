﻿using Database;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Response
{
    public class ResponseRepository: BaseRepository<Models.Responses.Response>, IResponseRepository
    {
        public ResponseRepository (IDbContextFactory<AppDbContext> factory) : base (factory)
        {
        }
    }
}