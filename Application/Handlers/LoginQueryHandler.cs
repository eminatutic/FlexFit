using FlexFit.Application.Queries;
using FlexFit.Data;
using FlexFit.MongoModels.Models;
using FlexFit.MongoModels.Repositories;
using FlexFit.Token;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace FlexFit.Application.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly LoginRepository _loginRepository;

        public LoginQueryHandler(AppDbContext context, ITokenService tokenService, LoginRepository loginRepository)
        {
            _context = context;
            _tokenService = tokenService;
            _loginRepository = loginRepository;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // 1?? Provera korisnika u SQL bazi
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.LoginDto.Email, cancellationToken);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.LoginDto.Password, user.Password))
            {
                return null; // Nevalidni kredencijali
            }

            // 2?? Kreiranje tokena
            var token = _tokenService.CreateToken(user);

            // 3?? Logovanje u MongoDB kolekciju Login
            var log = new Login
            {
                UserId = user.Id.ToString(),     // ili samo user.Id ako je string
                Email = user.Email,
                Role = user.Role.ToString(),     // "Member" ili "Employee"
                Time = DateTime.UtcNow
            };

            await _loginRepository.AddAsync(log);

            return token;
        }
    }
}