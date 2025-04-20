using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.DTOs.User;
using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Core.Specifications.UserSpecs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController(IGenericRepository<User> repo, IJwtTokenService tokenService) : BaseApiController
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<User>>> GetUsers(
        [FromQuery]UserSpecParams specParams)
    {
        var spec = new PaginatedUsersSpec(specParams);

        return await CreatePagedResult(repo, spec, specParams.PageIndex,specParams.PageSize);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await repo.GetByIdAsync(id);

        if (user == null) return NotFound();

        return user;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterUser(RegisterDto dto, CancellationToken cancellationToken)
    {
        var spec = new UserWithEmailSpec(dto.Email);
        var existingUser = await repo.GetEntityWithSpec(spec);

        if (existingUser != null)
            return BadRequest("Email is already in use");
        
        using var hmac = new HMACSHA512();

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email.ToLower(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key
        };

        repo.Add(user);

        if (await repo.SaveAllAsync(cancellationToken))
        {
            return new UserDto { Id = user.Id, Username = user.Username, Email = user.Email };
        }
        
        return BadRequest("Problem registering user");

    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginUser(LoginDto dto)
    {
        var spec = new UserWithEmailSpec(dto.Email);
        var user = await repo.GetEntityWithSpec(spec);

        if (user == null) return Unauthorized("Invalid credentials");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        if (!computedHash.SequenceEqual(user.PasswordHash)) return Unauthorized("Invalid email or password");

        var token = tokenService.CreateToken(user);

        return new UserDto
        { 
            Id = user.Id, 
            Username = user.Username, 
            Email = user.Email,
            Token = token
        };
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, User user, CancellationToken cancellationToken)
    {
        if (user.Id != id || !UserExists(id)) return BadRequest("Cannot update this user");

        repo.Update(user);

        if (await repo.SaveAllAsync(cancellationToken))
        {
            return NoContent();
        }

        return BadRequest("Problem updating the user");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var user = await repo.GetByIdAsync(id);

        if (user == null) return NotFound();

        repo.Remove(user);

        if (await repo.SaveAllAsync(cancellationToken))
        {
            return NoContent();
        }

        return BadRequest("Problem deleting user");
    }

    


    private bool UserExists(int id)
    {
        return repo.Exists(id);
    }

}
