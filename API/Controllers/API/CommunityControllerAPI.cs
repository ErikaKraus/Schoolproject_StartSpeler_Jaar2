using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using API.Data.UnitofWork;

namespace API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityControllerAPI : ControllerBase
    {
        private readonly IUnitofWork _context;

        public CommunityControllerAPI(IUnitofWork context)
        {
            _context = context;
        }

        // GET: api/CommunityControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Community>>> GetAllCommunities()
        {
            var communities = await _context.CommunityRepository.GetAllAsync();
            return Ok(communities);
        }

        // GET: api/CommunityControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Community>> GetCommunity(int id)
        {
          if (_context.CommunityRepository == null)
          {
              return NotFound();
          }
            var community = await _context.CommunityRepository.GetByIdAsync(id);

            if (community == null)
            {
                return NotFound();
            }

            return community;
        }

        // PUT: api/CommunityControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommunity(int id, Community community)
        {
            if (id != community.Id)
            {
                return BadRequest();
            }

            _context.CommunityRepository.Update(community);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CommunityControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Community>> CreateCommunity(Community community)
        {
          if (_context.CommunityRepository == null)
          {
              return Problem("Entity set 'StartspelerContext.Communities'  is null.");
          }
            await _context.CommunityRepository.AddAsync(community);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommunity", new { id = community.Id }, community);
        }

        // DELETE: api/CommunityControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            if (_context.CommunityRepository == null)
            {
                return NotFound();
            }
            var community = await _context.CommunityRepository.GetByIdAsync(id);
            if (community == null)
            {
                return NotFound();
            }

            _context.CommunityRepository.Delete(community);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommunityExists(int id)
        {
            return (_context.CommunityRepository.GetByIdAsync(id).IsCompletedSuccessfully);
        }
    }
}
