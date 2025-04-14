using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController(IGenericRepository<User> repo) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<User>>> GetUsers(CancellationToken cancellationToken)
    {
        return Ok(await repo.ListAllAsync(cancellationToken));
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

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user, CancellationToken cancellationToken)
    {
        repo.Add(user);

        if (await repo.SaveAllAsync(cancellationToken))
        {
            return CreatedAtAction("GetUser", new {id = user.Id}, user);
        }

        return BadRequest("Problem creating this user");
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
