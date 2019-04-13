using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountOwnerServer.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly ILoggerManager logger;
        private readonly IRepositoryWrapper repositoryWrapper;

        public OwnerController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper)
        {
            this.logger = logger;
            this.repositoryWrapper = repositoryWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            try
            {
                var owners = await repositoryWrapper.OwnerRepository.GetAllOwnersAsync();
                logger.LogInfo($"Get all owners from database");

                return Ok(owners);
            }
            catch(Exception ex)
            {
                logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "OwnerById")]
        public async Task<IActionResult> GetOwnerById(Guid id)
        {
            try
            {
                var owner = await repositoryWrapper.OwnerRepository.GetOwnerByIdAsync(id);

                if(owner.Id.Equals(Guid.Empty))
                {
                    logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    logger.LogInfo($"Returned owner with id: {id}");
                    return Ok(owner);
                }
            }
            catch(Exception ex)
            {
                logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/Account")]
        public async Task<IActionResult> GetOwnerWithDetails(Guid id)
        {
            try
            {
                //TODO: Will be implemented
                var owner = await repositoryWrapper.OwnerRepository.GetOwnerWithDetailsAsync(id);
                if (owner.IsEmptyObject())
                {
                    logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    logger.LogInfo($"Returned owner with details for id: {id}");
                    return Ok(owner);
                }
                return Ok(owner);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromBody]Owner owner)
        {
            try
            {
                //TODO: Change it to owner.IsObjectNull
                if (owner.IsObjectNull())
                {
                    logger.LogError("Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                await repositoryWrapper.OwnerRepository.CreateOwnerAsync(owner);

                return CreatedAtRoute("OwnerById", new { id = owner.Id }, owner);
            }
            catch(Exception ex)
            {
                logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromBody]Owner owner)
        {
            try
            {
                //TODO: change it to owner.IsObjectNull
                if (owner.IsObjectNull())
                {
                    logger.LogError("Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbOwner = await repositoryWrapper.OwnerRepository.GetOwnerByIdAsync(id);

                //TODO: change it to dbOwner.IsEmptyObject
                if (dbOwner.IsEmptyObject())
                {
                    logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                await repositoryWrapper.OwnerRepository.UpdateOwnerAsync(dbOwner, owner);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var owner = await repositoryWrapper.OwnerRepository.GetOwnerByIdAsync(id);
                //TODO: change it to owner.EmptyObject()
                if (owner.IsEmptyObject())
                {
                    logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var accounts = await repositoryWrapper.AccountRepository.AccountsByOwner(id);
                if (accounts.Any())
                {
                    logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                }

                await repositoryWrapper.OwnerRepository.DeleteOwnerAsync(owner);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}